using System;
using System.Collections.Generic;
using System.Text;

namespace GroundStation
{
	/// <summary>
	/// ImuEulerMessage class manages the airborne IMU telemetry data
	/// i.e. roll, pitch, yaw. Moreover, it also manages the 3-axis
	/// accelerometer raw data.
	/// </summary>
    public class ImuEulerMessage : Message
    {
		/// <summary>
		/// The roll field
		/// </summary>
        public Field2 roll;
		
		/// <summary>
		/// The pitch field
		/// </summary>
        public Field2 pitch;
		
		/// <summary>
		/// The yaw field.
		/// </summary>
        public Field2 yaw;
		
		/// <summary>
		/// The x-axis acceleration
		/// </summary>
		public Field2 accelX;
		
		/// <summary>
		/// The y-axis acceleration
		/// </summary>
        public Field2 accelY;
		
		/// <summary>
		/// The z.axis acceleration
		/// </summary>
        public Field2 accelZ;
		
		/// <summary>
		/// Minimum expected roll [deg]
		/// </summary>
        private const int rollMin = -90;
		
		/// <summary>
		/// Maximum expected roll [deg]
		/// </summary>
        private const int rollMax = 90;
		
		/// <summary>
		/// Initial roll previous value [deg]
		/// </summary>
        private const int rollPrevValue = 0;
		
		/// <summary>
		/// Initial roll previous value [deg]
		/// </summary>
        private const int rollPrevPrevValue = 0;
		
		/// <summary>
		/// Maximum expected roll variation [deg/sample]
		/// </summary>
        private const int rollMaxVar = 15;
		
		/// <summary>
		/// Minimum expected pitch [deg]
		/// </summary>
        private const int pitchMin = -90;
		
		/// <summary>
		/// Maximum expected pitch [deg]
		/// </summary>
        private const int pitchMax = 90;
		
		/// <summary>
		/// Initial pitch previous value [deg]
		/// </summary>
        private const int pitchPrevValue = 0;
		
		/// <summary>
		/// Initial pitch previous value [deg]
		/// </summary>
        private const int pitchPrevPrevValue = 0;
		
		/// <summary>
		/// Maximum expected pitch variation [deg/sample]
		/// </summary>
        private const int pitchMaxVar = 10;
		
		/// <summary>
		/// Minimum expected yaw [deg]
		/// </summary>
        private const int yawMin = -180;
		
		/// <summary>
		/// Maximum expected yaw [deg]
		/// </summary>
        private const int yawMax = 180;
		
		/// <summary>
		/// Initial yaw previous value [deg]
		/// </summary>
        private const int yawPrevValue = -90;
		
		/// <summary>
		/// Initial yaw previous value [deg]
		/// </summary>
        private const int yawPrevPrevValue = 90;
		
		/// <summary>
		/// Maximum expected yaw variation [deg/sample]
		/// </summary>
        private const int yawMaxVar = 10;
		
		/// <summary>
		/// Minimum expected x-axis acceleration [g]
		/// </summary>
        private const int accelXMin = -2;
		
		/// <summary>
		/// Maximum expected x-axis acceleration [g]
		/// </summary>
        private const int accelXMax = 2;
		
		/// <summary>
		/// Initial x-axis acceleration previous value [g]
		/// </summary>
        private const int accelXPrevValue = 0;
		
		/// <summary>
		/// Initial x-axis acceleration previous value [g]
		/// </summary>
        private const int accelXPrevPrevValue = 0;
		
		/// <summary>
		/// Maximum expected x-axis acceleration variation [g/sample]
		/// </summary>
        private const int accelXMaxVar = 1;
		
		/// <summary>
		/// Minimum expected y-axis acceleration [g]
		/// </summary>
        private const int accelYMin = -2;
		
		/// <summary>
		/// Maximum expected y-axis acceleraiton [g]
		/// </summary>
        private const int accelYMax = 2;
		
		/// <summary>
		/// Initial y-axis acceleration previous value [g]
		/// </summary>
        private const int accelYPrevValue = 0;
		
		/// <summary>
		/// Initial y-axis acceleration previous value [g]
		/// </summary>
        private const int accelYPrevPrevValue = 0;
		
		/// <summary>
		/// Maximum expected y-axis acceleration variation [g]
		/// </summary>
        private const int accelYMaxVar = 1;
		
		/// <summary>
		/// Minimum expected z-axis acceleration [g]
		/// </summary>
        private const int accelZMin = -2;
		
