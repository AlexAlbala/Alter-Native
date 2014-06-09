using System;
using System.Collections.Generic;
using System.Text;
using GroundStation;
using System.Threading;

namespace GroundStation
{
    public class Program
    {
        static void Main(string[] args)
        {
            Input p;
            Database db = Database.GetInstance();
            Output op = new Output("Logs");
            //if (Settings.Default.Mode == 0)
            p = new SerialInput("COM19");
            // else if (Settings.Default.Mode == 1)
            //p = new FileInput("LOG06.TXT");
            //else
            //    return;

            ulong time = 0;
            int i = 0;
            while (true)
            {
                p.CheckStartOfMessage();
                byte[] header = p.ReadNBytes(1);
                byte[] m;
                switch (header[0])
                {
                    case (byte)0: //IMU-Euler Angles
                        m = p.ReadNBytes(7);
                        time += m[0];
                        ImuEulerMessage imuEuler = new ImuEulerMessage(time, m);
                        db.Add(imuEuler);
                        break;
                    case (byte)2://Imu-Raw data
                        m = p.ReadNBytes(19);
                        time += m[0];
                        ImuRawMessage imuRaw = new ImuRawMessage(time, m);
                        db.Add(imuRaw);
                        break;
                    case (byte)3: //Adc
                        m = p.ReadNBytes(7);
                        time += m[0];
                        AdcMessage adc = new AdcMessage(time, m);
                        db.Add(adc);
                        break;
                    case (byte)4: //Pwm
                        m = p.ReadNBytes(11);
                        time += m[0];
                        PwmMessage pwm = new PwmMessage(time, m);
                        db.Add(pwm);
                        break;
                    case (byte)5: //Gps
                        m = p.ReadNBytes(13);
                        time += m[0];
                        GpsMessage gps = new GpsMessage(time, m);
                        db.Add(gps);
                        break;
                }
                if (i == 10)
                {
                    op.Flush(db);
                    op.Close();
                    op = new Output("Logs");
                    i = 0;
                }
                i++;
            }
        }


    }
}
