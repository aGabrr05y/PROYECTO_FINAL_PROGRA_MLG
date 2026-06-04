// ============================================
// SISTEMA DE GASOLINERA - 2 BOMBAS
// ADAPTADO PARA JSON (COMUNICACIÓN CON C#)
// Versión: parser JSON más robusto
// ============================================

// ============================================
// DEFINICIÓN DE PINES - BOMBA 1
// ============================================
const int BOTON1_B1 = 11;
const int BOTON2_B1 = 12;
const int BOTON3_B1 = 13;
const int SENSOR_B1 = A0;
const int MOTOR_B1 = 8;

// ============================================
// DEFINICIÓN DE PINES - BOMBA 2
// ============================================
const int BOTON1_B2 = 2;
const int BOTON2_B2 = 3;
const int BOTON3_B2 = 4;
const int SENSOR_B2 = A1;
const int MOTOR_B2 = 7;

// ============================================
// ESTRUCTURA DE BOMBA
// ============================================
struct Bomba {
  int boton1, boton2, boton3;
  int sensor, motor;

  bool encendida;
  double litrosTotales;
  double limiteLitros;
  int pulsos;
  int ultimoEstadoSensor;

  unsigned long ultimoEnvio;
  unsigned long ultimoPulsoDetectado;
  unsigned long ultimoTiempoPulso;
};

Bomba bomba1;
Bomba bomba2;

// ============================================
// CONFIGURACIÓN
// ============================================
const int UMBRAL_SENSOR = 400;
const double FACTOR_CONVERSION = 0.0102; // litros por pulso en escala real
// Escala: representar 1 galón real como 50 mL en el sistema reducido
// 1 galón (US) = 3.78541 litros -> escala a litros: 0.05 L / 3.78541 L
const double SCALE_LITROS = 0.05 / 3.78541; // ≈ 0.013208
// Factor de conversión ajustado a la escala usada por el experimento
const double FACTOR_CONVERSION_SCALED = FACTOR_CONVERSION * SCALE_LITROS;
const unsigned long DEBOUNCE_DELAY = 50;

// ============================================
// VARIABLES PARA JSON
// ============================================
String inputJSON = "";
bool ordenRecibida = false;
int bombaSeleccionada = 0;
double limiteLitrosExterno = 0;

// ============================================
// HELPERS DE PARSEO SIMPLES (no dependen de librería externa)
// ============================================
int findKeyColonIndex(String json, const char* key) {
  String pattern = String("\"") + String(key) + String("\"");
  int k = json.indexOf(pattern);
  if (k == -1) return -1;
  int colon = json.indexOf(':', k + pattern.length());
  return colon;
}

void skipSpaces(const String &s, int &i) {
  while (i < s.length()) {
	char c = s.charAt(i);
	if (c == ' ' || c == '\t' || c == '\r' || c == '\n') i++;
	else break;
  }
}

int parseIntAfter(const String &json, int colon) {
  if (colon < 0) return 0;
  int i = colon + 1;
  skipSpaces(json, i);
  String num = "";
  if (i < json.length() && (json.charAt(i) == '+' || json.charAt(i) == '-')) {
	num += json.charAt(i++);
  }
  while (i < json.length()) {
	char c = json.charAt(i);
	if (c >= '0' && c <= '9') {
	  num += c;
	  i++;
	} else break;
  }
  return num.length() ? num.toInt() : 0;
}

double parseFloatAfter(const String &json, int colon) {
  if (colon < 0) return 0.0;
  int i = colon + 1;
  skipSpaces(json, i);
  String num = "";
  if (i < json.length() && (json.charAt(i) == '+' || json.charAt(i) == '-')) {
	num += json.charAt(i++);
  }
  bool seenDot = false;
  while (i < json.length()) {
	char c = json.charAt(i);
	if (c >= '0' && c <= '9') {
	  num += c;
	  i++;
	} else if (c == '.' && !seenDot) {
	  num += c;
	  seenDot = true;
	  i++;
	} else break;
  }
  return num.length() ? num.toFloat() : 0.0;
}

bool parseBoolAfter(const String &json, int colon) {
  if (colon < 0) return false;
  int i = colon + 1;
  skipSpaces(json, i);
  if (i + 3 < json.length() && json.substring(i, i + 4) == "true") return true;
  return false;
}

// ============================================
// SETUP
// ============================================
void setup() {
  Serial.begin(9600);

  // Inicializar bomba 1
  bomba1 = {BOTON1_B1, BOTON2_B1, BOTON3_B1, SENSOR_B1, MOTOR_B1,
			false, 0.0, 0.0, 0, HIGH, 0, 0, 0};

  pinMode(bomba1.boton1, INPUT);
  pinMode(bomba1.boton2, INPUT);
  pinMode(bomba1.boton3, INPUT);
  pinMode(bomba1.motor, OUTPUT);
  pinMode(bomba1.sensor, INPUT);

  // Inicializar bomba 2
  bomba2 = {BOTON1_B2, BOTON2_B2, BOTON3_B2, SENSOR_B2, MOTOR_B2,
			false, 0.0, 0.0, 0, HIGH, 0, 0, 0};

  pinMode(bomba2.boton1, INPUT);
  pinMode(bomba2.boton2, INPUT);
  pinMode(bomba2.boton3, INPUT);
  pinMode(bomba2.motor, OUTPUT);
  pinMode(bomba2.sensor, INPUT);
}

