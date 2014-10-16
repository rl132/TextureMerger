using System;

namespace TextureMerger
{
	public partial class PreferenceDialog : Gtk.Dialog
	{
		public Prefs currentPrefs = new Prefs();

		public PreferenceDialog (Prefs initialPrefs)
		{
			this.Build ();

			currentPrefs.format = initialPrefs.format;
			currentPrefs.size = initialPrefs.size;
		}

		public Prefs getPref()
		{
			return currentPrefs;
		}

		public void setPref(Prefs.prefFormat formatIn, Prefs.prefSize sizeIn)
		{
			currentPrefs.format = formatIn;
			currentPrefs.size = sizeIn;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			// set the pref info
			currentPrefs.format = (Prefs.prefFormat)comboFormat.Active;
			currentPrefs.size = (Prefs.prefSize)comboSize.Active;

			this.Destroy ();
		}

		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			this.Destroy ();
		}

		protected void OnDeleteEvent (object sender, EventArgs e)
		{
			this.Destroy ();
		}
	}
}

