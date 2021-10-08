#include <SoftwareSerial.h>
#include <ArduinoJson.h>
SoftwareSerial s(5,6);
 
void setup() {
s.begin(115200);
}
 
void loop() {
delay(5000);
StaticJsonBuffer<200> jsonBuffer;
 JsonObject& root = jsonBuffer.createObject();
  root["data1"] = 100;
  root["data2"] = 200;
if(s.available()>0)
{
 root.printTo(s);
}
}
