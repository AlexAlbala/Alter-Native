using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace GroundStation
{
    public class PIDManager
    {
		private static PIDManager instance = null;
		
		public enum Ctrl
		{
			THROTTLE,
			ROLL,
			PITCH,
			YAW
		};
		
		
		private object pidMutex;
        private PID rollPid;
        private PID pitchPid;
        private PID yawPid;
        private PID throttlePid;
		private PID[] pids;
		
		private object chInMutex;
		private byte[] chIn;
		public byte[] ChIn
		{
			get
			{
				lock(this.chInMutex)
				{
					return this.chIn;
				}
			}
		}
		
		private PIDManager()
		{
			this.chInMutex = new object();
			this.pidMutex = new object();
			lock(this.pidMutex)
			{
				this.pids = new PID[4];
				this.chIn = new byte[4];
				for(int i = 0; i < this.pids.Length; i++)
				{
					this.pids[i] = new PID();
				}
			}
			this.SetInfo();
		}
		
		public static PIDManager GetInstance()
		{
			if(instance == null)
				instance = new PIDManager();
			return instance;
		}
		        
        private void SetInfo()
        {
			StreamReader sr = new StreamReader("PidConfig.txt");
			string line = null;
			int ch, offset, minVal, maxVal, meanVal;
			double spanFactor, imuTs = -1, adcTs = -1, kp, ki, kd, refValue;
			while((line = sr.ReadLine()) != null)
			{
				if(line.StartsWith("#") || line == "")
					continue;
				string[] words = line.Split(new char[]{' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
				switch(words[0])
				{
				case "imuTs":
					imuTs = double.Parse(words[1]);	
					break;
				case "adcTs":
					adcTs = double.Parse(words[1]);
					break;
				case "roll":
					kp = double.Parse(words[1]);
					ki = double.Parse(words[2]);
					kd = double.Parse(words[3]);
					ch = int.Parse(words[4]);
					offset = int.Parse(words[5]);
					spanFactor = double.Parse(words[6]);
					minVal = int.Parse(words[7]);
					maxVal = int.Parse(words[8]);
					meanVal = int.Parse(words[9]);
					refValue = double.Parse(words[10]);
					lock(this.pidMutex)
					{
						this.rollPid = new PID(imuTs, kp, ki, kd, offset, spanFactor, minVal, maxVal, meanVal, refValue, false, false);
						this.pids[ch-1] = this.rollPid;
					}
					break;
				case "pitch":
					kp = double.Parse(words[1]);
					ki = double.Parse(words[2]);
					kd = double.Parse(words[3]);
					ch = int.Parse(words[4]);
					offset = int.Parse(words[5]);
					spanFactor = double.Parse(words[6]);
					minVal = int.Parse(words[7]);
					maxVal = int.Parse(words[8]);
					meanVal = int.Parse(words[9]);
					refValue = double.Parse(words[10]);
					lock(this.pidMutex)
					{
						this.pitchPid = new PID(imuTs, kp, ki, kd, offset, spanFactor, minVal, maxVal, meanVal, refValue, false, false);
						this.pids[ch-1] = this.pitchPid;
					}
					break;
				case "yaw":
					kp = double.Parse(words[1]);
					ki = double.Parse(words[2]);
					kd = double.Parse(words[3]);
					ch = int.Parse(words[4]);
					offset = int.Parse(words[5]);
					spanFactor = double.Parse(words[6]);
					minVal = int.Parse(words[7]);
					maxVal = int.Parse(words[8]);
					meanVal = int.Parse(words[9]);
					refValue = double.Parse(words[10]);
					lock(this.pidMutex)
					{
						this.yawPid = new PID(imuTs, kp, ki, kd, offset, spanFactor, minVal, maxVal, meanVal, refValue, true, false);
						this.pids[ch-1] = this.yawPid;
					}
					break;
				case "throttle":
					kp = double.Parse(words[1]);
					ki = double.Parse(words[2]);
					kd = double.Parse(words[3]);
					ch = int.Parse(words[4]);
					offset = int.Parse(words[5]);
					spanFactor = double.Parse(words[6]);
					minVal = int.Parse(words[7]);
					maxVal = int.Parse(words[8]);
					meanVal = int.Parse(words[9]);
					refValue = double.Parse(words[10]);
					lock(this.pidMutex)
					{
						this.throttlePid = new PID(adcTs, kp, ki, kd, offset, spanFactor,minVal,maxVal,meanVal, refValue, false,true);
						this.pids[ch-1] = this.throttlePid;
					}
					break;
				}
			}
            
        }
		
		public int GetChInWidth(int n)
		{
			int ans;
			byte val;
			lock(this.chInMutex)
		    {
				val = this.chIn[n-1];
			}
			lock(this.pidMutex)
		    {
				PID pid = this.pids[n-1];
				ans = (int)Math.Round(val*pid.GetSpan() + pid.GetOffset());
			}
			return ans;
		}

        public void SetChThrottle(AdcMessage adcMessage)
        {
			double vel = adcMessage.tas;
			lock(this.pidMutex)
		    {
				if(this.throttlePid != null)
					this.throttlePid.SetValue(vel);
			}
        }

        public void SetChRoll(ImuEulerMessage eulerMessage)
        {
			double roll = eulerMessage.roll.V; 
			roll = -roll;
			lock(this.pidMutex)
		    {
				if(this.rollPid != null)
        			this.rollPid.SetValue(roll);
			}
        }
  
		public void SetChPitch(ImuEulerMessage eulerMessage)
        {
         	double pitch = eulerMessage.pitch.V;   
			lock(this.pidMutex)
		    {
				if(this.pitchPid != null)
					this.pitchPid.SetValue(pitch);
			}
		}

        public void SetChYaw(ImuEulerMessage eulerMessage)
        {
			double yaw = eulerMessage.yaw.V;
			lock(this.pidMutex)
		    {
				if(this.yawPid != null)
					this.yawPid.SetValue(yaw);
			}
		}

		public byte GetCh(int n)
		{
			byte ans;
			lock(this.pidMutex)
		    {
				PID ch = this.pids[n-1];
				ans = ch.GetValue();
			}
			return ans;
		}
		
		public int GetChWidth(int n)
		{
			int ans;
			lock(this.pidMutex)
		    {
				PID ch = this.pids[n-1];
				ans =(int)Math.Round(ch.GetValue()*ch.GetSpan()+ch.GetOffset());
			}
			return ans;
		}
		
		public void SetCh(int n, byte val)
		{
			lock(this.pidMutex)
		    {
				PID ch = this.pids[n-1];
				ch.SetPwmIn(val);
			}
			lock(this.chInMutex)
			{
				this.chIn[n-1] = val;
			}
		}
		
		public void SetRef(Ctrl ctrl, double val)
		{
			lock(this.pidMutex)
			{
				switch(ctrl)
				{
				case Ctrl.THROTTLE:
					if(this.throttlePid != null)
						this.throttlePid.SetRefValue(val);
					break;
				case Ctrl.ROLL:
					if(this.rollPid != null)
						this.rollPid.SetRefValue(val);
					break;
				case Ctrl.PITCH:
					if(this.pitchPid != null)
						this.pitchPid.SetRefValue(val);
					break;
				case Ctrl.YAW:
					if(this.yawPid != null)
						this.yawPid.SetRefValue(val);
					break;
				}
			}
		}
		
		public int GetMeanValue(Ctrl ctrl)
		{
			int ans = -1;
			lock(this.pidMutex)
			{
				switch(ctrl)
				{
				case Ctrl.THROTTLE:
					ans = this.throttlePid.GetMeanVal();
					break;
				case Ctrl.ROLL:
					ans = this.rollPid.GetMeanVal();
					break;
				case Ctrl.PITCH:
					ans = this.pitchPid.GetMeanVal();
					break;
				case Ctrl.YAW:
					ans = this.yawPid.GetMeanVal();
					break;
				}
			}
			return ans;
		}
		
		public double GetSpanValue(Ctrl ctrl)
		{
			double ans = 0;
			lock(this.pidMutex)
			{
				switch(ctrl)
				{
				case Ctrl.THROTTLE:
					if(this.throttlePid != null)
						ans = this.throttlePid.GetSpan();
					break;
				case Ctrl.ROLL:
					if(this.rollPid != null)
						ans = this.rollPid.GetSpan();
					break;
				case Ctrl.PITCH:
					if(this.pitchPid != null)
						ans = this.pitchPid.GetSpan();
					break;
				case Ctrl.YAW:
					if(this.yawPid != null)
						ans = this.yawPid.GetSpan();
					break;
				}	
			}
			return ans;
		}
		
		public int GetOffsetValue(Ctrl ctrl)
		{
			int ans = 0;
			lock(this.pidMutex)
			{
				switch(ctrl)
				{
				case Ctrl.THROTTLE:
					if(this.throttlePid != null)
						ans = this.throttlePid.GetOffset();
					break;
				case Ctrl.ROLL:
					if(this.rollPid != null)
						ans = this.rollPid.GetOffset();
					break;
				case Ctrl.PITCH:
					if(this.pitchPid != null)
						ans = this.pitchPid.GetOffset();
					break;
				case Ctrl.YAW:
					if(this.yawPid != null)
						ans = this.yawPid.GetOffset();
					break;
				}	
			}
			return ans;
		}
		
		public bool RefreshParam(Ctrl ctrl, PID.Param param, double val)
		{
			bool ok = false;
			lock(this.pidMutex)
			{
				switch(ctrl)
				{
				case Ctrl.THROTTLE:
					if(this.throttlePid != null)
						ok = this.throttlePid.RefreshParam(param, val);
					break;
				case Ctrl.ROLL:
					if(this.rollPid != null)
						ok =  this.rollPid.RefreshParam(param, val);
					break;
				case Ctrl.PITCH:
					if(this.pitchPid != null)
						ok =  this.pitchPid.RefreshParam(param, val);
					break;
				case Ctrl.YAW:
					if(this.yawPid != null)
						ok =  this.yawPid.RefreshParam(param, val);
					break;
				}
			}
			return ok;
		}
		
		public double GetParam(Ctrl ctrl, PID.Param param)
		{
			double ans = double.MaxValue;
			lock(this.pidMutex)
			{
				switch(ctrl)
				{
				case Ctrl.THROTTLE:
					if(this.throttlePid != null)
						ans = this.throttlePid.GetParam(param);
					break;
				case Ctrl.ROLL:
					if(this.rollPid != null)
						ans =  this.rollPid.GetParam(param);
					break;
				case Ctrl.PITCH:
					if(this.pitchPid != null)
						ans =  this.pitchPid.GetParam(param);
					break;
				case Ctrl.YAW:
					if(this.yawPid != null)
						ans =  this.yawPid.GetParam(param);
					break;
				}
			}
			return ans;
		}
		
		public bool Activate(Ctrl ctrl)
		{
			bool ans = false;
			lock(this.pidMutex)
			{
				switch(ctrl)
				{
				case Ctrl.THROTTLE:
					if(this.throttlePid != null)
						ans = this.throttlePid.ReStart();
					break;
				case Ctrl.ROLL:
					if(this.rollPid != null)
						ans = this.rollPid.ReStart();
					break;
				case Ctrl.PITCH:
					if(this.pitchPid != null)
						ans = this.pitchPid.ReStart();
					break;
				case Ctrl.YAW:
					if(this.yawPid != null)
						ans = this.yawPid.ReStart();
					break;
				}
			}
			return ans;
		}
		
		public bool Deactivate(Ctrl ctrl)
		{
			lock(this.pidMutex)
			{
				switch(ctrl)
				{
				case Ctrl.THROTTLE:
					if(this.throttlePid != null)
						this.throttlePid.Deactivate();
					break;
				case Ctrl.ROLL:
					if(this.rollPid != null)
						this.rollPid.Deactivate();
					break;
				case Ctrl.PITCH:
					if(this.pitchPid != null)
						this.pitchPid.Deactivate();
					break;
				case Ctrl.YAW:
					if(this.yawPid != null)
						this.yawPid.Deactivate();
					break;
				}
			}
			return true;
		}
		
		public bool isAct(Ctrl ctrl)
		{
			bool ans = false;
			lock(this.pidMutex)
			{
				switch(ctrl)
				{
				case Ctrl.THROTTLE:
					if(this.throttlePid != null)
						ans = this.throttlePid.Act;
					break;
				case Ctrl.ROLL:
					if(this.rollPid != null)
						ans = this.rollPid.Act;
					break;
				case Ctrl.PITCH:
					if(this.pitchPid != null)
						ans = this.pitchPid.Act;
					break;
				case Ctrl.YAW:
					if(this.yawPid != null)
						ans = this.yawPid.Act;
					break;
				}
			}
			return ans;
		}
    }
}
