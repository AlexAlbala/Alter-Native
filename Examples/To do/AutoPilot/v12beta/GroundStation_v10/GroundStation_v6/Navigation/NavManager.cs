using System;
using System.Threading;

namespace GroundStation
{
	public class NavManager
	{
		private static NavManager instance = null;
		
		public static NavManager GetInstance()
		{
			if(instance == null)
				instance = new NavManager();
			return instance;
		}
		
		public enum Mode
		{
			MANUAL,
			DIRECTED,
			AUTONOMOUS,
			CALIBRATION_THROTTLE,
			CALIBRATION_ROLL,
			CALIBRATION_PITCH,
			CALIBRATION_YAW
		};
		
		private Mode currMode;
		private UpperLayer alt;
		private UpperLayer head;
		private UpperLayer speed;
		public ConsoleInput ci;
		private PIDManager pid;
		
		private bool isFirst;
		
		public void Initialize (double altRef, double headRef, double speedRef)
		{
			this.pid = PIDManager.GetInstance();
			this.alt = new Altitude(altRef);
			this.head = new Heading(headRef);
			this.speed = new Speed(speedRef);
			
			if(this.isFirst == true)
			{
				this.isFirst = false;
				this.ci = new ConsoleInput(this);
				ThreadStart thsCi = new ThreadStart(this.ci.Run);
				Thread th = new Thread(thsCi);
				th.Start();
			}
		}
		
		private NavManager()
		{
			this.isFirst = true;
		}
		
		public void Switch(Mode m)
		{
			this.currMode = m;
			switch(this.currMode)
			{
			case Mode.AUTONOMOUS:
				this.alt.activate();
				this.head.activate();
				this.speed.activate();
				break;
			case Mode.DIRECTED:
			case Mode.MANUAL:
			case Mode.CALIBRATION_THROTTLE:
			case Mode.CALIBRATION_ROLL:
			case Mode.CALIBRATION_PITCH:
			case Mode.CALIBRATION_YAW:
				this.alt.deactivate();
				this.head.deactivate();
				this.speed.deactivate();
				break;
			}
		}
		
		public Mode GetCurrentMode()
		{
			return this.currMode;
		}
			
		public void SetAltitude(double altRef)
		{
			if(this.alt != null)
				this.alt.SetParam(altRef);
			else
				this.alt = new Altitude(altRef);
		}
		
		public void SetHeading(double headRef)
		{
			if(this.head != null)
				this.head.SetParam(headRef);
			else
				this.head = new Altitude(headRef);
		}
		
		public void SetSpeed(double speedRef)
		{
			if(this.speed != null)
				this.speed.SetParam(speedRef);
			else
				this.speed = new Speed(speedRef);
		}
		
		public void UpdateAltRef()
		{
			 this.alt.GetRef();
		}
		
		public void UpdateHeadRef()
		{
			this.head.GetRef();
		}
		
		public void UpdateSpeedRef()
		{
			this.speed.GetRef();
		}
		
		//TODO: No em queda clar com fer el throttle...
		public void UpdateThrottleRef(double t)
		{
			if(this.currMode == Mode.DIRECTED)
				this.pid.SetRef(PIDManager.Ctrl.THROTTLE, t);	
		}
		
		public void UpdateRollRef(double r)
		{
			if(this.currMode == Mode.DIRECTED)
				this.pid.SetRef(PIDManager.Ctrl.ROLL, r);
		}
		
		public void UpdatePitchRef(double p)
		{
			if(this.currMode == Mode.DIRECTED)
				this.pid.SetRef(PIDManager.Ctrl.PITCH, p);
		}
		
		public void UpdateYawRef(double y)
		{
			if(this.currMode == Mode.DIRECTED)
				this.pid.SetRef(PIDManager.Ctrl.YAW, y);
		}
	}
}

