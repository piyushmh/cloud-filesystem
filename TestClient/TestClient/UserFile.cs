using System;
using System.Collections.Generic;
using System.Text;
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

		
		public override string ToString ()
		{
			try {
				return string.Format ("[UserFile: filepath={0}, owner={1}, filecontent={2}, filesize={3}, " +
					"sharedwithclients={4}, versionNumber={5}]", filemetadata.filepath, filemetadata.owner, Encoding.UTF8.GetString(filecontent), 
			                      filemetadata.filesize, filemetadata.sharedwithclients, filemetadata.versionNumber);
			} catch (Exception e) {
				throw e;
			}
		}
	}
}

