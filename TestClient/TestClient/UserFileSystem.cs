using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * Author - piyush
 */

namespace TestClient
{	
	[Serializable]
	public class UserFileSystem
	{
		private object privateLock;//this object is used to synchronize access over this object

		public UserMetaData metadata  {get; set;}
		
		public Dictionary<string, UserFile> filemap {get; set;}

		//this represents the list of shared files which have been shared with this user
		public List<SharedFile> sharedFiles { get; set; }

		public UserFileSystem (){
			this.filemap = new Dictionary<string, UserFile>();
			this.privateLock = new object();
			this.sharedFiles = new List<SharedFile>();
		}

		public UserFileSystem (UserMetaData metadata){	
			this.filemap = new Dictionary<string, UserFile>();
			this.sharedFiles = new List<SharedFile>();
			this.privateLock = new object();
			this.metadata = metadata;

		}
	}
}

