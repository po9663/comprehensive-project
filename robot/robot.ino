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
 /*reset1();*/
  //4번 모터 기준 0도 3번 0도 2번 0도 1번 45도가 정확히 중간 
if(Serial.available()>0){
    char ch[100];
String a= Serial.readString();

   a.toCharArray(ch,a.length());
int num=0;
int cc=4;
/*pwm.setPWM(0,0,300);//집게열고
delay(2000);
pwm.setPWM(0,0,400);//집게열고*/
int cnt = 0;
  int cnt2 = 0;
  int cnt3 = 0;
  int ptr2len = 0;
  int j = 0;
  int ok = 0;
  char *ptr = strtok(ch, "/");
  cnt = atoi(ptr);
  char *b[5][20];
  int arr[5][7];
  int bo = 0;
  int first = 0;
  char *ptr2;
  
  while (cnt2 < cnt) {
    ptr = strtok(NULL, "/");
      b[cnt2][0] = ptr;
    cnt2++;
  }
  while (cnt3 < cnt) {
     ptr2 = strtok(b[cnt3][0], ",");
      arr[cnt3][0] = atoi(ptr2);
      Serial.print(arr[cnt3][0]);
      for (j = 0; j < 6; j++) {
        ptr2 = strtok(NULL, ",");
        arr[cnt3][j + 1] = atoi(ptr2);
         Serial.print(arr[cnt3][j + 1]);
      }
  Serial.println(" ");
    cnt3++;
  }
  int cnt5=0;
  /*left();*/
  /*straight();*/
 
  while(cnt5<cnt){
        pickbox();
        delay(3000);
        left(arr[cnt5][0],arr[cnt5][3],arr[cnt5][2]);
        delay(3000);
        reset1();
        cnt5++;
    
  }
 exit(0);
/*reset1();
delay(1500);
 
pickbox();
delay(8000);
straight();
 
*/
}
}

void reset1(){
  int m0 = constrain(map(35,0,180,150,600),150,600);
int m1 = constrain(map(80,0,180,150,600),150,600);
int m2 = constrain(map(0,0,180,150,600),150,600);
int m4 = constrain(map(20,0,180,150,600),150,600);
int m5 = constrain(map(1,0,180,150,600),150,600);
int m6 = constrain(map(35,0,180,150,600),150,600);
  pwm.setPWM(0,0,350);
  pwm.setPWM(1,0,m1);
  pwm.setPWM(2,0,130);
  pwm.setPWM(4,0,m4);
  pwm.setPWM(5,0,m5);
  pwm.setPWM(6,0,m6);
  
}

void pickbox(){
  int m0 = constrain(map(35,0,180,150,600),150,600);
int m1 = constrain(map(80,0,180,150,600),150,600);
int m2 = constrain(map(0,0,180,150,600),150,600);
int m4 = constrain(map(20,0,180,150,600),150,600);
int m5 = constrain(map(50,0,180,150,600),150,600);
int m6 = constrain(map(110,0,180,150,600),150,600);
int rem5 = constrain(map(1,0,180,150,600),150,600);
int rem15 = constrain(map(60,0,180,150,600),150,600);
int fm5 = constrain(map(1,0,180,150,600),150,600);
int rm6 = constrain(map(35,0,180,150,600),150,600);
  pwm.setPWM(0,0,300);//집게열고
  delay(1500);
  for(int j=rm6; j<=m6; j++){//돌고
     pwm.setPWM(6,0,j);
     delay(5);
  }

   delay(1500);
  pwm.setPWM(4,0,m4);//유지
   
   for(int i=fm5; i<=m5;i++){//내려가
    pwm.setPWM(5,0,i);
    delay(10);
   }
    delay(1000);
   pwm.setPWM(5,0,rem15);//좀더내려가
  
   delay(1500);
     pwm.setPWM(0,0,400);//집어
     delay(1500);

  pwm.setPWM(5,0,rem5);
  delay(1500);
  reset1();
}

 void left(int x,int sizex,int sizez){
  int sizenum=0;
  int cc=9;
  int znum= cc - sizez;
  if(sizex==2){
    sizenum=6;
  }
  else if(sizex==1){
    sizenum=5;
  }
int m1 = constrain(map(80,0,180,150,600),150,600);
int m2 = constrain(map(0,0,180,150,600),150,600);
int m4 = constrain(map(20,0,180,150,600),150,600);
int m5 = constrain(map(90,0,180,150,600),150,600);
int m6 = constrain(map(55-(x*3+sizenum),0,180,150,600),150,600);
int mg5 = constrain(map(1,0,180,150,600),150,600);
int cm5 = constrain(map(90,0,180,150,600),150,600);
int bm5 = constrain(map(45,0,180,150,600),150,600);
int mg6 = constrain(map(35,0,180,150,600),150,600);
int zm5 = constrain(map(90-(znum*22),0,180,150,600),150,600);
  if(znum == 0){
  pwm.setPWM(0,0,400);
delay(1500);
 pwm.setPWM(2,0,145);
  pwm.setPWM(6,0,m6);
  for(int i=mg5; i<=cm5;i++){
    pwm.setPWM(5,0,i);
    delay(20);
  }
   delay(1500);
   pwm.setPWM(0,0,300);
  delay(1500);
  pwm.setPWM(4,0,m4);
  pwm.setPWM(5,0,bm5);
  delay(1500);
   pwm.setPWM(0,0,400);
   
   delay(1500);
   for(int bm=bm5; bm<=m5;bm++){
    pwm.setPWM(5,0,bm);
    delay(10);
  }
 for(int j=m5; j>=mg5;j--){
    pwm.setPWM(5,0,j);
    delay(20);
  }
  pwm.setPWM(6,0,mg6);
  delay(1500);
   pwm.setPWM(2,0,130);
    pwm.setPWM(0,0,300);
}
else{
   pwm.setPWM(0,0,400);
delay(1500);
 pwm.setPWM(2,0,145);
  pwm.setPWM(6,0,m6);
  for(int i=mg5; i<=zm5;i++){
    pwm.setPWM(5,0,i);
    delay(20);
  }
   delay(1500);
   pwm.setPWM(0,0,300);
  delay(1500);
  pwm.setPWM(4,0,m4);
  pwm.setPWM(5,0,bm5);
  delay(1500);
   pwm.setPWM(0,0,400);
   
   delay(1500);
    for(int bm=bm5; bm<=zm5+5;bm++){
    pwm.setPWM(5,0,bm);
    delay(10);
  }
 for(int j=zm5+5; j>=mg5;j--){
    pwm.setPWM(5,0,j);
    delay(20);
  }
  pwm.setPWM(6,0,mg6);
  delay(1500);
   pwm.setPWM(2,0,130);
    pwm.setPWM(0,0,300);
}
}

 
