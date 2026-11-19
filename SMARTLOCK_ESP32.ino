#include <WiFi.h>
#include <HTTPClient.h>
#include <ESP32Servo.h>
#include <LiquidCrystal_I2C.h>
#include <LittleFS.h>
#include <ArduinoJson.h>
#include <Wire.h>
#include <time.h>
#include <sys/time.h>
#include <esp_system.h>
#include <mbedtls/base64.h>
#include <vector>

LiquidCrystal_I2C lcd(0x27, 16, 2);
Servo servo;

const int trigPin = 25;
const int echoPin = 26;
const int boton = 27;
const int qrRx = 16;
const int qrTx = 17;
const int servoPin = 18;

HardwareSerial qrSerial(2);

const char* ssid = "CONFIGURAR_WIFI_SSID";
const char* password = "CONFIGURAR_WIFI_PASSWORD";
const char* serverBaseUrl = "http://192.168.1.100:8080";
const char* httpUsername = "esp32";
const char* httpPassword = "CAMBIAR_EN_ESP32_Y_PYTHON";

bool abrirProceso = false;
unsigned long tiempoApertura = 20000;
unsigned long tiempoInicio = 0;
int posActual = 85;
const int posMax = 160;
const int distanciaSegura = 9;
const int velocidad = 5;

bool wifiListo = false;
bool sistemaArchivosListo = false;
bool cacheDisponible = false;
bool estadoLocalValido = false;
bool sqlUtcOffsetDisponible = false;
bool internetSimuladoDesconectado = false;
String qrBuffer = "";
unsigned long lastQrTime = 0;
unsigned long ultimoIntentoRed = 0;
unsigned long ultimaActualizacionCache = 0;
unsigned long ultimaConsultaEstado = 0;

const uint32_t cacheTtlMaximoSegundos = 60UL * 60UL;
const uint32_t intervaloReintentoRedMs = 15000UL;
const uint32_t intervaloActualizacionCacheMs = 5UL * 60UL * 1000UL;
const uint16_t maxRegistrosPendientes = 128;
const uint16_t maxCodigosUsados = 512;
const time_t epochMinimoConfiable = 1735689600;

struct QrAutorizado {
  int idDetalleEvento;
  String codigoQr;
  int idUsuario;
  bool estadoUsuario;
  bool pinConfigurado;
  int idEvento;
  String nombreEvento;
  bool estadoEvento;
  bool qrUsado;
  bool tieneFechaInicio;
  int64_t fechaInicioEpoch;
  bool tieneFechaLimite;
  int64_t fechaLimiteEpoch;
  uint32_t disponibleEnSegundos;
  uint32_t validoPorSegundos;
};

struct CacheQr {
  uint32_t vigenciaSegundos;
  int64_t snapshotSqlEpoch;
  int64_t serverUtcEpoch;
  int64_t sqlUtcOffsetSegundos;
  std::vector<QrAutorizado> qrs;
};

struct RegistroLocal {
  String registroId;
  String tipo;
  int idDetalleEvento;
  int idUsuario;
  int idEvento;
  String codigoQr;
  String codigoQrHash;
  String motivo;
  int64_t snapshotSqlEpoch;
  int64_t fechaAccesoSqlEpoch;
  bool estadoUsuario;
  bool pinConfigurado;
  bool estadoEvento;
};

struct EstadoLocal {
  uint32_t revision;
  int slot;
  std::vector<RegistroLocal> pendientes;
  std::vector<String> codigosUsados;
};

enum ResultadoCache {
  CACHE_ACTUALIZADA,
  CACHE_NO_DISPONIBLE,
  CACHE_AUTENTICACION_FALLIDA,
  CACHE_RESPUESTA_INVALIDA
};

struct ResultadoSync {
  bool respuestaValida;
  std::vector<String> confirmados;
  std::vector<String> conflictos;
};

CacheQr cacheActual;
EstadoLocal estadoLocal;
int cacheSlotActivo = -1;
int64_t sqlUtcOffsetSegundos = 0;
String comandoSerial;

bool configurarAlmacenamiento();
void configurarWiFi();
void sincronizarReloj();
ResultadoCache actualizarCacheQr();
bool cargarCacheDesdeLittleFS();
bool guardarCacheAtomico(const String& json, const CacheQr& cacheNueva);
bool parsearCache(const String& json, CacheQr& resultado);
bool cargarEstadoLocal();
bool leerEstadoArchivo(const char* ruta, EstadoLocal& resultado);
bool guardarEstadoLocalAtomico();
bool agregarRegistroLocal(const RegistroLocal& registro);
bool registrarRechazo(int idDetalleEvento, const String& codigoQr, const String& motivo);
bool sincronizarPendientes();
ResultadoSync enviarPendientes();
bool validarQrLocal(const String& id, const String& codigoQr, QrAutorizado& autorizado);
bool obtenerHoraSqlActual(int64_t& epochSql);
bool relojConfiable();
bool codigoMarcadoUsado(const String& codigoQr);
bool codigoPendiente(const String& codigoQr);
String generarUuid();
String huellaQr(const String& codigoQr);
String crearAutorizacionBasica();
int peticionHttp(const String& metodo, const String& ruta, const String& cuerpo,
                 String& respuesta, uint16_t timeoutMs);
bool extraerIdQr(const String& texto, int& id);
bool esUuidValido(const String& valor);
String extraerParametro(String url, String parametro);
void procesarQR(String id, String token);
void leerQR();
void leerComandoSerial();
void servicioRed();
bool actualizarCacheYSync();
void abrirAcceso(const String& id, const String& registroId, bool offline);
void controlarServo(long distancia);
void actualizarLCD();
void lcdMessage(String msg);
long medirDistancia();
void cerrarPuerta();

