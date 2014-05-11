using System;
using System.Collections.Generic;

namespace TestClient
{	
	[Serializable]
	public class CheckPointObject
	{
		public List<UserFileSystem> userfilesystemlist { get; set;}

		public DateTime lastcheckpoint { get; set; }

		public CheckPointObject ()
		{
			this.userfilesystemlist = new List<UserFileSystem> ();
			this.lastcheckpoint = new DateTime ();
		}
		
		
	}
}
