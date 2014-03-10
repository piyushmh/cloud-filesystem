using System;
using System.Collections.Generic;

//Author - piyush

namespace cloudfileserver
{
	public class UserFileSystem
	{
		public UserMetaData metadata {get;set;}

		public Dictionary<string, UserFile> filemap {get; set;}



		public UserFileSystem ()
		{
			filemap = new Dictionary<string, UserFile>();
		}

		public UserFileSystem (UserMetaData metadata)
		{	
			this.metadata = metadata;
		}

		public void addFile(UserFile file){
			this.filemap.Add(file.filepath, file);
		}
	}
}

