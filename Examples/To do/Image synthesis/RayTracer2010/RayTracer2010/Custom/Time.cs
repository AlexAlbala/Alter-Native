using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace Custom
{
    public class Time
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);

        public static long timer_frequency;
        public static double timer_ms_per_tick;

        static Time()
        {
            QueryPerformanceFrequency(out timer_frequency);
            timer_ms_per_tick = 1000.0 / timer_frequency;
        }

        /// <summary>
        /// Returns the current time in miliseconds. The timer is high resolution, so the number of milliseconds may be fractional.
        /// </summary>
        /// <returns></returns>
        public static double getTime()
        {
            long t;
            QueryPerformanceCounter(out t);
            return t * timer_ms_per_tick;
        }
    }
}