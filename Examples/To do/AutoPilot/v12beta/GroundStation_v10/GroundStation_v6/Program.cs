using System;
using System.Collections.Generic;
using System.Text;
using GroundStation;
using System.IO;
using System.Threading;

namespace GroundStation
{
     public class Main
     {
		private static readonly string path = "/dev/";
		private static readonly string pwmIn = "ttyUSB1";
		private static readonly string pwmOut = "ttyUSB2";
		private static readonly string telIn = "ttyUSB0";
		private static readonly int b19200 = 19200;
		private static readonly int b57600 = 57600;
		private static ulong time = 0;
		private static Output op = new Output();
        public static void Run()
        {
			ThreadStart thsTel = new ThreadStart (RunTelemetry);
			Thread tTel = new Thread(thsTel);
			tTel.Start();
			GC.KeepAlive(tTel);
			
			ThreadStart thsPwm = new ThreadStart(RunPwm);
			Thread tPwm = new Thread(thsPwm);
			tPwm.Start();
			GC.KeepAlive(tPwm);
	    }
		
		private static void RunTelemetry()
		{
            GlobalArea ga = GlobalArea.GetInstance();
            Database db = Database.GetInstance();
			PIDManager pid = PIDManager.GetInstance();
			NavManager nav = NavManager.GetInstance();
			AdcMessage adc = new AdcMessage();
			ImuEulerMessage imu = new ImuEulerMessage();
			PwmMessage pwm = new PwmMessage();
			GpsMessage gps = new GpsMessage();
			nav.Initialize(0, 0, 0);
            
            Input p;
			p = new SerialInput(path + telIn, b19200);
			
            int i = 0;
            while (true)
            {
                p.CheckStartOfMessage();
				//Console.WriteLine("Message Received");
                byte[] header = p.ReadNBytes(1);
                byte[] m;
                switch (header[0])
                {
                    case (byte)0: //IMU-Euler Angles
                        m = p.ReadNBytes(25);
                        time += m[0];
                        imu.CreateMessage(time, m);
                        ga.Imu = imu;
                        db.Add(ga.Imu);
						pid.SetChPitch(ga.Imu);
						pid.SetChRoll(ga.Imu);
						pid.SetChYaw(ga.Imu);
						if(ga.IsReady())
						{
							nav.UpdateAltRef();
							nav.UpdateHeadRef();
						}
                        break;
                    case (byte)3: //Adc
                        m = p.ReadNBytes(7);
                        time += m[0];
                        adc.CreateMessage(time, m);
                        ga.Adc = adc;
                        db.Add(ga.Adc);
						pid.SetChThrottle(adc);
						if(ga.IsReady())
						{
							nav.UpdateAltRef();
							nav.UpdateHeadRef();
						}
                        break;
                    case (byte)4: //Pwm
                        m = p.ReadNBytes(9);
                        time += m[0];
                        pwm.CreateMessage(time, m);
                        ga.Pwm = pwm;
                        db.Add(ga.Pwm);
                        break;
                    case (byte)5: //Gps
                        m = p.ReadNBytes(13);
                        time += m[0];
                        gps.CreateMessage(time, m);
                        ga.Gps = gps;
                        db.Add(ga.Gps);
                        break;
                }
                if (i == 10)
                {
                    op.Flush(db);
                    //op.Close();
                    //op = new Output();
                    i = 0;
                }
                i++;
            }
        } 
    	