void setup() {
  Serial.begin(115200);
  delay(1000);

  servo.setPeriodHertz(50);
  servo.attach(servoPin, 500, 2400);
  cerrarPuerta();

  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  pinMode(boton, INPUT_PULLUP);

  Wire.begin(21, 22);
  lcd.init();
  lcd.backlight();
  lcdMessage("INICIANDO...");

  qrSerial.begin(9600, SERIAL_8N1, qrRx, qrTx);
  configurarAlmacenamiento();
  cargarCacheDesdeLittleFS();

  lcdMessage("CONECTANDO WIFI");
  configurarWiFi();

  if (wifiListo) {
    configTime(0, 0, "pool.ntp.org", "time.nist.gov");
    sincronizarReloj();
    actualizarCacheYSync();
  }

  lcdMessage(wifiListo ? "SISTEMA LISTO" : "MODO SIN RED");
  Serial.println("Comandos de prueba: OFFLINE / ONLINE");
  delay(1500);
}

void loop() {
  leerComandoSerial();
  wifiListo = WiFi.status() == WL_CONNECTED;

  long distancia = medirDistancia();
  leerQR();

  if (digitalRead(boton) == LOW) {
    abrirProceso = !abrirProceso;
    lcdMessage(abrirProceso ? "MANUAL: ABIERTO" : "MANUAL: CERRADO");
    delay(500);
  }

  controlarServo(distancia);
  actualizarLCD();

  if (abrirProceso && posActual == posMax &&
      millis() - tiempoInicio >= tiempoApertura &&
      distancia >= distanciaSegura) {
    abrirProceso = false;
    tiempoInicio = 0;
    lcdMessage("CIERRE AUTO");
  }

  servicioRed();
  delay(100);
}

bool configurarAlmacenamiento() {
  sistemaArchivosListo = LittleFS.begin(false);
  if (!sistemaArchivosListo) {
    Serial.println("ERROR: LittleFS no disponible; no se formateara para proteger datos.");
    return false;
  }

  estadoLocalValido = cargarEstadoLocal();
  if (!estadoLocalValido)
    Serial.println("ERROR: cola local invalida; acceso offline bloqueado.");

  return true;
}

void configurarWiFi() {
  WiFi.mode(WIFI_STA);
  WiFi.setAutoReconnect(true);
  WiFi.begin(ssid, password);

  Serial.println("CONECTANDO A WIFI...");
  unsigned long inicio = millis();

  while (WiFi.status() != WL_CONNECTED && millis() - inicio < 15000) {
    delay(500);
    Serial.print(".");
  }

  wifiListo = WiFi.status() == WL_CONNECTED;
  if (wifiListo) {
    Serial.println("\nWIFI CONECTADO");
    Serial.print("IP: ");
    Serial.println(WiFi.localIP());
  } else {
    Serial.println("\nERROR DE CONEXION WIFI");
  }
}

void leerQR() {
  while (qrSerial.available()) {
    char caracter = (char)qrSerial.read();

    if (caracter == '\n' || caracter == '\r') {
      if (qrBuffer.length() > 0) {
        String id = extraerParametro(qrBuffer, "id=");
        String token = extraerParametro(qrBuffer, "token=");
        qrBuffer = "";
        procesarQR(id, token);
        return;
      }
    } else if (caracter >= 32 && caracter <= 126) {
      if (qrBuffer.length() < 256)
        qrBuffer += caracter;
      lastQrTime = millis();
    }

    if (qrBuffer.length() > 0 && millis() - lastQrTime > 1000)
      qrBuffer = "";
  }
}

String extraerParametro(String url, String parametro) {
  int inicio = url.indexOf(parametro);
  if (inicio < 0)
    return "";

  inicio += parametro.length();
  int fin = url.indexOf('&', inicio);
  if (fin < 0)
    fin = url.length();
  return url.substring(inicio, fin);
}

bool extraerIdQr(const String& texto, int& id) {
  if (texto.length() == 0)
    return false;
  for (size_t i = 0; i < texto.length(); i++) {
    if (texto[i] < '0' || texto[i] > '9')
      return false;
  }
  id = texto.toInt();
  return id > 0 && String(id) == texto;
}

bool esUuidValido(const String& valor) {
  if (valor.length() != 36)
    return false;

  for (size_t i = 0; i < valor.length(); i++) {
    char caracter = valor[i];
    if (i == 8 || i == 13 || i == 18 || i == 23) {
      if (caracter != '-')
        return false;
    } else if (!((caracter >= '0' && caracter <= '9') ||
                 (caracter >= 'a' && caracter <= 'f') ||
                 (caracter >= 'A' && caracter <= 'F'))) {
      return false;
    }
  }
  return true;
}

