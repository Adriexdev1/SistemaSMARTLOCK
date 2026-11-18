const { Client, LocalAuth, MessageMedia } = require("whatsapp-web.js");
const qrcode = require("qrcode-terminal");
const QRCode = require("qrcode");
const apiUrl = (process.env.SMARTLOCK_API_URL || "http://localhost:5000").replace(/\/+$/, "");

// Se comparan los ÚLTIMOS 10 DÍGITOS: así da igual si WhatsApp entrega 52..., 521... o el número a secas.
const last10 = number => String(number || "").replace(/\D/g, "").slice(-10);
const allowedSenders = new Set(
    (process.env.WHATSAPP_ALLOWED_NUMBERS || "")
        .split(",")
        .map(last10)
        .filter(n => n.length === 10)
);

if (allowedSenders.size === 0) {
    throw new Error("Configura WHATSAPP_ALLOWED_NUMBERS con un número de 10 dígitos o más (ej. 8112345678).");
}

let botReadyAt = null;
const headless = (process.env.WHATSAPP_HEADLESS || "true").toLowerCase() !== "false";

const client = new Client({
    authStrategy: new LocalAuth(),
    puppeteer: { headless, args: ['--no-sandbox', '--disable-setuid-sandbox'] }
});

const userSessions = new Map();

// msg.reply marca el chat como leído antes de enviar; en algunas versiones de WhatsApp Web eso falla
// ("markedUnread", etc.) y el mensaje nunca sale. Si pasa, se reintenta sin marcar como leído.
async function safeReply(msg, content, options = {}) {
    try {
        return await msg.reply(content, undefined, options);
    } catch (error) {
        console.warn("⚠️ reply falló, reintento sin 'visto':", error.message);
        return client.sendMessage(msg.from, content, { ...options, sendSeen: false });
    }
}

// =========================================================
// INICIO DEL BOT
// =========================================================
client.on("qr", qr => {
    console.log("📱 Escanea este QR para iniciar el bot:");
    qrcode.generate(qr, { small: true });
});

client.on("authenticated", () => console.log("🔑 Sesión de WhatsApp autenticada."));
client.on("auth_failure", m => console.error("❌ Falló la autenticación:", m));
client.on("disconnected", m => console.warn("⚠️ WhatsApp desconectado:", m));

client.on("ready", () => {
    botReadyAt = Math.floor(Date.now() / 1000);
    console.log(`✅ Bot de SmartLock conectado a WhatsApp! Números autorizados (últimos 10 dígitos): ${[...allowedSenders].map(n => "..." + n.slice(-4)).join(", ")}`);
});

// Número real del remitente. WhatsApp puede entregar un LID interno en lugar del número.
async function getSenderNumber(msg) {
    if (msg.from.endsWith("@c.us")) {
        return msg.from.split("@")[0];
    }

    if (!msg.from.endsWith("@lid")) {
        return "";
    }

    try {
        const [contact] = await client.getContactLidAndPhone([msg.from]);
        if (contact?.pn?.endsWith("@c.us")) {
            return contact.pn.split("@")[0];
        }
    } catch (error) {
        console.warn("⚠️ No se pudo resolver el número del remitente desde su LID:", error.message);
    }

    return "";
}

client.on("message", async msg => {
    const phone = msg.from;

    // Este log va PRIMERO: si no lo ves al escribirle al bot, el evento ni siquiera está llegando.
    console.log(`📥 Evento message: from=${phone} fromMe=${msg.fromMe} tipo=${msg.type}`);

    if (msg.fromMe || msg.isStatus || phone === "status@broadcast" || phone.endsWith("@g.us")) {
        return;
    }
    if (!phone.endsWith("@c.us") && !phone.endsWith("@lid")) {
        console.log("⏭️ Ignorado: no es un chat individual.");
        return;
    }

    const senderPhone = (await getSenderNumber(msg)).replace(/\D/g, "");
    if (!senderPhone) {
        console.warn("⏭️ Ignorado: WhatsApp no proporcionó un número telefónico para este remitente.");
        return;
    }

    const senderNumber = last10(senderPhone);
    const senderLabel = senderNumber.length > 4 ? `...${senderNumber.slice(-4)}` : "****";
    const isAllowedSender = allowedSenders.has(senderNumber);
    console.log(`📩 Mensaje entrante de ${senderLabel}. Autorizado: ${isAllowedSender ? "sí" : "no"}.`);
    if (!isAllowedSender) return;

    // Tolerancia de 60 s: msg.timestamp es hora de los servidores de WhatsApp y botReadyAt la hora de tu PC;
    // si el reloj de la PC está adelantado, comparar sin margen descarta mensajes válidos.
    if (botReadyAt === null || !Number.isFinite(msg.timestamp) || msg.timestamp < botReadyAt - 60) {
        console.log(`⏭️ Mensaje ignorado: anterior a la conexión (msg=${msg.timestamp}, listo=${botReadyAt}).`);
        return;
    }

    const message = (msg.body || "").trim();
    if (!message) return;
    
    if (!userSessions.has(phone)) {
        userSessions.set(phone, { step: "menu_principal", data: { telefonoWhatsApp: senderPhone }, lastActivity: new Date() });
        try {
            await safeReply(msg, "👋 ¡Hola! Bienvenido al *Sistema SmartLock*\n\nElige una opción:\n1️⃣ *Agendar cita*\n\nPor ahora las cancelaciones no están disponibles. Escribe *1* para agendar.");
        } catch (error) {
            userSessions.delete(phone);
            console.error("❌ No se pudo enviar el mensaje inicial de WhatsApp (ni con reintento):", error);
        }
        return;
    }

    const session = userSessions.get(phone);
    session.lastActivity = new Date();

    try {
        await handleConversation(msg, session, message);
    } catch (err) {
        console.error("Error:", err);
        await safeReply(msg, "❌ Ocurrió un error. Intenta más tarde.");
    }
});

