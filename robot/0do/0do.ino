
#include<Servo.h>

Servo servo;
void setup() {
  Serial.begin(9600);
 Serial.println("arduino is redy");
 servo.attach(9);
 
}

void loop() {
if(Serial.available()){
  int dd=1;
  String a= Serial.readString();
  String b1="1,2,3,2,1,2/1,3,2,1,2,2/";

 if(b1.compareTo(a)==1){
  Serial.write(dd);
 }
  
  /*char re[4];
  a.substring(0,3).toCharArray(re,4);
  int b= atoi(re);
  
    for(int j=2; j<=b; j++){
  servo.write(j);
  delay(20);
    }
   for(int m=b; m>=2; m--){
  servo.write(m);
  delay(20);
   }*/
  
}

//값을 가져와서 형변환 
  
  
    
  
}