void procesarQR(String idTexto, String token) {
  if (abrirProceso)
    return;

  int idDetalleEvento = 0;
  if (!extraerIdQr(idTexto, idDetalleEvento) || !esUuidValido(token)) {
    lcdMessage("QR INVALIDO");
    Serial.println("PARAMETROS QR INVALIDOS");
    registrarRechazo(0, token, "formato_invalido");
    delay(1200);
    return;
  }

  token.toLowerCase();
  ResultadoCache resultadoCache = CACHE_NO_DISPONIBLE;

  if (WiFi.status() == WL_CONNECTED && !internetSimuladoDesconectado) {
    if (!relojConfiable())
      sincronizarReloj();
    resultadoCache = actualizarCacheQr();
  }

  if (resultadoCache == CACHE_AUTENTICACION_FALLIDA) {
    lcdMessage("ERROR AUTENTIC.");
    registrarRechazo(idDetalleEvento, token, "autenticacion_servidor");
    delay(1500);
    return;
  }

  if (!estadoLocalValido || !sistemaArchivosListo) {
    lcdMessage("SIN ALMACEN");
    Serial.println("ACCESO BLOQUEADO: LittleFS no esta disponible.");
    delay(1500);
    return;
  }

  QrAutorizado autorizado;
  if (!validarQrLocal(idTexto, token, autorizado)) {
    lcdMessage("ACCESO DENEGADO");
    registrarRechazo(idDetalleEvento, token, "no_autorizado_o_vencido");
    Serial.println("QR RECHAZADO POR CACHE LOCAL");
    if (resultadoCache == CACHE_ACTUALIZADA)
      sincronizarPendientes();
    delay(1500);
    return;
  }

  int64_t horaSqlActual = 0;
  if (!obtenerHoraSqlActual(horaSqlActual)) {
    lcdMessage("HORA NO VALIDA");
    registrarRechazo(idDetalleEvento, token, "hora_no_confiable");
    delay(1500);
    return;
  }

  RegistroLocal registro;
  registro.registroId = generarUuid();
  registro.tipo = "acceso";
  registro.idDetalleEvento = autorizado.idDetalleEvento;
  registro.idUsuario = autorizado.idUsuario;
  registro.idEvento = autorizado.idEvento;
  registro.codigoQr = token;
  registro.codigoQrHash = "";
  registro.motivo = "";
  registro.snapshotSqlEpoch = cacheActual.snapshotSqlEpoch;
  registro.fechaAccesoSqlEpoch = horaSqlActual;
  registro.estadoUsuario = autorizado.estadoUsuario;
  registro.pinConfigurado = autorizado.pinConfigurado;
  registro.estadoEvento = autorizado.estadoEvento;

  if (codigoMarcadoUsado(token) || codigoPendiente(token)) {
    lcdMessage("QR YA UTILIZADO");
    registrarRechazo(idDetalleEvento, token, "qr_ya_utilizado_localmente");
    delay(1500);
    return;
  }

  if (estadoLocal.pendientes.size() >= maxRegistrosPendientes ||
      estadoLocal.codigosUsados.size() >= maxCodigosUsados) {
    lcdMessage("COLA LLENA");
    Serial.println("ACCESO BLOQUEADO: sincroniza registros para liberar espacio.");
    delay(1500);
    return;
  }

  estadoLocal.pendientes.push_back(registro);
  estadoLocal.codigosUsados.push_back(token);
  if (!guardarEstadoLocalAtomico()) {
    estadoLocal.pendientes.pop_back();
    estadoLocal.codigosUsados.pop_back();
    lcdMessage("ERROR GUARDADO");
    Serial.println("ACCESO BLOQUEADO: no se pudo persistir el registro.");
    delay(1500);
    return;
  }

  bool offline = resultadoCache != CACHE_ACTUALIZADA;
  if (!offline) {
    ResultadoSync sincronizacion = enviarPendientes();
    for (const String& conflicto : sincronizacion.conflictos) {
      if (conflicto == registro.registroId) {
        lcdMessage("QR EN CONFLICTO");
        Serial.println("ACCESO DENEGADO: el QR ya no esta disponible en SQL Server.");
        delay(1500);
        return;
      }
    }
  }

  abrirAcceso(idTexto, registro.registroId, offline);
  delay(1500);
}

bool validarQrLocal(const String& idTexto, const String& codigoQr, QrAutorizado& autorizado) {
  int idDetalleEvento = 0;
  int64_t horaSqlActual = 0;
  if (!extraerIdQr(idTexto, idDetalleEvento) ||
      !obtenerHoraSqlActual(horaSqlActual) ||
      !cacheDisponible)
    return false;

  int64_t edadCache = horaSqlActual - cacheActual.snapshotSqlEpoch;
  if (edadCache < 0 || (uint64_t)edadCache >= cacheActual.vigenciaSegundos)
    return false;

  for (const QrAutorizado& qr : cacheActual.qrs) {
    if (qr.idDetalleEvento != idDetalleEvento || qr.codigoQr != codigoQr)
      continue;

    if (qr.qrUsado || qr.estadoUsuario != true || qr.pinConfigurado ||
        qr.estadoEvento != true || codigoMarcadoUsado(codigoQr) ||
        codigoPendiente(codigoQr))
      return false;

    if ((uint64_t)edadCache >= qr.validoPorSegundos ||
        (uint64_t)edadCache < qr.disponibleEnSegundos)
      return false;

    if (qr.tieneFechaInicio && horaSqlActual < qr.fechaInicioEpoch)
      return false;
    if (qr.tieneFechaLimite && horaSqlActual >= qr.fechaLimiteEpoch)
      return false;

    autorizado = qr;
    return true;
  }
  return false;
}

bool relojConfiable() {
  return time(nullptr) >= epochMinimoConfiable && sqlUtcOffsetDisponible;
}

bool obtenerHoraSqlActual(int64_t& epochSql) {
  if (!relojConfiable())
    return false;
  epochSql = (int64_t)time(nullptr) + sqlUtcOffsetSegundos;
  return epochSql > 0;
}

void sincronizarReloj() {
  if (WiFi.status() != WL_CONNECTED || internetSimuladoDesconectado)
    return;

  String respuesta;
  int codigo = peticionHttp("GET", "/estado", "", respuesta, 2500);
  if (codigo != HTTP_CODE_OK)
    return;

  DynamicJsonDocument documento(respuesta.length() * 2 + 1024);
  if (deserializeJson(documento, respuesta))
    return;

  int64_t serverUtcEpoch = documento["serverUtcEpoch"] | 0LL;
  if (serverUtcEpoch < epochMinimoConfiable)
    return;

  time_t epoch = (time_t)serverUtcEpoch;
  timeval tiempo;
  tiempo.tv_sec = epoch;
  tiempo.tv_usec = 0;
  settimeofday(&tiempo, nullptr);

  JsonVariant sqlEpochValue = documento["sqlNowEpoch"];
  if (!sqlEpochValue.isNull()) {
    int64_t sqlEpoch = sqlEpochValue.as<int64_t>();
    sqlUtcOffsetSegundos = sqlEpoch - serverUtcEpoch;
    sqlUtcOffsetDisponible = true;
  }
  ultimaConsultaEstado = millis();
}

