using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroundStation
{
    public class PID
    {
		public enum Param
		{
			TS,
			KP,
			KI,
			KD,
			INITIAL_VAL,
			MEAN_VAL
		}
		
		private bool act;
		public bool Act
		{
			get
			{
				return this.act;
			}
		}
		
        private double ts;
		
		private bool isYaw = false;
		private bool isThrottle = false;
		
		private bool filled;
        private double kp;
		public double Kp
		{
			get
			{
				return this.kp;
			}
		}
		
        private double ki;
		public double Ki
		{
			get
			{
				return this.ki;
			}
		}
        private double kd;
		public double Kd
		{
			get
			{
				return this.kd;
			}
		}
		
        private double acc;
        private double prev;
		
		private byte pwmIn;
		private byte currValue;
		private double refValue;
		
		private int offset;
		private double spanFactor;
		
		private int minVal;
		private int maxVal;
		private int meanVal;
		public int MeanVal
		{
			get
			{
				return this.meanVal;
			}
		}
		
		public object mutex;
		
		private double initialVal;
		public double InitialVal
		{
			get
			{
				return this.meanVal;
			}
		}
		
		public PID()
		{
			this.act = false;
			this.filled = false;
			this.mutex = new object();
		}
		
        public PID(double ts, double kp, double ki, double kd, int offset, double spanFactor, int minVal, int maxVal, int meanVal, double refValue, bool isYaw, bool isThrottle)
        {
			this.act = true;
			this.filled = true;
			
			this.mutex = new object();
			
            this.ts = ts;
            
            this.kp = kp;
            this.ki = ki;
            this.kd = kd;
			
			this.isYaw = isYaw;
			this.isThrottle = isThrottle;
			
			this.offset = offset;
			this.spanFactor = spanFactor;
            
			this.acc = 0;
            this.prev = 0;
			
			this.minVal = minVal;
			this.maxVal = maxVal;
			this.meanVal = meanVal;
			
			this.refValue = 0;
			this.initialVal = refValue;
        }

        public void SetValue(double input)
        {
			lock(this.mutex)
			{
				//Console.WriteLine("Input: " + input);
				int sign = Math.Sign(input);
				input = (input - this.refValue - this.initialVal);
				//Console.WriteLine("Input: " + input);
				if(isYaw)
				{
					if(input > 180)
					{
						input = (360 - (input));
						if(sign > 0)
							input = -input;
					}
					else if(input < -180)
						input = (360 + (input));
				}
				if(this.isThrottle)
				{
					input = -input;
				}
				//Console.WriteLine("RefValue: " + refValue);
				//Console.WriteLine("Output: " + input);
	            this.acc += input;
	            double diff = input - prev;
	            this.prev = input;
	
	            double ans = this.kp*input + this.ki * ts * this.acc + this.kd * diff;
				
				ans += this.meanVal;
				ans = ans < this.minVal ? ans = this.minVal : ans;
				ans = ans > this.maxVal ? ans = this.maxVal : ans;
				//Console.WriteLine("ans: " + ans);
				//Console.WriteLine();
				this.currValue = (byte)Math.Round((ans - this.offset)/this.spanFactor);
				//Console.WriteLine("CurrValue: " + this.currValue);
			}
        }
		
		public byte GetValue()
		{
			Console.WriteLine("ACT: " + act.ToString());
			if(this.act)
			{
				Console.WriteLine("GetCurrVal: " + this.currValue);
				byte ans;
				lock(this.mutex)
				{
					ans = this.currValue;
				}
				
				return ans;
			}
			Console.WriteLine("GetPwmIn: " + this.pwmIn);
			return this.pwmIn;
		
		}
		
		public void SetRefValue(double refValue)
		{
			if(refValue == double.MaxValue)
				return;
			lock(this.mutex)
			{
				this.refValue = refValue;
			}
		}
		
		public void SetPwmIn(byte pwmIn)
		{
			lock(this.mutex)
			{
				this.pwmIn = pwmIn;
			}
		}
		
		public void Deactivate()
		{
			this.act = false;
		}
		
		public void Activate(double ts, double kp, double ki, double kd, int offset, double spanFactor, int minVal, int maxVal, int meanVal, double refValue)
		{
			this.ts = ts;
			this.kp = kp;
			this.kd = kd;
			this.offset = offset;
			this.spanFactor = spanFactor;
			this.minVal = minVal;
			this.maxVal = maxVal;
			this.meanVal = meanVal;
			this.filled = true;
			this.act = true;
			this.refValue = 0;
			this.initialVal = refValue;
			
			this.acc = 0;
			this.prev = 0;
			
		}
		
		public bool RefreshParam(Param p, double val)
		{
			bool ok;
			if(!this.filled)
				return false;
			switch(p)
			{
			case Param.INITIAL_VAL:
				lock(this.mutex)
				{
					this.initialVal = val;
					this.refValue = 0;
					ok = this.ReStart();
					Console.WriteLine ("OK: " + ok.ToString());
				}
				return ok;
			case Param.TS:
				lock(this.mutex)
				{
					this.ts = val;
					ok = this.ReStart();
				}
				return ok;
			case Param.KP:
				lock(this.mutex)
				{
					this.kp = val;
					ok = this.ReStart();
				}
				return ok;
			case Param.KI:
				lock(this.mutex)
				{
					this.ki = val;
					ok = this.ReStart();
				}
				return ok;
			case Param.KD:
				lock(this.mutex)
				{
					this.kd = val;
					ok = this.ReStart();
				}
				return ok;
			case Param.MEAN_VAL:
				lock(this.mutex)
				{
					this.meanVal = (int)val;
					ok = this.ReStart();
					Console.WriteLine ("OK: " + ok.ToString());
				}
				return ok;
			default:
				return false;
			}
		}
		
		public double GetParam(Param p)
		{
			if(!this.filled)
				return double.MinValue;
			double ans = double.MaxValue;
			switch(p)
			{
			case Param.INITIAL_VAL:
				lock(this.mutex)
				{
					ans = this.initialVal;
				}
				break;
			case Param.TS:
				lock(this.mutex)
				{
					ans = this.ts;
				}
				break;
			case Param.KP:
				lock(this.mutex)
				{
					ans = this.kp;
				}
				break;
			case Param.KI:
				lock(this.mutex)
				{
					ans = this.ki;
				}
				break;
			case Param.KD:
				lock(this.mutex)
				{
					ans = this.kd;
				}
				break;
			case Param.MEAN_VAL:
				lock(this.mutex)
				{
					ans = this.meanVal;
				}
				break;
			default:
				return double.MinValue;
			}
			return ans;
		}
		
		public bool ReStart()
		{
			if(!this.filled)
				return false;
			
			this.act = true;
			this.acc = 0;
			this.prev = 0;
			return true;
		}
		
		public int GetMeanVal()
		{
			return this.meanVal;
		}

		public int GetOffset()
		{
			return this.offset;
		}
		
		public double GetSpan()
		{
			return this.spanFactor;
		}
			
    }
}
