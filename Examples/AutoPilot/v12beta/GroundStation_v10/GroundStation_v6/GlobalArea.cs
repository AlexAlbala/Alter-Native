using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroundStation
{
    public class GlobalArea
    {
		private object mutAdc, mutGps, mutPwm, mutImu;
        private AdcMessage adc = null;
        public AdcMessage Adc
        {
            get
            {
                if (this.adc == null)
                    return null;
                AdcMessage ans;
                lock (this.mutAdc)
                {
                    ans = AdcMessage.DeepCopy(this.adc);
                }
                return ans;
            }
            set
            {
                this.adc = value;
            }
        }
        private ImuEulerMessage imu = null;
        public ImuEulerMessage Imu
        {
            get
            {
                if (this.imu == null)
                    return null;
                ImuEulerMessage ans;
                lock (this.mutImu)
                {
                    ans = ImuEulerMessage.DeepCopy(this.imu);
                }
                return ans;
            }
            set
            {
                this.imu = value;
            }
        }
        private PwmMessage pwm = null;
        public PwmMessage Pwm
        {
            get
            {
                if (this.pwm == null)
                    return null;
                PwmMessage ans;
                lock (this.mutPwm)
                {
                    ans = PwmMessage.DeepCopy(this.pwm);
                }
                return ans;
            }
            set
            {
                this.pwm = value;
            }
        }
        private GpsMessage gps = null;
        public GpsMessage Gps
        {
            get
            {
                if (this.gps == null)
                    return null;
                GpsMessage ans;
                lock (this.mutGps)
                {
                    ans = GpsMessage.DeepCopy(this.gps);
                }
                return ans;
            }
            set
            {
                this.gps = value;
            }
        }

        private static GlobalArea instance = null;

        public static GlobalArea GetInstance()
        {
            if (instance == null)
                instance = new GlobalArea();
            return instance;
        }

        private GlobalArea()
        {
            this.mutAdc = new object();
            this.mutGps = new object();
            this.mutImu = new object();
            this.mutPwm = new object();
        }

		public bool IsReady()
		{
			return (this.adc != null && this.imu != null);
		}
    }
}