// =========================================================
// MANEJO DE CONVERSACIÓN
// =========================================================
async function handleConversation(msg, session, message) {
    switch (session.step) {
        // ===== MENÚ PRINCIPAL =====
        case "menu_principal":
            if (message === "1") {
                session.step = "tiene_cuenta";
                await safeReply(msg, "¿Ya tienes una cuenta registrada con tu correo electrónico? (responde *sí* o *no*)");
            } else {
                await safeReply(msg, "❌ Opción no válida. Escribe *1* para agendar una cita.");
            }
            break;

        // ===== AGENDAR CITA =====
        case "tiene_cuenta":
            if (["si", "sí"].includes(message.toLowerCase())) {
                session.step = "correo_existente";
                await safeReply(msg, "📧 Por favor, escribe tu *correo electrónico* registrado:");
            } else {
                session.step = "nombre";
                await safeReply(msg, "Perfecto. Empecemos tu registro.\nPor favor, dime tu *nombre completo*:");
            }
            break;

        case "correo_existente":
            session.data.correo = message.trim().toLowerCase();
            session.step = "motivo";
            await safeReply(msg, "✅ Gracias. ¿Cuál es el motivo de tu cita?");
            break;

        case "nombre":
            session.data.nombre = message;
            session.step = "correo";
            await safeReply(msg, "📧 Escribe tu *correo electrónico*:");
            break;

        case "correo":
            session.data.correo = message;
            session.step = "nacimiento";
            await safeReply(msg, "🎂 Indica tu *fecha de nacimiento* (YYYY-MM-DD):");
            break;

        case "nacimiento":
            session.data.nacimiento = message;
            session.step = "motivo";
            await safeReply(msg, "✅ Datos listos. ¿Cuál es el motivo de tu cita?");
            break;

        case "motivo":
            session.data.motivo = message;
            session.step = "fecha";
            await safeReply(msg, "🗓️ ¿Qué *día* deseas tu cita? (formato: YYYY-MM-DD)");
            break;

        case "fecha":
            session.data.fecha = message;
            session.step = "hora";
            await safeReply(msg, "🕒 ¿A qué *hora* prefieres? (formato 24h, ejemplo: 15:00)");
            break;

        case "hora": {
            if (!/^\d{2}:\d{2}$/.test(message)) {
                await safeReply(msg, "❌ Escribe la hora en formato 24 h, por ejemplo 15:00.");
                return;
            }
            
            const [hora, minuto] = message.split(":").map(Number);
            if (hora > 22 || minuto > 59) {
                await safeReply(msg, "❌ Elige una hora válida que permita una cita de una hora antes de medianoche.");
                return;
            }
            
            const totalMinutosFin = hora * 60 + minuto + 60;
            const horaFin =
            `${String(Math.floor(totalMinutosFin / 60)).padStart(2, "0")}:` +
            `${String(totalMinutosFin % 60).padStart(2, "0")}:00`;
            
            const apiKey = process.env.SMARTLOCK_API_KEY;
            if (!apiKey) {
                console.error("Falta configurar SMARTLOCK_API_KEY.");
                await safeReply(msg, "❌ El servicio de citas no está configurado. Intenta más tarde.");
                return;
            }
            
            const respuestaApi = await fetch(`${apiUrl}/api/whatsapp/citas`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "X-WhatsApp-Key": apiKey
                },
                
                body: JSON.stringify({
                    telefonoWhatsApp: session.data.telefonoWhatsApp,
                    correoUsuario: session.data.correo,
                    nombreUsuario: session.data.nombre,
                    fechaNacimiento: session.data.nacimiento,
                    motivo: session.data.motivo,
                    fecha: session.data.fecha,                    
                    horaInicio: `${message}:00`,
                    horaFin                
                })
            });
            
            const textoRespuesta = await respuestaApi.text();
            let resultado = null;
            
            if (textoRespuesta) {
                try {
                    resultado = JSON.parse(textoRespuesta);
                } catch {
                    if (respuestaApi.ok) {
                        throw new Error("La API devolvió una respuesta JSON no válida.");
                    }
                }
            }
            
            if (!respuestaApi.ok) {
                const detalle = resultado?.mensaje ||
                `No se pudo registrar la cita (HTTP ${respuestaApi.status}).`;
                await safeReply(msg, `❌ ${detalle}`);
                return;
            }
            
            if (!resultado?.codigoQr || !resultado?.idDetalleEvento) {
                throw new Error("La respuesta de la API no contiene el QR o el ID del detalle.");
            }
            
            const qrDataURL = await QRCode.toDataURL(resultado.codigoQr);
            const base64Data = qrDataURL.replace(/^data:image\/png;base64,/, "");
            const media = new MessageMedia("image/png", base64Data, "cita_qr.png");
            
            await safeReply(msg, media);
            await safeReply(msg, `✅ *Cita confirmada*
📅 Fecha: ${session.data.fecha}
🕒 Hora: ${message}
📋 Motivo: ${session.data.motivo}
🔗 ID Detalle: ${resultado.idDetalleEvento}

Escribe *menu* para regresar al inicio.`);

    userSessions.delete(msg.from);
    break;
}

        // ===== CANCELAR CITA =====
    }
}

client.initialize();