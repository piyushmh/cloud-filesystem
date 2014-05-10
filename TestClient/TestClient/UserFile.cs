using System;
using System.Collections.Generic;

namespace TestClient
{
	/*
	 * This class represents one file in the file system 
	 * Author - piyush
	 */
	[Serializable]
	public class UserFile	{

		//file content
		public byte[] filecontent { get; set;}

		//metadata
		public FileMetaData filemetadata { get; set; }
		
		//private object privateLock = new object();
		
		public UserFile (string filepath, string owner)
		{	
			this.filemetadata = new FileMetaData (filepath, owner);
			this.filecontent = new byte[0];
		}

	}
}

