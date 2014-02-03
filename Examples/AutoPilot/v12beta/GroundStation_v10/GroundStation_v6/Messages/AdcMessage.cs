using System;
using System.Collections.Generic;

using System.Text;

namespace GroundStation
{
	/// <summary>
	/// AdcMessage class manages ADC airborne telemetry data
	/// i.e. barometer, thermometer and pitot. Moreover, it
	/// calculates True Airspeed and barometric altitude (MSL)
	/// </summary>
    public class AdcMessage : Message
    {
		/// <summary>
		/// The barometer field
		/// </summary>
        public Field2 barometer;
		
		/// <summary>
		/// The thermometer field
		/// </summary>
        public Field2 thermometer;
		
		/// <summary>
		/// The pitot field
		/// </summary>
        public Field2 pitot;
        
		/// <summary>
		/// The true airspeed (TAS)
		/// </summary>
        public double tas;
		
		/// <summary>
		/// The barometric altitude (MSL)
		/// </summary>
        public double altitude;
		
		/// <summary>
		/// Constant a0. Standard speed of sound at sea level. [m/s]
		/// </summary>
        private const double a0 = 661.4788;
		
		/// <summary>
		/// Constant T0. Standard temperature at sea level. [K]
		/// </summary>
        private const double T0 = 288.15;
		
		/// <summary>
		/// Constant p0. Standard static pressure at sea level. [Pa]
		/// </summary>
        private const int P0 = 101325;
		
		/// <summary>
		/// Constant lambda. Standard thermal gradient. [K/m]
		/// </summary>
        private const double lambda = -0.0065;
		
		/// <summary>
		/// Constant R. Ideal gas constant  [m²/(s²·K)]
		/// </summary>
        private const int R = 287;
		
		/// <summary>
		/// Constant g. Standard gravity at sea level [m/s²]
		/// </summary>
        private const double g = 9.80665;
		
		/// <summary>
		/// Minimum expected static pressure [Pa]
		/// </summary>
        private const int barMin = 97000;
		
		/// <summary>
		/// Maximum expected static pressure [Pa]
		/// </summary>
        private const int barMax = 100000;
		
		/// <summary>
		/// Initial static pressure previous value [Pa]
		/// </summary>
        private const int barPrevValue = 99500;
		
		/// <summary>
		/// Initial static pressure previous value [Pa]
		/// </summary>
        private const int barPrevPrevValue = 99500;
		
		/// <summary>
		/// Maximum expected static pressure variation [Pa/sample]
		/// </summary>
        private const int barMaxVar = 500;
		
		/// <summary>
		/// Minimum expected temperature. [K]
		/// </summary>
        private const int tempMin = 273; //0 ºC
		
		/// <summary>
		/// Maximum expected temperature. [K]
		/// </summary>
        private const int tempMax = 313; //40 ºC
		
		/// <summary>
		/// Initial temperature previous value [K]
		/// </summary>
        private const int tempPrevValue = 300;
		
		/// <summary>
		/// Initial temperature previous value [K]
		/// </summary>
        private const int tempPrevPrevValue = 300;
		
		/// <summary>
		/// Maximum expected temperature variation [K/sample]
		/// </summary>
        private const int tempMaxVar = 2;
		
		/// <summary>
		/// Minimum expected dynamic pressure [Pa]
		/// </summary>
        private const int pitotMin = 80;
		
		/// <summary>
		/// Maximum expected dynamic pressure [Pa]
		/// </summary>
        private const int pitotMax = 600;
		
		/// <summary>
		/// Initial previous dynamic pressure value [Pa]
		/// </summary>
        private const int pitotPrevValue = 130;
		
		/// <summary>
		/// Initial previous dynamic pressure value [Pa]
		/// </summary>
        private const int pitotPrevPrevValue = 130;
		
