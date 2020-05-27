#include <PCA9685.h>

#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>

Adafruit_PWMServoDriver pwm=Adafruit_PWMServoDriver();

PCA9685 pwmController(Wire, PCA9685_PhaseBalancer_Weaved); // Library using Wire1 and weaved phase balancing scheme

// Linearly interpolates between standard 2.5%/12.5% phase length (102/512) for -90°/+90°
PCA9685_ServoEvaluator pwmServo1;
PCA9685_ServoEvaluator pwmServo2(128,324,526);
void setup() {
  Serial.begin(115200);
  Wire.begin();                      // Wire must be started first
  Wire.setClock(400000);             // Supported baud rates are 100kHz, 400kHz, and 1000kHz
  pwmController.init(B000000);        // Address pins A5-A0 set to B000000
  pwmController.setPWMFrequency(50);  // 50Hz provides 20ms standard servo phase length
  pwmController.setChannelPWM(0, pwmServo1.pwmForAngle(-90));
  Serial.println(pwmController.getChannelPWM(0)); // Should output 102 for -90°

  // Showing linearity for midpoint, 205 away from both -90° and 90°
    Serial.println(pwmServo1.pwmForAngle(0));   // Should output 307 for 0°

    pwmController.setChannelPWM(0, pwmServo1.pwmForAngle(90));
    Serial.println(pwmController.getChannelPWM(0)); // Should output 512 for +90°

    pwmController.setChannelPWM(1, pwmServo2.pwmForAngle(-90));
    Serial.println(pwmController.getChannelPWM(1)); // Should output 128 for -90°

    // Showing less resolution in the -90° to 0° range
    Serial.println(pwmServo2.pwmForAngle(-45)); // Should output 225 for -45°, 97 away from -90°

    pwmController.setChannelPWM(1, pwmServo2.pwmForAngle(0));
    Serial.println(pwmController.getChannelPWM(1)); // Should output 324 for 0°

    // Showing more resolution in the 0° to +90° range
    Serial.println(pwmServo2.pwmForAngle(45));  // Should output 424 for +45°, 102 away from +90°

    pwmController.setChannelPWM(1, pwmServo2.pwmForAngle(90));
    Serial.println(pwmController.getChannelPWM(1)); // Should output 526 for +90°
}

void loop() {
  int a = Serial.parseInt(); //시리얼 통신으로 값을 받아옴
  
  if (Serial.available()) {
    
    int ra = constrain(map(a, 0, 180, 150, 600), 150, 600); //받은 값의 범위 0~180을 펄스길이150~600으로 매핑해주고, ra의 최소,최대를 150,600을 넘지 않게 해준다.

    pwm.setPWM(0,0,ra); //pca9685모듈의 0번 포트에 연결된 서보를 ra만큼 회전
    
    Serial.print('(');
    Serial.print(a);
    Serial.print(',');
    Serial.print(ra);
    Serial.println(')');
    delay(10);
  }
}
