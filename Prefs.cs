using System;
using System.IO;
using System.Drawing.Imaging;

using RLToolkit;
using RLToolkit.Basic;

namespace TextureMerger
{
	public class Prefs
	{
		public prefFormat format { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public bool keepProportion { get; set; }

		private CfgManager mgr;
		private string prefFile;

		public Prefs()
		{
			this.Log ().Debug ("Constructor for a preference set");

			prefFile = Path.Combine (AppDomain.CurrentDomain.BaseDirectory, "pref.xml");
			mgr = new CfgManager (prefFile, typeof(XmlConfigSystem));
			mgr.ReadConfig ();
			// todo: initialze and maybe load config

			string input;
			int output;
			
			input = mgr.GetValue ("format");
			if (input != "") {
				Int32.TryParse (input, out output);
				format = (prefFormat)output;
			} else {
				format = prefFormat.Bmp;
			}
			
			input = mgr.GetValue ("width");
			if (input != "") {
				Int32.TryParse (input, out output);
				width = output;
			} else {
				width = 1024;
			}
			
			input = mgr.GetValue ("height");
			if (input != "") {
				Int32.TryParse (input, out output);
				height = output;
			} else {
				height = 1024;
			}

			bool outputBool;
			input = mgr.GetValue ("proportion");
			if (input != "") {
				bool.TryParse (input, out outputBool);
				keepProportion = outputBool;
			} else {
				keepProportion = true;
			}
		}

		public enum prefFormat
		{
			Bmp,
			Png,
			Jpg
		}

		public void savePref()
		{
			this.Log ().Info ("Trying to save the preferences");
			int output;
			Int32.TryParse (format.ToString (), out output);
			mgr.SetValue ("format", output.ToString());
			mgr.SetValue ("width", width.ToString());
			mgr.SetValue ("height", height.ToString());
			mgr.SetValue ("proportion", keepProportion.ToString());

			mgr.WriteConfig ();
		}

		public ImageFormat getImageFormat()
		{
			this.Log ().Debug ("Feching the Image Format Preference");
			switch (format)
			{
				case Prefs.prefFormat.Bmp:
				{
					return System.Drawing.Imaging.ImageFormat.Bmp;
				}
				case Prefs.prefFormat.Png:
				{
				return System.Drawing.Imaging.ImageFormat.Png;
				}
				case Prefs.prefFormat.Jpg:
				{
					return System.Drawing.Imaging.ImageFormat.Jpeg;
				}
				default:
				{
					return null;
				}
			}
		}
	}
}

