using System;
using System.Drawing;

using RLToolkit;

namespace TextureMerger
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ImageSelectorRow : Gtk.Bin
	{
		private int numberButton = 1;
		private ButtonImage[] arrayButton;
		private int imageSize = 64;

		private const int MAX_IMAGES = 8;

		public ImageSelectorRow ()
		{
			this.Log ().Debug ("Constructor with the default size");
			Init ();
		}

		public ImageSelectorRow (int size)
		{
			this.Log ().Debug ("Constructor with size: " + size.ToString());
			imageSize = size;
			Init ();
		}

		private void Init()
		{
			this.Log ().Debug ("Initialization");
			this.Build ();

			arrayButton = new ButtonImage[1];
			ButtonImage newButton = new ButtonImage (imageSize);

			arrayButton [0] = newButton;

			RefreshButtons ();
		}

		public Bitmap[] GetImages()
		{
			this.Log ().Debug ("Fetching images");
			Bitmap[] retArray = new Bitmap[arrayButton.Length];
			int i = 0;
			foreach (ButtonImage b in arrayButton) {
				retArray [i] = b.GetImage ();
				i++;
			}
			return retArray;
		}

		public void Clear()
		{
			this.Log ().Debug ("Clearing the row");
			foreach (ButtonImage b in arrayButton) {
				b.Clear ();
			}
			RefreshButtons ();
		}

		protected void OnRowMinusClicked (object sender, EventArgs e)
		{
			this.Log ().Info ("Trying to remove a button");
			if (numberButton > 1) {
				ButtonImage[] newArray = new ButtonImage[numberButton - 1];
				for (int i = 0; i<numberButton-1; i++) {
					newArray [i] = arrayButton [i];
				}
				arrayButton = newArray;
				numberButton--;

				RefreshButtons ();
			} else {
				// i'm sorry dave but i cannot let you do this.
				this.Log ().Warn ("Min number of button reached");
			}
		}

		protected void OnRowPlusClicked (object sender, EventArgs e)
		{
			this.Log ().Info ("Trying to add a button");
			if (numberButton <= MAX_IMAGES) {
				ButtonImage[] newArray = new ButtonImage[numberButton + 1];
				for (int i = 0; i<numberButton; i++) {
					newArray [i] = arrayButton [i];
				}
				numberButton++;
				arrayButton = newArray;
				ButtonImage newButton = new ButtonImage (imageSize);
				arrayButton [numberButton - 1] = newButton;

				RefreshButtons ();
			} else {
				// i'm sorry dave but i cannot let you do this.
				this.Log ().Warn ("Max number of button reached");
			}
		}

		private void RefreshButtons()
		{
			this.Log ().Debug ("Refreshing buttons");

			// wipe everything
			foreach (Gtk.Widget ch in buttonBox.Children) {
				buttonBox.Remove (ch);
			}

			// add the buttons
			int i = 0;
			foreach (ButtonImage b in arrayButton) {
				buttonBox.Spacing = i;
				buttonBox.Add (b);
				i++;
			}

			// refresh all
			buttonBox.ShowAll ();
		}

		public void UpdateSize(int newSize)
		{
			this.Log ().Debug ("updating the size of the row's items");

			imageSize = newSize;

			int i = 0;
			ButtonImage item;
			foreach (Gtk.Widget ch in buttonBox.Children) {
				if (i < buttonBox.Children.Length) {
					item = (ButtonImage)ch;
					item.HeightRequest = newSize;
					item.WidthRequest = newSize;
					item.UpdateSize (newSize);
				}
				i++;
			}
		}
	}
}