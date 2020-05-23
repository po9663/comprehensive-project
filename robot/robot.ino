#include <Adafruit_PWMServoDriver.h>




#include <SoftwareSerial.h> 
#include <PS2X_lib.h>
#include <Wire.h>
#include "PCA9685.h"
Adafruit_PWMServoDriver pwm=Adafruit_PWMServoDriver();


#define IN1 4
#define IN2 9
#define IN3 6
#define IN4 0
#define IN5 1
#define IN6 2
#define PS2_DAT     12 
#define PS2_CMD     11  
#define PS2_SEL     10  
#define PS2_CLK     13  

//#define pressures   true
#define pressures   false
//#define rumble      true
#define rumble      false

PCA9685 pwmController;
int angle;
int error = 0;
byte type = 0;
byte vibrate = 0;

PCA9685_ServoEvaluator pwmServo1;
PCA9685_ServoEvaluator pwmServo2;
PCA9685_ServoEvaluator pwmServo3;
PCA9685_ServoEvaluator pwmServo4;
PCA9685_ServoEvaluator pwmServo5;
PCA9685_ServoEvaluator pwmServo6;
int a;
int b;
int c;
int d;
int e;
int f;

void setup() {
   Serial.begin(57600);
  pwm.begin();
  Wire.setClock(400000);
  Serial.begin(57600);
  
}

void loop() {
if (Serial.available()) {
   
    String ist = Serial.readStringUntil('\n');
    
    int comma = ist.indexOf(','); //콤마(,)를 찾아 번호를 매긴다
    int len = ist.length(); //문자의길이를 측정한다

    int x = ist.substring(0,comma).toInt(); //첫번째문자부터 콤마전까지의 수를 정수로 변환
    int y = ist.substring(comma+1,len).toInt(); //콤마이후의 수를 정수로 변환

    int rx = constrain(map(x,0,180,150,600),150,600); //입력받은 x값을 매핑, contrain으로 범위를 한정
    int ry = constrain(map(y,0,180,150,600),150,600); //위와같음

    pwm.setPWM(0,0,rx); //pca9685의 0번포트에 연결된 서보를 rx만큼 회전
    pwm.setPWM(1,0,ry); //pca9685의 1번에 연결된 서보를 ry만큼 회전
    
    Serial.print('(');
    Serial.print(x);
    Serial.print(',');
    Serial.print(y);
    Serial.print(')');
    Serial.print('-');
    Serial.print('>');
    Serial.print('(');
    Serial.print(rx);
    Serial.print(',');
    Serial.print(ry);
    Serial.println(')');
    
    delay(10);
  }



    
  }
