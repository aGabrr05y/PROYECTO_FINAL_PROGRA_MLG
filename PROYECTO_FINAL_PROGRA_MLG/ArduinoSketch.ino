// ArduinoSketch.ino
// Sketch corregido: maneja parar, prepago, tanque lleno y calibración

// PINES - BOMBA 1
const int BOTON1_B1 = 11;
const int BOTON2_B1 = 12;
const int BOTON3_B1 = 13;
const int SENSOR_B1 = A0;
const int MOTOR_B1 = 8;

// PINES - BOMBA 2
const int BOTON1_B2 = 2;
const int BOTON2_B2 = 3;
const int BOTON3_B2 = 4;
const int SENSOR_B2 = A1;
const int MOTOR_B2 = 7;

// Escala: 1 "litro app" = 50 ml reales = 0.05 litros reales
const double ESCALA_LITROS = 0.05;

struct Bomba {
  int boton1, boton2, boton3;
  int sensor, motor;
  bool encendida;
  double litrosTotales;
  double limiteLitros;   // en litros REALES (ya escalado)
  bool tanqueLleno;
  int pulsos;
  int ultimoEstadoSensor;
  unsigned long ultimoEnvio;
  unsigned long ultimoTiempoPulso;
  unsigned long ultimoTiempoBoton;
};

Bomba bomba1;
Bomba bomba2;

const int UMBRAL_SENSOR = 400;
const unsigned long DEBOUNCE_DELAY = 50;
const unsigned long ENVIO_INTERVAL = 500;

double PULSOS_POR_LITRO = 100.0;

String inputJSON = "";

// ---- Parsers ----
int findKeyColonIndex(String json, const char* key) {
  String pattern = String("\"") + String(key) + String("\"");
  int k = json.indexOf(pattern);
  if (k == -1) return -1;
  return json.indexOf(':', k + pattern.length());
}

void skipSpaces(const String &s, int &i) {
  while (i < (int)s.length() && (s.charAt(i)==' '||s.charAt(i)=='\t'||s.charAt(i)=='\r'||s.charAt(i)=='\n')) i++;
}

int parseIntAfter(const String &json, int colon) {
  if (colon < 0) return 0;
  int i = colon + 1; skipSpaces(json, i);
  String num = "";
  if (i < (int)json.length() && (json.charAt(i)=='+'||json.charAt(i)=='-')) num += json.charAt(i++);
  while (i < (int)json.length()) { char c=json.charAt(i); if(c>='0'&&c<='9'){num+=c;i++;}else break; }
  return num.length() ? num.toInt() : 0;
}

double parseFloatAfter(const String &json, int colon) {
  if (colon < 0) return 0.0;
  int i = colon + 1; skipSpaces(json, i);
  String num = "";
  if (i < (int)json.length() && (json.charAt(i)=='+'||json.charAt(i)=='-')) num += json.charAt(i++);
  bool dot = false;
  while (i < (int)json.length()) {
    char c = json.charAt(i);
    if (c>='0'&&c<='9'){num+=c;i++;}
    else if(c=='.'&&!dot){num+=c;dot=true;i++;}
    else break;
  }
  return num.length() ? num.toFloat() : 0.0;
}

bool parseBoolAfter(const String &json, int colon) {
  if (colon < 0) return false;
  int i = colon + 1; skipSpaces(json, i);
  return (i+3 < (int)json.length() && json.substring(i,i+4)=="true");
}

String parseStringAfter(const String &json, int colon) {
  if (colon < 0) return "";
  int i = colon + 1; skipSpaces(json, i);
  if (i >= (int)json.length() || json.charAt(i) != '"') return "";
  i++;
  String result = "";
  while (i < (int)json.length() && json.charAt(i) != '"') { result += json.charAt(i++); }
  return result;
}

// ---- Setup ----
void setup() {
  Serial.begin(9600);

  bomba1 = {BOTON1_B1,BOTON2_B1,BOTON3_B1,SENSOR_B1,MOTOR_B1,
            false,0.0,0.0,false,0,HIGH,0,0,0};
  pinMode(bomba1.boton1,INPUT_PULLUP); pinMode(bomba1.boton2,INPUT_PULLUP);
  pinMode(bomba1.boton3,INPUT_PULLUP); pinMode(bomba1.motor,OUTPUT);
  digitalWrite(bomba1.motor, LOW); // asegurar motor apagado al iniciar
  pinMode(bomba1.sensor,INPUT);

  bomba2 = {BOTON1_B2,BOTON2_B2,BOTON3_B2,SENSOR_B2,MOTOR_B2,
            false,0.0,0.0,false,0,HIGH,0,0,0};
  pinMode(bomba2.boton1,INPUT_PULLUP); pinMode(bomba2.boton2,INPUT_PULLUP);
  pinMode(bomba2.boton3,INPUT_PULLUP); pinMode(bomba2.motor,OUTPUT);
  digitalWrite(bomba2.motor, LOW); // asegurar motor apagado al iniciar
  pinMode(bomba2.sensor,INPUT);
}

// ---- Loop ----
void loop() {
  if (Serial.available()) {
    inputJSON = Serial.readStringUntil('\n');
    inputJSON.trim();
    if (inputJSON.length() > 0) procesarOrdenJSON(inputJSON);
  }
  procesarBomba(bomba1, 1);
  procesarBomba(bomba2, 2);
  delay(10);
}

