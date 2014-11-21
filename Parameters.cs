using System;
using System.Text;

using RLToolkit;
using RLToolkit.Basic;

namespace TextureMerger
{
	public class Parameters
	{
		public int previewSize { get; set; }

		public Parameters ()
		{
			this.Log ().Debug ("Constructor for a parameters set using default values.");
			previewSize = 64;
		}

		public void LoadParametersFromManager(CfgManager mgr)
		{
			this.Log ().Debug ("Loading a parameter set using the manager");

			string input;
			int output;

			input = mgr.GetValue ("previewsize");
			if (input != "") {
				Int32.TryParse (input, out output);
				previewSize = output;
			} else {
				previewSize = 64;
			}
		}

		public void SaveParametersToManager(CfgManager mgr)
		{
			this.Log ().Info ("Trying to set the values of the parameters in the configuration manager");

			mgr.SetValue ("previewsize", previewSize.ToString());
		}

		public bool ValidateParameters(out string messages)
		{
			StringBuilder sb = new StringBuilder ();
			bool retVal = true;
			if ((previewSize != 16) && 
			    (previewSize != 32) &&
			    (previewSize != 64) &&
			    (previewSize != 128)) {
				retVal = false;
				sb.Append ("Preview size needs to be either 126, 32, 64 or 128" + Environment.NewLine);
			}
			messages = sb.ToString ();
			return retVal;
		}
	}
}