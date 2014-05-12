using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage
{
	[Serializable]
	public class UserFileSystemMetaData
	{
		//this is the user meta data object
		public UserMetaData userMetaData { get; set;}
		
		//this is the list of file meta data
		public List<FileMetaData> fileMDList { get; set; }
		
		//this is the list of shared files
		public List<SharedFile> sharedFileList { get; set; }
		
		public UserFileSystemMetaData ()
		{
			this.fileMDList = new List<FileMetaData> ();
			this.sharedFileList = new List<SharedFile> ();
		}
	}
}
