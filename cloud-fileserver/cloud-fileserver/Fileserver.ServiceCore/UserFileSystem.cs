using System;
using System.Collections.Generic;

//Author - piyush

namespace cloudfileserver
{
	public class UserFileSystem
	{
		private UserMetaData metadata;

		private Dictionary<string, UserFile> filemap;



		public UserFileSystem ()
		{
			//Custom constructor
		}

		public UserFileSystem (UserMetaData metadata)
		{	
			this.metadata = metadata;
		}

		void addFile(UserFile file){
			this.filemap.Add(file.filepath, file);
		}
	}
}

