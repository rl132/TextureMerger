using System;
using RLToolkit;
using Gtk;
using System.Text;

using RLToolkit.Extensions;

namespace TextureMerger
{ 
	public partial class PreferenceDialog : Gtk.Dialog
	{
		public Prefs currentPrefs = new Prefs();
		public Parameters currentParam = new Parameters();

		public PreferenceDialog (Prefs initialPrefs, Parameters initialParams)
		{
			this.Log ().Debug ("Creating a new Preference Dialog");
			this.Build ();

			// fill in the preferences & param
			currentPrefs = initialPrefs;
			currentParam = initialParams;
		
			// fill the preferences UI
			txtWidth.Text = currentPrefs.width.ToString ();
			txtHeight.Text = currentPrefs.height.ToString ();
			comboFormat.Active = (int)currentPrefs.format;
			chkProportion.Active = currentPrefs.keepProportion;

			// fill the parameter UI
			int index = comboPreviewSize.FindIndex(currentParam.previewSize.ToString());
			if (index != -1) {
				comboPreviewSize.Active = index;
			}
		}

		public Prefs GetPref()
		{
			this.Log ().Debug ("Fetching the Preferences");
			return currentPrefs;
		}

		public Parameters GetParam()
		{
			this.Log ().Debug ("Fetching the Parameters");
			return currentParam;
		}

		public void SetPref(Prefs input)
		{
			this.Log ().Debug (String.Format("Setting the Preferences"));
			currentPrefs = input;
		}

		public void SetParam(Parameters input)
		{
			this.Log ().Debug (String.Format("Setting the Parameters"));
			currentParam = input;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			this.Log ().Debug ("Dialog Accepted");

			bool hasError = false;
			string errorData = "";

			// Test for the new defaults
			Prefs prefsToTest = new Prefs ();
			prefsToTest.prefPrefix = "testDefaults";
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

			// test for the new parameters
			Parameters paramToTest = new Parameters ();
			output = 0;
			Int32.TryParse (comboPreviewSize.ActiveText, out output);
			paramToTest.previewSize = output;

			if (!paramToTest.ValidateParameters (out message)) {
				if (hasError) {
					errorData += Environment.NewLine;
				}
				hasError = true;
				errorData += message;
			}

			if (hasError) {
				// bad input
				MessageDialog result = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Invalid inputs. Please make sure you are using correct inputs." + Environment.NewLine + Environment.NewLine + "Error(s) found:" + Environment.NewLine + message);
				if (result.Run () == (int)ResponseType.Ok) {
					result.Destroy ();
				}
			} else {
				currentPrefs = prefsToTest;
				currentParam = paramToTest;
				currentPrefs.prefPrefix = "Default";
			}

			// clear the dialog
			this.Destroy ();
		}

		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			this.Log ().Debug ("Dialog Canceled");
			this.Destroy ();
		}

		protected void OnDeleteEvent (object sender, EventArgs e)
		{
			this.Log ().Debug ("Dialog Closed");
			this.Destroy ();
		}
	}
}