#define LCDI2C_ENABLED 0

#if LCDI2C_ENABLED
#include <LCD_I2C.h>
#else
#include <LiquidCrystal.h>
#endif

// Define the LCD pins
#if LCDI2C_ENABLED
LCD_I2C lcd(0x27, 16, 2);
#else
const int rs = 8, en = 9, d4 = 4, d5 = 5, d6 = 6, d7 = 7;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);

const int keypadPin = A0;
const int btnRIGHT = 0;
const int btnUP = 100;
const int btnDOWN = 250;
const int btnLEFT = 400;
const int btnSELECT = 600;
const int btnNONE = 1023;

int readKeypad() {
  int adc_key_in = analogRead(keypadPin);

  if (adc_key_in > 1000) return btnNONE;
  if (adc_key_in < 50) return btnRIGHT;
  if (adc_key_in < 200) return btnUP;
  if (adc_key_in < 350) return btnDOWN;
  if (adc_key_in < 550) return btnLEFT;
  if (adc_key_in < 750) return btnSELECT;

  return btnNONE; //for safety
}
#endif

const int LED_PASS_PIN = 12;
const int LED_FAIL_PIN = 13;

bool gIsPass = false;
int gStartButtonPressedState = 0;
int gResetButtonPressedState = 0;

void initStates() {
  gIsPass = false;
  gStartButtonPressedState = 0;
  gResetButtonPressedState = 0;
  
  return;
}

void resetAll() {
  initStates();

  delay(200);
  digitalWrite(LED_PASS_PIN, LOW);
  
  delay(200);
  digitalWrite(LED_FAIL_PIN, LOW);

  return;
}

void monitorStartButtonPressed() {
  int startButtonPressedTimeCounter = 0;
  while(1)
  {
    if (startButtonPressedTimeCounter == 500) {
      lcd.clear();
      lcd.print("Reset Monitor");
      delay(1000);
      lcd.clear();
      gStartButtonPressedState = 0;
      startButtonPressedTimeCounter = 0;
      break;
    }

    if (Serial.available() > 0) {
      String command = Serial.readStringUntil('\n');
      command.trim();
      if (command == "MonitorStartAction") {
        if (gStartButtonPressedState = 1) {
          Serial.println("MonitorStartAction_ACK_1"); // Send acknowledgment
          gStartButtonPressedState = 0;
          startButtonPressedTimeCounter = 0;
          break;
        }
        else {
          Serial.println("MonitorStartAction_ACK_0"); // Send acknowledgment
        }    
      }
    }

    startButtonPressedTimeCounter = startButtonPressedTimeCounter + 1;

    delay(10);
  }

  return;
}

void monitorResetButtonPressed() {
  int resetButtonPressedTimeCounter = 0;
  while(1)
  {
    if (resetButtonPressedTimeCounter == 500) {
      lcd.clear();
      lcd.print("Reset Monitor");
      delay(1000);
      lcd.clear();
      gResetButtonPressedState = 0;
      resetButtonPressedTimeCounter = 0;
      break;
    }

    if (Serial.available() > 0) {
      String command = Serial.readStringUntil('\n');
      command.trim();
      if (command == "MonitorStartAction") {
        if (gResetButtonPressedState = 1) {
          Serial.println("MonitorStartAction_ACK_2"); // Send acknowledgment
          gResetButtonPressedState = 0;
          resetButtonPressedTimeCounter = 0;
          break;
        }
        else {
          Serial.println("MonitorStartAction_ACK_0"); // Send acknowledgment
        }    
      }
    }

    resetButtonPressedTimeCounter = resetButtonPressedTimeCounter + 1;

    delay(10);
  }

  return;
}

void setup() {
  lcd.begin(16, 2);
  Serial.begin(9600);
#if LCDI2C_ENABLED  
  lcd.backlight();
#endif
  lcd.print("Serial Control"); 

  pinMode(LED_PASS_PIN, OUTPUT);
  pinMode(LED_FAIL_PIN, OUTPUT); 
  resetAll();
}

void loop() {
#if LCDI2C_ENABLED
#else
  int key = readKeypad();
  if (key == btnRIGHT) {
    lcd.clear();
    lcd.print("StartTest");
    delay(500);
    gStartButtonPressedState = 1;
    lcd.clear();  
    monitorStartButtonPressed();
  } 
  else if (key == btnLEFT) {
    lcd.clear();
    lcd.print("ResetTest");
    delay(500);
    gResetButtonPressedState = 1;
    lcd.clear();  
    monitorResetButtonPressed();
  }   
#endif

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
      Serial.println("MonitorStartAction_ACK"); // Send acknowledgment
    } else if (command == "ForceFailTest") {
      gIsPass = false;
      lcd.clear();
      lcd.print("ForceFailTest");
      delay(500);
      digitalWrite(LED_FAIL_PIN, HIGH);
      Serial.println("ForceFailTest_ACK"); // Send acknowledgment
      lcd.clear();
    } else if (command == "ForcePassTest") {
      gIsPass = true;
      lcd.clear();
      lcd.print("ForcePassTest");
      delay(500);
      digitalWrite(LED_PASS_PIN, HIGH);
      Serial.println("ForcePassTest_ACK"); // Send acknowledgment
      lcd.clear();
    } else if (command == "ResetTest") {
      lcd.clear();
      lcd.print("ResetTest");
      delay(500);
      Serial.println("ResetTest_ACK"); // Send acknowledgment
      resetAll();
      lcd.clear();
    } else if (command == "CompleteTest") {
      lcd.clear();
      lcd.print("CompleteTest");
      //delay(500);
    
      if (gIsPass) {
        Serial.println("CompleteTest_ACK_PASS"); // Send acknowledgment  
        gIsPass = false;    
      }
      else {
        Serial.println("CompleteTest_ACK"); // Send acknowledgment
      }

      resetAll();      
      lcd.clear();
    } else {
      lcd.clear();
      lcd.print("Unknown Cmd");
      Serial.println("UNKNOWN_ACK");
    }
    
    delay(10);
  }
}