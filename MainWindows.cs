using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Gtk;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace TextureMerger
{
	public partial class MainWindows : Gtk.Window
	{
		private Prefs currentPrefs = new Prefs ();
		private string currentPath;

		public MainWindows () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			GLib.Idle.Add (new GLib.IdleHandler (initFields));
		}

		public static void Main (string[] args)
		{
			Application.Init ();
			TextureMerger.MainWindows win = new TextureMerger.MainWindows ();
			win.Show ();
			Application.Run ();
		}

		private bool initFields()
		{
			// set the title/version info
			title.Text = "Texture Merger " + Assembly.GetExecutingAssembly().GetName().Version;

			// set the selector's text
			selector1.setText ("Image 1: ");
			selector2.setText ("Image 2: ");
			selector3.setText ("Image 3: ");
			selector4.setText ("Image 4: ");

			// set the path to the current folder
			currentPath = AppDomain.CurrentDomain.BaseDirectory;
			lblPath.Text = currentPath;

			// update the UI for the preferences
			updateFromPref ();

			return false;
		}

		private bool showPrefDialog()
		{
			PreferenceDialog preferenceDg = new PreferenceDialog (currentPrefs);
			if (preferenceDg.Run() == (int)ResponseType.Ok)
			{
				// update the preferences
				currentPrefs = preferenceDg.getPref ();

				// update the UI
				updateFromPref ();
			}

			return false;
		}

		private bool updateFromPref()
		{
			// update the extension label
			switch (currentPrefs.format)
			{
				case Prefs.prefFormat.Bmp:
				{
					lblExtension.Text = ".bmp";
					break;
				}
				case Prefs.prefFormat.Png:
				{
					lblExtension.Text = ".png";
					break;
				}
				case Prefs.prefFormat.Jpg:
				{
					lblExtension.Text = ".jpg";
					break;
				}
				default:
				{
					lblExtension.Text = "N/A";
					break;
				}
			}

			return false;
		}

		private bool clearInfo()
		{
			// clear the selectors
			selector1.clear ();
			selector2.clear ();
			selector3.clear ();
			selector4.clear ();

			// reset the output
			currentPath = AppDomain.CurrentDomain.BaseDirectory;
			lblPath.Text = currentPath;
			txtFilename.Text = "Output";

			return false;
		}

		private void showTargetOutputDialog(string currentPath)
		{
			// new dialog
			FileChooserDialog fcd = new FileChooserDialog ("Select the output folder", this, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel, "Select", ResponseType.Ok);
			fcd.SetCurrentFolder (currentPath);

			if (fcd.Run() == (int)ResponseType.Ok)
			{
				// cache the folder
				currentPath = fcd.CurrentFolder;

				// update the UI
				lblPath.Text = currentPath;
			}
			fcd.Destroy ();
		}

		// todo: refactor if we can
		private System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
		{
			int sourceWidth = imgToResize.Width;
			int sourceHeight = imgToResize.Height;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)size.Width / (float)sourceWidth);
			nPercentH = ((float)size.Height / (float)sourceHeight);

			if (nPercentH < nPercentW)
				nPercent = nPercentH;
			else
				nPercent = nPercentW;

			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap b = new Bitmap(destWidth, destHeight);
			Graphics g = Graphics.FromImage((System.Drawing.Image)b);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
			g.Dispose();

			return (System.Drawing.Image)b;
		}

		// todo: refactor if we can
		private bool processImages()
		{
			MessageDialog result;

			// missing a target filename, return
			if (txtFilename.Text.Trim () == "") {
				result = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Output filename cannot be empty");
				if (result.Run () == (int)ResponseType.Ok) {
					result.Destroy ();
				}
				return false;
			}

			// missing a file, return
			if ((selector1.getFilename () == null) ||
				(selector2.getFilename () == null) ||
				(selector3.getFilename () == null) ||
				(selector4.getFilename () == null)) {

				result = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Missing content. Need to have 4 images for this to work.");
				if (result.Run () == (int)ResponseType.Ok) {
					result.Destroy ();
				}
				return false;
			}

			// initialize
			string finalImage = System.IO.Path.Combine(currentPath, txtFilename.Text + lblExtension.Text);
			Bitmap imgOutput = new Bitmap(1024, 1024);
			Graphics g = Graphics.FromImage(imgOutput);
			g.Clear(SystemColors.AppWorkspace);
			System.Drawing.Image img;

			// first one
			img = System.Drawing.Image.FromFile(selector1.getFilename());
			img = resizeImage(img, new Size(512,512));
			g.DrawImage(img, new Point(0, 0));
			img.Dispose ();

			// second one
			img = System.Drawing.Image.FromFile(selector2.getFilename());
			img = resizeImage(img, new Size(512,512));
			g.DrawImage(img, new Point(512, 0));
			img.Dispose ();

			// third one
			img = System.Drawing.Image.FromFile(selector3.getFilename());
			img = resizeImage(img, new Size(512,512));
			g.DrawImage(img, new Point(0, 512));
			img.Dispose ();

			// fourth one
			img = System.Drawing.Image.FromFile(selector4.getFilename());
			img = resizeImage(img, new Size(512,512));
			g.DrawImage(img, new Point(512, 512));
			img.Dispose ();

			// finish up
			g.Dispose();
			System.Drawing.Imaging.ImageFormat format;
			format = currentPrefs.getImageFormat ();
			imgOutput.Save(finalImage, format);
			imgOutput.Dispose();

			result = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Output successful");
			if (result.Run () == (int)ResponseType.Ok) {
				result.Destroy ();
			}

			return false;
		}

		// EVENTS HANDLERS
		protected void OnQuitActionActivated (object sender, EventArgs e)
		{
			Application.Quit ();
		}

		protected void OnDeleteEvent (object sender, EventArgs e)
		{
			Application.Quit ();
		}

		protected void OnBtnGoClicked (object sender, EventArgs e)
		{
			// make sure we have 4 loaded textures
			GLib.Idle.Add (new GLib.IdleHandler (processImages));
		}

		protected void OnClearActionActivated (object sender, EventArgs e)
		{
			GLib.Idle.Add (new GLib.IdleHandler (clearInfo));
		}

		protected void OnBtnBrowseClicked (object sender, EventArgs e)
		{
			showTargetOutputDialog (currentPath);
		}

		protected void OnPreferencesActionActivated (object sender, EventArgs e)
		{
			GLib.Idle.Add (new GLib.IdleHandler (showPrefDialog));
		}
	}
}

