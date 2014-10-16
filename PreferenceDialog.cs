using System;
using RLToolkit;

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
			currentPrefs.size = initialPrefs.size;
		}

		public Prefs getPref()
		{
			this.Log ().Debug ("Fetching the Preferences");
			return currentPrefs;
		}

		public void setPref(Prefs.prefFormat formatIn, Prefs.prefSize sizeIn)
		{
			this.Log ().Debug (String.Format("Setting the Preferences to: Format: {0}, Size: {1}", formatIn.ToString(), sizeIn.ToString()));
			currentPrefs.format = formatIn;
			currentPrefs.size = sizeIn;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			this.Log ().Debug ("Dialog Accepted");
			// set the pref info
			currentPrefs.format = (Prefs.prefFormat)comboFormat.Active;
			currentPrefs.size = (Prefs.prefSize)comboSize.Active;

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

