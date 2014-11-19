using System;
using System.Drawing;

using RLToolkit;

namespace TextureMerger
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ImageSelector : Gtk.Bin
	{
		private int numberRow = 1;
		private ImageSelectorRow[] arrayRow;
		private int imageSize = 64;

		private const int MAX_ROW = 8;

		public ImageSelector ()
		{
			this.Log ().Debug ("Constructor with default size");
			Init();
		}

		public ImageSelector (int size)
		{
			this.Log ().Debug ("Constructor using size: " + size.ToString());
			imageSize = size;
			Init();
		}

		public void Init()
		{
			this.Log ().Debug ("Initialization");
			this.Build ();

			arrayRow = new ImageSelectorRow[1];
			ImageSelectorRow newRow = new ImageSelectorRow (imageSize);

			arrayRow [0] = newRow;

			RefreshRow ();
		}

		public Bitmap[][] GetImages()
		{
			this.Log ().Debug ("Fetching all lines images");
			Bitmap[][] retArray = new Bitmap[arrayRow.Length][];
			int i = 0;
			foreach (ImageSelectorRow r in arrayRow) {
				retArray [i] = r.GetImages ();
				i++;
			}
			return retArray;
		}

		public void Clear()
		{
			this.Log ().Debug ("Clearing all lines");
			foreach (ImageSelectorRow r in arrayRow) {
				r.Clear ();
			}
			RefreshRow ();
		}

		protected void OnSelectorPlusClicked (object sender, EventArgs e)
		{
			this.Log ().Info ("Trying to add a selector row");
			if (numberRow <= MAX_ROW) {
				ImageSelectorRow[] newArray = new ImageSelectorRow[numberRow + 1];
				for (int i = 0; i<numberRow; i++) {
					newArray [i] = arrayRow [i];
				}
				numberRow++;
				arrayRow = newArray;
				ImageSelectorRow newRow = new ImageSelectorRow ();
				arrayRow [numberRow - 1] = newRow;

				RefreshRow ();
			} else {
				// i'm sorry dave but i cannot let you do this.
				this.Log ().Warn ("Max Row achieved");
			}	
		}

		protected void OnSelectorMinusClicked (object sender, EventArgs e)
		{
			this.Log ().Info ("Trying to remove a selector row");
			if (numberRow > 1) {
				ImageSelectorRow[] newArray = new ImageSelectorRow[numberRow - 1];
				for (int i = 0; i<numberRow-1; i++) {
					newArray [i] = arrayRow [i];
				}
				arrayRow = newArray;
				numberRow--;

				RefreshRow ();
			} else {
				// i'm sorry dave but i cannot let you do this.
				this.Log ().Warn ("Min Row achieved");
			}
		}

		private void RefreshRow()
		{
			this.Log ().Debug ("Refreshing rows");
			// backup the last line
			Gtk.Widget lastLine = selector.Children[selector.Children.Length-1];

			// wipe everything
			foreach (Gtk.Widget ch in selector.Children) {
				selector.Remove (ch);
			}

			// add the buttons
			int i = 0;
			foreach (ImageSelectorRow b in arrayRow) {
				selector.Spacing = i;
				selector.Add (b);
				i++;
			}

			// add the last line
			selector.Add (lastLine);

			// refresh all
			selector.ShowAll ();
		}
	}
}