using System;

namespace GroundStation
{
	public abstract class UpperLayer
	{
		protected PIDManager pid;
		protected GlobalArea ga;
		protected AircraftPerformance ap;
		protected bool activated;
		
		public UpperLayer ()
		{
			this.pid = PIDManager.GetInstance();
			this.ga = GlobalArea.GetInstance();
			this.ap = AircraftPerformance.GetInstance();
			this.activated = false;
		}
		
		public abstract void SetParam(double upperParam);
		public abstract void GetRef();
		
		public void activate()
		{
			this.activated = true;
		}
		
		public void deactivate()
		{
			this.activated = false;
		}
	}
}

