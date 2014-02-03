using System;
using System.Timers;
using Gtk;
using Cairo;
using GroundStation;

public partial class MainWindow: Gtk.Window
{	
	private GlobalArea ga = GlobalArea.GetInstance();
	private Timer t1 = new Timer(500);
	//private Timer t2 = new Timer(2000);
	private NavManager nav = NavManager.GetInstance();
	private PIDManager pid = PIDManager.GetInstance();
	
	private double currRoll;
	private double currPitch;
	private double currYaw;
	private object mutex;
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		t1.Elapsed += delegate{ShowData1();};
		t1.Start();
		this.mutex = new object();
		//t2.Elapsed += delegate{ShowData2();};
		//t2.Start();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	
	protected void SetAttitude()
	{
		if(this.ga.Imu != null)
		{
			lock(this.mutex)
			{
				this.currRoll = this.ga.Imu.roll.V;
				this.currPitch = this.ga.Imu.pitch.V;
				this.currYaw = this.ga.Imu.yaw.V;
				
				this.RollEntry.Text = this.currRoll.ToString("0.00");
				this.PitchEntry.Text = this.currPitch.ToString("0.00");
				this.YawEntry.Text = this.currYaw.ToString("0.00");
				this.CurHead.Text = this.YawEntry.Text;
			}
		}
		if(this.ga.Adc != null)
		{
			this.TasEntry.Text = this.ga.Adc.tas.ToString("0.00");
			this.CurAlt.Text = this.ga.Adc.altitude.ToString("0.00");
		}
	}
	
	protected void SetCh()
	{
		byte[] ans = new byte[4];
		if(pid.ChIn != null)
		{
			ans = pid.ChIn;
		
			this.InCh1Val.Text = ans[0].ToString();
			this.InCh2Val.Text = ans[1].ToString();
			this.InCh3Val.Text = ans[2].ToString();
			this.InCh4Val.Text = ans[3].ToString();
			
			this.InCh1Width.Text = pid.GetChInWidth(1).ToString();
			this.InCh2Width.Text = pid.GetChInWidth(2).ToString();
			this.InCh3Width.Text = pid.GetChInWidth(3).ToString();
			this.InCh4Width.Text = pid.GetChInWidth(4).ToString();
			
			this.OutCh1Val.Text = pid.GetCh(1).ToString();
			this.OutCh2Val.Text = pid.GetCh(2).ToString();
			this.OutCh3Val.Text = pid.GetCh(3).ToString();
			this.OutCh4Val.Text = pid.GetCh(4).ToString();
			
			this.OutCh1Width.Text = pid.GetChWidth(1).ToString();
			this.OutCh2Width.Text = pid.GetChWidth(2).ToString();
			this.OutCh3Width.Text = pid.GetChWidth(3).ToString();
			this.OutCh4Width.Text = pid.GetChWidth(4).ToString();
		}
		else
			this.InCh1Val.Text = "ERROR!!";
	}
	
	protected void ShowData1()
	{
		Gtk.Application.Invoke(delegate {this.SetAttitude();});
		//Gtk.Application.Invoke(delegate {this.SetCh();});
	}
	
	protected void ShowData2()
	{
		Gtk.Application.Invoke(delegate {this.SetCh();});
	}

	protected void AcceptQueryEvent (object sender, System.EventArgs e)
	{
		string query = this.QueryText.Text;
		if(query == string.Empty)
			this.QueryText.Text = "Wrong format";
		string[] words = query.Split(new char[]{' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
		if(words.Length > 2)
		{
			this.QueryText.Text = "Wrong format";
			return;
		}
		double val;
		this.nav.ci.ExecuteQuery(words, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
	}

	protected void OnRollKpClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"rkp",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.RollCurrKp.Text = this.QueryText.Text;
	}

	protected void OnPitchKpClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"pkp",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.PitchCurrKp.Text = this.QueryText.Text;
	}

	protected void OnYawKpClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"ykp",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.YawCurrKp.Text = this.QueryText.Text;
	}

