/* ******************************************************* */
/* I2C code for ADXL345 accelerometer                      */
/* and HMC5843 magnetometer                                */
/* ******************************************************* */

int AccelAddress = 0x53;
int CompassAddress = 0x1E;  //0x3C //0x3D;  //(0x42>>1);


void I2C_Init()
{
  Wire.begin();
}

void Accel_Init()
{
  Wire.beginTransmission(AccelAddress);
  Wire.write(0x2D);  // power register
  Wire.write(0x08);  // measurement mode
  Wire.endTransmission();
  delay(20);
  Wire.beginTransmission(AccelAddress);
  Wire.write(0x31);  // Data format register
  Wire.write(0x08);  // set to full resolution
  Wire.endTransmission();
  delay(20);	
  // Because our main loop runs at 50Hz we adjust the output data rate to 50Hz (25Hz bandwith)
  //Wire.beginTransmission(AccelAddress);
  //Wire.send(0x2C);  // Rate
  //Wire.send(0x09);  // set to 50Hz, normal operation
  //Wire.endTransmission();
}

// Reads x,y and z accelerometer registers
void Read_Accel()
{
  int i = 0;
  byte buff[6];
  
  Wire.beginTransmission(AccelAddress); 
  Wire.write(0x32);        //sends address to read from
  Wire.endTransmission(); //end transmission
  
  Wire.beginTransmission(AccelAddress); //start transmission to device
  Wire.requestFrom(AccelAddress, 6);    // request 6 bytes from device
  
  while(Wire.available())   // ((Wire.available())&&(i<6))
  { 
    buff[i] = Wire.read();  // receive one byte
    i++;
  }
  Wire.endTransmission(); //end transmission
  
  if (i==6)  // All bytes received?
    {
    AN[4] = (((int)buff[1]) << 8) | buff[0];    // Y axis (internal sensor x axis)
    AN[3] = (((int)buff[3]) << 8) | buff[2];    // X axis (internal sensor y axis)
    AN[5] = (((int)buff[5]) << 8) | buff[4];    // Z axis
    accel_x = SENSOR_SIGN[3]*(AN[3]-AN_OFFSET[3]);
    accel_y = SENSOR_SIGN[4]*(AN[4]-AN_OFFSET[4]);
    accel_z = SENSOR_SIGN[5]*(AN[5]-AN_OFFSET[5]);
    }
  //else
    //Serial.println("!ERR: Error reading accelerometer info!");
}

void Compass_Init()
{
  Wire.beginTransmission(CompassAddress);
  Wire.write((byte)2); 
  Wire.write((byte)0);   // Set continouos mode (default to 10Hz)
  Wire.endTransmission(); //end transmission
}

void Read_Compass()
{
  int i = 0;
  byte buff[6];
 
  Wire.beginTransmission(CompassAddress); 
  Wire.write(0x03);        //sends address to read from
  Wire.endTransmission(); //end transmission
  
  //Wire.beginTransmission(CompassAddress); 
  Wire.requestFrom(CompassAddress, 6);    // request 6 bytes from device
  while(Wire.available())   // ((Wire.available())&&(i<6))
  { 
    buff[i] = Wire.read();  // receive one byte
    i++;
  }
  Wire.endTransmission(); //end transmission
  
  if (i==6)  // All bytes received?
    {
    // MSB byte first, then LSB, X,Y,Z
    magnetom_x = SENSOR_SIGN[6]*((((int)buff[2]) << 8) | buff[3]);    // X axis (internal sensor y axis)
    magnetom_y = SENSOR_SIGN[7]*((((int)buff[0]) << 8) | buff[1]);    // Y axis (internal sensor x axis)
    magnetom_z = SENSOR_SIGN[8]*((((int)buff[4]) << 8) | buff[5]);    // Z axis
    //magnetom_x = buff[0];
    //magnetom_y = buff[2];
    //magnetom_z = buff[4];
    }
  //else
    //Serial.println("!ERR: Error reading magnetometer info!");
}

