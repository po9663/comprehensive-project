#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>
Adafruit_PWMServoDriver pwm=Adafruit_PWMServoDriver();
int pos =0;

void setup() {
  Serial.begin(9600);
  pwm.begin();
  pwm.setPWMFreq(51);
}

void loop() {
  //4번 모터 기준 0도 3번 0도 2번 0도 1번 45도가 정확히 중간 
  if (Serial.available()) {
    
    
    String ist = Serial.readString();
    //좌표에 따른 매핑
    int oneComma = ist.indexOf(','); //콤마(,)를 찾아 번호를 매긴다
    int twoComma = ist.indexOf(",",oneComma+1);
    int threeComma = ist.indexOf(",",oneComma+2);
    
  
    int len = ist.length(); //문자의길이를 측정한다
  Serial.write(len);
    int x = ist.substring(0,oneComma).toInt(); //첫번째문자부터 콤마전까지의 수를 정수로 변환
    int y = ist.substring(oneComma+1,twoComma).toInt(); //콤마이후의 수를 정수로 변환
    int z = ist.substring(twoComma+1).toInt(); //첫번째문자부터 콤마전까지의 수를 정수로 변환
    Serial.write(x);
    Serial.write(y);
    Serial.write(z);
   //좌표값 유니티에서 받아옴
   /*
   if(x==0){

   if(y==0){
        if(z==0){
             }

        if(z==1){
             }
        if(z==2){
             }
   }
   if(y==1){
         if(z==0){
             }

         if(z==1){
             }
        if(z==2){
             }
   }
   if(y==2){
        if(z==0){
             }

          if(z==1){
             }
        if(z==2){
             }
   }
   
   }

   if(x==1){

   if(y==0){
        if(z==0){
             }
        if(z==1){
             }
        if(z==2){
             }
   }
   if(y==1){
        if(z==0){
             }
        if(z==1){
             }
        if(z==2){
             }
   }
   if(y==2){
       if(z==0){
             }
        if(z==1){
             }
        if(z==2){
             }
   }
   
   }

   if(x==2){

   if(y==0){
        if(z==0){
             }
        if(z==1){
             }
        if(z==2){
             }
   }
   if(y==1){
         if(z==0){
             }
        if(z==1){
             }
        if(z==2){
             }
   }
   if(y==2){
        if(z==0){
             }
        if(z==1){
             }
        if(z==2){
             }
   }
   
   }
   
   */

   /* int rx = constrain(map(x,0,180,150,600),150,600); //입력받은 x값을 매핑, contrain으로 범위를 한정
  
    int ry = constrain(map(y,0,180,150,600),150,600); //위와같음
   
    int rz = constrain(map(z,0,180,150,600),150,600); //입력받은 z값을 매핑, contrain으로 범위를 한정
   

    pwm.setPWM(0,0,rx); //pca9685의 0번포트에 연결된 서보를 rx만큼 회전
  delay(4000);
    pwm.setPWM(1,0,ry); //pca9685의 1번에 연결된 서보를 ry만큼 회전
    delay(4000);
    pwm.setPWM(2,0,rz);
    
    Serial.print('(');
    Serial.print(x);
    Serial.print(',');
    Serial.print(y);
    Serial.print(',');
    Serial.print(z);
    Serial.print(')');
    Serial.print('-');
    Serial.print('>');
    Serial.print('(');
    Serial.print(rx);
    Serial.print(',');
    Serial.print(ry);
    Serial.print(',');
    Serial.print(rz);
    Serial.println(')');*/
    
    
  }
}
