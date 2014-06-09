namespace Interficie_Grafica
{
    partial class MainForm
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
            this.AltitudeLabel = new System.Windows.Forms.Label();
            this.AltitudeValue = new System.Windows.Forms.Label();
            this.AirspeedLabel = new System.Windows.Forms.Label();
            this.AirspeedValue = new System.Windows.Forms.Label();
            this.YawLabel = new System.Windows.Forms.Label();
            this.YawValue = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // AltitudeLabel
            // 
            this.AltitudeLabel.AutoSize = true;
            this.AltitudeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AltitudeLabel.Location = new System.Drawing.Point(57, 35);
            this.AltitudeLabel.Name = "AltitudeLabel";
            this.AltitudeLabel.Size = new System.Drawing.Size(142, 42);
            this.AltitudeLabel.TabIndex = 0;
            this.AltitudeLabel.Text = "Altitude";
            // 
            // AltitudeValue
            // 
            this.AltitudeValue.AutoSize = true;
            this.AltitudeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AltitudeValue.Location = new System.Drawing.Point(764, 11);
            this.AltitudeValue.Name = "AltitudeValue";
            this.AltitudeValue.Size = new System.Drawing.Size(98, 108);
            this.AltitudeValue.TabIndex = 1;
            this.AltitudeValue.Text = "0";
            // 
            // AirspeedLabel
            // 
            this.AirspeedLabel.AutoSize = true;
            this.AirspeedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AirspeedLabel.Location = new System.Drawing.Point(57, 135);
            this.AirspeedLabel.Name = "AirspeedLabel";
            this.AirspeedLabel.Size = new System.Drawing.Size(166, 42);
            this.AirspeedLabel.TabIndex = 2;
            this.AirspeedLabel.Text = "Airspeed";
            // 
            // AirspeedValue
            // 
            this.AirspeedValue.AutoSize = true;
            this.AirspeedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AirspeedValue.Location = new System.Drawing.Point(764, 116);
            this.AirspeedValue.Name = "AirspeedValue";
            this.AirspeedValue.Size = new System.Drawing.Size(98, 108);
            this.AirspeedValue.TabIndex = 3;
            this.AirspeedValue.Text = "0";
            // 
            // YawLabel
            // 
            this.YawLabel.AutoSize = true;
            this.YawLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YawLabel.Location = new System.Drawing.Point(57, 236);
            this.YawLabel.Name = "YawLabel";
            this.YawLabel.Size = new System.Drawing.Size(91, 42);
            this.YawLabel.TabIndex = 4;
            this.YawLabel.Text = "Yaw";
            // 
            // YawValue
            // 
            this.YawValue.AutoSize = true;
            this.YawValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YawValue.Location = new System.Drawing.Point(764, 212);
            this.YawValue.Name = "YawValue";
            this.YawValue.Size = new System.Drawing.Size(98, 108);
            this.YawValue.TabIndex = 5;
            this.YawValue.Text = "0";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 369);
            this.Controls.Add(this.YawValue);
            this.Controls.Add(this.YawLabel);
            this.Controls.Add(this.AirspeedValue);
            this.Controls.Add(this.AirspeedLabel);
            this.Controls.Add(this.AltitudeValue);
            this.Controls.Add(this.AltitudeLabel);
            this.Name = "MainForm";
            this.Text = "Attitude Monitor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AltitudeLabel;
        private System.Windows.Forms.Label AltitudeValue;
        private System.Windows.Forms.Label AirspeedLabel;
        private System.Windows.Forms.Label AirspeedValue;
        private System.Windows.Forms.Label YawLabel;
        private System.Windows.Forms.Label YawValue;
        private System.Windows.Forms.Timer timer1;
    }
}

