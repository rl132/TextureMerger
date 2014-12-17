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
		#region variables
		private Prefs currentPrefs = new Prefs ();
		private Prefs defaultPrefs = new Prefs ();
		private Parameters currentParams = new Parameters();
		private Configurations mgr = new Configurations();
		private string currentPath;
		private bool isUpdating = false;
		#endregion

		#region program
		public static void Main (string[] args)
		{
			LogManager.Instance.Log ().Info ("Application start.");
			Application.Init ();
			TextureMerger.MainWindows win = new TextureMerger.MainWindows ();
			win.Show ();
			Application.Run ();
		}
		#endregion

		#region main window constructor
		public MainWindows () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Log ().Debug ("Creating a new TextureMerger main window");
			this.Build ();
			dynamicrow1.SetControlType (typeof(RLToolkit.Widgets.DynamicColumn), new object[] {typeof(RLToolkit.Widgets.ButtonImage)});

			GLib.Idle.Add (new GLib.IdleHandler (initFields));
		}
		#endregion

		#region initialization
		private bool initFields()
		{
			this.Log ().Debug ("Initialization");

			// set the title/version info
			title.Text = "Texture Merger " + Assembly.GetExecutingAssembly().GetName().Version;

			// set the path to the current folder
			currentPath = AppDomain.CurrentDomain.BaseDirectory;
			lblPath.Text = currentPath;

			// load the prefs & parameters
			this.Log ().Debug ("Loading initial preferences");
			defaultPrefs.prefPrefix = "Default";
			defaultPrefs.LoadPrefsFromManager (mgr.GetManager ());
			currentPrefs.prefPrefix = "Current";
			currentPrefs.LoadPrefsFromManager (mgr.GetManager ());
			currentParams.LoadParametersFromManager (mgr.GetManager ());

			string notUsed;
			// make sure we have a default set of pref, in case it's empty
			if (!defaultPrefs.ValidatePrefs(out notUsed))
			{
				this.Log ().Warn ("no Default preferences found, using hardcoded defaults");
				defaultPrefs = new Prefs ();
			}

			// make sure we have a current set of pref, in case it's empty
			if (!currentPrefs.ValidatePrefs(out notUsed))
			{
				this.Log ().Warn ("no Current preferences found, using Defaults");
				currentPrefs = defaultPrefs;
			}

			// update the UI for the preferences
			updateFromPref ();

			// add the EH for the row controller
			dynamicrow1.OnNewControl += OnNewControl_Row;

			// update the events for the +/- on the dymnamic controls
			AddEventHandlers ();

			return false;
		}
		#endregion

		#region methods - Actions
		private bool showPrefDialog()
		{
			this.Log ().Debug ("Trying to display the preference Dialog");

			PreferenceDialog preferenceDg = new PreferenceDialog (defaultPrefs, currentParams);
			if (preferenceDg.Run() == (int)ResponseType.Ok)
			{
				this.Log ().Debug ("dialog accepted. trying to save prefs.");
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

		private bool clearInfo()
		{
			this.Log ().Info ("Clearing the content");

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
			ClearSelector ();

			return false;
		}

		private void ClearSelector()
		{
			Gtk.Widget[] arrayCol = dynamicrow1.GetControlArray ();
			Gtk.Widget[] newArrayCol = new Gtk.Widget[arrayCol.Length]; 

			int i = 0;
			foreach (RLToolkit.Widgets.DynamicColumn col in arrayCol) {

				RLToolkit.Widgets.DynamicColumn newCol = col;
				Gtk.Widget[] arrayRow = newCol.GetControlArray ();
				Gtk.Widget[] newArrayRow = new Gtk.Widget[arrayRow.Length]; 

				int j = 0;
				foreach (RLToolkit.Widgets.ButtonImage b in arrayRow) {
					b.Clear ();
					newArrayRow [j] = b;
					j++;
				}

				newCol.SetControlArray(newArrayRow);
				newArrayCol [i] = newCol;
				i++;
			}

			dynamicrow1.SetControlArray (newArrayCol);
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

				this.Log ().Info ("Changing the output target to: " + currentPath);
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
			Bitmap[][] inputArray = FetchImages();

			// initialize
			this.Log ().Info ("Generation of the target bitmap");
			TextureHandler output = new TextureHandler (currentPrefs.width, currentPrefs.height);
			output.Combine (inputArray, currentPrefs.keepProportion);
			string finalImage = System.IO.Path.Combine(currentPath, txtFilename.Text + lblExtension.Text);
			output.Save (finalImage);

			output.Dispose ();

			this.Log ().Info ("Output successful");
			result = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Output successful");
			if (result.Run () == (int)ResponseType.Ok) {
				result.Destroy ();
			}

			return false;
		}

		private Bitmap[][] FetchImages()
		{
			Gtk.Widget[] array = dynamicrow1.GetControlArray ();
			int maxY = array.Length;
			int maxX = 1;

			// find out how many X we have at most
			foreach (RLToolkit.Widgets.DynamicColumn col in array) {
				Gtk.Widget[] arraybutton = col.GetControlArray ();
				if (maxX < arraybutton.Length) {
					maxX = arraybutton.Length;
				}
			}

			// init the output size
			Bitmap[][] output = new Bitmap[maxY][];

			// fetch the images of each nodes and make the final output
			int i = 0;
			foreach (RLToolkit.Widgets.DynamicColumn col in array) {
				Bitmap[] line = new Bitmap[maxX];
				int j = 0;
				foreach (RLToolkit.Widgets.ButtonImage b in col.GetControlArray()) {
					line [j] = b.GetImage ();
					j++;
				}
				output [i] = line;
				i++;
			}

			// return the full output
			return output;
		}
		#endregion

		#region methods - Updates
		private void updateSelectors()
		{
			this.Log ().Debug ("Sending an update request to the dynamic selector");
			// propagate the new size to the selector's children

			Gtk.Widget[] arrayCol = dynamicrow1.GetControlArray ();
			Gtk.Widget[] newArrayCol = new Gtk.Widget[arrayCol.Length]; 

			int i = 0;
			foreach (RLToolkit.Widgets.DynamicColumn col in arrayCol) {

				RLToolkit.Widgets.DynamicColumn newCol = col;
				Gtk.Widget[] arrayRow = newCol.GetControlArray ();
				Gtk.Widget[] newArrayRow = new Gtk.Widget[arrayRow.Length]; 

				int j = 0;
				foreach (RLToolkit.Widgets.ButtonImage b in arrayRow) {
					b.UpdateSize (currentParams.previewSize);
					newArrayRow [j] = b;
					j++;
				}

				newCol.HeightRequest = (currentParams.previewSize > 64) ? currentParams.previewSize : 64;
				newCol.SetControlArray(newArrayRow);
				newArrayCol [i] = newCol;
				i++;
			}

			dynamicrow1.SetControlArray (newArrayCol);
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

			this.Log ().Debug ("Update completed");
			isUpdating = false;
		}

		private bool updatePrefs()
		{
			this.Log ().Debug ("Updating the prefs");

			if (isUpdating) {
				this.Log ().Debug ("UI is currently updating. No need to update.");
				return false;
			}

			bool hasError = false;
			string errorData = "";

			// Test for the new defaults
			this.Log ().Info ("Validating input preferences");
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
				this.Log ().Warn ("Error found in the validation: " + Environment.NewLine + Environment.NewLine + message);
				// update with defaults
				if (prefsToTest.width <= 0) {
					this.Log ().Debug ("Bad width input, using default");
					txtWidth.Text = defaultPrefs.width.ToString ();
				}
				if (prefsToTest.height <= 0) {
					this.Log ().Debug ("Bad height input, using default");
					txtHeight.Text = defaultPrefs.height.ToString();
				}

				// bad input
				MessageDialog result = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Invalid inputs. Please make sure you are using correct inputs." + Environment.NewLine + Environment.NewLine + "Error(s) found:" + Environment.NewLine + message);
				if (result.Run () == (int)ResponseType.Ok) {
					result.Destroy ();
				}
			} else {
				this.Log ().Info ("Updating valid prefs and save");
				currentPrefs = prefsToTest;
				currentPrefs.prefPrefix = "Current";

				currentPrefs.SavePrefToManager (mgr.GetManager());
				mgr.Save ();
				updateFromPref ();
			}
				
			return true;
		}

		private void AddEventHandlers()
		{
			this.Log ().Debug ("Adding event handlers to all column controls");
			// iterate through the control and add the event handler
			Gtk.Widget[] arrayControl = dynamicrow1.GetControlArray ();
			Gtk.Widget[] newArray = new Gtk.Widget[arrayControl.Length];

			int i = 0;
			foreach (RLToolkit.Widgets.DynamicColumn c in arrayControl) {
				c.OnNewControl -= OnNewControl_Col;
				c.OnNewControl += OnNewControl_Col;
				newArray [i] = c;
				i++;
			}

			dynamicrow1.SetControlArray (newArray);
		}
		#endregion

		#region evenbt handlers
		protected void OnNewControl_Row(object sender, EventArgs e)
		{
			this.Log ().Debug ("new row added, refreshing events and size");

			// add the eventhandler for the size update on the new row
			AddEventHandlers ();

			// update the size of the one around
			updateSelectors ();
		}

		protected void OnNewControl_Col(object sender, EventArgs e)
		{
			this.Log ().Debug ("New Button added, refreshing size");
			// update the size of each control
			updateSelectors ();
		}

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
		#endregion
	}
}