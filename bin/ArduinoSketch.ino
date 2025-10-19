const int fanPins[] = {2, 4};
const char* fanNames[] = {"CPU Fan", "System Fan"};
const int FAN_COUNT = sizeof(fanPins) / sizeof(fanPins[0]);

int fanPWM[FAN_COUNT];
unsigned long lastUpdate[FAN_COUNT];
const unsigned long timeout = 5000; // 5s -> default 30%

void setup() {
  Serial.begin(115200);
  for (int i = 0; i < FAN_COUNT; i++) {
    pinMode(fanPins[i], OUTPUT);
    fanPWM[i] = 77; // 30% PWM
    analogWrite(fanPins[i], fanPWM[i]);
    lastUpdate[i] = millis();
  }
}

void loop() {
  if (Serial.available() > 0) {
    String line = Serial.readStringUntil('\n');
    line.trim();

    if (line.startsWith("SET_FAN:")) {
      int firstColon = line.indexOf(':');
      int secondColon = line.indexOf(':', firstColon + 1);
      if (firstColon > 0 && secondColon > firstColon) {
        int fanNum = line.substring(firstColon + 1, secondColon).toInt();
        int pwmVal = line.substring(secondColon + 1).toInt();
        if (fanNum >= 1 && fanNum <= FAN_COUNT && pwmVal >= 0 && pwmVal <= 255) {
          fanPWM[fanNum - 1] = pwmVal;
          analogWrite(fanPins[fanNum - 1], fanPWM[fanNum - 1]);
          lastUpdate[fanNum - 1] = millis();
        }
      }
    } 
    else if (line == "PING") {
      Serial.println("PONG");
    } 
    else if (line == "NUM_FANS") {
      Serial.println(FAN_COUNT);
    }
    else if (line == "FAN_NAMES") {
      for (int i = 0; i < FAN_COUNT; i++) {
        Serial.println(fanNames[i]);
      }
    }
  }

  unsigned long now = millis();
  for (int i = 0; i < FAN_COUNT; i++) {
    if (now - lastUpdate[i] > timeout) {
      fanPWM[i] = 77;
      analogWrite(fanPins[i], fanPWM[i]);
      lastUpdate[i] = now;
    }
  }
}
