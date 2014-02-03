
// Sparkfun 9DOF Razor IMU AHRS
// 9 Degree of Measurement Attitude and Heading Reference System
// Firmware v1.0
//
// Released under Creative Commons License 
// Code by Doug Weibel and Jose Julio
// Based on ArduIMU v1.5 by Jordi Munoz and William Premerlani, Jose Julio and Doug Weibel

// Axis definition: 
   // X axis pointing forward (to the FTDI connector)
   // Y axis pointing to the right 
   // and Z axis pointing down.
// Positive pitch : nose up
// Positive roll : right wing down
// Positive yaw : clockwise

/* Hardware version - v13
	
	ATMega328@3.3V w/ external 8MHz resonator
	High Fuse DA
        Low Fuse FF
	
	ADXL345: Accelerometer
	HMC5843: Magnetometer
	LY530:	Yaw Gyro
	LPR530:	Pitch and Roll Gyro

        Programmer : 3.3v FTDI
        Arduino IDE : Select board  "Arduino Duemilanove w/ATmega328"
*/

#include <Wire.h>

// ADXL345 Sensitivity(from datasheet) => 4mg/LSB   1G => 1000mg/4mg = 256 steps (mpbatlle::??)
// mpbatlle: From specifications, directly: 256 <--> 1g if span = +-2g and resolution 10 bits --> 1g = 256 levels 
// Tested value : 256
#define GRAVITY 256  //this equivalent to 1G in the raw data coming from the accelerometer 
//#define Accel_Scale(x) x*(GRAVITY/9.81)//Scaling the raw data of the accel to actual acceleration in meters for seconds square
#define Accel_Scale(x) x*(GRAVITY/9.80665) //mpbatlle::mora accuracy on constants!!

#define ToRad(x) (x*0.01745329252)  // *pi/180
#define ToDeg(x) (x*57.2957795131)  // *180/pi

// LPR530 & LY530 Sensitivity (from datasheet) => 3.33mV/º/s, 3.22mV/ADC step => 1.03
// Tested values : 
#define Gyro_Gain_X 0.92 //X axis Gyro gain
#define Gyro_Gain_Y 0.92 //Y axis Gyro gain
#define Gyro_Gain_Z 0.92 //Z axis Gyro gain
#define Gyro_Scaled_X(x) x*ToRad(Gyro_Gain_X) //Return the scaled ADC raw data of the gyro in radians for second
#define Gyro_Scaled_Y(x) x*ToRad(Gyro_Gain_Y) //Return the scaled ADC raw data of the gyro in radians for second
#define Gyro_Scaled_Z(x) x*ToRad(Gyro_Gain_Z) //Return the scaled ADC raw data of the gyro in radians for second

#define Kp_ROLLPITCH 0.0125
#define Ki_ROLLPITCH 0.000008
#define Kp_YAW 1.2
#define Ki_YAW 0.000008

/*For debugging purposes*/
//OUTPUTMODE=1 will print the corrected data, 
//OUTPUTMODE=0 will print uncorrected data of the gyros (with drift)
//OUTPUTMODE=2 will print accelerometer only data and magnetometer data
#define OUTPUTMODE 1

#define PRINT_DCM 0     //Will print the whole direction cosine matrix
#define PRINT_ANALOGS 1 //Will print the analog raw data
#define PRINT_EULER 1   //Will print the Euler angles Roll, Pitch and Yaw
//#define PRINT_GPS 0     //Will print GPS data
//#define PRINT_BINARY 0  //Will print binary message and suppress ASCII messages (above)

#define ADC_WARM_CYCLES 50
#define STATUS_LED 13  //5?

#define FALSE 0
#define TRUE 1

int8_t sensors[3] = {1,2,0};  // Map the ADC channels gyro_x, gyro_y, gyro_z
int SENSOR_SIGN[9] = {-1,1,-1,1,1,1,-1,-1,-1};  //Correct directions x,y,z - gyros, accels, magnetormeter

float G_Dt=0.02;    // Integration time (DCM algorithm)  We will run the integration loop at 50Hz if possible

long timer=0;   //general purpuse timer
long timer_old;
long timer24=0; //Second timer used to print values 
float AN[9]; //array that store the 3 ADC filtered data
float AN_OFFSET[9] = {0,0,0,0,0,0,0,0,0}; //Array that stores the Offset of the sensors

float accel_x;
float accel_y;
float accel_z;
float magnetom_x;
float magnetom_y;
float magnetom_z;
float MAG_Heading;

float Accel_Vector[3]= {0,0,0}; //Store the acceleration in a vector
float Mag_Vector[3]= {0,0,0};  //Store the magnetometer direction in a vector
float Gyro_Vector[3]= {0,0,0};//Store the gyros turn rate in a vector
float Omega_Vector[3]= {0,0,0}; //Corrected Gyro_Vector data
float Omega_P[3]= {0,0,0};//Omega Proportional correction
float Omega_I[3]= {0,0,0};//Omega Integrator
float Omega[3]= {0,0,0};

