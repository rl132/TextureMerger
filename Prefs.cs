using System;
using System.Drawing.Imaging;
using RLToolkit;

namespace TextureMerger
{
	public class Prefs
	{
		public prefFormat format { get; set; }
		public prefSize size { get; set; }

		public Prefs()
		{
			// set the defaults
			format = prefFormat.Bmp;
			size = prefSize.Size1024;
		}

		public enum prefFormat
		{
			Bmp,
			Png,
			Jpg
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

		public enum prefSize
		{
			Size1024
		}
	}
}

