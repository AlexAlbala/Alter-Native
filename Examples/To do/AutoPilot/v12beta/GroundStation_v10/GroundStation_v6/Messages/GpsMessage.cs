using System;
using System.Collections.Generic;

using System.Text;

namespace GroundStation
{
	/// <summary>
	/// GpsMessage class manages the GPS airborne telemetry data
	/// i.e. latitude, longitude, ground speed and track angle.
	/// Moreover, it converts the received data in a UTM projection.
	/// </summary>
    public class GpsMessage : Message
    {
		/// <summary>
		/// The latitude field
		/// </summary>
        public Field latitude;
		
		/// <summary>
		/// The longitude field
		/// </summary>
        public Field longitude;
		
		/// <summary>
		/// The ground speed field
		/// </summary>
        public Field gndSpeed;
		
		/// <summary>
		/// The track angle field
		/// </summary>
        public Field trackAngle;
		
		/// <summary>
		/// Position object (lat, lon, UTM_X, UTM_Y)
		/// </summary>
        public WgsPoint pos;
		
		/// <summary>
		/// Minimum expected latitude [deg]
		/// </summary>
        private const int latMin = 40;
		
		/// <summary>
		/// Maximum expected latitude [deg]
		/// </summary>
        private const int latMax = 43;
		
		/// <summary>
		/// Initial previous latitude value [deg]
		/// </summary>
        private const int latPrevValue = 41;
		
		/// <summary>
		/// Initial previous latitude value [deg]
		/// </summary>
        private const int latPrevPrevValue = 42;
		
		/// <summary>
		/// Maximum  expected latitude variation [deg/sample]
		/// </summary>
        private const double latMaxVar = 0.02;
		
		/// <summary>
		/// Minimum expected longitude [deg]
		/// </summary>
        private const int lonMin = 2;
		
		/// <summary>
		/// Maximum expected longitude [deg]
		/// </summary>
        private const int lonMax = 4; 
		
		/// <summary>
		/// Initial previous longitude value [deg]
		/// </summary>
        private const int lonPrevValue = 3;
		
		/// <summary>
		/// Initial previous longitude value [deg]
		/// </summary>
        private const int lonPrevPrevValue = 3;
		
		/// <summary>
		/// Maximum expected longitude variation [deg/sample]
		/// </summary>
        private const double lonMaxVar = 0.02;
		
		/// <summary>
		/// Minimum expected ground speed [m/s]
		/// </summary>
        private const int gndSpeedMin = 0;
		
		/// <summary>
		/// Maximum expected ground speed [m/s]
		/// </summary>
        private const int gndSpeedMax = 40;
		
		/// <summary>
		/// Initial previous ground speed value [m/s]
		/// </summary>
        private const int gndSpeedPrevValue = 0;
		
		/// <summary>
		/// Initial previous ground speed value [m/s]
		/// </summary>
        private const int gndSpeedPrevPrevValue = 0;
		
		/// <summary>
		/// Maximum expected ground speed variation [m/(s·sample)]
		/// </summary>
        private const int gndSpeedMaxVar = 20;
		
		/// <summary>
		/// Minimum expected track angle [deg]
		/// </summary>
        private const int trackAngleMin = 0;
		
		/// <summary>
		/// Maximum expected track angle [deg]
		/// </summary>
        private const int trackAngleMax = 360;
		
		/// <summary>
		/// Initial previous track angle value [deg]
		/// </summary>
        private const int trackAnglePrevValue = 100;
		
		/// <summary>
		/// Initial previous track angle value [deg]
		/// </summary>
		private const int trackAnglePrevPrevValue = 100;
		
		/// <summary>
		/// Maximum expected track angle variation [deg/sample]
		/// </summary>
        private const int trackAngleMaxVar = 180;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GroundStation.GpsMessage"/> class.
        /// </summary>
        /// <param name='time'>
        /// Timestamp
        /// </param>
        /// <param name='b'>
        /// Message as a byte array
        /// </param>
        public GpsMessage()
        : base ()
        {
            this.latitude = new Field(latMin, latMax, latPrevValue, latPrevPrevValue, latMaxVar);
            this.longitude = new Field(lonMin, lonMax, lonPrevValue, lonPrevPrevValue, lonMaxVar);
            this.gndSpeed = new Field(gndSpeedMin, gndSpeedMax, gndSpeedPrevValue, gndSpeedPrevPrevValue, gndSpeedMaxVar);
            this.trackAngle = new Field(trackAngleMin, trackAngleMax, trackAnglePrevValue, trackAnglePrevPrevValue, trackAngleMaxVar);
        }

		
		/// <summary>
		/// Creates the message.
		/// </summary>
		/// <param name='b'>
		/// Message as a byte array
		/// </param>
        public override void CreateMessage(ulong time, byte[] b)
        {
            Array.Reverse(b, 1, 4);
            this.latitude.V = BitConverter.ToInt32(b, 1)/1000000.0;
            Array.Reverse(b, 5, 4);
            this.longitude.V = BitConverter.ToInt32(b, 5)/100000.0;
            Array.Reverse(b, 9, 2);
            this.gndSpeed.V = BitConverter.ToInt16(b, 9) * 1852 / 360000.0 ;
            Array.Reverse(b, 11, 2);
            this.trackAngle.V = BitConverter.ToInt16(b, 11) / 100.0;

            WgsPoint wgs = new WgsPoint(this.latitude.V, this.longitude.V, null);
        }
		
		/// <summary>
		/// Creates the message.
		/// </summary>
		/// <param name='m'>
		/// Message as a string
		/// </param>
        public override void CreateMessage(string m)
        {
            try
            {
                string[] words = m.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
                this.latitude.V = Convert.ToDouble(words[3].Substring(0, 2)) + Convert.ToDouble(words[3].Substring(2)) / 60;
                this.longitude.V = Convert.ToDouble(words[5].Substring(0, 3)) + Convert.ToDouble(words[5].Substring(3)) / 60;
                this.gndSpeed.V = Convert.ToDouble(words[7]);
                this.trackAngle.V = Convert.ToDouble(words[8]);

                this.pos = new WgsPoint(this.latitude.V, this.longitude.V, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error GPS: " + e.Message);
            }
        }
		
		/// <summary>
		/// Creates an exact copy of gps
		/// </summary>
		/// <returns>
		/// The copy.
		/// </returns>
		/// <param name='gps'>
		/// gps to be copied.
		/// </param>
        public static GpsMessage DeepCopy(GpsMessage gps)
        {
            GpsMessage ans = new GpsMessage();
            ans.time = gps.time;
            ans.latitude = Field.DeepCopy(gps.latitude);
            ans.longitude = Field.DeepCopy(gps.longitude);
            ans.gndSpeed = Field.DeepCopy(gps.gndSpeed);
            ans.trackAngle = Field.DeepCopy(gps.trackAngle);
            ans.pos = new WgsPoint(ans.latitude.V, ans.longitude.V, null);
            return ans;
        }
    }
}