// Euler angles
float roll;
float pitch;
float yaw;

float errorRollPitch[3]= {0,0,0}; 
float errorYaw[3]= {0,0,0};

unsigned int counter=0;
byte gyro_sat=0;

float DCM_Matrix[3][3]= {
  {
    1,0,0  }
  ,{
    0,1,0  }
  ,{
    0,0,1  }
}; 
float Update_Matrix[3][3]={{0,1,2},{3,4,5},{6,7,8}}; //Gyros here


float Temporary_Matrix[3][3]={
  {
    0,0,0  }
  ,{
    0,0,0  }
  ,{
    0,0,0  }
};
 
//ADC variables
volatile uint8_t MuxSel=0;
volatile uint8_t analog_reference = DEFAULT;
volatile uint16_t analog_buffer[8];
volatile uint8_t analog_count[8];

void setup()
{ 
  Serial.begin(19200);
  pinMode (STATUS_LED,OUTPUT);  // Status LED
  
  Analog_Reference(DEFAULT); 
  Analog_Init();
  //mpbatlle::No more println!!!!
  /*
  Serial.println();
  Serial.println("Sparkfun 9DOF Razor IMU v1.06");
  Serial.println("9 Degree of Measurement Attitude and Heading Reference System");
  Serial.println("Initialization...(IMU flat and still)");
  */
  for(int c=0; c<ADC_WARM_CYCLES; c++)
  { 
    digitalWrite(STATUS_LED,LOW);
    delay(50);
    Read_adc_raw();
    digitalWrite(STATUS_LED,HIGH);
    delay(50);
  }
  digitalWrite(STATUS_LED,LOW);
  
  // Acceleromter initialization
  I2C_Init();
  delay(20);
  Accel_Init();
  delay(60);
  Read_Accel();
  
  // Magnetometer initialization
  Compass_Init();
  
  // Initialze ADC readings and buffers
  Read_adc_raw();
  delay(20);
  Read_adc_raw();

  for(int y=0; y<6; y++)   // Use last initial ADC values for initial offset.
    AN_OFFSET[y]=AN[y];
  delay(20);
  for(int i=0;i<400;i++)    // We take some readings...
    {
    Read_adc_raw();
    Read_Accel();
    for(int y=0; y<6; y++)   // Read ADC values for offset (averaging).
      AN_OFFSET[y]=AN_OFFSET[y]*0.9 + AN[y]*0.1;
    delay(20);
    }
  AN_OFFSET[5]-=GRAVITY*SENSOR_SIGN[5];
  
  // ******  Need to do something here to handle initial condition of magnetometer??  
  /*
  Serial.println("Offset values:");
  for(int y=0; y<6; y++)
    Serial.println(AN_OFFSET[y]);
  */
  
  delay(2000);
  digitalWrite(STATUS_LED,HIGH);
    
  Read_adc_raw();     // ADC initialization
  timer=millis();
  delay(20);
  counter=0;
}

void loop() //Main Loop
{
//  if((DIYmillis()-timer)>=20)  // Main loop runs at 50Hz
    if((DIYmillis()-timer)>=100)  // mpbatlle::Main loop runs at 10Hz
  {
    counter++;
    timer_old = timer;
    timer=DIYmillis();
    G_Dt = (timer-timer_old)/1000.0;    // Real time of loop run. We use this on the DCM algorithm (gyro integration time)
    if(G_Dt > 1)
        G_Dt = 0;  //keeps dt from blowing up, goes to zero to keep gyros from departing
    
    // *** DCM algorithm
    // Data adquisition
    Read_adc_raw();   // This read gyro data
    Read_Accel();     // Read I2C accelerometer
    
    if (counter > 5)  // Read compass data at 10Hz... (5 loop runs)
      {
      counter=0;
      Read_Compass();    // Read I2C magnetometer
      Compass_Heading(); // Calculate magnetic heading  
      }
    
    // Calculations...
    Matrix_update(); 
    Normalize();
    Drift_correction();
    Euler_angles();
    // ***
   
    printdata();
    
    //Turn off the LED when you saturate any of the gyros.
    if((abs(Gyro_Vector[0])>=ToRad(300))||(abs(Gyro_Vector[1])>=ToRad(300))||(abs(Gyro_Vector[2])>=ToRad(300)))
    {
      gyro_sat=1;
      digitalWrite(STATUS_LED,LOW);  
    }
    else
    {
      gyro_sat=0;
      digitalWrite(STATUS_LED,HIGH);  
    } 
  }
   
}