		private static void RunPwm()
		{
			StreamWriter swCal = new StreamWriter("Logs/Calib");
			SerialInput si = new SerialInput(path + pwmIn, b57600);
			SerialInput so = new SerialInput(path + pwmOut, b57600);
			Thread.Sleep(1000);
			si.WriteNBytes(new byte[]{1,1,1});
			si.CheckStartOfMessage();
			so.WriteNBytes(new byte[]{1});
			
			byte[] b = new byte[4];
			byte[] ans = new byte[4];
			
			PIDManager pid = PIDManager.GetInstance();
			NavManager nav = NavManager.GetInstance();
			GlobalArea ga = GlobalArea.GetInstance();
			
			while(true)
			{
				switch(nav.GetCurrentMode())
				{
				case NavManager.Mode.MANUAL:
					b = si.ReadNBytes(4);
					so.WriteNBytes(b);
					Console.WriteLine("entrada: " + b[0] + " " + b[1] + " " + b[2] + " " + b[3]);
					int[] aux = new int[4];
					aux[0] = (int)Math.Round(b[0] * pid.GetSpanValue(PIDManager.Ctrl.THROTTLE) + pid.GetOffsetValue(PIDManager.Ctrl.THROTTLE));
					aux[1] = (int)Math.Round(b[1] * pid.GetSpanValue(PIDManager.Ctrl.ROLL) + pid.GetOffsetValue(PIDManager.Ctrl.ROLL));
					aux[2] = (int)Math.Round(b[2] * pid.GetSpanValue(PIDManager.Ctrl.PITCH) + pid.GetOffsetValue(PIDManager.Ctrl.PITCH));
					aux[3] = (int)Math.Round(b[3] * pid.GetSpanValue(PIDManager.Ctrl.YAW) + pid.GetOffsetValue(PIDManager.Ctrl.YAW));
						
					Console.WriteLine("entrada: " +  aux[0] + " " + aux[1] + " " + aux[2] + " " + aux[3]); 
					pid.SetCh(1, b[0]);
					pid.SetCh(2, b[1]);
					pid.SetCh(3, b[2]);
					pid.SetCh(4, b[3]);
					op.WritePwm(time, b);
					op.WritePwm(time, b);
					break;
				case NavManager.Mode.CALIBRATION_THROTTLE:
					swCal.WriteLine(time + " " + "throt");
					b = si.ReadNBytes(4);
					Console.WriteLine("entrada:" + b[0] + " " + b[1] + " " + b[2] + " " + b[3]);
					pid.SetCh(1, b[0]);
					pid.SetCh(2, b[1]);
					pid.SetCh(3, b[2]);
					pid.SetCh(4, b[3]);
					ans[0] = ThrottleCalib(pid, ga.Adc);
					ans[1] = b[1];
					ans[2] = b[2];
					ans[3] = b[3];
					so.WriteNBytes(ans);
					Console.WriteLine("sortida:" + ans[0] + " " +  ans[1] + " " + ans[2] + " " + ans[3]);
					op.WritePwm(time, b);
					op.WritePwm(time, ans);
					break;
				case NavManager.Mode.CALIBRATION_ROLL:
					swCal.WriteLine(time + " " + "roll");
					b = si.ReadNBytes(4);
					Console.WriteLine("entrada:" + b[0] + " " + b[1] + " " + b[2] + " " + b[3]);
					pid.SetCh(1, b[0]);
					pid.SetCh(2, b[1]);
					pid.SetCh(3, b[2]);
					pid.SetCh(4, b[3]);
					ans[0] = b[0];
					ans[1] = RollCalib(pid, ga.Imu);
					ans[2] = b[2];
					ans[3] = b[3];
					so.WriteNBytes(ans);
					Console.WriteLine("sortida:" + ans[0] + " " +  ans[1] + " " + ans[2] + " " + ans[3]);
					op.WritePwm(time, b);
					op.WritePwm(time, ans);
					break;
				case NavManager.Mode.CALIBRATION_PITCH:
					swCal.WriteLine(time + " " + "pitch");
					b = si.ReadNBytes(4);
					Console.WriteLine("entrada:" + b[0] + " " + b[1] + " " + b[2] + " " + b[3]);
					pid.SetCh(1, b[0]);
					pid.SetCh(2, b[1]);
					pid.SetCh(3, b[2]);
					pid.SetCh(4, b[3]);
					ans[0] = b[0];
					ans[1] = b[1];
					ans[2] = PitchCalib(pid, ga.Imu);
					ans[3] = b[3];
					so.WriteNBytes(ans);
					Console.WriteLine("sortida:" + ans[0] + " " +  ans[1] + " " + ans[2] + " " + ans[3]);
					op.WritePwm(time, b);
					op.WritePwm(time, ans);
					break;
				case NavManager.Mode.CALIBRATION_YAW:
					swCal.WriteLine(time + " " + "yaw");
					b = si.ReadNBytes(4);
					Console.WriteLine("entrada:" + b[0] + " " + b[1] + " " + b[2] + " " + b[3]);
					pid.SetCh(1, b[0]);
					pid.SetCh(2, b[1]);
					pid.SetCh(3, b[2]);
					pid.SetCh(4, b[3]);
					ans[0] = b[0];
					ans[1] = b[1];
					ans[2] = b[2];
					ans[3] = YawCalib(pid, ga.Imu);
					Console.WriteLine("sortida:" + ans[0] + " " +  ans[1] + " " + ans[2] + " " + ans[3]);
					op.WritePwm(time, b);
					op.WritePwm(time, ans);
					so.WriteNBytes(ans);
					break;
				default:
					b = si.ReadNBytes(4);
					Console.WriteLine("entrada:" + b[0] + " " + b[1] + " " + b[2] + " " + b[3]);
					pid.SetCh(1, b[0]);
					pid.SetCh(2, b[1]);
					pid.SetCh(3, b[2]);
					pid.SetCh(4, b[3]);
					Console.WriteLine("sortida:" + ans[0] + " " +  ans[1] + " " + ans[2] + " " + ans[3]);
					Console.WriteLine();
					ans[0] = pid.GetCh(1);
					ans[1] = pid.GetCh(2);
					ans[2] = pid.GetCh(3);
					ans[3] = pid.GetCh(4);
					so.WriteNBytes(ans);
					op.WritePwm(time, b);
					op.WritePwm(time, ans);
					break;
				}
			}
		}
		
