using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GroundStation;

namespace Interficie_Grafica
{
    public partial class MainForm : Form
    {
        private GlobalArea ga;

        public MainForm()
        {
            InitializeComponent();
        }

        public void SetAltitude()
        {
            if (this.ga.Adc != null)
            {
                double altitude = this.ga.Adc.altitude;
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(SetAltitude));
                    return;
                }
                this.AltitudeValue.Text = ((int)Math.Round(altitude)).ToString();
            }
        }

        public void SetYaw()
        {
            if (this.ga.Imu != null)
            {
                double yaw = this.ga.Imu.yaw.V;
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(SetYaw));
                    return;
                }
                this.YawValue.Text = ((int)Math.Round(yaw)).ToString();
            }
        }

        public void SetAirspeed()
        {
            if(this.ga.Adc != null)
            {
                double tas = this.ga.Adc.tas;

                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(SetAirspeed));
                    return;
                }
                this.AirspeedValue.Text = ((int)Math.Round(tas)).ToString();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.ga = GlobalArea.GetInstance();
            this.timer1.Interval = 500;
            this.timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.SetAirspeed();
            this.SetAltitude();
            this.SetYaw();
        }
    }
}
