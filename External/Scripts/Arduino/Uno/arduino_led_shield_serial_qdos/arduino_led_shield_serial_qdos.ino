
#include <LiquidCrystal.h>
//#include <LCD_I2C.h>

// Define the LCD pins
//LCD_I2C lcd(0x27, 16, 2);
const int rs = 8, en = 9, d4 = 4, d5 = 5, d6 = 6, d7 = 7;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);

void setup() {
  lcd.begin(16, 2);
  Serial.begin(9600);
  //lcd.backlight();
  lcd.print("Serial Control");
}

void loop() {
  if (Serial.available() > 0) {
    String command = Serial.readStringUntil('\n');
    command.trim();
    if (command == "UpVolume") {
      lcd.clear();
      lcd.print("UpVolume");
      delay(1000);
      Serial.println("UpVolume_ACK"); // Send acknowledgment
      lcd.clear();
    } else if (command == "DownVolume") {
      lcd.clear();
      lcd.print("DownVolume");
      delay(1000);
      Serial.println("DownVolume_ACK"); // Send acknowledgment
      lcd.clear();
    } else if (command == "PowerUp") {
      lcd.clear();
      lcd.print("PowerUp");
      delay(1000);
      Serial.println("PowerUp_ACK"); // Send acknowledgment
      lcd.clear();
    } else if (command == "PowerDown") {
      lcd.clear();
      lcd.print("PowerDown");
      delay(1000);
      Serial.println("PowerDown_ACK"); // Send acknowledgment
      lcd.clear();
    } else if (command == "PTT") {
      lcd.clear();
      lcd.print("PTT");
      delay(1000);
      Serial.println("PTT_ACK"); // Send acknowledgment
      lcd.clear();
    } else if (command == "MeasureMic") {
      lcd.clear();
      lcd.print("MeasureMic");
      delay(1000);
      Serial.println("MeasureMic_ACK"); // Send acknowledgment
      lcd.clear();
    } else if (command == "MonitorStartAction") {
      lcd.clear();
      lcd.print("MonitorStartAction");
      delay(1000);
      Serial.println("MonitorStartAction_ACK_1"); // Send acknowledgment
      lcd.clear();
    } else {
      lcd.clear();
      lcd.print("Unknown Cmd");
      Serial.println("UNKNOWN_ACK");
    }
    delay(10);
  }
}