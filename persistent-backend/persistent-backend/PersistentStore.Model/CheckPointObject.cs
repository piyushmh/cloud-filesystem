using System;
using System.Collections.Generic;

namespace persistentbackend
{	
	[Serializable]
	public class CheckPointObject
	{
		public List<UserFileSystem> userfilesystemlist { get; set;}
	
		public CheckPointObject (){}

	}
}
