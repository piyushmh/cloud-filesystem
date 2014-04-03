using System;
using System.Collections.Generic;

namespace persistentbackend
{
	/*
	 * This class represents one file in the file system 
	 * Author - piyush
	 */
	[Serializable]
	public class UserFile	{

		public string filepath { get; set;}

		public string owner { get; set;}

		//access to this must be through synchronized methods if needed, they are defined below
		public byte[] filecontent { get; set;}

		//DAFUQ, ideally access to this should be synchronized as well, maybe later
		public long filesize { get; set;}

		public List<string> sharedwithclients { get; set;}

		public long versionNumber {get;set;}

		private object privateLock = new object();

		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(UserFile));


		public UserFile (string filepath, string owner)
		{	
			this.filepath = filepath;
			this.owner = owner;
			this.versionNumber = -1;
			this.filesize = 0;
			this.filecontent = new byte[0];
			this.sharedwithclients = new List<string>();
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
			logger.Debug("Set file content called on file with path :" + this.filepath);
			if( this.versionNumber < newversionNumber){ //only if the file is of a newer version
				this.filecontent = new byte[newcontent.Length];
				System.Array.Copy(newcontent, this.filecontent, newcontent.Length);
				this.versionNumber = newversionNumber;
				this.filesize = newcontent.Length;
				return true;
			}else{
				logger.Debug("File over write attempt with smaller version number, ignoring");
				return false;
			}	
		}

		//This will be used in checkpointing
		public string GenerateMetaDataStringFromFile ()
		{
			string r = this.owner + "\n" + this.filesize.ToString() + "\n";
			foreach (string sharedclient in this.sharedwithclients) {
				r += sharedclient + " ";
			}
			return  r;
		}



	}
}

