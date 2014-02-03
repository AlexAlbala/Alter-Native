void PWMSetup()
{
  Serial3.begin(19200); //PWM Port
  
}



void PWMLoop(){
  
  if(!Serial3.available()) //Channel with no data
  {
    return;
  }
  
  byte ansPWM[14];
  
  ansPWM[0] = 1; //PWM Header 1114
  ansPWM[1] = 1;
  ansPWM[2] = 1;
  ansPWM[3] = 4;
  
  ansPWM[4] = SetTime();
  

  
  for(int i = 5; i < 14; i++){
    
    while(!Serial3.available()) //Wait for data arrival
    {      
      
    }
    
    byte b = Serial3.read();
    
    if(b = (byte)10) //EOF
    {
      
      break;
      
    }else
    {
      
      ansPWM[i] = b;
      
    }
    
  }
  
  //Output
  Serial.write(ansPWM, 14);
    
  
}
