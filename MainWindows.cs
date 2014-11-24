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
		private Prefs defaultPrefs = new Prefs ();
		private Parameters currentParams = new Parameters();
		private Configurations mgr = new Configurations();
		private string currentPath;

		private bool isUpdating = false;

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

			// load the prefs & parameters
			defaultPrefs.prefPrefix = "Default";
			defaultPrefs.LoadPrefsFromManager (mgr.GetManager ());
			currentPrefs.prefPrefix = "Current";
			currentPrefs.LoadPrefsFromManager (mgr.GetManager ());
			currentParams.LoadParametersFromManager (mgr.GetManager ());

			string notUsed;
			// make sure we have a default set of pref, in case it's empty
			if (!defaultPrefs.ValidatePrefs(out notUsed))
			{
				defaultPrefs = new Prefs ();
			}

			// make sure we have a current set of pref, in case it's empty
			if (!currentPrefs.ValidatePrefs(out notUsed))
			{
				currentPrefs = defaultPrefs;
			}

			// update the UI for the preferences
			updateFromPref ();

			return false;
		}

		private bool showPrefDialog()
		{
			this.Log ().Debug ("Trying to display the preference Dialog");

			PreferenceDialog preferenceDg = new PreferenceDialog (defaultPrefs, currentParams);
			if (preferenceDg.Run() == (int)ResponseType.Ok)
			{
				// update the preferences
				defaultPrefs = preferenceDg.GetPref ();
				currentParams = preferenceDg.GetParam ();

				// write the new prefs and params to the configuration manager
				defaultPrefs.SavePrefToManager(mgr.GetManager());
				currentParams.SaveParametersToManager (mgr.GetManager ());

				// save the configs
				mgr.Save ();

				// update the UI
				updateFromPref ();
			}

			return false;
		}

		private void updateSelectors()
		{
			// propagate the new size to the selector's children
			dynamicSelector.UpdateSize (currentParams.previewSize);
		}

		private void updateFromPref()
		{
			this.Log ().Debug ("Updating preferences");

			isUpdating = true;

			// update the extension label
			switch (currentPrefs.format)
			{
				case Prefs.prefFormat.Bmp:
				{
					lblExtension.Text = ".bmp";
					comboFormat.Active = 0;
					break;
				}
				case Prefs.prefFormat.Png:
				{
					lblExtension.Text = ".png";
					comboFormat.Active = 1;
					break;
				}
				case Prefs.prefFormat.Jpg:
				{
					lblExtension.Text = ".jpg";
					comboFormat.Active = 2;
					break;
				}
				default:
				{
					lblExtension.Text = "N/A";
					comboFormat.Active = 0;
					break;
				}
			}

			// update the params
			txtWidth.Text = currentPrefs.width.ToString ();
			txtHeight.Text = currentPrefs.height.ToString ();
			chkProportion.Active = currentPrefs.keepProportion;

			// update selectors amount
			updateSelectors ();

			isUpdating = false;
		}

		private bool updatePrefs()
		{
			if (isUpdating) {
				return false;
			}

			this.Log ().Debug ("Updating the prefs");

			bool hasError = false;
			string errorData = "";

			// Test for the new defaults
			Prefs prefsToTest = new Prefs ();
			prefsToTest.prefPrefix = "testCurrent";
			prefsToTest.format = (Prefs.prefFormat)comboFormat.Active;
			int output;
			Int32.TryParse (txtWidth.Text, out output);
			prefsToTest.width = output;
			Int32.TryParse (txtHeight.Text, out output);
			prefsToTest.height = output;
			prefsToTest.keepProportion = chkProportion.Active;

			string message;
			if (!prefsToTest.ValidatePrefs (out message)) {
				hasError = true;
				errorData += message;
			}

			if (hasError) {
				// update with defaults
				if (prefsToTest.width <= 0) {
					txtWidth.Text = defaultPrefs.width.ToString ();
				}
				if (prefsToTest.height <= 0) {
					txtHeight.Text = defaultPrefs.height.ToString();
				}

				// bad input
				MessageDialog result = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Invalid inputs. Please make sure you are using correct inputs." + Environment.NewLine + Environment.NewLine + "Error(s) found:" + Environment.NewLine + message);
				if (result.Run () == (int)ResponseType.Ok) {
					result.Destroy ();
				}


			} else {
				currentPrefs = prefsToTest;
				currentPrefs.prefPrefix = "Current";

				currentPrefs.SavePrefToManager (mgr.GetManager());
				mgr.Save ();
				updateFromPref ();
			}
				
			return true;
		}

		private bool clearInfo()
		{
			this.Log ().Debug ("Clearing the content");

			// reset the output
			currentPath = AppDomain.CurrentDomain.BaseDirectory;
			lblPath.Text = currentPath;
			txtFilename.Text = "Output";

			// update the prefs
			currentPrefs = defaultPrefs;
			currentPrefs.prefPrefix = "Current";
			currentPrefs.SavePrefToManager (mgr.GetManager());
			mgr.Save ();
			updateFromPref ();

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

		protected void OnComboFormatChanged (object sender, EventArgs e)
		{
			this.Log ().Debug ("Switching format");
			updatePrefs ();
		}

		protected void OnChkProportionToggled (object sender, EventArgs e)
		{
			this.Log ().Debug ("Proportion Toggled On/Off");
			updatePrefs ();
		}

		protected void OnTxtWidthFocusOutEvent (object o, FocusOutEventArgs args)
		{
			this.Log ().Debug ("Width Text changed - lost focus");
			updatePrefs ();
		}

		protected void OnTxtHeightFocusOutEvent (object o, FocusOutEventArgs args)
		{
			this.Log ().Debug ("Height Text changed - lost focus");
			updatePrefs ();

		}
	}
}