using System;
using System.IO;

namespace GroundStation
{
	public class Path
	{
		private static readonly string path = "Logs/";
		private string folder;
		public Path ()
		{
			string[] dirPaths = Directory.GetDirectories(path);
			this.folder = dirPaths.Length.ToString();
			Directory.CreateDirectory(path + this.folder);
		}
		
		public string GetPath()
		{
			return (path + this.folder);
		}
	}
}