		private static bool stateThrottle = true;
		private static int calValueThrottle = 180; //20% span
		private static byte ThrottleCalib(PIDManager pid, AdcMessage m)
		{
			byte ans;
			if(m.tas > 12)
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.THROTTLE);
				double span = pid.GetSpanValue(PIDManager.Ctrl.THROTTLE);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.THROTTLE);
				ans = (byte)(((meanVal - calValueThrottle)-offset)/span);
				stateThrottle = false;
			}
			else if(m.tas < 8)
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.THROTTLE);
				double span = pid.GetSpanValue(PIDManager.Ctrl.THROTTLE);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.THROTTLE);
				ans = (byte)(((meanVal + calValueThrottle)-offset)/span);
				stateThrottle = true;
			}
			else if(!stateThrottle)
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.THROTTLE);
				double span = pid.GetSpanValue(PIDManager.Ctrl.THROTTLE);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.THROTTLE);
				ans = (byte)(((meanVal - calValueThrottle)-offset)/span);
			}
			else
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.THROTTLE);
				double span = pid.GetSpanValue(PIDManager.Ctrl.THROTTLE);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.THROTTLE);
				ans = (byte)(((meanVal + calValueThrottle)-offset)/span);
			}
			return ans;
		}
		
		private static bool stateRoll = true;
		private static int calValueRoll = 200; //20% span
		private static byte RollCalib(PIDManager pid, ImuEulerMessage m)
		{
			byte ans;
			if(m.roll.V > 2)
			{	
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.ROLL);
				double span = pid.GetSpanValue(PIDManager.Ctrl.ROLL);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.ROLL);
				Console.WriteLine("MeanValue: " + meanVal);
				Console.WriteLine("Span: " + span);
				Console.WriteLine("Offset: " + offset);
				ans = (byte)(((meanVal - calValueRoll)-offset)/span);
				stateRoll = false;
			}
			else if(m.roll.V < -2)
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.ROLL);
				double span = pid.GetSpanValue(PIDManager.Ctrl.ROLL);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.ROLL);
				Console.WriteLine("MeanValue: " + meanVal);
				Console.WriteLine("Span: " + span);
				Console.WriteLine("Offset: " + offset);
				ans = (byte)(((meanVal + calValueRoll)-offset)/span);
				stateRoll = true;
			}
			else if(!stateRoll)
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.ROLL);
				double span = pid.GetSpanValue(PIDManager.Ctrl.ROLL);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.ROLL);
				Console.WriteLine("MeanValue: " + meanVal);
				Console.WriteLine("Span: " + span);
				Console.WriteLine("Offset: " + offset);
				ans = (byte)(((meanVal - calValueRoll)-offset)/span);
			}
			else
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.ROLL);
				double span = pid.GetSpanValue(PIDManager.Ctrl.ROLL);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.ROLL);
				Console.WriteLine("MeanValue: " + meanVal);
				Console.WriteLine("Span: " + span);
				Console.WriteLine("Offset: " + offset);
				ans = (byte)(((meanVal + calValueRoll)-offset)/span);
			}
			return ans;
		}
		
		private static int calValuePitch = 200; //20% span
		private static bool statePitch = true;
		private static byte PitchCalib(PIDManager pid, ImuEulerMessage m)
		{
			byte ans;
			if(m.pitch.V > 2)
			{
				
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.PITCH);
				double span = pid.GetSpanValue(PIDManager.Ctrl.PITCH);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.PITCH);
				Console.WriteLine("MeanValue: " + meanVal);
				Console.WriteLine("Span: " + span);
				Console.WriteLine("Offset: " + offset);
				ans = (byte)(((meanVal + calValuePitch)-offset)/span);
				statePitch = false;
			}
			else if(m.pitch.V < -2)
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.PITCH);
				double span = pid.GetSpanValue(PIDManager.Ctrl.PITCH);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.PITCH);
				Console.WriteLine("MeanValue: " + meanVal);
				Console.WriteLine("Span: " + span);
				Console.WriteLine("Offset: " + offset);
				ans = (byte)(((meanVal - calValuePitch)-offset)/span);
				statePitch = true;
			}
			else if(!statePitch)
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.PITCH);
				double span = pid.GetSpanValue(PIDManager.Ctrl.PITCH);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.PITCH);
				Console.WriteLine("MeanValue: " + meanVal);
				Console.WriteLine("Span: " + span);
				Console.WriteLine("Offset: " + offset);
				ans = (byte)(((meanVal + calValuePitch)-offset)/span);
			}
			else
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.PITCH);
				double span = pid.GetSpanValue(PIDManager.Ctrl.PITCH);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.PITCH);
				Console.WriteLine("MeanValue: " + meanVal);
				Console.WriteLine("Span: " + span);
				Console.WriteLine("Offset: " + offset);
				ans = (byte)(((meanVal - calValuePitch)-offset)/span);
			}
			return ans;
		}
		
		private static int calValueYaw = 210; //20% span
		private static bool stateYaw = true;
		private static byte YawCalib(PIDManager pid, ImuEulerMessage m)
		{
			byte ans;
			Console.WriteLine("INITIAL VAL: " + pid.GetParam(PIDManager.Ctrl.YAW, PID.Param.INITIAL_VAL));
			if((m.yaw.V - pid.GetParam(PIDManager.Ctrl.YAW, PID.Param.INITIAL_VAL)) > 2)
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.YAW);
				double span = pid.GetSpanValue(PIDManager.Ctrl.YAW);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.YAW);
				ans = (byte)(((meanVal - calValueYaw)-offset)/span);
				stateYaw = true;
			}
			else if(m.yaw.V - pid.GetParam(PIDManager.Ctrl.YAW, PID.Param.INITIAL_VAL) < -2)
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.YAW);
				double span = pid.GetSpanValue(PIDManager.Ctrl.YAW);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.YAW);
				ans = (byte)(((meanVal + calValueYaw)-offset)/span);
				stateYaw = false;
			}
			else if(!stateYaw)
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.YAW);
				double span = pid.GetSpanValue(PIDManager.Ctrl.YAW);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.YAW);
				ans = (byte)(((meanVal - calValueYaw)-offset)/span);
			}
			else
			{
				int meanVal = pid.GetMeanValue(PIDManager.Ctrl.YAW);
				double span = pid.GetSpanValue(PIDManager.Ctrl.YAW);
				int offset = pid.GetOffsetValue(PIDManager.Ctrl.YAW);
				ans = (byte)(((meanVal - calValueYaw)-offset)/span);
			}
			return ans;
		}
	}
}