bool parsearCache(const String& json, CacheQr& resultado) {
  DynamicJsonDocument documento(json.length() * 2 + 2048);
  DeserializationError error = deserializeJson(documento, json);
  if (error) {
    Serial.printf("JSON de cache invalido: %s\n", error.c_str());
    return false;
  }

  uint32_t ttl = documento["cacheTtlSeconds"] | 0;
  int64_t snapshotSqlEpoch = documento["snapshotSqlEpoch"] | 0LL;
  int64_t serverUtcEpoch = documento["serverUtcEpoch"] | 0LL;
  int64_t offsetSql = documento["sqlUtcOffsetSeconds"] | 0LL;
  JsonArray qrs = documento["qrAutorizados"].as<JsonArray>();

  if (ttl == 0 || ttl > cacheTtlMaximoSegundos ||
      snapshotSqlEpoch <= 0 ||
      serverUtcEpoch < epochMinimoConfiable ||
      qrs.isNull()) {
    Serial.println("Metadatos de cache incompletos o fuera de limites.");
    return false;
  }

  CacheQr nuevaCache;
  nuevaCache.vigenciaSegundos = ttl;
  nuevaCache.snapshotSqlEpoch = snapshotSqlEpoch;
  nuevaCache.serverUtcEpoch = serverUtcEpoch;
  nuevaCache.sqlUtcOffsetSegundos = offsetSql;

  for (JsonObject item : qrs) {
    QrAutorizado qr;
    qr.idDetalleEvento = item["idDetalleEvento"] | 0;
    const char* codigo = item["codigoQr"] | "";
    qr.codigoQr = String(codigo);
    qr.idUsuario = item["idUsuario"] | 0;
    qr.estadoUsuario = item["estadoUsuario"] | false;
    qr.pinConfigurado = item["pinConfigurado"] | true;
    qr.idEvento = item["idEvento"] | 0;
    qr.nombreEvento = String((const char*)(item["nombreEvento"] | ""));
    qr.estadoEvento = item["estadoEvento"] | false;
    qr.qrUsado = item["qrUsado"] | true;
    qr.disponibleEnSegundos = item["disponibleEnSegundos"] | 0;
    qr.validoPorSegundos = item["validoPorSegundos"] | 0;

    JsonVariant inicio = item["fechaProgramadaEpoch"];
    qr.tieneFechaInicio = !inicio.isNull();
    qr.fechaInicioEpoch = qr.tieneFechaInicio ? inicio.as<int64_t>() : 0;
    JsonVariant limite = item["fechaLimiteEpoch"];
    qr.tieneFechaLimite = !limite.isNull();
    qr.fechaLimiteEpoch = qr.tieneFechaLimite ? limite.as<int64_t>() : 0;

    if (qr.idDetalleEvento <= 0 || qr.idUsuario <= 0 || qr.idEvento <= 0 ||
        !esUuidValido(qr.codigoQr) || !qr.estadoUsuario || qr.pinConfigurado ||
        !qr.estadoEvento || qr.qrUsado || qr.validoPorSegundos == 0 ||
        qr.validoPorSegundos > ttl || qr.disponibleEnSegundos >= ttl) {
      Serial.println("Un registro del snapshot no cumple las validaciones esperadas.");
      return false;
    }

    if (qr.tieneFechaInicio && qr.fechaInicioEpoch <= 0)
      return false;
    if (qr.tieneFechaLimite && qr.fechaLimiteEpoch <= 0)
      return false;

    nuevaCache.qrs.push_back(qr);
  }

  resultado = nuevaCache;
  return true;
}

bool leerArchivo(const char* ruta, String& contenido) {
  if (!sistemaArchivosListo || !LittleFS.exists(ruta))
    return false;
  File archivo = LittleFS.open(ruta, FILE_READ);
  if (!archivo)
    return false;
  contenido = archivo.readString();
  archivo.close();
  return contenido.length() > 0;
}

bool cargarCacheDesdeLittleFS() {
  if (!sistemaArchivosListo)
    return false;

  const char* rutas[] = {"/qr_cache_a.json", "/qr_cache_b.json"};
  bool existeCache = false;
  bool cacheValida[2] = {false, false};
  CacheQr candidatas[2];

  for (int i = 0; i < 2; i++) {
    if (!LittleFS.exists(rutas[i]))
      continue;
    existeCache = true;
    String contenido;
    if (leerArchivo(rutas[i], contenido))
      cacheValida[i] = parsearCache(contenido, candidatas[i]);
  }

  if (!cacheValida[0] && !cacheValida[1]) {
    cacheDisponible = false;
    if (existeCache)
      Serial.println("No se pudo leer un snapshot local valido; se conserva sin borrar.");
    return false;
  }

  int elegido = cacheValida[0] ? 0 : 1;
  if (cacheValida[0] && cacheValida[1] &&
      candidatas[1].snapshotSqlEpoch > candidatas[0].snapshotSqlEpoch)
    elegido = 1;

  cacheActual = candidatas[elegido];
  cacheSlotActivo = elegido;
  cacheDisponible = true;
  sqlUtcOffsetSegundos = cacheActual.sqlUtcOffsetSegundos;
  sqlUtcOffsetDisponible = true;
  Serial.printf("Cache local cargada: %u QR\n",
                (unsigned)cacheActual.qrs.size());
  return true;
}

