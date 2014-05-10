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

		public string filepath { get; set;}

		public string owner { get; set;}

		private byte[] filecontent;//access to this must be through synchronized methods

		//DAFUQ, ideally access to this should be synchronized as well, maybe later
		public long filesize { get; set;}

		public List<string> sharedwithclients { get; set;}

		public long versionNumber {get;set;}

		private object privateLock = new object();

		public UserFile (string filepath, string owner)
		{	
			this.filepath = filepath;
			this.owner = owner;
			this.versionNumber = -1;
			this.filesize = 0;
			this.filecontent = new byte[0];
		}

		public byte[] ReadFileContentSynchronized ()
		{
			lock (privateLock) {
				return ReadFileContent();
			}
		}

		public byte[] ReadFileContent ()
		{
			return this.filecontent;

		}

		public bool SetFileContentSynchronized (UserFile newfile)
		{
			return SetFileContentSynchronized( newfile.filecontent, newfile.versionNumber);
		}

		/* Change the file contents only if new version number is greater than the current 
		 * version number. Also update the size and the version number accordingly
		 */
		public bool SetFileContentSynchronized( byte[] newcontent, long newversionNumber){

			lock (privateLock) {
				return SetFileContent( newcontent, newversionNumber);
			}
		}

		public bool SetFileContent( byte[] newcontent, long newversionNumber){
			if( this.versionNumber < newversionNumber){ //only if the file is of a newer version
				this.filecontent = new byte[newcontent.Length];
				System.Array.Copy(newcontent, this.filecontent, newcontent.Length);
				this.versionNumber = newversionNumber;
				this.filesize = newcontent.Length;
				return true;
			}else{
				return false;
			}	
		}



	}
}

