#include<Servo.h>

void setup() {
 Serial.begin(9600);
  
}

void loop() {
  int czcz=1;
  char ch[100];
  String a= Serial.readString();
  String dda="2/4,0,9,1,1,1/4,0,8,2,1,1/";  
  if(a.compareTo(dda)){
      Serial.write(czcz+1);
  }
  
  
  a.toCharArray(ch,a.length());
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
      for (j = 0; j < 5; j++) {
        ptr2 = strtok(NULL, ",");
        arr[cnt3][j + 1] = atoi(ptr2);
         Serial.print(arr[cnt3][j + 1]);
      }
  Serial.println(" ");
    cnt3++;
  }
 
}
