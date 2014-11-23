using System;
using Gtk;
using System.Drawing;

using RLToolkit;

namespace TextureMerger
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ButtonImage : Gtk.Bin
	{
		private string filename;
		private int imageSize = 64;
		private Bitmap fromFile;

		public ButtonImage ()
		{
			this.Log ().Debug ("Constructor with default size");
			Init ();
		}

		public ButtonImage(int size)
		{
			this.Log ().Debug ("Constructor with size " + size.ToString());
			imageSize = size;
			Init ();
		}

		public void Init()
		{
			this.Log ().Debug ("Initialization");
			this.Build ();

			UpdateImage ();
		}

		protected void OnBtnClicked (object sender, EventArgs e)
		{
			this.Log ().Info ("Button clicked");
			FileChooserDialog chooser = new FileChooserDialog(
				"Please select an image",
				null,
				FileChooserAction.Open,
				new object[] {
				"Cancel", ResponseType.Cancel,
				"Open", ResponseType.Accept 
				});
		
			int result = chooser.Run ();
			this.Log ().Info ("Dialog result: " + result.ToString());
			if( result == ( int )ResponseType.Accept )
			{
				this.Log ().Info ("Dialog accepted, filename picked: " + chooser.Filename);
				filename = chooser.Filename;

				ValidateImage ();
			}
			
			UpdateImage ();

			chooser.Hide ();		
		}

		private void ValidateImage()
		{
			this.Log ().Info ("Validating image");
			// make sure it's an image
			try
			{
				fromFile = new Bitmap(System.Drawing.Image.FromFile(filename));
			}
			catch (Exception e) 
			{
				this.Log ().Warn ("Error encountered while opening image.");
				this.Log ().Debug ("Error info:" + Environment.NewLine + e.Data);
				MessageDialog dialog = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Cannot fetch image. This file cannot be used." + Environment.NewLine + Environment.NewLine + "Error info:" + Environment.NewLine + e.Data);
				if (dialog.Run () == (int)ResponseType.Ok) {
					dialog.Destroy ();
				}
				filename = null;
			}
		}

		public Bitmap GetImage()
		{
			this.Log ().Debug ("Fetching image");
			return fromFile;
		}

		public string GetFilename()
		{
			this.Log ().Debug ("Fetching filename");
			return filename;
		}

		private void UpdateImage()
		{
			this.Log ().Info ("Updating image preview");
			if ((filename == null) || (filename == "")) {
				this.Log ().Debug ("No filename, using stock image");
				img.SetSizeRequest(imageSize,imageSize);
				img.SetFromStock (Gtk.Stock.No, Gtk.IconSize.Button);
			} else {
				this.Log ().Debug ("filename supplied, using file");
				img.Pixbuf = new Gdk.Pixbuf (filename).ScaleSimple (imageSize, imageSize, Gdk.InterpType.Bilinear);
			}
		}

		public void Clear()
		{
			this.Log ().Debug ("Cleaning button");
			filename = null;
			imageSize = 64;
			fromFile = null;
			UpdateImage ();
		}

		public void UpdateSize(int newSize)
		{
			imageSize = newSize;
			UpdateImage ();
		}
	}
}