bool guardarCacheAtomico(const String& json, const CacheQr& cacheNueva) {
  const char* rutas[] = {"/qr_cache_a.json", "/qr_cache_b.json"};
  const char* temporal = "/qr_cache_tmp.json";
  if (LittleFS.exists(temporal))
    LittleFS.remove(temporal);

  File archivo = LittleFS.open(temporal, FILE_WRITE);
  if (!archivo)
    return false;
  size_t escrito = archivo.print(json);
  archivo.flush();
  archivo.close();
  if (escrito != json.length()) {
    LittleFS.remove(temporal);
    return false;
  }

  String verificacion;
  CacheQr cacheVerificada;
  if (!leerArchivo(temporal, verificacion) ||
      !parsearCache(verificacion, cacheVerificada) ||
      cacheVerificada.snapshotSqlEpoch != cacheNueva.snapshotSqlEpoch) {
    LittleFS.remove(temporal);
    return false;
  }

  int destino = cacheSlotActivo == 0 ? 1 : 0;
  if (LittleFS.exists(rutas[destino]))
    LittleFS.remove(rutas[destino]);
  if (!LittleFS.rename(temporal, rutas[destino])) {
    Serial.println("No se pudo sustituir la copia inactiva; se conserva la anterior.");
    return false;
  }

  cacheActual = cacheVerificada;
  cacheSlotActivo = destino;
  cacheDisponible = true;
  sqlUtcOffsetSegundos = cacheActual.sqlUtcOffsetSegundos;
  sqlUtcOffsetDisponible = true;

  if (!relojConfiable()) {
    timeval tiempo;
    tiempo.tv_sec = (time_t)cacheActual.serverUtcEpoch;
    tiempo.tv_usec = 0;
    settimeofday(&tiempo, nullptr);
  }
  Serial.printf("Cache actualizada: %u QR; vigencia %lu s.\n",
                (unsigned)cacheActual.qrs.size(),
                (unsigned long)cacheActual.vigenciaSegundos);
  return true;
}

ResultadoCache actualizarCacheQr() {
  if (!sistemaArchivosListo || WiFi.status() != WL_CONNECTED ||
      internetSimuladoDesconectado)
    return CACHE_NO_DISPONIBLE;

  String respuesta;
  int codigo = peticionHttp("GET", "/qr-autorizados", "", respuesta, 3500);
  if (codigo == 401 || codigo == 403) {
    Serial.println("Autenticacion HTTP rechazada por el servidor.");
    return CACHE_AUTENTICACION_FALLIDA;
  }
  if (codigo != HTTP_CODE_OK) {
    Serial.printf("No se pudo descargar la cache (HTTP %d).\n", codigo);
    return CACHE_NO_DISPONIBLE;
  }

  CacheQr cacheNueva;
  if (!parsearCache(respuesta, cacheNueva))
    return CACHE_RESPUESTA_INVALIDA;
  return guardarCacheAtomico(respuesta, cacheNueva)
      ? CACHE_ACTUALIZADA
      : CACHE_RESPUESTA_INVALIDA;
}

bool leerEstadoArchivo(const char* ruta, EstadoLocal& resultado) {
  String contenido;
  if (!leerArchivo(ruta, contenido))
    return false;

  DynamicJsonDocument documento(contenido.length() * 2 + 2048);
  if (deserializeJson(documento, contenido))
    return false;

  EstadoLocal nuevo;
  nuevo.revision = documento["revision"] | 0;
  nuevo.slot = -1;
  JsonArray pendientes = documento["pendientes"].as<JsonArray>();
  JsonArray usados = documento["codigosUsados"].as<JsonArray>();
  if (pendientes.isNull() || usados.isNull() ||
      pendientes.size() > maxRegistrosPendientes ||
      usados.size() > maxCodigosUsados)
    return false;

  for (JsonObject item : pendientes) {
    RegistroLocal registro;
    registro.registroId = String((const char*)(item["registroId"] | ""));
    registro.tipo = String((const char*)(item["tipo"] | ""));
    registro.idDetalleEvento = item["idDetalleEvento"] | 0;
    registro.idUsuario = item["idUsuario"] | 0;
    registro.idEvento = item["idEvento"] | 0;
    registro.codigoQr = String((const char*)(item["codigoQr"] | ""));
    registro.codigoQrHash = String((const char*)(item["codigoQrHash"] | ""));
    registro.motivo = String((const char*)(item["motivo"] | ""));
    registro.snapshotSqlEpoch = item["snapshotSqlEpoch"] | 0LL;
    registro.fechaAccesoSqlEpoch = item["fechaAccesoSqlEpoch"] | 0LL;
    registro.estadoUsuario = item["estadoUsuario"] | false;
    registro.pinConfigurado = item["pinConfigurado"] | true;
    registro.estadoEvento = item["estadoEvento"] | false;

    if (!esUuidValido(registro.registroId) ||
        (registro.tipo != "acceso" && registro.tipo != "rechazo") ||
        (registro.tipo == "acceso" &&
         (!esUuidValido(registro.codigoQr) ||
          registro.idDetalleEvento <= 0 ||
          registro.fechaAccesoSqlEpoch <= 0 ||
          registro.snapshotSqlEpoch <= 0))) {
      return false;
    }
    nuevo.pendientes.push_back(registro);
  }

  for (JsonVariant valor : usados) {
    String codigo = valor.as<String>();
    if (!esUuidValido(codigo))
      return false;
    codigo.toLowerCase();
    nuevo.codigosUsados.push_back(codigo);
  }

  resultado = nuevo;
  return true;
}

