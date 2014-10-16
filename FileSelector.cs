using System;
using System.Drawing;
using Gtk;
using RLToolkit;

namespace TextureMerger
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class FileSelector : Gtk.Bin
	{
		public FileSelector ()
		{
			this.Log ().Debug ("Creating a new FileSelector");
			this.Build ();
			previewImage.SetSizeRequest(64,64);
			previewImage.SetFromStock (Gtk.Stock.No, Gtk.IconSize.Button);
		}

		public void setText(string t)
		{
			this.Log ().Debug ("Setting FileSelector text to: " + t);
			textField.Text = t;
		}

		public void clear()
		{
			this.Log ().Debug ("Clearing the FileSelector content");
			fileBrowser.SetCurrentFolder(AppDomain.CurrentDomain.BaseDirectory);
			fileBrowser.UnselectAll ();
			previewImage.SetFromStock (Gtk.Stock.No, Gtk.IconSize.Button);
		}

		public void setImage (string path)
		{
			this.Log ().Debug ("Setting the FileSelector image to " + path);
			previewImage.SetSizeRequest(64,64);
			try
			{
				previewImage.Pixbuf = new Gdk.Pixbuf (path).ScaleSimple (64, 64, Gdk.InterpType.Bilinear);
			}
			catch (Exception e) 
			{
				MessageDialog dialog = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Cannot fetch preview image. This file cannot be used." + Environment.NewLine + e.Data);
				if (dialog.Run () == (int)ResponseType.Ok) {
					dialog.Destroy ();
				}
				clear ();
			}
		}

		protected void OnFileBrowserSelectionChanged (object sender, EventArgs e)
		{
			this.Log ().Debug ("FileSelector selection changed");
			if (fileBrowser.Filename != null) {
				setImage (fileBrowser.Filename);
			}
		}

		public string getFilename()
		{
			this.Log ().Debug ("Fetching the FileSelector filename");
			return fileBrowser.Filename;
		}
	}
}

