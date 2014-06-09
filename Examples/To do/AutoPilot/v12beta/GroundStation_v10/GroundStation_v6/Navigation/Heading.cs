using System;

namespace GroundStation
{
	public class Heading : UpperLayer
	{
		private double[] headings;
		private double selHeading;
		private bool cw;
		
		public Heading (double initialRef)
			: base()
		{
			this.headings = new double[8];
			this.headings[0] = 1;
			this.headings[1] = 4;
			this.headings[2] = 6;
			this.headings[3] = 8;
			this.headings[4] = 10;
			this.headings[5] = 12;
			this.headings[6] = 17;
			this.headings[7] = 20;
			
			this.selHeading = initialRef;
			
		}
		
		public override void SetParam (double upperParam)
		{
			this.selHeading = upperParam;
		}
		
		//
		public override void GetRef ()
		{
			if(!this.activated)
				return;
			double currHeading = this.ga.Imu.yaw.V;
			double diffHeading = (double)((this.selHeading - currHeading)%180);
			double vStall = this.ap.stallTas;
			double currTas = this.ga.Adc.tas;
			double currRoll = this.ga.Imu.roll.V;
			
			if(diffHeading < 0)
				this.cw = false;
			else
				this.cw = true;
			if(currTas < vStall + 5)
			{
				double ans;
				if (currRoll > 1)
					ans = currRoll -1;
				else if (currRoll < 1)
					ans = currRoll + 1;
				else
					ans = 0;
				Console.WriteLine("WARNING: Heading change not set due to low airspeed --> roll reference set to " + ans + " degrees");
				this.pid.SetRef(PIDManager.Ctrl.ROLL, ans);
			}
			
			int headLevel = (int)Math.Round(Math.Abs(diffHeading/10));
			if(headLevel > 7)
				headLevel = 7;
			
			if(this.cw)
				this.pid.SetRef(PIDManager.Ctrl.ROLL, -this.headings[headLevel]);
			else
				this.pid.SetRef(PIDManager.Ctrl.ROLL, this.headings[headLevel]);
		}
	}
}

