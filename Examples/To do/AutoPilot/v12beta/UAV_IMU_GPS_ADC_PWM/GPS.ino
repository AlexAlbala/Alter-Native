#define ToRad(x) (x*0.01745329252)
#define ToDeg(x) (x*57.2957795131)
#define LAMBDA0 0.0523598775598
#define PRC 6399593.626
#define SSE 0.006739497

#define pdopmin 10

#include <math.h>

//Variables per gestionar el DOP
float pdop, hdop, vdop;
//Variables per gestionar coordenades geometriques
long lat0, lon0, difLat, difLon, curLat, curLon;
//Variables per gestionar el mode de funcionament i la longitud del missatge
int lengthMess = 0, heading, gndSpeed;
//Variable auxiliar
char aux2[4];

void GPSSetup()
{
  //Inicialitzem el port serie on esta connectat el GPS a 38400 bauds
  Serial2.begin(38400); //Port GPS
  //Ens guardem l'opcio escollida per l'usuari
 while (true){
   //Llegim un missatge GPS
   String str = ObtainGpsString();
   //Comprovem que el missatge sigui del tipus GPGSA
   if(!str.startsWith("$GPGSA")){
     //En el cas que no ho sigui, tornem al inici del bucle
     continue;
   }
   //ASSERT: El missatge es del tipus GPGSA
   int nComma = 0;
   //Convertim el missatge a char array per processar-lo caracter a caracter
   char aux[lengthMess];
   str.toCharArray(aux, lengthMess);
   //Variable de control d'acces
   boolean isFirst = true;
   //Bucle de processat de missatge GPGSA. Volem extreure el PDOP.
   for(int i = 0; i < lengthMess && nComma < 16; i++)
   {
     //Comptem el nombre de comes. PDOP esta a partir de la coma 15
     if(aux[i] == ','){
       nComma++;  
     }
     //Switch, per si volem processar, a mes del pdop, el hdop o el vdop
     switch(nComma)
     {
       case 15:
       //Comprovem que sigui el primer cop que hem accedit
       if(isFirst)
       {
         //Indique que ja em accedit un cop
         isFirst = false;
         //Bucle de lectura del pdop
         for(int j = 0; j < 4; j++){
           aux2[j] = aux[i+1+j];
         }
         //Conversio ascii to float del pdop
         pdop = atof(aux2);
       }//if(isFirst)
      }//switch(nComma)
   }//for(int i = 0; i < lengthMess && nComma < 16; i++)
   //En el cas en que el pdop llegit sigui mes petit
   // que el llindar establert. Ja podem llegir la posicio
   //de referencia per realitzar el posicionament diferencial
   if(pdop < pdopmin)
   {
     //Bucle de lectura
     while(true)
     {
       //Llegim una cadena GPS
       str = ObtainGpsString();
       //Comprovem que la cadena sigui del tipus GPRMC.
       if(!str.startsWith("$GPRMC")){
         //En cas que no ho sigui, tornem a l'inici del bucle
         continue;
       }
       //Assert: la cadena llegida es del tipus GPRMC
       //Obtenim les coordenades del missatge
       ObtainCoords(str, false);
       //Ja no hem de fer res mes, sortim del bucle
       break;
     }//while(true)
     //Ja no hem de fer res mes, sortim del bucle
     break; 
   }//if(pdop < pdopmin)
 }//while(true)
}//GPSSetup()

//Funcio de loop del gps
void GPSLoop()
{ 
   ProcessDiffGPS();
}


