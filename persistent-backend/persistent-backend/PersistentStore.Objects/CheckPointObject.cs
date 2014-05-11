using System;
using System.Collections.Generic;

namespace persistentbackend
{	
	[Serializable]
	public class CheckPointObject
	{
		public List<UserFileSystem> userfilesystemlist { get; set;}

		public DateTime lastcheckpoint { get; set; }

		public CheckPointObject (){
			this.userfilesystemlist = new List<UserFileSystem>();
		}
		
	}
}
