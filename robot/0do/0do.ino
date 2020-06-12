
#include<Servo.h>

Servo servo;
void setup() {
  Serial.begin(9600);
 Serial.println("arduino is redy");
 servo.attach(9);
 
}

void loop() {
if(Serial.available()){
  String a= Serial.readString();
  
  char re[4];
  a.substring(0,3).toCharArray(re,4);
  int b= atoi(re);
  Serial.write(b);
    for(int j=0; j<=b; j++){
  servo.write(j);
  delay(20);
    }
   for(int m=b; m>=0; m--){
  servo.write(m);
  delay(20);
   }
  
}

//값을 가져와서 형변환 
  
  
    
  
}
