void IMUSetup()
{
  Serial1.begin(19200); //IMU Port
}


void IMULoop(){
  if(!Serial1.available()) return;  
  
  byte ansIMU[29];
  
  ansIMU[0] = 1; //IMU Header 1110
  ansIMU[1] = 1;
  ansIMU[2] = 1;
  ansIMU[3] = 0;
  ansIMU[4] = SetTime();
  
  for(int i = 5; i < 29; i++)
  {
    while(!Serial1.available()){} //Wait for data arrival
    byte b = Serial1.read(); 
    ansIMU[i] = b;
  }
  
  //Output
  Serial.write(ansIMU, 29);
}
