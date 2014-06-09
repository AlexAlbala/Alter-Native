using System;
using System.IO;

namespace GroundStation
{
	public class AircraftPerformance
	{
		//Stall Speed [m/s]
		public double stallTas;
		//Maximum bank angle [deg]
		public double maxBank;
		//Pitch angles [maxDown, maxUp] eg. -15, 10
		public double[] maxPitch;
		//
		
		
		public static string path = "AircraftPerformance.txt";
		
		private static AircraftPerformance instance = null;
		
		public static AircraftPerformance GetInstance()
		{
			if(instance == null)
				instance = new AircraftPerformance();
			return instance;
		}
		
		private AircraftPerformance ()
		{
			StreamReader sr = new StreamReader(path);
			
			string line = "";
			
			while((line = sr.ReadLine()) != null)
			{
				if(line.StartsWith("#"))
					continue;
				string[] words = line.Split(new char[]{' ', '\t'});
				if(words.Length != 2)
					continue;
				switch(words[0])
				{
				case "stallTas":
					this.stallTas = double.Parse(words[1]);
					break;
				case "maxBank":
					this.maxBank = double.Parse(words[1]);
					break;
				}
			}
		}
		
		
	}
}

