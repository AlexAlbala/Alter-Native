//Define A/C Converter ADS8344 Ports
#define ADC_CLK  34
#define ADC_CS   36
#define ADC_DIN  38
#define ADC_DOUT 40

//Define ADS8344 Lecture Channels
const int channel0 = 0X8F; //Channel 0 Barometer
const int channel1 = 0XCF; //Channel 1 Thermometer
const int channel2 = 0X9F; //Channel 2 Pitot

unsigned int measADC = 0;


void ADCSetup(){
  
  //ADS8344 Pin Mode
  pinMode(ADC_CLK, OUTPUT);
  pinMode(ADC_CS, OUTPUT);
  pinMode(ADC_DIN, OUTPUT);
  pinMode(ADC_DOUT, INPUT);
  
}


void ADCLoop(){
  
  byte ansADC[11];
  
  ansADC[0] = (byte)1; //ADC Header 1113
  ansADC[1] = (byte)1;
  ansADC[2] = (byte)1;
  ansADC[3] = (byte)3;
  
  ansADC[4] = SetTime(); //Timming
  
  //ADC Channel 0 Lecture (Barometer)
  measADC = ReadADC(channel0);
  ansADC[5] = (byte)((measADC >> 8) & 0XFF);
  ansADC[6] = (byte)(measADC & 0XFF);
  //ADC Channel 1 Lecture (Thermometer)
  measADC = ReadADC(channel1);
  ansADC[7] = (byte)((measADC >> 8) & 0XFF);
  ansADC[8] = (byte)(measADC & 0XFF);
  //ADC Channel 2 Lecture (Pitot)
  measADC = ReadADC(channel2); 
  ansADC[9] = (byte)((measADC >> 8) & 0XFF);
  ansADC[10] = (byte)(measADC & 0XFF);
  
  //Output
  Serial.write(ansADC, 11);
  
  
}


unsigned int ReadADC(int channel)
{
  
  int d1, d2;
  unsigned int dd;
  
  digitalWrite(ADC_CS, LOW);
  
  shiftOut(ADC_DIN, ADC_CLK, MSBFIRST, channel);
  
  delayMicroseconds(20);
  
  //CLK Pulse Generation
  digitalWrite(ADC_CLK, HIGH);
  digitalWrite(ADC_CLK, LOW);
  
  //A/C Converter Lecture
  d1 = shiftIn(ADC_DOUT, ADC_CLK, MSBFIRST);
  d2 = shiftIn(ADC_DOUT, ADC_CLK, MSBFIRST);
  
  //Rearrange the measurement result
  dd = 256 * d1 + d2;
  
  digitalWrite(ADC_CS, HIGH);
  
  return(dd);
  
}
