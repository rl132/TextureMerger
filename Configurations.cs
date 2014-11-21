using System;
using System.IO;

using RLToolkit;
using RLToolkit.Basic;

namespace TextureMerger
{
	public class Configurations
	{
		private CfgManager mgr;
		private string prefFile;

		public Configurations ()
		{
			this.Log ().Debug ("Creating a configuration systrem.");
			prefFile = Path.Combine (AppDomain.CurrentDomain.BaseDirectory, "pref.xml");
			mgr = new CfgManager (prefFile, typeof(XmlConfigSystem));
			mgr.ReadConfig ();
		}

		public void Save()
		{
			this.Log ().Info ("Trying to save the preferences");
			mgr.WriteConfig ();
		}

		public CfgManager GetManager()
		{
			return mgr;
		}

	}
}

