using System;
using System.Threading;
using Gtk;
using GroundStation;

namespace Visual
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Thread t = new Thread(new ThreadStart(GroundStation.Main.Run));
			t.Start();
			Console.WriteLine("Please wait...");
			Thread.Sleep(5000);
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
