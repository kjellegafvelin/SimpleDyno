/*
  Sketch for use with SimpleDyno
  Developed on Arduino Uno Platform
  DamoRC - 2013-2014
  
  ALWAYS use the Sketch distributed with each new version of SimpleDyno
  
  Transmits:
    1 x Session timestamp 
    1 x Interrupt timestamp and 1 x time interval since last interrupt for INT0 / Pin2 / RPM1
    1 x Interrupt timestamp and 1 x time interval since last interrupt for INT1 / Pin3 / RPM2
    6 x Analog Inputs (A0 and A1 are Voltage and Current, A2 and A3 are Temperature, A4 and A5 are open)
  Values are comma delimeted
  Baud rates selected in SD must match coded values in this Sketch.
 */

  const int NumPortsToRead = 6;
  int AnalogResult[NumPortsToRead];
  volatile unsigned long TimeStamp = 0;
  volatile unsigned long time1 = 0;
  volatile unsigned long time2 = 0;
  volatile unsigned long Oldtime1 = 0;
  volatile unsigned long Oldtime2 = 0;
  volatile unsigned long TempTime1 = 0;
  volatile unsigned long TempTime2 = 0;
  String AllResult = "";

void setup() {
  // Initialize serial communication
  // Ensure that Baud rate specified here matches that selected in SimpleDyno
  // Availailable Baud rates are:
  // 9600, 14400, 19200, 28800, 38400, 57600, 115200
  Serial.begin(9600);
  // Initialize interupts (Pin2 is interrupt 0 = RPM1, Pin3 in interrupt 1 = RPM2)
  attachInterrupt(0,channel1,FALLING);
  attachInterrupt(1,channel2,FALLING);
}

void loop() {
  AllResult = "";
  AllResult += micros();
  AllResult += ",";
  AllResult += TempTime1;
  AllResult += ",";
  AllResult += time1;
  AllResult += ",";
  AllResult += TempTime2;
  AllResult += ",";
  AllResult += time2;
  for (int Looper = 0; Looper < NumPortsToRead;Looper++){
    AnalogResult[Looper] = analogRead(Looper);
    AllResult += ",";
    AllResult += AnalogResult[Looper];
  }
  Serial.println (AllResult);
  Serial.flush();
  delay(1);
}

//Interrupt routine for RPM1
void channel1(){
  TempTime1 = micros();
  time1 = TempTime1-Oldtime1;
  Oldtime1 = TempTime1;
}

//Interrupt routine for RPM2
void channel2(){
    TempTime2 = micros();
  time2 = TempTime2-Oldtime2;
  Oldtime2 = TempTime2;
}
