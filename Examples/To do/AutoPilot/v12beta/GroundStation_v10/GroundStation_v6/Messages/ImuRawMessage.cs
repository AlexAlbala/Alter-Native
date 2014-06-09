using System;
using System.Collections.Generic;
using System.Text;

namespace GroundStation
{
	/// <summary>
	/// DEPRECATED
	/// </summary>
    public class ImuRawMessage : Message
    {
        public Field accelX;
        public Field accelY;
        public Field accelZ;

        public Field magnetomX;
        public Field magnetomY;
        public Field magnetomZ;

        //TODO: Asignar constant segons especificacions
        private const int accelXMin = 0;
        private const int accelXMax = 0;
        private const int accelXPrevValue = 0;
        private const int accelXPrevPrevValue = 0;
        private const int accelXMaxVar = 0;

        private const int accelYMin = 0;
        private const int accelYMax = 0;
        private const int accelYPrevValue = 0;
        private const int accelYPrevPrevValue = 0;
        private const int accelYMaxVar = 0;

        private const int accelZMin = 0;
        private const int accelZMax = 0;
        private const int accelZPrevValue = 0;
        private const int accelZPrevPrevValue = 0;
        private const int accelZMaxVar = 0;

        private const int magnetomXMin = 0;
        private const int magnetomXMax = 0;
        private const int magnetomXPrevValue = 0;
        private const int magnetomXPrevPrevValue = 0;
        private const int magnetomXMaxVar = 0;

        private const int magnetomYMin = 0;
        private const int magnetomYMax = 0;
        private const int magnetomYPrevValue = 0;
        private const int magnetomYPrevPrevValue = 0;
        private const int magnetomYMaxVar = 0;

        private const int magnetomZMin = 0;
        private const int magnetomZMax = 0;
        private const int magnetomZPrevValue = 0;
        private const int magnetomZPrevPrevValue = 0;
        private const int magnetomZMaxVar = 0;

        public ImuRawMessage()
            : base()
        { }

        public override void CreateMessage(ulong time, byte[] b)
        {
            Array.Reverse(b, 1, 4);
            this.accelX.V = BitConverter.ToUInt32(b, 1) / 100.0;
            Array.Reverse(b, 5, 4);
            this.accelY.V = BitConverter.ToUInt32(b, 5) / 100.0;
            Array.Reverse(b, 9, 4);
            this.accelZ.V = BitConverter.ToUInt32(b, 9) / 100.0;

            Array.Reverse(b, 13, 2);
            this.magnetomX.V = BitConverter.ToInt16(b, 13);
            Array.Reverse(b, 15, 2);
            this.magnetomY.V = BitConverter.ToInt16(b, 15);
            Array.Reverse(b, 17, 2);
            this.magnetomZ.V = BitConverter.ToInt16(b, 17);
			
			this.sw.WriteLine(this.accelX.V.ToString("0.000000"));
			this.sw.WriteLine(this.accelY.V.ToString("0.000000"));
			this.sw.WriteLine(this.accelZ.V.ToString("0.000000"));
			this.sw.WriteLine(this.magnetomX.V.ToString("0.000000"));
			this.sw.WriteLine(this.magnetomY.V.ToString("0.000000"));
			this.sw.WriteLine(this.magnetomZ.V.ToString("0.000000"));
			this.sw.Close();
		}

        public override void CreateMessage(string m)
        {
            string[] words = m.Split(new char[] { ',', ':' }, StringSplitOptions.RemoveEmptyEntries);

            this.accelX.V = Convert.ToDouble(words[1]);
            this.accelY.V = Convert.ToDouble(words[2]);
            this.accelZ.V = Convert.ToDouble(words[3]);

            this.magnetomX.V = Convert.ToInt16(words[4]);
            this.magnetomY.V = Convert.ToInt16(words[5]);
            this.magnetomZ.V = Convert.ToInt16(words[6]);
        }
    }
}
