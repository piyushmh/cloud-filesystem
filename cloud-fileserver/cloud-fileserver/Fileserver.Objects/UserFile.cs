using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace cloudfileserver
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
		
		private object privateLock = new object();

		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(UserFile));

		
		public UserFile getFileCloneSynchronized ()
		{
			object obj;
			lock (this.privateLock) {
				MemoryStream ms = new MemoryStream ();
				BinaryFormatter bf = new BinaryFormatter ();
				bf.Serialize (ms, this);
				ms.Position = 0;
				obj = bf.Deserialize (ms);
				ms.Close ();
			}
			return obj as UserFile;
		}
		
		public FileMetaData getFileMetaDataCloneSynchronized(){
			lock( this.privateLock){
				return this.filemetadata.cloneMetaDataObject();
			}
		}
		
		private bool ifSharedUserPresentSynchronized (string username)
		{
			logger.Debug ("Checking if shared user :" + username + " is present in the shared file list");
			bool present = false;
			lock (this.privateLock) {
				foreach (string user in this.filemetadata.sharedwithclients) {
					if (username.Equals (user)) {
						present = true;
						break;
					}
				}
			}
			return present;
		}

		
		public bool markForDeletionSynchronized ()
		{
			bool delete = false;
			lock (this.privateLock) {
				logger.Debug ("Marking file for deletion : " + this.filemetadata.filepath);
				if (this.filemetadata.markedForDeletion == false) {
					this.filemetadata.markedForDeletion = true;
					delete = true;
				}
			}
			return delete;
		}
		
		/* Method to check if the user has access to the file by checking in the shared user list*/ 
		public bool checkUserAccessSynchronized (string user)
		{
			logger.Debug ("Checking if user : " + user + " has access to the file :" + this.filemetadata.filepath);
			
			bool access = false;
			lock (this.privateLock) {
				foreach (string u in this.filemetadata.sharedwithclients) {
					if (u.Equals (user)) {
						access = true;
						break;
					}
				}
			}
			return access;
		}
		
		public void addSharedSynchronized (string username)
		{
			logger.Debug ("Adding user : " + username + " to user file :" + this.filemetadata.filepath);
			bool present = ifSharedUserPresentSynchronized (username);
			if (present) {
				logger.Debug ("Shared user :" + username + " is already added to the shared list, skipping");
			}
			lock (this.privateLock) {
				this.filemetadata.sharedwithclients.Add (username);
			}
		}
		
		public void initializePrivateLock ()
		{
			
			logger.Debug ("Setting private lock to a new object for file : " + this.filemetadata.filepath);
			if (this.privateLock == null) {
				this.privateLock = new object ();
			} else {
				logger.Debug ("Private lock is already initiazed for filr  : " + this.filemetadata.filepath + " skipping");
			}
		}
		
		public UserFile (string filepath, string owner)
		{	
			this.filemetadata = new FileMetaData (filepath, owner);
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
			byte[] b = new byte[this.filecontent.Length];
			Buffer.BlockCopy (this.filecontent, 0, b, 0, this.filecontent.Length);
			return b;
		}

		public bool SetFileContentSynchronized (UserFile newfile)
		{
			return SetFileContentSynchronized( newfile.filecontent, newfile.filemetadata.versionNumber);
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
			logger.Debug("Set file content called on file with path :" + this.filemetadata.filepath);
			if( this.filemetadata.versionNumber < newversionNumber){ //only if the file is of a newer version
				this.filecontent = new byte[newcontent.Length];
				System.Array.Copy(newcontent, this.filecontent, newcontent.Length);
				this.filemetadata.versionNumber = newversionNumber;
				this.filemetadata.filesize = newcontent.Length;
				return true;
			}else{
				logger.Debug("File over write attempt with smaller version number, ignoring");
				return false;
			}	
		}

		//This will be used in checkpointing
		public string GenerateMetaDataStringFromFile ()
		{
			string r = this.filemetadata.owner + "\n" + this.filemetadata.filesize.ToString() + "\n" + this.filemetadata.versionNumber.ToString() + "\n";
			string joined = string.Join(",", this.filemetadata.sharedwithclients.ToArray());
			return  r + joined;
		}

		public override string ToString ()
		{
			try {
				return string.Format ("[UserFile: filepath={0}, owner={1}, filecontent={2}, filesize={3}, " +
					"sharedwithclients={4}, versionNumber={5}]", filemetadata.filepath, filemetadata.owner, Utils.getStringFromByteArray (filecontent), 
			                      filemetadata.filesize, filemetadata.sharedwithclients, filemetadata.versionNumber);
			} catch (Exception e) {
				logger.Debug(e);
				throw e;
			}
		}

	}
}