// ============================================
// LOOP PRINCIPAL
// ============================================
void loop() {

  // 1. Recibir JSON desde C# (espera hasta '\n')
  if (Serial.available()) {
	inputJSON = Serial.readStringUntil('\n');
	inputJSON.trim(); // elimina espacios y CR
	if (inputJSON.length() > 0) {
	  ordenRecibida = true;
	}
  }

  // 2. Procesar JSON recibido
  if (ordenRecibida) {
	procesarOrdenJSON(inputJSON);
	ordenRecibida = false;
	inputJSON = "";
  }

  // 3. Procesar bombas
  procesarBomba(bomba1);
  procesarBomba(bomba2);

  delay(10);
}

// ============================================
// PROCESAR ORDEN JSON DESDE C# (VERSION ROBUSTA)
// ============================================
void procesarOrdenJSON(String json) {
  // Buscar "bomba": <numero>
  int colonBomba = findKeyColonIndex(json, "bomba");
  if (colonBomba != -1) {
	bombaSeleccionada = parseIntAfter(json, colonBomba);
  }

  // Buscar "monto": <valor>
  int colonMonto = findKeyColonIndex(json, "monto");
	if (colonMonto != -1) {
	double monto = parseFloatAfter(json, colonMonto);
	// convertir monto a litros (factor según tu sistema) y aplicar escala
	if (monto > 0) {
	  double litrosReales = monto / 37.35; // litros reales
	  limiteLitrosExterno = litrosReales * SCALE_LITROS; // litros en escala reducida
	}
  }

  // Buscar "tanqueLleno": true
  int colonTanque = findKeyColonIndex(json, "tanqueLleno");
  if (colonTanque != -1) {
	bool tanq = parseBoolAfter(json, colonTanque);
	if (tanq) {
	  limiteLitrosExterno = 9999; // marca tanque lleno -> bomba se detendrá por otro criterio
	}
  }

  // Encender bomba correspondiente y asignar límite
  if (bombaSeleccionada == 1) {
	bomba1.limiteLitros = limiteLitrosExterno;
	encenderBomba(bomba1);
	  enviarAck(1, bomba1.limiteLitros);
  }
  else if (bombaSeleccionada == 2) {
	bomba2.limiteLitros = limiteLitrosExterno;
	encenderBomba(bomba2);
	  enviarAck(2, bomba2.limiteLitros);
  }
}

// ============================================
// ENVIAR ACK AL SISTEMA (CONFIRMACIÓN DE RECEPCIÓN)
// ============================================
void enviarAck(int bombaId, double limite) {
  if (bombaId <= 0) {
	Serial.println("{\"ok\":false}");
	return;
  }
  Serial.print("{\"ok\":true,\"bomba\":");
  Serial.print(bombaId);
  Serial.print(",\"limiteLitros\":");
  Serial.print(limite, 3);
  Serial.println("}");
}

// ============================================
// PROCESAR BOMBA
// ============================================
void procesarBomba(Bomba &b) {

  int lecturaSensor = analogRead(b.sensor);
  int estadoSensor = (lecturaSensor < UMBRAL_SENSOR) ? LOW : HIGH;

  if (b.ultimoEstadoSensor == HIGH && estadoSensor == LOW) {
	if (millis() - b.ultimoTiempoPulso > DEBOUNCE_DELAY) {
	  b.pulsos++;
	  // Usar factor escalado para representar 1 galón = 50 mL
	  b.litrosTotales = b.pulsos * FACTOR_CONVERSION_SCALED;
	  b.ultimoPulsoDetectado = millis();
	}
	b.ultimoTiempoPulso = millis();
  }

  b.ultimoEstadoSensor = estadoSensor;

  // Si está encendida, enviar JSON cada segundo
  if (b.encendida) {
	if (millis() - b.ultimoEnvio > 1000) {
	  enviarJSON(b, false);
	  b.ultimoEnvio = millis();
	}

	// Si llegó al límite
	if (b.litrosTotales >= b.limiteLitros) {
	  apagarBomba(b);
	  enviarJSON(b, true);
	  resetearContador(b);
	}
  }
}

// ============================================
// ENVIAR JSON AL SISTEMA
// ============================================
void enviarJSON(Bomba &b, bool finalizado) {
  Serial.print("{\"bomba\":");
  Serial.print(getBombaId(b));
  Serial.print(",\"litrosServidos\":");
  Serial.print(b.litrosTotales, 3);

  if (finalizado) {
	Serial.println(",\"finalizado\":true}");
  } else {
	Serial.println("}");
  }
}

// ============================================
// CONTROL DE BOMBA
// ============================================
void encenderBomba(Bomba &b) {
  b.encendida = true;
  analogWrite(b.motor, 200);
}

void apagarBomba(Bomba &b) {
  b.encendida = false;
  analogWrite(b.motor, 0);
}

void resetearContador(Bomba &b) {
  b.pulsos = 0;
  b.litrosTotales = 0.0;
}

// ============================================
// IDENTIFICAR BOMBA
// ============================================
int getBombaId(Bomba &b) {
  if (b.motor == MOTOR_B1) return 1;
  if (b.motor == MOTOR_B2) return 2;
  return 0;
}