		/// <summary>
		/// Maximum expected dynamic pressure variation [Pa/sample].
		/// </summary>
        private const int pitotMaxVar = 300;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GroundStation.AdcMessage"/> class.
		/// </summary>
		/// <param name='time'>
		/// Timestamp.
		/// </param>
		/// <param name='b'>
		/// Message as an array of bytes
		/// </param>
        public AdcMessage()
            : base()
        {
            //this.barometer = new Field(barMin, barMax, barPrevValue, barPrevPrevValue, barMaxVar);
            //this.thermometer = new Field(tempMin, tempMax, tempPrevValue, tempPrevPrevValue, tempMaxVar);
            //this.pitot = new Field(pitotMin, pitotMax, pitotPrevValue, pitotPrevPrevValue, pitotMaxVar);
			this.barometer = new Field2(barMin, barMax, barPrevValue);
            this.thermometer = new Field2(tempMin, tempMax, tempPrevValue);
            this.pitot = new Field2(pitotMin, pitotMax, pitotPrevValue);
        }

		
		/// <summary>
		/// Creates the message.
		/// </summary>
		/// <param name='bArray'>
		/// Message as an array of bytes
		/// </param>
        public override void CreateMessage(ulong time, byte[] bArray)
        {
            double b, t, p;
			this.time = time;
            Array.Reverse(bArray, 1, 2);
            b = BitConverter.ToUInt16(bArray, 1);
            Array.Reverse(bArray, 3, 2);
            t = BitConverter.ToUInt16(bArray, 3);
            Array.Reverse(bArray, 5, 2);
            p = BitConverter.ToUInt16(bArray, 5);
            this.ConvertData(b, t, p);
            this.CalculateExtras();
        }
		
		/// <summary>
		/// Creates the message.
		/// </summary>
		/// <param name='m'>
		/// Message as a string
		/// </param>
        public override void CreateMessage(string m)
        {
            string[] words = m.Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                double b, t, p;
                b = Convert.ToUInt16(words[1]);
                t = Convert.ToUInt16(words[2]);
                p = Convert.ToUInt16(words[3]);
                this.ConvertData(b, t, p);
                this.CalculateExtras();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in ADC: " + e.Message);
            }
        }
		
		/// <summary>
		/// Converts the from Volts to its appropiated unit
		/// </summary>
		/// <param name='b'>
		/// Voltage level from the barometer 
		/// </param>
		/// <param name='t'>
		/// Voltage level from the thermometer
		/// </param>
		/// <param name='p'>
		/// Voltage level from the pitot tube
		/// </param>
        private void ConvertData(double b, double t, double p)
        {
            b = 5 * b / 65536.0;
            t = 5 * t / 65536.0;
            p = 5 * p / 65536.0; 
            this.barometer.V = (b / 5.1 + 0.095) / 0.009 * 1000.0;
            this.thermometer.V = t / 0.01 + 273.0;
            this.pitot.V = (p / 5.0 - 0.2) / 0.2 * 1000.0;
        }
		
		/// <summary>
		/// Calculates TAS and barometric altitude.
		/// </summary>
        private void CalculateExtras()
        {
            //tas[kt] = a0[kt] * sqrt(5*((qc[Hg]/P[Hg]+1)^2/7 - 1)*T[K]/T0[K]
            this.tas = a0 * Math.Sqrt(5 * (Math.Pow(this.pitot.V / this.barometer.V + 1, 2 / 7.0) - 1) * this.thermometer.V / T0);
            //Pz = P0*((T0+lambda*z)/T0)^(-g/(R*lambda))
            //z = T0*((P0/Pz)^(R*lambda/g)-1)/lambda
            this.altitude = T0 * (Math.Pow((P0 / this.barometer.V), R * lambda / g) - 1) / lambda;
        }
		
		/// <summary>
		/// Makes and exact copy of the adc message
		/// </summary>
		/// <returns>
		/// The copy.
		/// </returns>
		/// <param name='adc'>
		/// The adc message to copy
		/// </param>
        public static AdcMessage DeepCopy(AdcMessage adc)
        {
            AdcMessage ans = new AdcMessage();
            ans.time = adc.time;
            ans.altitude = adc.altitude;
            ans.tas = adc.tas - 29; //Offset degut a error en la tensio.

            ans.barometer = Field2.DeepCopy(adc.barometer);
            ans.thermometer = Field2.DeepCopy(adc.thermometer);
            ans.pitot = Field2.DeepCopy(adc.pitot);

            return ans;
        }

    }
}
