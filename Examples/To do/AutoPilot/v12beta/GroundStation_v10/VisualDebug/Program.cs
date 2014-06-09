using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace VisualDebug
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread th = new Thread(new ThreadStart(GroundStation.Main.Run));
            th.Start();
            GC.KeepAlive(th);
            Application.Run(new Form1());
        }
    }
}