//GPS diferencial amb coordenades geografiques
void ProcessDiffGPS()
{
  //Si hi ha dades esperant...
  if(!Serial2.available())
    return;
  //Obtenim el misatge GPS
  String str = ObtainGpsString();  
  //Si el missatge no es del tipus GPRMC sortim
  if(!str.startsWith("$GPRMC"))
    return;
  //Obtenim les coordenades del missatge
  ObtainCoords(str, true);
  //Enviem els resultats
  byte b[17];
  b[0] = (byte)1;
  b[1] = (byte)1;
  b[2] = (byte)1;
  b[3] = (byte)5;
  b[4] = (byte)SetTime();
  b[5] = (byte)((difLat >> 24) & 0XFF) ;
  b[6] = (byte)((difLat >> 16) & 0XFF) ;
  b[7] = (byte)((difLat >> 8) & 0XFF);
  b[8] = (byte)((difLat & 0XFF));
  b[9] = (byte)((difLon >> 24) & 0XFF) ;
  b[10] = (byte)((difLon >> 16) & 0XFF) ;
  b[11] = (byte)((difLon >> 8) & 0XFF);
  b[12] = (byte)((difLon & 0XFF));
  b[13] = (byte)((gndSpeed >> 8) & 0XFF);
  b[14] = (byte)((gndSpeed & 0XFF));
  b[15] = (byte)((heading >> 8) & 0XFF);
  b[16] = (byte)((heading & 0XFF));
  Serial.write(b,17);
}
byte aux[200];
String ObtainGpsString()
{
  lengthMess = 0;
  //Lectura del missatge GPS
  while(true)
  {
    //Esperem a que hi hagi noves dades per llegir
    while(!Serial2.available()){}
    //Llegim un caracter
    byte b = Serial2.read();
    aux[++lengthMess] = b;
    
    if(b == (byte)10)
      break;
  }
  //Conversio d'array de caracters a string
  String str = String("");
  for(int i = 0;  i <= lengthMess; i++)
  {
    char c = (char)aux[i];
    String strChar = String(c);
    str.concat(strChar);
  }
  return str;
}

//Variables auxiliars
char aux3[8], aux4[8], aux5[5], aux6[6];
//Procedimient d'obtencio de coordenades
void ObtainCoords(String str, boolean _loop)
{
  //Serial.println(str);
  //Comptador de comes
  int nComma = 0;
  //char array del missatge gps per processar-lo caracter a caracter
  char charStr[lengthMess];
  float lat = 0.0, lon = 0.0, head = 0.0, gndSp = 0.0;
  str.toCharArray(charStr, lengthMess);
  //Serial.println(charStr);
  boolean isFirst = true, isFirst2 = true, isFirst3 = true, isFirst4 = true;
  char bigLat[3];
  char bigLon[3];
  for(int i = 0; i < lengthMess && nComma < 9; i++)
  {
    //Comptem les comes
    if(aux[i] == ',')
      nComma++;  
    switch(nComma)
    {
      //En el cas que portem tres comes, llegim la latitud
      case 3:
      if(isFirst)
      {
        
        isFirst = false;
        bigLat[0]=charStr[i];
        bigLat[1] =charStr[i+1];
        bigLat[2]='\0';
        for(int j = 0; j < 8; j++)
          aux3[j] = charStr[i+2+j];  
        lat = atof(aux3);
        for(int j = 0;j < 8; j++)
        {
          if(aux3[j] != '0')
            break;
          lon = lon/10;
        }
      }
      break;
      //En el cas que portem cinc comes, llegim la longitud
     case 5:
      if(isFirst2)
      {
        isFirst2 = false;
        bigLon[0] = charStr[i+1];
        bigLon[1] = charStr[i+2];
        bigLon[2] = '\0';
        for(int j = 0; j < 8; j++)
          aux4[j] = charStr[i+3+j];
        lon = atof(aux4);
        for(int j = 0;j < 8; j++)
        {
          if(aux4[j] != '0')
            break;
          lon = lon/10;
        }
       }
       break;
     case 7:
       if(isFirst3)
       {
         isFirst3 = false;
         for(int j = 0; j < 4; j++)
           aux5[j] = charStr[i+j];
         gndSp = atof(aux5);
         //Serial.print("GNDSPEED: ");Serial.println(gndSp);
       }
       break;
     case 8:
       if(isFirst4)
       {
         isFirst4 = false;
         for(int j = 0; j < 6; j++)
           aux6[j] = charStr[i+j];
         head = atof(aux6);
         //Serial.print("HEADING: ");Serial.println(head);
       }
       break;
    }
  }
  
  //Convertim la latitud i la longitud a long int
  if(_loop)
  {
    curLon = (atoi(bigLon)*10 + lon) * 10000;
    curLat = (atoi(bigLat)*100 + lat) * 10000;
    difLat = curLat;
    difLon = curLon;
    gndSpeed = gndSp * 100;
    heading = head * 100;
    //Serial.print("GNDSPEED: ");Serial.println(gndSpeed);
    //Serial.print("HEADING: ");Serial.println(heading);
  }
  else
  {
    lon0 = (atoi(bigLon)*10 + lon) * 10000;
    lat0 = (atoi(bigLat)*100 + lat) * 10000;
  }
}
