using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Gtk;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using GLib;

using RLToolkit;
using RLToolkit.Basic;

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

				// save the new prefs
				currentPrefs.savePref ();

				// update the UI
				updateFromPref ();
			}

			return false;
		}

		private void updateSelectors()
		{
			// TODO: update the selector (and it items) for size changes.
		}

		private void updateFromPref()
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

			// update selectors amount
			updateSelectors ();
		}

		private bool clearInfo()
		{
			this.Log ().Debug ("Clearing the content");

			// reset the output
			currentPath = AppDomain.CurrentDomain.BaseDirectory;
			lblPath.Text = currentPath;
			txtFilename.Text = "Output";

			// selector reset
			dynamicSelector.Clear ();

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

			this.Log ().Info ("Fetching the bitmaps");
			Bitmap[][] inputArray = dynamicSelector.GetImages();

			// initialize
			this.Log ().Info ("Generation of the target bitmap");
			TextureHandler output = new TextureHandler (currentPrefs.width, currentPrefs.height);
			output.Combine (inputArray, currentPrefs.keepProportion);
			string finalImage = System.IO.Path.Combine(currentPath, txtFilename.Text + lblExtension.Text);
			output.Save (finalImage);

			output.Dispose ();

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