bool cargarEstadoLocal() {
  if (!sistemaArchivosListo)
    return false;

  const char* rutas[] = {"/estado_a.json", "/estado_b.json"};
  bool existeEstado = false;
  EstadoLocal candidatas[2];
  bool valida[2] = {false, false};

  for (int i = 0; i < 2; i++) {
    if (!LittleFS.exists(rutas[i]))
      continue;
    existeEstado = true;
    valida[i] = leerEstadoArchivo(rutas[i], candidatas[i]);
    if (valida[i])
      candidatas[i].slot = i;
  }

  if (!valida[0] && !valida[1]) {
    if (existeEstado)
      return false;
    estadoLocal.revision = 0;
    estadoLocal.slot = -1;
    estadoLocal.pendientes.clear();
    estadoLocal.codigosUsados.clear();
    return true;
  }

  int elegido = valida[0] ? 0 : 1;
  if (valida[0] && valida[1] &&
      candidatas[1].revision > candidatas[0].revision)
    elegido = 1;
  estadoLocal = candidatas[elegido];
  estadoLocal.slot = elegido;
  return true;
}

bool guardarEstadoLocalAtomico() {
  if (!sistemaArchivosListo ||
      estadoLocal.pendientes.size() > maxRegistrosPendientes ||
      estadoLocal.codigosUsados.size() > maxCodigosUsados)
    return false;

  size_t capacidad = 4096 + estadoLocal.pendientes.size() * 512 +
                     estadoLocal.codigosUsados.size() * 80;
  DynamicJsonDocument documento(capacidad);
  uint32_t nuevaRevision = estadoLocal.revision + 1;
  documento["revision"] = nuevaRevision;

  JsonArray pendientes = documento.createNestedArray("pendientes");
  for (const RegistroLocal& registro : estadoLocal.pendientes) {
    JsonObject item = pendientes.createNestedObject();
    item["registroId"] = registro.registroId;
    item["tipo"] = registro.tipo;
    item["idDetalleEvento"] = registro.idDetalleEvento;
    item["idUsuario"] = registro.idUsuario;
    item["idEvento"] = registro.idEvento;
    item["codigoQr"] = registro.codigoQr;
    item["codigoQrHash"] = registro.codigoQrHash;
    item["motivo"] = registro.motivo;
    item["snapshotSqlEpoch"] = registro.snapshotSqlEpoch;
    item["fechaAccesoSqlEpoch"] = registro.fechaAccesoSqlEpoch;
    item["estadoUsuario"] = registro.estadoUsuario;
    item["pinConfigurado"] = registro.pinConfigurado;
    item["estadoEvento"] = registro.estadoEvento;
  }

  JsonArray usados = documento.createNestedArray("codigosUsados");
  for (const String& codigo : estadoLocal.codigosUsados)
    usados.add(codigo);

  String contenido;
  serializeJson(documento, contenido);
  const char* temporal = "/estado_tmp.json";
  const char* rutas[] = {"/estado_a.json", "/estado_b.json"};
  if (LittleFS.exists(temporal))
    LittleFS.remove(temporal);

  File archivo = LittleFS.open(temporal, FILE_WRITE);
  if (!archivo)
    return false;
  size_t escrito = archivo.print(contenido);
  archivo.flush();
  archivo.close();
  if (escrito != contenido.length()) {
    LittleFS.remove(temporal);
    return false;
  }

  EstadoLocal verificacion;
  if (!leerEstadoArchivo(temporal, verificacion) ||
      verificacion.revision != nuevaRevision) {
    LittleFS.remove(temporal);
    return false;
  }

  int destino = estadoLocal.slot == 0 ? 1 : 0;
  if (LittleFS.exists(rutas[destino]))
    LittleFS.remove(rutas[destino]);
  if (!LittleFS.rename(temporal, rutas[destino]))
    return false;

  estadoLocal.revision = nuevaRevision;
  estadoLocal.slot = destino;
  estadoLocalValido = true;
  return true;
}

bool codigoMarcadoUsado(const String& codigoQr) {
  for (const String& usado : estadoLocal.codigosUsados) {
    if (usado == codigoQr)
      return true;
  }
  return false;
}

bool codigoPendiente(const String& codigoQr) {
  for (const RegistroLocal& registro : estadoLocal.pendientes) {
    if (registro.tipo == "acceso" && registro.codigoQr == codigoQr)
      return true;
  }
  return false;
}

String generarUuid() {
  uint8_t bytes[16];
  esp_fill_random(bytes, sizeof(bytes));
  bytes[6] = (bytes[6] & 0x0F) | 0x40;
  bytes[8] = (bytes[8] & 0x3F) | 0x80;

  char salida[37];
  snprintf(
      salida, sizeof(salida),
      "%02x%02x%02x%02x-%02x%02x-%02x%02x-%02x%02x-%02x%02x%02x%02x%02x%02x",
      bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6],
      bytes[7], bytes[8], bytes[9], bytes[10], bytes[11], bytes[12], bytes[13],
      bytes[14], bytes[15]);
  return String(salida);
}

String huellaQr(const String& codigoQr) {
  uint64_t hash = 1469598103934665603ULL;
  for (size_t i = 0; i < codigoQr.length(); i++) {
    hash ^= (uint8_t)codigoQr[i];
    hash *= 1099511628211ULL;
  }
  char salida[17];
  snprintf(salida, sizeof(salida), "%016llx", (unsigned long long)hash);
  return String(salida);
}

bool agregarRegistroLocal(const RegistroLocal& registro) {
  if (!estadoLocalValido ||
      estadoLocal.pendientes.size() >= maxRegistrosPendientes)
    return false;

  estadoLocal.pendientes.push_back(registro);
  if (guardarEstadoLocalAtomico())
    return true;
  estadoLocal.pendientes.pop_back();
  return false;
}

