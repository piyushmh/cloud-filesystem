using System;
using System.Collections.Generic;

namespace cloudfileserver
{
	public class UserFileSystem
	{
		public string username { get; set;}

		public string password { get; set;}

		private Dictionary<string, UserFile> filemap;



		public UserFileSystem ()
		{
		}
	}
}

