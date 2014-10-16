using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Gtk;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using GLib;
using RLToolkit;

namespace TextureMerger
{
	public partial class MainWindows : Gtk.Window
	{
		private Prefs currentPrefs = new Prefs ();
		private string currentPath;

		public MainWindows () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Log ().Debug ("Creating a new TextureMerger main window");
			this.Build ();
			GLib.Idle.Add (new GLib.IdleHandler (initFields));
		}

		public static void Main (string[] args)
		{
			LogManager.Instance.Log ().Info ("Application start.");
			Application.Init ();
			TextureMerger.MainWindows win = new TextureMerger.MainWindows ();
			win.Show ();
			Application.Run ();
		}

		private bool initFields()
		{
			this.Log ().Debug ("Initialization");

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
			this.Log ().Debug ("Trying to display the preference Dialog");

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
			this.Log ().Debug ("Updating preferences");

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
			this.Log ().Debug ("Clearing the content");

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
			this.Log ().Debug ("Trying to change output target");

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
			this.Log ().Debug (string.Format("Resizing an image"));

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
			this.Log ().Debug ("Processing the final image");
			MessageDialog result;

			// missing a target filename, return
			if (txtFilename.Text.Trim () == "") {
				this.Log ().Warn ("Target Filename empty. Abort!");
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

				this.Log ().Warn ("One of the selector is empty. Abort!");
				result = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Missing content. Need to have 4 images for this to work.");
				if (result.Run () == (int)ResponseType.Ok) {
					result.Destroy ();
				}
				return false;
			}

			// initialize
			this.Log ().Info ("Generation of the target bitmap");
			string finalImage = System.IO.Path.Combine(currentPath, txtFilename.Text + lblExtension.Text);
			Bitmap imgOutput = new Bitmap(1024, 1024);
			Graphics g = Graphics.FromImage(imgOutput);
			g.Clear(SystemColors.AppWorkspace);
			System.Drawing.Image img;

			// first one
			this.Log ().Info ("preparation and draw of the first image");
			img = System.Drawing.Image.FromFile(selector1.getFilename());
			img = resizeImage(img, new Size(512,512));
			g.DrawImage(img, new Point(0, 0));
			img.Dispose ();

			// second one
			this.Log ().Info ("preparation and draw of the second image");
			img = System.Drawing.Image.FromFile(selector2.getFilename());
			img = resizeImage(img, new Size(512,512));
			g.DrawImage(img, new Point(512, 0));
			img.Dispose ();

			// third one
			this.Log ().Info ("preparation and draw of the third image");
			img = System.Drawing.Image.FromFile(selector3.getFilename());
			img = resizeImage(img, new Size(512,512));
			g.DrawImage(img, new Point(0, 512));
			img.Dispose ();

			// fourth one
			this.Log ().Info ("preparation and draw of the fourth image");
			img = System.Drawing.Image.FromFile(selector4.getFilename());
			img = resizeImage(img, new Size(512,512));
			g.DrawImage(img, new Point(512, 512));
			img.Dispose ();

			// finish up
			this.Log ().Info ("Wrapping up the final bitmap");
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
			this.Log ().Debug ("Closing");
			Application.Quit ();
		}

		protected void OnDeleteEvent (object sender, EventArgs e)
		{
			this.Log ().Debug ("Closing");
			Application.Quit ();
		}

		protected void OnBtnGoClicked (object sender, EventArgs e)
		{
			this.Log ().Debug ("button Merge clicked");
			// make sure we have 4 loaded textures
			GLib.Idle.Add (new GLib.IdleHandler (processImages));
		}

		protected void OnClearActionActivated (object sender, EventArgs e)
		{
			this.Log ().Debug ("menu Clear clicked");
			GLib.Idle.Add (new GLib.IdleHandler (clearInfo));
		}

		protected void OnBtnBrowseClicked (object sender, EventArgs e)
		{
			this.Log ().Debug ("button Browse clicked");
			showTargetOutputDialog (currentPath);
		}

		protected void OnPreferencesActionActivated (object sender, EventArgs e)
		{
			this.Log ().Debug ("menu Preferences clicked");
			GLib.Idle.Add (new GLib.IdleHandler (showPrefDialog));
		}
	}
}

