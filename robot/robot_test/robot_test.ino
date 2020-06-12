#include <Wire.h>


void setup() {
  Serial.begin(9600);
 
}

void loop() {
  char ch = Serial.read(); 
 Serial.print(ch);
 exit(0);
}