		/// <summary>
		/// Maximum expected z-axis acceleration [g]
		/// </summary>
        private const int accelZMax = 2;
		
		/// <summary>
		/// Initial z-axis acceleration previous value [g]
		/// </summary>
        private const int accelZPrevValue = 0;
		
		/// <summary>
		/// Initial z-axis acceleration previous value [g]
		/// </summary>
        private const int accelZPrevPrevValue = 0;
		
		/// <summary>
		/// Maximum expected z-axis acceleration variation [g/sample] 
		/// </summary>
        private const int accelZMaxVar = 1;
		
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GroundStation.ImuEulerMessage"/> class.
		/// </summary>
		/// <param name='time'>
		/// Timestamp
		/// </param>
		/// <param name='b'>
		/// Message as a byte array
		/// </param>
        public ImuEulerMessage()
        : base ()
        {
            //this.roll = new Field(rollMin, rollMax, rollPrevValue, rollPrevPrevValue, rollMaxVar);
            //this.pitch = new Field(pitchMin, pitchMax, pitchPrevValue, pitchPrevPrevValue, pitchMaxVar);
            //this.yaw = new Field(yawMin, yawMax,yawPrevValue, yawPrevPrevValue, yawMaxVar);
            //this.accelX = new Field(accelXMin, accelXMax, accelXPrevValue, accelXPrevPrevValue, accelXMaxVar);
            //this.accelY = new Field(accelYMin, accelYMax, accelYPrevValue, accelYPrevPrevValue, accelYMaxVar);
            //this.accelZ = new Field(accelZMin, accelZMax, accelZPrevValue, accelZPrevPrevValue, accelZMaxVar);
			this.roll = new Field2(rollMin, rollMax, rollPrevValue, rollMaxVar);
            this.pitch = new Field2(pitchMin, pitchMax, pitchPrevValue, pitchMaxVar);
            this.yaw = new Field2(yawMin, yawMax, yawPrevValue, yawMaxVar);
            this.accelX = new Field2(accelXMin, accelXMax, accelXPrevValue);
            this.accelY = new Field2(accelYMin, accelYMax, accelYPrevValue);
            this.accelZ = new Field2(accelZMin, accelZMax, accelZPrevValue);
        }
		
		/// <summary>
		/// Creates the message.
		/// </summary>
		/// <param name='b'>
		/// Message as a byte array
		/// </param>
        public override void CreateMessage(ulong time, byte[] b)
        {
			this.time = time;
            Array.Reverse(b, 1, 4);
            this.roll.V = BitConverter.ToUInt32(b, 1) / 10000.0-180.0;
            Array.Reverse(b, 5, 4);
			double val = BitConverter.ToUInt32(b, 5) / 10000.0-180.0;
			this.pitch.V = val;
			//Console.WriteLine("Pitch: " + val);
            Array.Reverse(b, 9, 4);
            this.yaw.V = BitConverter.ToUInt32(b, 9) / 10000.0-180.0;
            Array.Reverse(b, 13, 4);
            this.accelX.V = BitConverter.ToInt32(b, 13) / 2560000.0;
            Array.Reverse(b, 17, 4);
            this.accelY.V = BitConverter.ToInt32(b, 17) / 2560000.0;
            Array.Reverse(b, 21, 4);
            this.accelZ.V = BitConverter.ToInt32(b, 21) / 2560000.0;
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
                this.roll.V = Convert.ToDouble(words[1]);
                this.pitch.V = Convert.ToDouble(words[2]);
                this.yaw.V = Convert.ToDouble(words[3]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in IMU: " + e.Message);
            }
        }
		
		/// <summary>
		/// Makes an exact copy of imu
		/// </summary>
		/// <returns>
		/// The copy.
		/// </returns>
		/// <param name='imu'>
		/// The imu message to be copied
		/// </param>
        public static ImuEulerMessage DeepCopy(ImuEulerMessage imu)
        {
            ImuEulerMessage ans = new ImuEulerMessage();
			ans.time = imu.time;
            ans.accelX = Field2.DeepCopy(imu.accelX);
            ans.accelY = Field2.DeepCopy(imu.accelY);
            ans.accelZ = Field2.DeepCopy(imu.accelZ);
            ans.roll = Field2.DeepCopy(imu.roll);
            ans.pitch = Field2.DeepCopy(imu.pitch);
            ans.yaw = Field2.DeepCopy(imu.yaw);

            return ans;
        }
    }
}
