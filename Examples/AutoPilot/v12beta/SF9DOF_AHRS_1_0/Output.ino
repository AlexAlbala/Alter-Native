
void printdata(void)
{    
      unsigned long ulRoll = convert_to_dec(ToDeg(roll)+180.0);
      unsigned long ulPitch = convert_to_dec(ToDeg(pitch)+180.0);
      unsigned long ulYaw = convert_to_dec(ToDeg(yaw)+180.0);
      byte euler[24];
      euler[0] = (byte)((ulRoll >> 24) & 0XFF) ;
      euler[1] = (byte)((ulRoll >> 16) & 0XFF) ;
      euler[2] = (byte)((ulRoll >> 8) & 0XFF);
      euler[3] = (byte)((ulRoll & 0XFF));
      
      euler[4] = (byte)((ulPitch >> 24) & 0XFF) ;
      euler[5] = (byte)((ulPitch >> 16) & 0XFF) ;
      euler[6] = (byte)((ulPitch >> 8) & 0XFF);
      euler[7] = (byte)((ulPitch & 0XFF));
      
      euler[8] = (byte)((ulYaw >> 24) & 0XFF) ;
      euler[9] = (byte)((ulYaw >> 16) & 0XFF) ;
      euler[10] = (byte)((ulYaw >> 8) & 0XFF);
      euler[11] = (byte)((ulYaw & 0XFF));
      
      unsigned long ulAccelX = convert_to_dec_unsig(accel_x);
      unsigned long ulAccelY = convert_to_dec_unsig(accel_y);
      unsigned long ulAccelZ = convert_to_dec_unsig(accel_z);
      
      euler[12] = (byte)((ulAccelX >> 24) & 0XFF) ;
      euler[13] = (byte)((ulAccelX >> 16) & 0XFF) ;
      euler[14] = (byte)((ulAccelX >> 8) & 0XFF);
      euler[15] = (byte)((ulAccelX & 0XFF));
      
      euler[16] = (byte)((ulAccelY >> 24) & 0XFF) ;
      euler[17] = (byte)((ulAccelY >> 16) & 0XFF) ;
      euler[18] = (byte)((ulAccelY >> 8) & 0XFF);
      euler[19] = (byte)((ulAccelY & 0XFF));
      
      euler[20] = (byte)((ulAccelZ >> 24) & 0XFF) ;
      euler[21] = (byte)((ulAccelZ >> 16) & 0XFF) ;
      euler[22] = (byte)((ulAccelZ >> 8) & 0XFF);
      euler[23] = (byte)((ulAccelZ & 0XFF));
      Serial.write(euler, 24);
}

long convert_to_dec(float x)
{
  return x*10000;
}

unsigned long convert_to_dec_unsig(float x)
{
  return x*10000;
}

