using System;
using System.Collections.Generic;

using System.Text;

namespace GroundStation
{
	/// <summary>
	/// PwmMessage class manages airborne received PWM 
	/// data for each servo channel i.e. ch1, ch2, ch3
	/// ch4.
	/// </summary>    
	public class PwmMessage : Message
    {
		/// <summary>
		/// The ch1 field
		/// </summary>
        public Field ch1;
		
		/// <summary>
		/// The ch2 field
		/// </summary>
        public Field ch2;
		
		/// <summary>
		/// The ch3 field 
		/// </summary>
        public Field ch3;
		
		/// <summary>
		/// The ch4 field
		/// </summary>
        public Field ch4;
		
		/// <summary>
		/// Minimum expected ch1 [us]
		/// </summary>
        private const int ch1Min = 1000;
		
		/// <summary>
		/// Maximum expected ch1 [us]
		/// </summary>
        private const int ch1Max = 2000;
		
		/// <summary>
		/// Initial previous ch1 value [us]
		/// </summary>
        private const int ch1PrevValue = 1500;
		
		/// <summary>
		/// Initial previous ch1 value [us]
		/// </summary>
        private const int ch1PrevPrevValue = 1500;
		
		/// <summary>
		/// Maximum expected ch1 variation [us/sample]
		/// </summary>
        private const int ch1MaxVar = 800;
		
		/// <summary>
		/// Minimum  expected ch2 [us]
		/// </summary>
        private const int ch2Min = 1000;
		
		/// <summary>
		/// Maximum expected ch2 [us]
		/// </summary>
        private const int ch2Max = 2000;
		
		/// <summary>
		/// Initial previous ch2 value [us]
		/// </summary>
        private const int ch2PrevValue = 1500;
		
		/// <summary>
		/// Initial previous ch2 value [us]
		/// </summary>
        private const int ch2PrevPrevValue = 1500;
		
		/// <summary>
		/// Maximum expected ch2 variation [us/sample]
		/// </summary>
        private const int ch2MaxVar = 300;
		
		/// <summary>
		/// Minimum expected ch3 [us]
		/// </summary>
        private const int ch3Min = 1000;
		
		/// <summary>
		/// Maximum expected ch2 [us]
		/// </summary>
        private const int ch3Max = 2000;
		
		/// <summary>
		/// Initial previous ch3 value [us]
		/// </summary>
        private const int ch3PrevValue = 1500;
		
		/// <summary>
		/// Initial previous ch3 value [us]
		/// </summary>
        private const int ch3PrevPrevValue = 1500;
		
		/// <summary>
		/// Maximum expected ch3 variation [us/sample]
		/// </summary>
        private const int ch3MaxVar = 300;
		
		/// <summary>
		/// Minimum expected ch4 [us]
		/// </summary>
        private const int ch4Min = 1000;
		
		/// <summary>
		/// Maximum expected ch4 [us]
		/// </summary>
        private const int ch4Max = 2000;
		
		/// <summary>
		/// Initial previous ch4 value [us]
		/// </summary>
        private const int ch4PrevValue = 1500;
		
		/// <summary>
		/// Initial previous ch4 value [us]
		/// </summary>
        private const int ch4PrevPrevValue = 1500;
        
		/// <summary>
		/// Maximum expected ch4 variation [us/sample]
		/// </summary>
		private const int ch4MaxVar = 300;

		/// <summary>
		/// Initializes a new instance of the <see cref="GroundStation.PwmMessage"/> class.
		/// </summary>
		/// <param name='time'>
		/// Timestamp
		/// </param>
		/// <param name='b'>
		/// Message as an array of bytes
		/// </param>
        public PwmMessage()
            : base()
        {
            this.ch1 = new Field(ch1Min, ch1Max, ch1PrevValue, ch1PrevPrevValue, ch1MaxVar);
            this.ch2 = new Field(ch2Min, ch2Max, ch2PrevValue, ch2PrevPrevValue, ch2MaxVar);
            this.ch3 = new Field(ch3Min, ch3Max, ch3PrevValue, ch3PrevPrevValue, ch3MaxVar);
            this.ch4 = new Field(ch4Min, ch4Max, ch4PrevValue, ch4PrevPrevValue, ch4MaxVar);
        }

		/// <summary>
		/// Creates the message.
		/// </summary>
		/// <param name='b'>
		/// Message as an array of bytes
		/// </param>
        public override void CreateMessage(ulong time, byte[] b)
        {
            Array.Reverse(b, 1, 2);
            this.ch1.V = BitConverter.ToUInt16(b, 1);
            Array.Reverse(b, 3, 2);
            this.ch2.V = BitConverter.ToUInt16(b, 3);
            Array.Reverse(b, 5, 2);
            this.ch3.V = BitConverter.ToUInt16(b, 5);
            Array.Reverse(b, 7, 2);
            this.ch4.V = BitConverter.ToUInt16(b, 7);
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
                this.ch1.V = Convert.ToUInt16(words[1]);
                this.ch2.V = Convert.ToUInt16(words[2]);
                this.ch3.V = Convert.ToUInt16(words[3]);
                this.ch4.V = Convert.ToUInt16(words[4]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in PWM: " + e.Message);
            }
        }
		
		/// <summary>
		/// Makes an exact copy of pwm
		/// </summary>
		/// <returns>
		/// The copy.
		/// </returns>
		/// <param name='pwm'>
		/// The pwm to be copied.
		/// </param>
        public static PwmMessage DeepCopy(PwmMessage pwm)
        {
            PwmMessage ans = new PwmMessage();
            ans.ch1 = Field.DeepCopy(pwm.ch1);
            ans.ch2 = Field.DeepCopy(pwm.ch2);
            ans.ch3 = Field.DeepCopy(pwm.ch3);
            ans.ch4 = Field.DeepCopy(pwm.ch4);
            return ans;
        }
    }
}
