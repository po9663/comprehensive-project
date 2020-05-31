
#include<Servo.h>

Servo servo;
void setup() {
 servo.attach(9);
 delay(1000);
}

void loop() {
 for(int j=0; j<=160; j++){
  servo.write(j);
  delay(50);
    
  }
   for(int m=160; m>=0; m--){
  servo.write(m);
  delay(50);
    
  }
}
