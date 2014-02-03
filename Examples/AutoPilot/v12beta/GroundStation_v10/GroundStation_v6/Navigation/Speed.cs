using System;

namespace GroundStation
{
	//La velocitat la tenim directament relacionada
	//amb el throttle a través de la funció de
	//transferència de la planta.
	public class Speed : UpperLayer
	{
		//Si la velocitat respecte el vent
		//difereix en menys de 2 m/s considerem
		//que ja l'hem assolit
		private const int deltaSpeed = 2;
		
		//Velocitat seleccionada
		private double selSpeed;
		
		
		public Speed (double initialRef)
			: base ()
		{
			this.selSpeed = initialRef;
		}
		
		public override void SetParam (double upperParam)
		{
			this.selSpeed = upperParam;
		}
		
		public override void GetRef ()
		{
			if(!activated)
				return;
			this.pid.SetRef(PIDManager.Ctrl.THROTTLE, this.selSpeed);
		}
	}
}

