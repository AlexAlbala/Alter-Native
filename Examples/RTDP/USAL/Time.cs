using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    public class Time
    {
        /// <summary>Float UAV local clock (ms) Range[UAV Range)</summary>
        public float Timef;
        /// <summary>Integer UAV local clock (ms) Range[UAV Range)</summary>
        public int TimeI;

        public Time()
        {
        }

        /// <summary>UAV local time</summary>
        /// <param name="Time"></param>
        public Time(float Time)
        {
            this.Timef = Time;

        }
        public float getmissionTime()
        {
            return (this.Timef);
        }
    }
}