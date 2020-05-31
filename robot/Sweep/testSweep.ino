/* Sweep
 by BARRAGAN <http://barraganstudio.com>
 This example code is in the public domain.

 modified 8 Nov 2013
 by Scott Fitzgerald
 http://www.arduino.cc/en/Tutorial/Sweep
*/
#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>

// called this way, it uses the default address 0x40
Adafruit_PWMServoDriver pwm = Adafruit_PWMServoDriver();

#define MIN_PULSE_WIDTH       600
#define MAX_PULSE_WIDTH       2350
#define DEFAULT_PULSE_WIDTH   1500
#define FREQUENCY             50
// our servo # counter
uint8_t servonum = 0;
int rese=0;
void setup() {
  Serial.begin(9600);
  Serial.println("16 channel Servo test!");
  pwm.begin();
  pwm.setPWMFreq(FREQUENCY);  // Analog servos run at ~60 Hz updates
}

void loop() {
  for(int i=0; i<=45;i++){
  pwm.setPWM(0, 0, pulseWidth(i));
  Serial.println("0");
  
   pwm.setPWM(1, 0, pulseWidth(i));
  Serial.println("0");
  
  pwm.setPWM(2, 0, pulseWidth(i));
  Serial.println("0");
  delay(50);
  }
   for(int m=45; m>=0;m--){
  pwm.setPWM(0, 0, pulseWidth(m));
  Serial.println("0");
  
   pwm.setPWM(1, 0, pulseWidth(m));
  Serial.println("0");
  
  pwm.setPWM(2, 0, pulseWidth(m));
  Serial.println("0");
  delay(50);
  }
 exit(0);
}

int pulseWidth(int angle)
{
  int pulse_wide, analog_value;
  pulse_wide   = map(angle, 0, 180, MIN_PULSE_WIDTH, MAX_PULSE_WIDTH);
  analog_value = int(float(pulse_wide) / 1000000 * FREQUENCY * 4096);
  Serial.println(analog_value);
  return analog_value;
}
