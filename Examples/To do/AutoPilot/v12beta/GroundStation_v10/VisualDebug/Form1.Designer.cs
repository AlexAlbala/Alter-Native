namespace VisualDebug
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AdcGB = new System.Windows.Forms.GroupBox();
            this.TASText = new System.Windows.Forms.TextBox();
            this.AltitudeText = new System.Windows.Forms.TextBox();
            this.PitotText = new System.Windows.Forms.TextBox();
            this.ThermometerText = new System.Windows.Forms.TextBox();
            this.BarometerText = new System.Windows.Forms.TextBox();
            this.AltitudeLabel = new System.Windows.Forms.Label();
            this.TasLabel = new System.Windows.Forms.Label();
            this.PitotLabel = new System.Windows.Forms.Label();
            this.ThermometerLabel = new System.Windows.Forms.Label();
            this.BarometerLabel = new System.Windows.Forms.Label();
            this.ImuGB = new System.Windows.Forms.GroupBox();
            this.AccelZText = new System.Windows.Forms.TextBox();
            this.AccelZLabel = new System.Windows.Forms.Label();
            this.AccelXText = new System.Windows.Forms.TextBox();
            this.AccelYText = new System.Windows.Forms.TextBox();
            this.YawText = new System.Windows.Forms.TextBox();
            this.PitchText = new System.Windows.Forms.TextBox();
            this.RollText = new System.Windows.Forms.TextBox();
            this.AccelYLabel = new System.Windows.Forms.Label();
            this.AccelXLabel = new System.Windows.Forms.Label();
            this.YawLabel = new System.Windows.Forms.Label();
            this.PitchLabel = new System.Windows.Forms.Label();
            this.RollLabel = new System.Windows.Forms.Label();
            this.GpsGB = new System.Windows.Forms.GroupBox();
            this.CourseText = new System.Windows.Forms.TextBox();
            this.CourseLabel = new System.Windows.Forms.Label();
            this.UtmYText = new System.Windows.Forms.TextBox();
            this.GndSpeedText = new System.Windows.Forms.TextBox();
            this.UtmXText = new System.Windows.Forms.TextBox();
            this.LonText = new System.Windows.Forms.TextBox();
            this.LatText = new System.Windows.Forms.TextBox();
            this.GndSpeedLabel = new System.Windows.Forms.Label();
            this.UtmYLabel = new System.Windows.Forms.Label();
            this.UtmXLabel = new System.Windows.Forms.Label();
            this.LonLabel = new System.Windows.Forms.Label();
            this.LatLabel = new System.Windows.Forms.Label();
            this.PwmGB = new System.Windows.Forms.GroupBox();
            this.Ch9Text = new System.Windows.Forms.TextBox();
            this.Ch9Label = new System.Windows.Forms.Label();
            this.Ch8Text = new System.Windows.Forms.TextBox();
            this.Ch8Label = new System.Windows.Forms.Label();
            this.Ch7Text = new System.Windows.Forms.TextBox();
            this.Ch7Label = new System.Windows.Forms.Label();
            this.Ch6Text = new System.Windows.Forms.TextBox();
            this.Ch6Label = new System.Windows.Forms.Label();
            this.Ch4Text = new System.Windows.Forms.TextBox();
            this.Ch5Text = new System.Windows.Forms.TextBox();
            this.Ch3Text = new System.Windows.Forms.TextBox();
            this.Ch2Text = new System.Windows.Forms.TextBox();
            this.Ch1Text = new System.Windows.Forms.TextBox();
            this.Ch5Label = new System.Windows.Forms.Label();
            this.Ch4Label = new System.Windows.Forms.Label();
            this.Ch3Label = new System.Windows.Forms.Label();
            this.Ch2Label = new System.Windows.Forms.Label();
            this.Ch1Label = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.AdcGB.SuspendLayout();
            this.ImuGB.SuspendLayout();
            this.GpsGB.SuspendLayout();
            this.PwmGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // AdcGB
            // 
            this.AdcGB.Controls.Add(this.TASText);
            this.AdcGB.Controls.Add(this.AltitudeText);
            this.AdcGB.Controls.Add(this.PitotText);
            this.AdcGB.Controls.Add(this.ThermometerText);
            this.AdcGB.Controls.Add(this.BarometerText);
            this.AdcGB.Controls.Add(this.AltitudeLabel);
            this.AdcGB.Controls.Add(this.TasLabel);
            this.AdcGB.Controls.Add(this.PitotLabel);
            this.AdcGB.Controls.Add(this.ThermometerLabel);
            this.AdcGB.Controls.Add(this.BarometerLabel);
            this.AdcGB.Location = new System.Drawing.Point(19, 12);
            this.AdcGB.Name = "AdcGB";
            this.AdcGB.Size = new System.Drawing.Size(150, 145);
            this.AdcGB.TabIndex = 0;
            this.AdcGB.TabStop = false;
            this.AdcGB.Text = "ADC Data";
            // 
            // TASText
            // 
            this.TASText.Location = new System.Drawing.Point(103, 89);
            this.TASText.Name = "TASText";
            this.TASText.Size = new System.Drawing.Size(41, 20);
            this.TASText.TabIndex = 9;
            // 
            // AltitudeText
            // 
            this.AltitudeText.Location = new System.Drawing.Point(103, 112);
            this.AltitudeText.Name = "AltitudeText";
            this.AltitudeText.Size = new System.Drawing.Size(41, 20);
            this.AltitudeText.TabIndex = 8;
            // 
            // PitotText
            // 
            this.PitotText.Location = new System.Drawing.Point(103, 66);
            this.PitotText.Name = "PitotText";
            this.PitotText.Size = new System.Drawing.Size(41, 20);
            this.PitotText.TabIndex = 7;
            // 
            // ThermometerText
            // 
            this.ThermometerText.Location = new System.Drawing.Point(103, 44);
            this.ThermometerText.Name = "ThermometerText";
            this.ThermometerText.Size = new System.Drawing.Size(41, 20);
            this.ThermometerText.TabIndex = 6;
            // 
            // BarometerText
            // 
            this.BarometerText.Location = new System.Drawing.Point(103, 25);
            this.BarometerText.Name = "BarometerText";
            this.BarometerText.Size = new System.Drawing.Size(41, 20);
            this.BarometerText.TabIndex = 5;
            // 
            // AltitudeLabel
            // 
            this.AltitudeLabel.AutoSize = true;
            this.AltitudeLabel.Location = new System.Drawing.Point(8, 119);
            this.AltitudeLabel.Name = "AltitudeLabel";
            this.AltitudeLabel.Size = new System.Drawing.Size(42, 13);
            this.AltitudeLabel.TabIndex = 4;
            this.AltitudeLabel.Text = "Altitude";
            // 
            // TasLabel
            // 
            this.TasLabel.AutoSize = true;
            this.TasLabel.Location = new System.Drawing.Point(8, 96);
            this.TasLabel.Name = "TasLabel";
            this.TasLabel.Size = new System.Drawing.Size(28, 13);
            this.TasLabel.TabIndex = 3;
            this.TasLabel.Text = "TAS";
            // 
            // PitotLabel
            // 
            this.PitotLabel.AutoSize = true;
            this.PitotLabel.Location = new System.Drawing.Point(8, 73);
            this.PitotLabel.Name = "PitotLabel";
            this.PitotLabel.Size = new System.Drawing.Size(28, 13);
            this.PitotLabel.TabIndex = 2;
            this.PitotLabel.Text = "Pitot";
            // 
            // ThermometerLabel
            // 
            this.ThermometerLabel.AutoSize = true;
            this.ThermometerLabel.Location = new System.Drawing.Point(8, 51);
            this.ThermometerLabel.Name = "ThermometerLabel";
            this.ThermometerLabel.Size = new System.Drawing.Size(69, 13);
            this.ThermometerLabel.TabIndex = 1;
            this.ThermometerLabel.Text = "Thermometer";
            // 
            // BarometerLabel
            // 
            this.BarometerLabel.AutoSize = true;
            this.BarometerLabel.Location = new System.Drawing.Point(8, 28);
            this.BarometerLabel.Name = "BarometerLabel";
            this.BarometerLabel.Size = new System.Drawing.Size(55, 13);
            this.BarometerLabel.TabIndex = 0;
            this.BarometerLabel.Text = "Barometer";
            // 
            // ImuGB
            // 
            this.ImuGB.Controls.Add(this.AccelZText);
            this.ImuGB.Controls.Add(this.AccelZLabel);
            this.ImuGB.Controls.Add(this.AccelXText);
            this.ImuGB.Controls.Add(this.AccelYText);
            this.ImuGB.Controls.Add(this.YawText);
            this.ImuGB.Controls.Add(this.PitchText);
            this.ImuGB.Controls.Add(this.RollText);
            this.ImuGB.Controls.Add(this.AccelYLabel);
            this.ImuGB.Controls.Add(this.AccelXLabel);
            this.ImuGB.Controls.Add(this.YawLabel);
            this.ImuGB.Controls.Add(this.PitchLabel);
            this.ImuGB.Controls.Add(this.RollLabel);
            this.ImuGB.Location = new System.Drawing.Point(184, 12);
            this.ImuGB.Name = "ImuGB";
            this.ImuGB.Size = new System.Drawing.Size(150, 175);
            this.ImuGB.TabIndex = 10;
            this.ImuGB.TabStop = false;
            this.ImuGB.Text = "IMU Data";
            // 
            // AccelZText
            // 
            this.AccelZText.Location = new System.Drawing.Point(103, 135);
            this.AccelZText.Name = "AccelZText";
            this.AccelZText.Size = new System.Drawing.Size(41, 20);
            this.AccelZText.TabIndex = 11;
            // 
            // AccelZLabel
            // 
            this.AccelZLabel.AutoSize = true;
            this.AccelZLabel.Location = new System.Drawing.Point(8, 142);
            this.AccelZLabel.Name = "AccelZLabel";
            this.AccelZLabel.Size = new System.Drawing.Size(44, 13);
            this.AccelZLabel.TabIndex = 10;
            this.AccelZLabel.Text = "Accel Z";
            // 
            // AccelXText
            // 
            this.AccelXText.Location = new System.Drawing.Point(103, 89);
            this.AccelXText.Name = "AccelXText";
            this.AccelXText.Size = new System.Drawing.Size(41, 20);
            this.AccelXText.TabIndex = 9;
            // 
            // AccelYText
            // 
            this.AccelYText.Location = new System.Drawing.Point(103, 112);
            this.AccelYText.Name = "AccelYText";
            this.AccelYText.Size = new System.Drawing.Size(41, 20);
            this.AccelYText.TabIndex = 8;
            // 
            // YawText
            // 
            this.YawText.Location = new System.Drawing.Point(103, 66);
            this.YawText.Name = "YawText";
            this.YawText.Size = new System.Drawing.Size(41, 20);
            this.YawText.TabIndex = 7;
            // 
            // PitchText
            // 
            this.PitchText.Location = new System.Drawing.Point(103, 44);
            this.PitchText.Name = "PitchText";
            this.PitchText.Size = new System.Drawing.Size(41, 20);
            this.PitchText.TabIndex = 6;
            // 
            // RollText
            // 
            this.RollText.Location = new System.Drawing.Point(103, 25);
            this.RollText.Name = "RollText";
            this.RollText.Size = new System.Drawing.Size(41, 20);
            this.RollText.TabIndex = 5;
            // 
            // AccelYLabel
            // 
            this.AccelYLabel.AutoSize = true;
            this.AccelYLabel.Location = new System.Drawing.Point(8, 119);
            this.AccelYLabel.Name = "AccelYLabel";
            this.AccelYLabel.Size = new System.Drawing.Size(44, 13);
            this.AccelYLabel.TabIndex = 4;
            this.AccelYLabel.Text = "Accel Y";
            // 
            // AccelXLabel
            // 
            this.AccelXLabel.AutoSize = true;
            this.AccelXLabel.Location = new System.Drawing.Point(8, 96);
            this.AccelXLabel.Name = "AccelXLabel";
            this.AccelXLabel.Size = new System.Drawing.Size(44, 13);
            this.AccelXLabel.TabIndex = 3;
            this.AccelXLabel.Text = "Accel X";
            // 
            // YawLabel
            // 
            this.YawLabel.AutoSize = true;
            this.YawLabel.Location = new System.Drawing.Point(8, 73);
            this.YawLabel.Name = "YawLabel";
            this.YawLabel.Size = new System.Drawing.Size(28, 13);
            this.YawLabel.TabIndex = 2;
            this.YawLabel.Text = "Yaw";
            // 
            // PitchLabel
            // 
            this.PitchLabel.AutoSize = true;
            this.PitchLabel.Location = new System.Drawing.Point(8, 51);
            this.PitchLabel.Name = "PitchLabel";
            this.PitchLabel.Size = new System.Drawing.Size(31, 13);
            this.PitchLabel.TabIndex = 1;
            this.PitchLabel.Text = "Pitch";
            // 
            // RollLabel
            // 
            this.RollLabel.AutoSize = true;
            this.RollLabel.Location = new System.Drawing.Point(8, 28);
            this.RollLabel.Name = "RollLabel";
            this.RollLabel.Size = new System.Drawing.Size(25, 13);
            this.RollLabel.TabIndex = 0;
            this.RollLabel.Text = "Roll";
            // 
            // GpsGB
            // 
            this.GpsGB.Controls.Add(this.CourseText);
            this.GpsGB.Controls.Add(this.CourseLabel);
            this.GpsGB.Controls.Add(this.UtmYText);
            this.GpsGB.Controls.Add(this.GndSpeedText);
            this.GpsGB.Controls.Add(this.UtmXText);
            this.GpsGB.Controls.Add(this.LonText);
            this.GpsGB.Controls.Add(this.LatText);
            this.GpsGB.Controls.Add(this.GndSpeedLabel);
            this.GpsGB.Controls.Add(this.UtmYLabel);
            this.GpsGB.Controls.Add(this.UtmXLabel);
            this.GpsGB.Controls.Add(this.LonLabel);
            this.GpsGB.Controls.Add(this.LatLabel);
            this.GpsGB.Location = new System.Drawing.Point(351, 12);
            this.GpsGB.Name = "GpsGB";
            this.GpsGB.Size = new System.Drawing.Size(150, 175);
            this.GpsGB.TabIndex = 12;
            this.GpsGB.TabStop = false;
            this.GpsGB.Text = "GPS Data";
            // 
            // CourseText
            // 
            this.CourseText.Location = new System.Drawing.Point(103, 135);
            this.CourseText.Name = "CourseText";
            this.CourseText.Size = new System.Drawing.Size(41, 20);
            this.CourseText.TabIndex = 11;
            // 
            // CourseLabel
            // 
            this.CourseLabel.AutoSize = true;
            this.CourseLabel.Location = new System.Drawing.Point(8, 142);
            this.CourseLabel.Name = "CourseLabel";
            this.CourseLabel.Size = new System.Drawing.Size(78, 13);
            this.CourseLabel.TabIndex = 10;
            this.CourseLabel.Text = "Ground Course";
            // 
            // UtmYText
            // 
            this.UtmYText.Location = new System.Drawing.Point(103, 89);
            this.UtmYText.Name = "UtmYText";
            this.UtmYText.Size = new System.Drawing.Size(41, 20);
            this.UtmYText.TabIndex = 9;
            // 
            // GndSpeedText
            // 
            this.GndSpeedText.Location = new System.Drawing.Point(103, 112);
            this.GndSpeedText.Name = "GndSpeedText";
            this.GndSpeedText.Size = new System.Drawing.Size(41, 20);
            this.GndSpeedText.TabIndex = 8;
            // 
            // UtmXText
            // 
            this.UtmXText.Location = new System.Drawing.Point(103, 66);
            this.UtmXText.Name = "UtmXText";
            this.UtmXText.Size = new System.Drawing.Size(41, 20);
            this.UtmXText.TabIndex = 7;
            // 
            // LonText
            // 
            this.LonText.Location = new System.Drawing.Point(103, 44);
            this.LonText.Name = "LonText";
            this.LonText.Size = new System.Drawing.Size(41, 20);
            this.LonText.TabIndex = 6;
            // 
            // LatText
            // 
            this.LatText.Location = new System.Drawing.Point(103, 25);
            this.LatText.Name = "LatText";
            this.LatText.Size = new System.Drawing.Size(41, 20);
            this.LatText.TabIndex = 5;
            // 
            // GndSpeedLabel
            // 
            this.GndSpeedLabel.AutoSize = true;
            this.GndSpeedLabel.Location = new System.Drawing.Point(8, 119);
            this.GndSpeedLabel.Name = "GndSpeedLabel";
            this.GndSpeedLabel.Size = new System.Drawing.Size(76, 13);
            this.GndSpeedLabel.TabIndex = 4;
            this.GndSpeedLabel.Text = "Ground Speed";
            // 
            // UtmYLabel
            // 
            this.UtmYLabel.AutoSize = true;
            this.UtmYLabel.Location = new System.Drawing.Point(8, 96);
            this.UtmYLabel.Name = "UtmYLabel";
            this.UtmYLabel.Size = new System.Drawing.Size(41, 13);
            this.UtmYLabel.TabIndex = 3;
            this.UtmYLabel.Text = "UTM Y";
            // 
            // UtmXLabel
            // 
            this.UtmXLabel.AutoSize = true;
            this.UtmXLabel.Location = new System.Drawing.Point(8, 73);
            this.UtmXLabel.Name = "UtmXLabel";
            this.UtmXLabel.Size = new System.Drawing.Size(41, 13);
            this.UtmXLabel.TabIndex = 2;
            this.UtmXLabel.Text = "UTM X";
            // 
            // LonLabel
            // 
            this.LonLabel.AutoSize = true;
            this.LonLabel.Location = new System.Drawing.Point(8, 51);
            this.LonLabel.Name = "LonLabel";
            this.LonLabel.Size = new System.Drawing.Size(54, 13);
            this.LonLabel.TabIndex = 1;
            this.LonLabel.Text = "Longitude";
            // 
            // LatLabel
            // 
            this.LatLabel.AutoSize = true;
            this.LatLabel.Location = new System.Drawing.Point(8, 28);
            this.LatLabel.Name = "LatLabel";
            this.LatLabel.Size = new System.Drawing.Size(45, 13);
            this.LatLabel.TabIndex = 0;
            this.LatLabel.Text = "Latitude";
            // 
            // PwmGB
            // 
            this.PwmGB.Controls.Add(this.Ch9Text);
            this.PwmGB.Controls.Add(this.Ch9Label);
            this.PwmGB.Controls.Add(this.Ch8Text);
            this.PwmGB.Controls.Add(this.Ch8Label);
            this.PwmGB.Controls.Add(this.Ch7Text);
            this.PwmGB.Controls.Add(this.Ch7Label);
            this.PwmGB.Controls.Add(this.Ch6Text);
            this.PwmGB.Controls.Add(this.Ch6Label);
            this.PwmGB.Controls.Add(this.Ch4Text);
            this.PwmGB.Controls.Add(this.Ch5Text);
            this.PwmGB.Controls.Add(this.Ch3Text);
            this.PwmGB.Controls.Add(this.Ch2Text);
            this.PwmGB.Controls.Add(this.Ch1Text);
            this.PwmGB.Controls.Add(this.Ch5Label);
            this.PwmGB.Controls.Add(this.Ch4Label);
            this.PwmGB.Controls.Add(this.Ch3Label);
            this.PwmGB.Controls.Add(this.Ch2Label);
            this.PwmGB.Controls.Add(this.Ch1Label);
            this.PwmGB.Location = new System.Drawing.Point(521, 12);
            this.PwmGB.Name = "PwmGB";
            this.PwmGB.Size = new System.Drawing.Size(150, 291);
            this.PwmGB.TabIndex = 13;
            this.PwmGB.TabStop = false;
            this.PwmGB.Text = "PWM Data";
            // 
            // Ch9Text
            // 
            this.Ch9Text.Location = new System.Drawing.Point(103, 211);
            this.Ch9Text.Name = "Ch9Text";
            this.Ch9Text.Size = new System.Drawing.Size(41, 20);
            this.Ch9Text.TabIndex = 17;
            // 
            // Ch9Label
            // 
            this.Ch9Label.AutoSize = true;
            this.Ch9Label.Location = new System.Drawing.Point(8, 214);
            this.Ch9Label.Name = "Ch9Label";
            this.Ch9Label.Size = new System.Drawing.Size(26, 13);
            this.Ch9Label.TabIndex = 16;
            this.Ch9Label.Text = "Ch9";
            // 
            // Ch8Text
            // 
            this.Ch8Text.Location = new System.Drawing.Point(103, 185);
            this.Ch8Text.Name = "Ch8Text";
            this.Ch8Text.Size = new System.Drawing.Size(41, 20);
            this.Ch8Text.TabIndex = 15;
            // 
            // Ch8Label
            // 
            this.Ch8Label.AutoSize = true;
            this.Ch8Label.Location = new System.Drawing.Point(8, 188);
            this.Ch8Label.Name = "Ch8Label";
            this.Ch8Label.Size = new System.Drawing.Size(26, 13);
            this.Ch8Label.TabIndex = 14;
            this.Ch8Label.Text = "Ch8";
            // 
            // Ch7Text
            // 
            this.Ch7Text.Location = new System.Drawing.Point(103, 159);
            this.Ch7Text.Name = "Ch7Text";
            this.Ch7Text.Size = new System.Drawing.Size(41, 20);
            this.Ch7Text.TabIndex = 13;
            // 
            // Ch7Label
            // 
            this.Ch7Label.AutoSize = true;
            this.Ch7Label.Location = new System.Drawing.Point(8, 162);
            this.Ch7Label.Name = "Ch7Label";
            this.Ch7Label.Size = new System.Drawing.Size(26, 13);
            this.Ch7Label.TabIndex = 12;
            this.Ch7Label.Text = "Ch7";
            // 
            // Ch6Text
            // 
            this.Ch6Text.Location = new System.Drawing.Point(103, 135);
            this.Ch6Text.Name = "Ch6Text";
            this.Ch6Text.Size = new System.Drawing.Size(41, 20);
            this.Ch6Text.TabIndex = 11;
            // 
            // Ch6Label
            // 
            this.Ch6Label.AutoSize = true;
            this.Ch6Label.Location = new System.Drawing.Point(8, 142);
            this.Ch6Label.Name = "Ch6Label";
            this.Ch6Label.Size = new System.Drawing.Size(26, 13);
            this.Ch6Label.TabIndex = 10;
            this.Ch6Label.Text = "Ch6";
            // 
            // Ch4Text
            // 
            this.Ch4Text.Location = new System.Drawing.Point(103, 89);
            this.Ch4Text.Name = "Ch4Text";
            this.Ch4Text.Size = new System.Drawing.Size(41, 20);
            this.Ch4Text.TabIndex = 9;
            // 
            // Ch5Text
            // 
            this.Ch5Text.Location = new System.Drawing.Point(103, 112);
            this.Ch5Text.Name = "Ch5Text";
            this.Ch5Text.Size = new System.Drawing.Size(41, 20);
            this.Ch5Text.TabIndex = 8;
            // 
            // Ch3Text
            // 
            this.Ch3Text.Location = new System.Drawing.Point(103, 66);
            this.Ch3Text.Name = "Ch3Text";
            this.Ch3Text.Size = new System.Drawing.Size(41, 20);
            this.Ch3Text.TabIndex = 7;
            // 
            // Ch2Text
            // 
            this.Ch2Text.Location = new System.Drawing.Point(103, 44);
            this.Ch2Text.Name = "Ch2Text";
            this.Ch2Text.Size = new System.Drawing.Size(41, 20);
            this.Ch2Text.TabIndex = 6;
            // 
            // Ch1Text
            // 
            this.Ch1Text.Location = new System.Drawing.Point(103, 25);
            this.Ch1Text.Name = "Ch1Text";
            this.Ch1Text.Size = new System.Drawing.Size(41, 20);
            this.Ch1Text.TabIndex = 5;
            // 
            // Ch5Label
            // 
            this.Ch5Label.AutoSize = true;
            this.Ch5Label.Location = new System.Drawing.Point(8, 119);
            this.Ch5Label.Name = "Ch5Label";
            this.Ch5Label.Size = new System.Drawing.Size(26, 13);
            this.Ch5Label.TabIndex = 4;
            this.Ch5Label.Text = "Ch5";
            // 
            // Ch4Label
            // 
            this.Ch4Label.AutoSize = true;
            this.Ch4Label.Location = new System.Drawing.Point(8, 96);
            this.Ch4Label.Name = "Ch4Label";
            this.Ch4Label.Size = new System.Drawing.Size(54, 13);
            this.Ch4Label.TabIndex = 3;
            this.Ch4Label.Text = "Ch4 (yaw)";
            // 
            // Ch3Label
            // 
            this.Ch3Label.AutoSize = true;
            this.Ch3Label.Location = new System.Drawing.Point(8, 73);
            this.Ch3Label.Name = "Ch3Label";
            this.Ch3Label.Size = new System.Drawing.Size(58, 13);
            this.Ch3Label.TabIndex = 2;
            this.Ch3Label.Text = "Ch3 (pitch)";
            // 
            // Ch2Label
            // 
            this.Ch2Label.AutoSize = true;
            this.Ch2Label.Location = new System.Drawing.Point(8, 51);
            this.Ch2Label.Name = "Ch2Label";
            this.Ch2Label.Size = new System.Drawing.Size(48, 13);
            this.Ch2Label.TabIndex = 1;
            this.Ch2Label.Text = "Ch2 (roll)";
            // 
            // Ch1Label
            // 
            this.Ch1Label.AutoSize = true;
            this.Ch1Label.Location = new System.Drawing.Point(8, 28);
            this.Ch1Label.Name = "Ch1Label";
            this.Ch1Label.Size = new System.Drawing.Size(67, 13);
            this.Ch1Label.TabIndex = 0;
            this.Ch1Label.Text = "Ch1 (throttle)";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 315);
            this.Controls.Add(this.PwmGB);
            this.Controls.Add(this.GpsGB);
            this.Controls.Add(this.ImuGB);
            this.Controls.Add(this.AdcGB);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.AdcGB.ResumeLayout(false);
            this.AdcGB.PerformLayout();
            this.ImuGB.ResumeLayout(false);
            this.ImuGB.PerformLayout();
            this.GpsGB.ResumeLayout(false);
            this.GpsGB.PerformLayout();
            this.PwmGB.ResumeLayout(false);
            this.PwmGB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox AdcGB;
        private System.Windows.Forms.Label TasLabel;
        private System.Windows.Forms.Label PitotLabel;
        private System.Windows.Forms.Label ThermometerLabel;
        private System.Windows.Forms.Label BarometerLabel;
        private System.Windows.Forms.TextBox TASText;
        private System.Windows.Forms.TextBox AltitudeText;
        private System.Windows.Forms.TextBox PitotText;
        private System.Windows.Forms.TextBox ThermometerText;
        private System.Windows.Forms.TextBox BarometerText;
        private System.Windows.Forms.Label AltitudeLabel;
        private System.Windows.Forms.GroupBox ImuGB;
        private System.Windows.Forms.TextBox AccelXText;
        private System.Windows.Forms.TextBox AccelYText;
        private System.Windows.Forms.TextBox YawText;
        private System.Windows.Forms.TextBox PitchText;
        private System.Windows.Forms.TextBox RollText;
        private System.Windows.Forms.Label AccelYLabel;
        private System.Windows.Forms.Label AccelXLabel;
        private System.Windows.Forms.Label YawLabel;
        private System.Windows.Forms.Label PitchLabel;
        private System.Windows.Forms.Label RollLabel;
        private System.Windows.Forms.TextBox AccelZText;
        private System.Windows.Forms.Label AccelZLabel;
        private System.Windows.Forms.GroupBox GpsGB;
        private System.Windows.Forms.TextBox CourseText;
        private System.Windows.Forms.Label CourseLabel;
        private System.Windows.Forms.TextBox UtmYText;
        private System.Windows.Forms.TextBox GndSpeedText;
        private System.Windows.Forms.TextBox UtmXText;
        private System.Windows.Forms.TextBox LonText;
        private System.Windows.Forms.TextBox LatText;
        private System.Windows.Forms.Label GndSpeedLabel;
        private System.Windows.Forms.Label UtmYLabel;
        private System.Windows.Forms.Label UtmXLabel;
        private System.Windows.Forms.Label LonLabel;
        private System.Windows.Forms.Label LatLabel;
        private System.Windows.Forms.GroupBox PwmGB;
        private System.Windows.Forms.TextBox Ch9Text;
        private System.Windows.Forms.Label Ch9Label;
        private System.Windows.Forms.TextBox Ch8Text;
        private System.Windows.Forms.Label Ch8Label;
        private System.Windows.Forms.TextBox Ch7Text;
        private System.Windows.Forms.Label Ch7Label;
        private System.Windows.Forms.TextBox Ch6Text;
        private System.Windows.Forms.Label Ch6Label;
        private System.Windows.Forms.TextBox Ch4Text;
        private System.Windows.Forms.TextBox Ch5Text;
        private System.Windows.Forms.TextBox Ch3Text;
        private System.Windows.Forms.TextBox Ch2Text;
        private System.Windows.Forms.TextBox Ch1Text;
        private System.Windows.Forms.Label Ch5Label;
        private System.Windows.Forms.Label Ch4Label;
        private System.Windows.Forms.Label Ch3Label;
        private System.Windows.Forms.Label Ch2Label;
        private System.Windows.Forms.Label Ch1Label;
        private System.Windows.Forms.Timer timer1;
    }
}

