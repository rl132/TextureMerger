
// This file has been generated by the GUI designer. Do not modify.
namespace TextureMerger
{
	public partial class PreferenceDialog
	{
		private global::Gtk.VBox vbox2;
		private global::Gtk.Label LabelDefaults;
		private global::Gtk.HSeparator hseparator1;
		private global::Gtk.HBox hbox1;
		private global::Gtk.Label lblFormat;
		private global::Gtk.ComboBox comboFormat;
		private global::Gtk.HBox hbox2;
		private global::Gtk.Label lblSize;
		private global::Gtk.HBox hbox3;
		private global::Gtk.Entry txtWidth;
		private global::Gtk.Label label1;
		private global::Gtk.Entry txtHeight;
		private global::Gtk.HBox hbox5;
		private global::Gtk.Label lblPreviewSize;
		private global::Gtk.ComboBox comboPreviewSize;
		private global::Gtk.Label LabelPref;
		private global::Gtk.HSeparator hseparator2;
		private global::Gtk.HBox hbox4;
		private global::Gtk.Label lblProportion;
		private global::Gtk.CheckButton chkProportion;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget TextureMerger.PreferenceDialog
			this.Name = "TextureMerger.PreferenceDialog";
			this.Title = global::Mono.Unix.Catalog.GetString ("Preferences");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child TextureMerger.PreferenceDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.LabelDefaults = new global::Gtk.Label ();
			this.LabelDefaults.Name = "LabelDefaults";
			this.LabelDefaults.LabelProp = global::Mono.Unix.Catalog.GetString ("Defaults");
			this.vbox2.Add (this.LabelDefaults);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.LabelDefaults]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hseparator1 = new global::Gtk.HSeparator ();
			this.hseparator1.Name = "hseparator1";
			this.vbox2.Add (this.hseparator1);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hseparator1]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			w3.Padding = ((uint)(2));
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Homogeneous = true;
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.lblFormat = new global::Gtk.Label ();
			this.lblFormat.Name = "lblFormat";
			this.lblFormat.Xalign = 0F;
			this.lblFormat.LabelProp = global::Mono.Unix.Catalog.GetString ("Format: ");
			this.hbox1.Add (this.lblFormat);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.lblFormat]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Padding = ((uint)(15));
			// Container child hbox1.Gtk.Box+BoxChild
			this.comboFormat = global::Gtk.ComboBox.NewText ();
			this.comboFormat.AppendText (global::Mono.Unix.Catalog.GetString ("Bmp"));
			this.comboFormat.AppendText (global::Mono.Unix.Catalog.GetString ("Png"));
			this.comboFormat.AppendText (global::Mono.Unix.Catalog.GetString ("Jpg"));
			this.comboFormat.Name = "comboFormat";
			this.comboFormat.Active = 0;
			this.hbox1.Add (this.comboFormat);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.comboFormat]));
			w5.Position = 1;
			w5.Expand = false;
			this.vbox2.Add (this.hbox1);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox1]));
			w6.Position = 2;
			w6.Expand = false;
			w6.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Homogeneous = true;
			this.hbox2.Spacing = 6;
			this.hbox2.BorderWidth = ((uint)(3));
			// Container child hbox2.Gtk.Box+BoxChild
			this.lblSize = new global::Gtk.Label ();
			this.lblSize.Name = "lblSize";
			this.lblSize.Xalign = 0F;
			this.lblSize.LabelProp = global::Mono.Unix.Catalog.GetString ("Size: ");
			this.hbox2.Add (this.lblSize);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.lblSize]));
			w7.Position = 0;
			w7.Expand = false;
			w7.Padding = ((uint)(15));
			// Container child hbox2.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.txtWidth = new global::Gtk.Entry ();
			this.txtWidth.CanFocus = true;
			this.txtWidth.Name = "txtWidth";
			this.txtWidth.IsEditable = true;
			this.txtWidth.InvisibleChar = '•';
			this.hbox3.Add (this.txtWidth);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.txtWidth]));
			w8.Position = 0;
			w8.Expand = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("x");
			this.hbox3.Add (this.label1);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.label1]));
			w9.Position = 1;
			w9.Expand = false;
			w9.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.txtHeight = new global::Gtk.Entry ();
			this.txtHeight.CanFocus = true;
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.IsEditable = true;
			this.txtHeight.InvisibleChar = '•';
			this.hbox3.Add (this.txtHeight);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.txtHeight]));
			w10.Position = 2;
			w10.Expand = false;
			this.hbox2.Add (this.hbox3);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.hbox3]));
			w11.Position = 1;
			this.vbox2.Add (this.hbox2);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox2]));
			w12.Position = 3;
			w12.Expand = false;
			w12.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox5 = new global::Gtk.HBox ();
			this.hbox5.Name = "hbox5";
			this.hbox5.Homogeneous = true;
			this.hbox5.Spacing = 6;
			// Container child hbox5.Gtk.Box+BoxChild
			this.lblPreviewSize = new global::Gtk.Label ();
			this.lblPreviewSize.Name = "lblPreviewSize";
			this.lblPreviewSize.Xalign = 0F;
			this.lblPreviewSize.LabelProp = global::Mono.Unix.Catalog.GetString ("Preview Size:");
			this.hbox5.Add (this.lblPreviewSize);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.lblPreviewSize]));
			w13.Position = 0;
			w13.Expand = false;
			w13.Padding = ((uint)(15));
			// Container child hbox5.Gtk.Box+BoxChild
			this.comboPreviewSize = global::Gtk.ComboBox.NewText ();
			this.comboPreviewSize.AppendText (global::Mono.Unix.Catalog.GetString ("16"));
			this.comboPreviewSize.AppendText (global::Mono.Unix.Catalog.GetString ("32"));
			this.comboPreviewSize.AppendText (global::Mono.Unix.Catalog.GetString ("64"));
			this.comboPreviewSize.AppendText (global::Mono.Unix.Catalog.GetString ("128"));
			this.comboPreviewSize.Name = "comboPreviewSize";
			this.comboPreviewSize.Active = 2;
			this.hbox5.Add (this.comboPreviewSize);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.comboPreviewSize]));
			w14.Position = 1;
			w14.Expand = false;
			this.vbox2.Add (this.hbox5);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox5]));
			w15.PackType = ((global::Gtk.PackType)(1));
			w15.Position = 4;
			w15.Expand = false;
			w15.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.LabelPref = new global::Gtk.Label ();
			this.LabelPref.Name = "LabelPref";
			this.LabelPref.LabelProp = global::Mono.Unix.Catalog.GetString ("Preferences");
			this.vbox2.Add (this.LabelPref);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.LabelPref]));
			w16.PackType = ((global::Gtk.PackType)(1));
			w16.Position = 5;
			w16.Expand = false;
			w16.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hseparator2 = new global::Gtk.HSeparator ();
			this.hseparator2.Name = "hseparator2";
			this.vbox2.Add (this.hseparator2);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hseparator2]));
			w17.PackType = ((global::Gtk.PackType)(1));
			w17.Position = 6;
			w17.Expand = false;
			w17.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox ();
			this.hbox4.Name = "hbox4";
			this.hbox4.Homogeneous = true;
			this.hbox4.Spacing = 6;
			this.hbox4.BorderWidth = ((uint)(3));
			// Container child hbox4.Gtk.Box+BoxChild
			this.lblProportion = new global::Gtk.Label ();
			this.lblProportion.Name = "lblProportion";
			this.lblProportion.Xalign = 0F;
			this.lblProportion.LabelProp = global::Mono.Unix.Catalog.GetString ("Keep Proportions:");
			this.hbox4.Add (this.lblProportion);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.lblProportion]));
			w18.Position = 0;
			w18.Expand = false;
			w18.Padding = ((uint)(15));
			// Container child hbox4.Gtk.Box+BoxChild
			this.chkProportion = new global::Gtk.CheckButton ();
			this.chkProportion.CanFocus = true;
			this.chkProportion.Name = "chkProportion";
			this.chkProportion.Label = global::Mono.Unix.Catalog.GetString ("Keep");
			this.chkProportion.DrawIndicator = true;
			this.chkProportion.UseUnderline = true;
			this.hbox4.Add (this.chkProportion);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.chkProportion]));
			w19.Position = 1;
			this.vbox2.Add (this.hbox4);
			global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox4]));
			w20.PackType = ((global::Gtk.PackType)(1));
			w20.Position = 7;
			w20.Expand = false;
			w20.Fill = false;
			w1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(w1 [this.vbox2]));
			w21.Position = 0;
			w21.Expand = false;
			w21.Fill = false;
			// Internal child TextureMerger.PreferenceDialog.ActionArea
			global::Gtk.HButtonBox w22 = this.ActionArea;
			w22.Name = "dialog1_ActionArea";
			w22.Spacing = 10;
			w22.BorderWidth = ((uint)(5));
			w22.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w23 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w22 [this.buttonCancel]));
			w23.Expand = false;
			w23.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget (this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w24 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w22 [this.buttonOk]));
			w24.Position = 1;
			w24.Expand = false;
			w24.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 750;
			this.DefaultHeight = 297;
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
			this.buttonCancel.Clicked += new global::System.EventHandler (this.OnButtonCancelClicked);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
