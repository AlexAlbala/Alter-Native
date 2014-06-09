using System;

namespace GroundStation
{
	public class Altitude : UpperLayer
	{
		//Si la l'altitud actual difereix menys
		//de deltaAlt metres de l'objectiu assumim
		//que ja hem arribat a l'objectiu
		private const int deltaAlt = 2;
		
		//Array de pitches corresponents a les 
		//diferencies en altitud que es volen 
		//assolir
		private double[] pitches = new double[10];
		
		private double selAlt;
		
		//El contructor seteja la referencia inicial
		//del pid
		public Altitude (double initialRef)
			: base()
		{
			double val = 0.5;
			for(int i = 0; i < this.pitches.Length; i++)
			{
				this.pitches[i] = val;
				val += 0.5;
			}
			
			this.selAlt = initialRef;
		}
		
		//Norma: Només pujo si la velocitat està per sobre
		//d'un cert llindar.
		//Norma: Els canvis d'altitud es controlen 
		//directament amb el pitch, sense tocar el throttle
		public override void SetParam (double upperParam)
		{
			this.selAlt = upperParam;
		}
		
		public override void GetRef ()
		{
			if(!this.activated)
				return;
			double currTas = this.ga.Adc.tas;
			double currAlt = this.ga.Adc.altitude;
			double currPitch = this.ga.Imu.pitch.V;
			
			//Positive if climbing, otherwise negative 
			double diffAlt = this.selAlt - currAlt;
			
			//En el cas que estiguem a l'altitud desitjada,
			//tornem double.MaxValue per indicar que no 
			//volem canviar la referencia.
			if(Math.Abs(diffAlt) < 2)
				this.pid.SetRef(PIDManager.Ctrl.PITCH, double.MaxValue);
			
			//En el cas en que la velocitat no estigui per
			//sobre d'un cert llindar i volguem pujar, 
			//Baixem el pitx un grau si aquest es positiu 
			//i avisem per consola del nou valor de pitch
			if(diffAlt > 0 && currTas < (this.ap.stallTas + 5))
			{
				double ans;
				if (currPitch > 0)
					ans = currPitch -1;
				else 
					ans = 0;
				Console.WriteLine("WARNING: Altitude not set due to low airspeed --> pitch reference set to " + ans + " degrees");
				this.pid.SetRef(PIDManager.Ctrl.PITCH, ans);
			}
			
			int altLevel = (int)Math.Round(Math.Abs(diffAlt/10));
			altLevel = altLevel > 9 ? altLevel = 9 : altLevel;
			
			if(diffAlt < 0)
				this.pid.SetRef(PIDManager.Ctrl.PITCH, -this.pitches[altLevel]);
			else
				this.pid.SetRef(PIDManager.Ctrl.PITCH, this.pitches[altLevel]);
		}
	}
}

