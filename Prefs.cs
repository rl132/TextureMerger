using System;
using System.Text;

using RLToolkit;
using RLToolkit.Basic;

namespace TextureMerger
{
	public class Prefs
	{
		public string prefPrefix { get; set; }
		public prefFormat format { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public bool keepProportion { get; set; }

		public enum prefFormat
		{
			Bmp,
			Png,
			Jpg
		}

		public Prefs ()
		{
			this.Log ().Debug ("Constructor for a preference set using default values.");
			prefPrefix = "unnamed";
			format = prefFormat.Bmp;
			width = 1024;
			height = 1024;
			keepProportion = true;
		}

		public void LoadPrefsFromManager(CfgManager mgr)
		{
			this.Log ().Debug ("Loading a preference set using the manager");

			string input;
			int output;

			input = mgr.GetValue (prefPrefix + "_format");
			if (input != "") {
				Int32.TryParse (input, out output);
				format = (prefFormat)output;
			} else {
				format = prefFormat.Bmp;
			}
			
			input = mgr.GetValue (prefPrefix + "_width");
			if (input != "") {
				Int32.TryParse (input, out output);
				width = output;
			} else {
				width = 1024;
			}
			
			input = mgr.GetValue (prefPrefix + "_height");
			if (input != "") {
				Int32.TryParse (input, out output);
				height = output;
			} else {
				height = 1024;
			}

			bool outputBool;
			input = mgr.GetValue (prefPrefix + "_proportion");
			if (input != "") {
				bool.TryParse (input, out outputBool);
				keepProportion = outputBool;
			} else {
				keepProportion = true;
			}
		}

		public void SavePrefToManager(CfgManager mgr)
		{
			this.Log ().Info ("Trying to set the values of the preferences in the configuration manager");

			mgr.SetValue (prefPrefix + "_format", ((int)format).ToString());
			mgr.SetValue (prefPrefix + "_width", width.ToString());
			mgr.SetValue (prefPrefix + "_height", height.ToString());
			mgr.SetValue (prefPrefix + "_proportion", keepProportion.ToString());
		}

		public bool ValidatePrefs(out string messages)
		{
			StringBuilder sb = new StringBuilder ();
			bool retVal = true;
			if (width <= 0) {
				retVal = false;
				sb.Append ("Width cannot be negative or zero." + Environment.NewLine);
			}
			if (height <= 0) {
				retVal = false;
				sb.Append ("Height cannot be negative or zero." + Environment.NewLine);
			}
			if ((format != Prefs.prefFormat.Bmp) && 
			    (format != Prefs.prefFormat.Jpg) &&
			    (format != Prefs.prefFormat.Png)) {
				retVal = false;
				sb.Append ("Pixel format not recognized." + Environment.NewLine);
			}
			messages = sb.ToString ();
			return retVal;
		}

		public System.Drawing.Imaging.ImageFormat GetImageFormat()
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