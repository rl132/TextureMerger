using System;
using RLToolkit;
using Gtk;
using System.Text;

namespace TextureMerger
{
	public partial class PreferenceDialog : Gtk.Dialog
	{
		public Prefs currentPrefs = new Prefs();

		public PreferenceDialog (Prefs initialPrefs)
		{
			this.Log ().Debug ("Creating a new Preference Dialog");
			this.Build ();

			currentPrefs.format = initialPrefs.format;
			currentPrefs.width = initialPrefs.width;
			currentPrefs.height = initialPrefs.height;
			currentPrefs.keepProportion = initialPrefs.keepProportion;

			txtWidth.Text = currentPrefs.width.ToString ();
			txtHeight.Text = currentPrefs.height.ToString ();
			comboFormat.Active = (int)currentPrefs.format;
			chkProportion.Active = currentPrefs.keepProportion;
		}

		public Prefs getPref()
		{
			this.Log ().Debug ("Fetching the Preferences");
			return currentPrefs;
		}

		public void setPref(Prefs.prefFormat formatIn, int widthIn, int heightIn)
		{
			this.Log ().Debug (String.Format("Setting the Preferences to: Format: {0}, Size: {1}x{2}", formatIn.ToString(), widthIn, heightIn));
			currentPrefs.format = formatIn;
			currentPrefs.width = widthIn;
			currentPrefs.height = heightIn;
			currentPrefs.keepProportion = chkProportion.Active;
		}

		private bool ValidatePrefs(Prefs input, out string messages)
		{
			StringBuilder sb = new StringBuilder ();
			bool retVal = true;
			if (input.width <= 0) {
				retVal = false;
				sb.Append ("Width cannot be negative or zero." + Environment.NewLine);
			}
			if (input.height <= 0) {
				retVal = false;
				sb.Append ("Height cannot be negative or zero." + Environment.NewLine);
			}
			if ((input.format != Prefs.prefFormat.Bmp) && 
				(input.format != Prefs.prefFormat.Jpg) &&
				(input.format != Prefs.prefFormat.Png)) {
				retVal = false;
				sb.Append ("Pixel format not recognized." + Environment.NewLine);
			}
			messages = sb.ToString ();
			return retVal;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			this.Log ().Debug ("Dialog Accepted");
			// set the pref info

			Prefs prefsToTest = new Prefs ();
			prefsToTest.format = (Prefs.prefFormat)comboFormat.Active;
			int output;
			Int32.TryParse (txtWidth.Text, out output);
			prefsToTest.width = output;
			Int32.TryParse (txtHeight.Text, out output);
			prefsToTest.height = output;
			prefsToTest.keepProportion = chkProportion.Active;

			string message;
			if (ValidatePrefs (prefsToTest, out message)) {
				currentPrefs = prefsToTest;
				this.Destroy ();
			} else {
				// bad preferences
				MessageDialog result = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Preferences invalid. Please make sure you are using correct inputs." + Environment.NewLine + Environment.NewLine +"Error(s) found:" + Environment.NewLine + message);
				if (result.Run () == (int)ResponseType.Ok) {
					result.Destroy ();
				}
			}
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