bool registrarRechazo(int idDetalleEvento, const String& codigoQr, const String& motivo) {
  if (!sistemaArchivosListo || !estadoLocalValido)
    return false;

  RegistroLocal rechazo;
  rechazo.registroId = generarUuid();
  rechazo.tipo = "rechazo";
  rechazo.idDetalleEvento = idDetalleEvento;
  rechazo.idUsuario = 0;
  rechazo.idEvento = 0;
  rechazo.codigoQr = "";
  rechazo.codigoQrHash = huellaQr(codigoQr);
  rechazo.motivo = motivo;
  rechazo.snapshotSqlEpoch = cacheDisponible ? cacheActual.snapshotSqlEpoch : 0;
  rechazo.fechaAccesoSqlEpoch = 0;
  rechazo.estadoUsuario = false;
  rechazo.pinConfigurado = false;
  rechazo.estadoEvento = false;

  int64_t ahoraSql = 0;
  if (obtenerHoraSqlActual(ahoraSql))
    rechazo.fechaAccesoSqlEpoch = ahoraSql;

  return agregarRegistroLocal(rechazo);
}

String crearAutorizacionBasica() {
  String credenciales = String(httpUsername) + ":" + String(httpPassword);
  unsigned char salida[256];
  size_t longitudSalida = 0;
  int resultado = mbedtls_base64_encode(
      salida, sizeof(salida) - 1, &longitudSalida,
      (const unsigned char*)credenciales.c_str(), credenciales.length());
  if (resultado != 0 || longitudSalida >= sizeof(salida))
    return "";
  salida[longitudSalida] = '\0';
  return String("Basic ") + (const char*)salida;
}

int peticionHttp(const String& metodo, const String& ruta, const String& cuerpo,
                 String& respuesta, uint16_t timeoutMs) {
  if (WiFi.status() != WL_CONNECTED)
    return -1;

  WiFiClient cliente;
  HTTPClient http;
  http.setTimeout(timeoutMs);
  http.setConnectTimeout(timeoutMs);

  String url = String(serverBaseUrl) + ruta;
  if (!http.begin(cliente, url))
    return -1;
  String autorizacion = crearAutorizacionBasica();
  if (autorizacion.length() == 0) {
    http.end();
    return -1;
  }
  http.addHeader("Authorization", autorizacion);

  int codigo = -1;
  if (metodo == "GET") {
    codigo = http.GET();
  } else if (metodo == "POST") {
    http.addHeader("Content-Type", "application/json");
    codigo = http.POST(cuerpo);
  }

  if (codigo > 0)
    respuesta = http.getString();
  http.end();
  return codigo;
}

ResultadoSync enviarPendientes() {
  ResultadoSync resultado;
  resultado.respuestaValida = false;
  if (!estadoLocalValido || estadoLocal.pendientes.empty() ||
      WiFi.status() != WL_CONNECTED || internetSimuladoDesconectado)
    return resultado;

  DynamicJsonDocument documento(
      4096 + estadoLocal.pendientes.size() * 512);
  JsonArray registros = documento.createNestedArray("registros");
  for (const RegistroLocal& registro : estadoLocal.pendientes) {
    JsonObject item = registros.createNestedObject();
    item["registroId"] = registro.registroId;
    item["tipo"] = registro.tipo;
    item["idDetalleEvento"] = registro.idDetalleEvento;
    item["idUsuario"] = registro.idUsuario;
    item["idEvento"] = registro.idEvento;
    item["codigoQr"] = registro.codigoQr;
    item["codigoQrHash"] = registro.codigoQrHash;
    item["motivo"] = registro.motivo;
    item["snapshotSqlEpoch"] = registro.snapshotSqlEpoch;
    item["fechaAccesoSqlEpoch"] = registro.fechaAccesoSqlEpoch;
    item["estadoUsuario"] = registro.estadoUsuario;
    item["pinConfigurado"] = registro.pinConfigurado;
    item["estadoEvento"] = registro.estadoEvento;
  }

  String cuerpo;
  serializeJson(documento, cuerpo);
  String respuesta;
  int codigo = peticionHttp("POST", "/sincronizar", cuerpo, respuesta, 2500);
  if (codigo != HTTP_CODE_OK) {
    if (codigo != -1)
      Serial.printf("Sincronizacion pendiente; HTTP %d.\n", codigo);
    return resultado;
  }

  DynamicJsonDocument respuestaJson(respuesta.length() * 2 + 1024);
  if (deserializeJson(respuestaJson, respuesta))
    return resultado;

  JsonArray confirmados = respuestaJson["confirmados"].as<JsonArray>();
  JsonArray conflictos = respuestaJson["conflictos"].as<JsonArray>();
  if (confirmados.isNull() || conflictos.isNull())
    return resultado;

  for (JsonVariant item : confirmados)
    resultado.confirmados.push_back(item.as<String>());
  for (JsonObject item : conflictos) {
    const char* registroId = item["registroId"] | "";
    if (registroId[0] != '\0')
      resultado.conflictos.push_back(String(registroId));
  }
  resultado.respuestaValida = true;

  if (!resultado.confirmados.empty()) {
    EstadoLocal estadoAnterior = estadoLocal;
    std::vector<RegistroLocal> restantes;
    for (const RegistroLocal& registro : estadoLocal.pendientes) {
      bool confirmado = false;
      for (const String& id : resultado.confirmados) {
        if (id == registro.registroId) {
          confirmado = true;
          break;
        }
      }
      if (!confirmado)
        restantes.push_back(registro);
    }
    estadoLocal.pendientes = restantes;
    if (!guardarEstadoLocalAtomico()) {
      estadoLocal = estadoAnterior;
      Serial.println("SQL confirmo registros, pero no se pudo limpiar la cola; se reintentara.");
    } else {
      Serial.printf("Registros confirmados y quitados de la cola: %u\n",
                    (unsigned)resultado.confirmados.size());
    }
  }

  if (!resultado.conflictos.empty())
    Serial.printf("Registros en conflicto conservados en LittleFS: %u\n",
                  (unsigned)resultado.conflictos.size());
  return resultado;
}

