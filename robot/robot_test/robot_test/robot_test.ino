#include "Ardunity.h"


void setup()
{
  ArdunityApp.resolution(256, 1024);
  ArdunityApp.timeout(5000);
  ArdunityApp.begin(115200);
}

void loop()
{
  ArdunityApp.process();
}