	protected void OnYawKiClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"yki",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.YawCurrKi.Text = this.QueryText.Text;
	}

	protected void OnPitchKiClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"pki",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.PitchCurrKi.Text = this.QueryText.Text;
	}

	protected void OnRollKiClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"rki",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.RollCurrKi.Text = this.QueryText.Text;
	}
	
	protected void OnThrottleKpClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"tkp",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.ThrottleCurrKp.Text = this.QueryText.Text;
	}
	
	protected void OnThrottleKiClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"tki",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.ThrottleCurrKi.Text = this.QueryText.Text;
	}

	protected void OnNewTasClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"nv",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.SelTas.Text = this.QueryText.Text;
	}

	protected void OnNewAltClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"na",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.SelAlt.Text = this.QueryText.Text;	
	}

	protected void OnManualModeClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"mm"};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = "OK";
		this.CurMode.Text = "MAN";
	}

	protected void OnDirectedModeClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"dm"};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = "OK!";
		this.CurMode.Text = "DIR";	
	}

	protected void OnAutoModeClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"am"};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = "OK!";
		this.CurMode.Text = "AUTO";
	}

	protected void OnThrottleCalibModeClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"ctm"};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = "OK!";
		this.CurMode.Text = "CTH";	
	}

	protected void OnRollCalibModeClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"crm"};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = "OK!";
		this.CurMode.Text = "CRO";
	}

	protected void OnPitchCalibModeClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"cpm"};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = "OK!";
		this.CurMode.Text = "CPI";
	}

	protected void OnYawCalibModeClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"cym"};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = "OK!";
		this.CurMode.Text = "CYA";
	}
	
	protected void OnNewHeadingClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"nah",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.SelHEad.Text = this.QueryText.Text;	
	}

	protected void OnNewRollRefClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"nr",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurRollRef.Text = this.QueryText.Text;	
	}

	protected void OnNewPitchRefClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"np",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurPitchRef.Text = this.QueryText.Text;	
	}

	protected void OnNewYawRefClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"ny",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurYawRef.Text = this.QueryText.Text;	
	}

	protected void OnNewThrottleRefClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"nv",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurThrotRef.Text = this.QueryText.Text;	
	}

	protected void OnThrottleMeanValClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"tmv",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurThrottleMeanVal.Text = this.QueryText.Text;
	}

	protected void OnRollMeanValClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"rmv",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurRollMeanVal.Text = this.QueryText.Text;
	}

	protected void OnPitchMeanValClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"pmv",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurPitchMeanVal.Text = this.QueryText.Text;
	}

	protected void OnYawMeanValClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"ymv",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurYawMeanVal.Text = this.QueryText.Text;
	}

	protected void OnThrottleInitialValueClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"tiv",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurThrotInitialValue.Text = this.QueryText.Text;
	}

	protected void OnRollInitialValueClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"riv",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurRollInitialVal.Text = this.QueryText.Text;
	}

	protected void OnPitchInitialValueClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"piv",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurPitchInitialVal.Text = this.QueryText.Text;
	}

	protected void OnYawInitialValueClicked (object sender, System.EventArgs e)
	{
		string[] query =  new string[]{"yiv",this.QueryText.Text};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurYawInitVal.Text = this.QueryText.Text;
	}

	protected void OnGetInitialDataClicked (object sender, System.EventArgs e)
	{	
		double val;
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.THROTTLE,PID.Param.INITIAL_VAL)) != double.MaxValue)
			this.CurThrotInitialValue.Text = val.ToString("0000");
		else 
			this.CurThrotInitialValue.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.ROLL,PID.Param.INITIAL_VAL)) != double.MaxValue)
			this.CurRollInitialVal.Text = val.ToString("0000");
		else 
			this.CurRollInitialVal.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.PITCH,PID.Param.INITIAL_VAL)) != double.MaxValue)
			this.CurPitchInitialVal.Text = val.ToString("0000");
		else 
			this.CurPitchInitialVal.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.YAW,PID.Param.INITIAL_VAL)) != double.MaxValue)
			this.CurYawInitVal.Text = val.ToString("0000");
		else 
			this.CurYawInitVal.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.THROTTLE,PID.Param.MEAN_VAL)) != double.MaxValue)
			this.CurThrottleMeanVal.Text = val.ToString("0000");
		else 
			this.CurThrottleMeanVal.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.ROLL,PID.Param.MEAN_VAL)) != double.MaxValue)
			this.CurRollMeanVal.Text = val.ToString("0000");
		else 
			this.CurRollMeanVal.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.PITCH,PID.Param.MEAN_VAL)) != double.MaxValue)
			this.CurPitchMeanVal.Text = val.ToString("0000");
		else 
			this.CurPitchMeanVal.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.YAW,PID.Param.MEAN_VAL)) != double.MaxValue)
			this.CurYawMeanVal.Text = val.ToString("0000");
		else 
			this.CurYawMeanVal.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.THROTTLE,PID.Param.KP)) != double.MaxValue)
			this.ThrottleCurrKp.Text = val.ToString("0000");
		else 
			this.ThrottleCurrKp.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.ROLL,PID.Param.KP)) != double.MaxValue)
			this.RollCurrKp.Text = val.ToString("0000");
		else 
			this.RollCurrKp.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.PITCH,PID.Param.KP)) != double.MaxValue)
			this.PitchCurrKp.Text = val.ToString("0000");
		else 
			this.PitchCurrKp.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.YAW,PID.Param.KP)) != double.MaxValue)
			this.YawCurrKp.Text = val.ToString("0000");
		else 
			this.YawCurrKp.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.THROTTLE,PID.Param.KI)) != double.MaxValue)
			this.ThrottleCurrKi.Text = val.ToString("0000");
		else 
			this.ThrottleCurrKi.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.ROLL,PID.Param.KI)) != double.MaxValue)
			this.RollCurrKi.Text = val.ToString("0000");
		else 
			this.RollCurrKi.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.PITCH,PID.Param.KI)) != double.MaxValue)
			this.PitchCurrKi.Text = val.ToString("0000");
		else 
			this.PitchCurrKi.Text = "N/A";
		
		if((val = this.pid.GetParam(PIDManager.Ctrl.YAW,PID.Param.KI)) != double.MaxValue)
			this.YawCurrKi.Text = val.ToString("0000");
		else 
			this.YawCurrKi.Text = "N/A";
		
		if(this.pid.isAct(PIDManager.Ctrl.THROTTLE))
			this.CurActThrottle.Text = "YES";
		else
		   this.CurActThrottle.Text = "NO";
		
		if(this.pid.isAct(PIDManager.Ctrl.ROLL))
			this.CuActRoll.Text = "YES";
		else
		   this.CuActRoll.Text = "NO";
		
		if(this.pid.isAct(PIDManager.Ctrl.PITCH))
			this.CurActPitch.Text = "YES";
		else
		   this.CurActPitch.Text = "NO";
		
		if(this.pid.isAct(PIDManager.Ctrl.YAW))
			this.CurActYaw.Text = "YES";
		else
		   this.CurActYaw.Text = "NO";
		
	}

	protected void OnThrottleClicked (object sender, System.EventArgs e)
	{
		if(this.pid.isAct(PIDManager.Ctrl.THROTTLE))
		{
			this.pid.Deactivate(PIDManager.Ctrl.THROTTLE);
			this.CurActThrottle.Text = this.pid.isAct(PIDManager.Ctrl.THROTTLE).ToString();
		}
		else
		{
			this.pid.Activate(PIDManager.Ctrl.THROTTLE);
			this.CurActThrottle.Text = this.pid.isAct(PIDManager.Ctrl.THROTTLE).ToString();
		}
	}
	
	protected void OnRollClicked (object sender, System.EventArgs e)
	{
		if(this.pid.isAct(PIDManager.Ctrl.ROLL))
		{
			this.pid.Deactivate(PIDManager.Ctrl.ROLL);
			this.CuActRoll.Text = this.pid.isAct(PIDManager.Ctrl.ROLL).ToString();
		}
		else
		{
			this.pid.Activate(PIDManager.Ctrl.ROLL);
			this.CuActRoll.Text = this.pid.isAct(PIDManager.Ctrl.ROLL).ToString();
		}
	}

	protected void OnActPitchClicked (object sender, System.EventArgs e)
	{
		if(this.pid.isAct(PIDManager.Ctrl.PITCH))
		{
			this.pid.Deactivate(PIDManager.Ctrl.PITCH);
			this.CurActPitch.Text = this.pid.isAct(PIDManager.Ctrl.PITCH).ToString();
		}
		else
		{
			this.pid.Activate(PIDManager.Ctrl.PITCH);
			this.CurActPitch.Text = this.pid.isAct(PIDManager.Ctrl.PITCH).ToString();
		}
	}

	protected void OnActYawClicked (object sender, System.EventArgs e)
	{
		if(this.pid.isAct(PIDManager.Ctrl.YAW))
		{
			this.pid.Deactivate(PIDManager.Ctrl.YAW);
			this.CurActYaw.Text = this.pid.isAct(PIDManager.Ctrl.YAW).ToString();
		}
		else
		{
			this.pid.Activate(PIDManager.Ctrl.YAW);
			this.CurActYaw.Text = this.pid.isAct(PIDManager.Ctrl.YAW).ToString();
		}
	}

	protected void OnGetCurrYawClicked (object sender, System.EventArgs e)
	{
		double yaw;
		lock(this.mutex)
		{
			yaw = this.currYaw;
		}
		string[] query =  new string[]{"yiv", yaw.ToString()};
		
		double val;
		this.nav.ci.ExecuteQuery(query, out val);
		this.QueryOutputText.Text = val.ToString("0.00");
		this.CurYawInitVal.Text = yaw.ToString();
	}
}
