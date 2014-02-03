using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GroundStation;

namespace VisualDebug
{
    public partial class Form1 : Form
    {
        private GlobalArea ga;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.SetAdc();
            this.SetGps();
            this.SetPwm();
            this.SetImu();
         }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ga = GlobalArea.GetInstance();
            this.timer1.Interval = 500;
            this.timer1.Enabled = true;
        }

        private void SetAdc()
        {
            AdcMessage adc = this.ga.Adc;
            if (adc != null)
            {
                double bar = adc.barometer.V;
                double therm = adc.termometer.V;
                double pit = adc.pitot.V;
                double tas = adc.tas;
                double alt = adc.altitude;
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(SetAdc));
                    return;
                }
                this.BarometerText.Text = bar.ToString("0");
                this.ThermometerText.Text = therm.ToString("0");
                this.PitotText.Text = pit.ToString("0.00");
                this.TASText.Text = tas.ToString("0");
                this.AltitudeText.Text = alt.ToString("0");
            }
        }

        private void SetImu()
        {
            ImuEulerMessage imu = this.ga.Imu;
            if(imu != null)
            {
                double roll = imu.roll.V;
                double pitch= imu.pitch.V;
                double yaw = imu.yaw.V;
                double accx = imu.accelX.V;
                double accy = imu.accelY.V;
                double accz = imu.accelZ.V;
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(SetAdc));
                    return;
                }
                this.RollText.Text = roll.ToString("0");
                this.PitchText.Text = pitch.ToString("0");
                this.YawText.Text = yaw.ToString("0");
                this.AccelXText.Text = accx.ToString("0.000");
                this.AccelYText.Text = accy.ToString("0.000");
                this.AccelZText.Text = accz.ToString("0.000");
            }
        }

        private void SetGps()
        {
            GpsMessage gps = this.ga.Gps;
            if (gps != null)
            {
                double lat = gps.latitude.V;
                double lon = gps.longitude.V;
                double utmx = gps.pos.getUtmX();
                double utmy = gps.pos.getUtmY();
                double gndspeed = gps.gndSpeed.V;
                double course = gps.trackAngle.V;
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(SetAdc));
                    return;
                }
                this.LatText.Text = lat.ToString("0.0000000");
                this.LonText.Text = lon.ToString("0.0000000");
                this.UtmXText.Text = utmx.ToString("0");
                this.UtmYText.Text = utmy.ToString("0");
                this.GndSpeedText.Text = gndspeed.ToString("0");
                this.CourseText.Text = course.ToString("0");
            }
        }

        private void SetPwm()
        {
            PwmMessage pwm = this.ga.Pwm;
            if (pwm != null)
            {
                double ch1 = pwm.ch1.V;
                double ch2 = pwm.ch2.V;
                double ch3 = pwm.ch3.V;
                double ch4 = pwm.ch4.V;
                double ch5 = pwm.ch5.V;
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(SetAdc));
                    return;
                }
                this.Ch1Text.Text = ch1.ToString("0");
                this.Ch2Text.Text = ch2.ToString("0");
                this.Ch3Text.Text = ch3.ToString("0");
                this.Ch4Text.Text = ch4.ToString("0");
                this.Ch5Text.Text = ch5.ToString("0");
            }
        }
    }
}
