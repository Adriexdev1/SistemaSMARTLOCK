
#include <WiFi.h>
#include <HTTPClient.h>
#include <WiFiClientSecure.h>
#include <ESP32Servo.h>
#include <LiquidCrystal_I2C.h>

LiquidCrystal_I2C lcd(0x27, 16, 2);
Servo servo;

const int trigPin = 25;
const int echoPin = 26;
const int boton = 27;
const int qrRx = 16;
const int qrTx = 17;
const int servoPin = 18;

HardwareSerial qrSerial(2);

const char* ssid = "ARPANET_2317_2.4G";
const char* password = "Yosh7415-*";
const char* serverURL = "smartlockone.somee.com";

bool abrirProceso = false;
unsigned long tiempoApertura = 20000;
unsigned long tiempoInicio = 0;
int posActual = 85;
const int posMax = 160;
const int distanciaSegura = 9;
const int velocidad = 5;

bool wifiListo = false;
String qrBuffer = "";
unsigned long lastQrTime = 0;

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
  delay(2000);

  qrSerial.begin(9600, SERIAL_8N1, qrRx, qrTx);

  lcdMessage("CONECTANDO WIFI");
  configurarWiFi();

  lcdMessage(wifiListo ? "SISTEMA LISTO" : "SIN WIFI");
  delay(2000);
}

void configurarWiFi() {
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  Serial.println("CONECTANDO A WIFI...");
  unsigned long inicio = millis();

  while (WiFi.status() != WL_CONNECTED && millis() - inicio < 15000) {
    delay(500);
    Serial.print(".");
  }

  if (WiFi.status() == WL_CONNECTED) {
    wifiListo = true;
    Serial.println("\nWIFI CONECTADO");
    Serial.print("IP: ");
    Serial.println(WiFi.localIP());
  } else {
    wifiListo = false;
    Serial.println("\nERROR DE CONEXION WIFI");
  }
}

void loop() {
  wifiListo = (WiFi.status() == WL_CONNECTED);
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

  delay(100);
}

void leerQR() {
  while (qrSerial.available()) {
    char c = qrSerial.read();

    if (c == '\n' || c == '\r') {
      if (qrBuffer.length() > 0) {
        Serial.print("QR: ");
        Serial.println(qrBuffer);

        String id = extraerParametro(qrBuffer, "id=");
        String token = extraerParametro(qrBuffer, "token=");

        Serial.println("ID: " + id);
        Serial.println("Token: " + token);

        procesarQR(id, token);
        qrBuffer = "";
        return;
      }
    } else if (c >= 32 && c <= 126) {
      qrBuffer += c;
      lastQrTime = millis();
    }

    if (qrBuffer.length() > 0 && millis() - lastQrTime > 1000) {
      qrBuffer = "";
    }
  }
}

String extraerParametro(String url, String parametro) {
  int inicio = url.indexOf(parametro);
  if (inicio == -1) return "";

  inicio += parametro.length();
  int fin = url.indexOf("&", inicio);
  if (fin == -1) fin = url.length();

  return url.substring(inicio, fin);
}

void procesarQR(String id, String token) {
  if (abrirProceso) return;

  if (id.length() == 0 || token.length() == 0) {
    lcdMessage("QR INVALIDO");
    Serial.println("PARAMETROS QR INVALIDOS");
    delay(2000);
    return;
  }

  lcdMessage("QR DETECTADO");
  Serial.println("=== PROCESANDO QR ===");

  if (wifiListo) {
    lcdMessage("VALIDANDO...");

    String endpoint = "/puerta.aspx?id=" + id + "&token=" + token;

    if (hacerPeticion(endpoint)) {
      abrirProceso = true;
      tiempoInicio = 0;
      lcdMessage("ID: " + id);
      Serial.println("ACCESO PERMITIDO");
    } else {
      lcdMessage("ACCESO DENEGADO");
      Serial.println("ACCESO DENEGADO");
    }
  } else {
    lcdMessage("SIN CONEXION");
    Serial.println("ACCESO DENEGADO: SIN WIFI");
  }

  delay(2000);
}

bool hacerPeticion(String endpoint) {
  Serial.println("--- PETICION HTTPS ---");

  if (WiFi.status() != WL_CONNECTED) {
    wifiListo = false;
    return false;
  }

  WiFiClientSecure client;
  client.setInsecure();

  HTTPClient http;
  String url = "https://" + String(serverURL) + endpoint;

  Serial.println(url);

  if (!http.begin(client, url)) {
    Serial.println("ERROR AL INICIAR HTTP");
    return false;
  }

  http.setTimeout(8000);
  http.setFollowRedirects(HTTPC_STRICT_FOLLOW_REDIRECTS);

  int codigoHTTP = http.GET();
  bool autorizado = false;

  if (codigoHTTP > 0) {
    Serial.print("CODIGO HTTP: ");
    Serial.println(codigoHTTP);

    String respuesta = http.getString();
    Serial.println("RESPUESTA DEL SERVIDOR:");
    Serial.println(respuesta);

    respuesta.trim();
    String respuestaMin = respuesta;
    respuestaMin.toLowerCase();

    if (codigoHTTP == HTTP_CODE_OK &&
        (respuestaMin == "true" ||
         respuestaMin == "success" ||
         respuestaMin.indexOf("\"success\":true") >= 0 ||
         respuestaMin.indexOf("\"acceso\":true") >= 0)) {
      autorizado = true;
    }
  } else {
    Serial.print("ERROR HTTP: ");
    Serial.println(http.errorToString(codigoHTTP));
  }

  http.end();

  Serial.print("RESULTADO: ");
  Serial.println(autorizado ? "EXITOSO" : "FALLIDO");

  return autorizado;
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

  if (abrirProceso && posActual == posMax && tiempoInicio == 0) {
    tiempoInicio = millis();
  }
}

void actualizarLCD() {
  static unsigned long lastUpdate = 0;
  if (millis() - lastUpdate < 1000) return;

  if (posActual == posMax) {
    lcdMessage("PUERTA ABIERTA");
  } else if (posActual == 85) {
    lcdMessage("PUERTA CERRADA");
  } else {
    lcdMessage("MOVIENDOSE...");
  }

  lastUpdate = millis();
}

void lcdMessage(String msg) {
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print(msg);
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