bool sincronizarPendientes() {
  if (estadoLocal.pendientes.empty())
    return true;
  ResultadoSync resultado = enviarPendientes();
  return resultado.respuestaValida && resultado.conflictos.empty() &&
         estadoLocal.pendientes.empty();
}

void sincronizarReloj();

bool actualizarCacheYSync() {
  if (WiFi.status() != WL_CONNECTED || internetSimuladoDesconectado)
    return false;

  String estado;
  int codigoEstado = peticionHttp("GET", "/estado", "", estado, 2500);
  if (codigoEstado == HTTP_CODE_OK) {
    DynamicJsonDocument documento(estado.length() * 2 + 1024);
    if (!deserializeJson(documento, estado)) {
      int64_t utc = documento["serverUtcEpoch"] | 0LL;
      JsonVariant sql = documento["sqlNowEpoch"];
      if (utc >= epochMinimoConfiable) {
        timeval tiempo;
        tiempo.tv_sec = (time_t)utc;
        tiempo.tv_usec = 0;
        if (!relojConfiable())
          settimeofday(&tiempo, nullptr);
      }
      if (!sql.isNull() && utc >= epochMinimoConfiable) {
        sqlUtcOffsetSegundos = sql.as<int64_t>() - utc;
        sqlUtcOffsetDisponible = true;
      }
      ultimaConsultaEstado = millis();
    }
  }

  ResultadoCache resultado = actualizarCacheQr();
  bool cacheActualizada = resultado == CACHE_ACTUALIZADA;
  if (cacheActualizada)
    ultimaActualizacionCache = millis();
  sincronizarPendientes();
  return cacheActualizada;
}

void servicioRed() {
  if (!wifiListo || internetSimuladoDesconectado ||
      millis() - ultimoIntentoRed < intervaloReintentoRedMs ||
      abrirProceso)
    return;

  ultimoIntentoRed = millis();
  if (!relojConfiable() ||
      millis() - ultimaConsultaEstado >= 60000UL)
    sincronizarReloj();

  if (!estadoLocal.pendientes.empty())
    sincronizarPendientes();

  int64_t horaSql = 0;
  bool cacheCaducada = !obtenerHoraSqlActual(horaSql) ||
      !cacheDisponible ||
      horaSql < cacheActual.snapshotSqlEpoch ||
      horaSql - cacheActual.snapshotSqlEpoch >= cacheActual.vigenciaSegundos;

  if (cacheCaducada ||
      millis() - ultimaActualizacionCache >= intervaloActualizacionCacheMs) {
    if (actualizarCacheQr() == CACHE_ACTUALIZADA)
      ultimaActualizacionCache = millis();
  }
}

void leerComandoSerial() {
  while (Serial.available()) {
    char caracter = (char)Serial.read();
    if (caracter == '\n' || caracter == '\r') {
      comandoSerial.trim();
      comandoSerial.toUpperCase();
      if (comandoSerial == "OFFLINE") {
        internetSimuladoDesconectado = true;
        Serial.println("SIMULACION OFFLINE ACTIVADA.");
      } else if (comandoSerial == "ONLINE") {
        internetSimuladoDesconectado = false;
        Serial.println("SIMULACION ONLINE ACTIVADA.");
        actualizarCacheYSync();
      }
      comandoSerial = "";
    } else if (caracter >= 32 && caracter <= 126 && comandoSerial.length() < 16) {
      comandoSerial += caracter;
    }
  }
}

void abrirAcceso(const String& id, const String& registroId, bool offline) {
  abrirProceso = true;
  tiempoInicio = 0;
  lcdMessage("ID: " + id);
  Serial.println(offline ? "ACCESO PERMITIDO OFFLINE" : "ACCESO PERMITIDO");
  Serial.println("Registro local: " + registroId);
}

void controlarServo(long distancia) {
  int posDestino;
  if (distancia < distanciaSegura) {
    posDestino = max(85, posActual - (distanciaSegura - (int)distancia));
  } else {
    posDestino = abrirProceso ? posMax : 85;
  }

  if (posActual < posDestino) {
    posActual = min(posActual + velocidad, posDestino);
    servo.write(posActual);
  } else if (posActual > posDestino) {
    posActual = max(posActual - velocidad, posDestino);
    servo.write(posActual);
  }

  if (abrirProceso && posActual == posMax && tiempoInicio == 0)
    tiempoInicio = millis();
}

void actualizarLCD() {
  static unsigned long ultimaActualizacion = 0;
  if (millis() - ultimaActualizacion < 1000)
    return;

  if (posActual == posMax)
    lcdMessage("PUERTA ABIERTA");
  else if (posActual == 85)
    lcdMessage("PUERTA CERRADA");
  else
    lcdMessage("MOVIENDOSE...");
  ultimaActualizacion = millis();
}

void lcdMessage(String mensaje) {
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print(mensaje);
}

long medirDistancia() {
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);

  long duracion = pulseIn(echoPin, HIGH, 30000);
  long distancia = duracion * 0.034 / 2;
  return (distancia > 0 && distancia <= 300) ? distancia : 100;
}

void cerrarPuerta() {
  for (int i = posMax; i >= 85; i--) {
    servo.write(i);
    delay(50);
  }
  posActual = 85;
}