// ---- Procesar JSON recibido desde C# ----
void procesarOrdenJSON(String json) {

  // 1) Calibración
  int colonCmd = findKeyColonIndex(json, "cmd");
  if (colonCmd != -1) {
    String cmd = parseStringAfter(json, colonCmd);
    if (cmd == "calibrar") {
      int colonP = findKeyColonIndex(json, "pulsosPorLitro");
      if (colonP != -1) {
        double nuevo = parseFloatAfter(json, colonP);
        if (nuevo > 0) {
          PULSOS_POR_LITRO = nuevo;
          Serial.print("{\"ok\":true,\"cmd\":\"calibrar\",\"pulsosPorLitro\":");
          Serial.print(PULSOS_POR_LITRO, 4);
          Serial.println("}");
        }
      }
      return; // comando de calibración procesado, salir
    }
  }

  int colonBomba = findKeyColonIndex(json, "bomba");
  if (colonBomba == -1) return;
  int bombaId = parseIntAfter(json, colonBomba);

  // 2) Comando PARAR
  int colonComando = findKeyColonIndex(json, "comando");
  if (colonComando != -1) {
    String cmd = parseStringAfter(json, colonComando);
    if (cmd == "parar") {
      if (bombaId == 1) { apagarBomba(bomba1); enviarJSON(bomba1, 1, true); resetearContador(bomba1); }
      else if (bombaId == 2) { apagarBomba(bomba2); enviarJSON(bomba2, 2, true); resetearContador(bomba2); }
      return;
    }
  }

  // 3) Iniciar abastecimiento
  // Leer limiteLitros (ya en litros REALES, escalado por C#)
  double limite = 0.0;
  int colonLimite = findKeyColonIndex(json, "limiteLitros");
  if (colonLimite != -1) {
    limite = parseFloatAfter(json, colonLimite);
  }

  bool esTanqueLleno = false;
  int colonTL = findKeyColonIndex(json, "tanqueLleno");
  if (colonTL != -1) esTanqueLleno = parseBoolAfter(json, colonTL);

  if (bombaId == 1) {
    resetearContador(bomba1);
    bomba1.limiteLitros = limite;
    bomba1.tanqueLleno = esTanqueLleno;
    encenderBomba(bomba1);
    enviarAck(1, bomba1.limiteLitros);
  } else if (bombaId == 2) {
    resetearContador(bomba2);
    bomba2.limiteLitros = limite;
    bomba2.tanqueLleno = esTanqueLleno;
    encenderBomba(bomba2);
    enviarAck(2, bomba2.limiteLitros);
  }
}

// ---- Procesar bomba en cada ciclo ----
void procesarBomba(Bomba &b, int id) {
  // Leer sensor de flujo
  int lectura = analogRead(b.sensor);
  int estadoSensor = (lectura < UMBRAL_SENSOR) ? LOW : HIGH;

  if (b.ultimoEstadoSensor == HIGH && estadoSensor == LOW) {
    if (millis() - b.ultimoTiempoPulso > DEBOUNCE_DELAY) {
      b.pulsos++;
      b.litrosTotales = (double)b.pulsos / PULSOS_POR_LITRO;
    }
    b.ultimoTiempoPulso = millis();
  }
  b.ultimoEstadoSensor = estadoSensor;

  // Botón físico DETENER
  if (digitalRead(b.boton3) == LOW && (millis() - b.ultimoTiempoBoton > DEBOUNCE_DELAY)) {
    if (b.encendida) {
      apagarBomba(b);
      enviarJSON(b, id, true);
      resetearContador(b);
    }
    b.ultimoTiempoBoton = millis();
  }

  if (b.encendida) {
    // Envío periódico
    if (millis() - b.ultimoEnvio > ENVIO_INTERVAL) {
      enviarJSON(b, id, false);
      b.ultimoEnvio = millis();
    }

    // *** DETENCIÓN AUTOMÁTICA PREPAGO ***
    // Solo si NO es tanque lleno y el límite es > 0
    if (!b.tanqueLleno && b.limiteLitros > 0.0 && b.litrosTotales >= b.limiteLitros * 0.97) {

      apagarBomba(b);
      enviarJSON(b, id, true);
      resetearContador(b);
    }
  }
}

// ---- Helpers ----
void enviarAck(int id, double limite) {
  Serial.print("{\"ok\":true,\"bomba\":"); Serial.print(id);
  Serial.print(",\"limiteLitros\":"); Serial.print(limite, 6);
  Serial.println("}");
}

void enviarJSON(Bomba &b, int id, bool finalizado) {
  Serial.print("{\"bomba\":"); Serial.print(id);
  Serial.print(",\"litrosServidos\":"); Serial.print(b.litrosTotales, 6);
  if (finalizado) Serial.println(",\"finalizado\":true}");
  else Serial.println("}");
}

void encenderBomba(Bomba &b) { b.encendida = true; digitalWrite(b.motor, HIGH); b.ultimoEnvio = millis(); }
void apagarBomba(Bomba &b)   { b.encendida = false; digitalWrite(b.motor, LOW); }
void resetearContador(Bomba &b) { b.pulsos = 0; b.litrosTotales = 0.0; }