/*
Arduino Mega 2560
IMU Razor 9DOF
GPS
ADC with LM35 Temperature Sensor, MPXV7002DP Pietot Tube and MPX5114 Barometer
PWM based on PIC18F4520
Logomatic v2

Capture the variables for the UAV with IMU, GPS, ADC and PWM data and send via Radiometrics and store into SD Card with Logomatic v2.
*/

#define PCB_LED 26

uint32_t syncTimeADC = 0; //Sync variable used to control ADCLoop execution frecuency

long int lastTime = 0;
long int curTime;
byte deltaTime;


void setup()
{
  
 Serial.begin(19200); //USB / Radiometrics / Logomatic Port Baudrate Configuration
 
 //IMUSetup();
 //GPSSetup();
 //PWMSetup();
 ADCSetup();
  
}


void loop()
{
  
  //IMULoop();
  //GPSLoop();
  //PWMLoop();
  
  if((millis() - syncTimeADC) > 200){ //Reading ADC Sensors aproximately 5 times / second
    
    syncTimeADC = millis();
    ADCLoop();
    
  }
  
}


byte SetTime() //Sets the incremental time between data strings for each sensor
{
  
  curTime = millis();
  deltaTime = byte(curTime - lastTime);
  lastTime = curTime;
  
  return deltaTime;
  
}

