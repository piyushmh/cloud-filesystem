using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * Author - piyush
 */

namespace cloudfileserver
{	
	[Serializable]
	public class UserFileSystem
	{
		private object privateLock;//this object is used to synchronize access over this object

		private static readonly log4net.ILog Logger = 
			log4net.LogManager.GetLogger(typeof(UserFileSystem));
		
		public UserMetaData metadata  {get; set;}
		
		public Dictionary<string, UserFile> filemap {get; set;}

		//this represents the list of shared files which have been shared with this user
		public List<SharedFile> sharedFiles { get; set; }

		public UserFileSystem ()
		{
			this.filemap = new Dictionary<string, UserFile> ();
			this.privateLock = new object ();
			this.sharedFiles = new List<SharedFile> ();
		}

		public UserFileSystem (UserMetaData metadata)
		{	
			this.filemap = new Dictionary<string, UserFile> ();
			this.sharedFiles = new List<SharedFile> ();
			this.privateLock = new object ();
			this.metadata = metadata;

		}
			
	
		/* This add the user to the shareduserlist of the file*/
		public void addSharedUserToFileSynchronized (string filename, string user)
		{
			
			Logger.Debug ("Adding shared user :" + user + " to file : " + filename);
			UserFile file = getFileSynchronized (filename);
			if (file == null) {
				throw new FileNotFoundException ("File not found for user name and filename : " + user + " " + filename); 
			}
			
			file.addSharedSynchronized (user);
			return;
		}
		
		public bool isFilePresentSynchronized (string filename)
		{
			Logger.Debug ("Checking if the file : " + filename + " is present for client " + this.metadata.clientId);
			
			bool present = false;
			lock (this.privateLock) {
				if (this.filemap.ContainsKey (filename)) {
					present = true;
				}
			}
			return present;
		}
		
		public List<FileMetaData> getFileMetaDataListCopySynchronized ()
		{
			Logger.Debug ("Getting file meta data list for user : " + this.metadata.clientId);
			
			List<FileMetaData> returnList = new List<FileMetaData> ();
			lock (this.privateLock) {
				//go over each file and update ,metadata
				foreach (KeyValuePair<string, UserFile> entry in  this.filemap) {
					returnList.Add (entry.Value.getFileMetaDataCloneSynchronized ());
				}
			}
			return returnList;
		}
		
		public List<SharedFile> getSharedMetaDataListCopySynchronized ()
		{
			Logger.Debug ("Getting shared file list for user : " + this.metadata.clientId);
			
			List<SharedFile> retList = new List<SharedFile> ();
			lock (this.privateLock) {
		
				foreach (SharedFile file in this.sharedFiles) {
					retList.Add(file.cloneObject ());
				}
			}
			
			return retList;
		}
		
		/* Internal synchronized method to get file from the user file system
		 	Use this to read from the map. Returns null if the file is not present in the memory
		 */
		public UserFile getFileSynchronized (string filename)
		{
			UserFile existingFile = null;
			lock (this.privateLock) {
				if (this.filemap.ContainsKey (filename)) {
					existingFile = this.filemap [filename];
				}
			}
			return existingFile;
		}

		public UserFile getFileCloneSynchronized (string filename)
		{
			UserFile existingFile = null;
			lock (this.privateLock) {
				if (this.filemap.ContainsKey (filename)) {
					existingFile = this.filemap [filename];
				}
			}
			if (existingFile == null)
				return null;
			else
				return existingFile.getFileCloneSynchronized();
		}
		
		/* 	Synchronized method to add file to the file system.
			If the file is not present then the file is added. If the file
		 	is already present it is overwritten if the new file has a 
		 	higher version number
		*/
		public bool addFileSynchronized (UserFile file)
		{
			Logger.Debug ("Adding file with file name : " + file.filemetadata.filepath);

			UserFile existingFile = getFileSynchronized (file.filemetadata.filepath);

			bool add = true;
			long existingSize = 0;
			if (existingFile != null) {
				existingSize = existingFile.getFileSizeSynchronized (); // this is the size of the existing file
				
				if (existingFile.filemetadata.markedForDeletion == false) {
					if (file.filemetadata.versionNumber <= existingFile.filemetadata.versionNumber) {
						Logger.Debug ("Existing higher number file found, skipping updation");
						add = false;
						throw new VersionNumberConflictException ("Version number of passed file and the existing files are : " + 
							file.filemetadata.versionNumber + " " + existingFile.filemetadata.versionNumber
						);
					} else {
						Logger.Debug ("Existing lower version number file exists");
					}
				} else {
					Logger.Debug ("Exiting file exists, but marked for deletion");
				}
			}
			
			if (add) {
				addFileToMapSynchronized (file);
				incrementTotalFileSystemSize (file.filemetadata.filesize - existingSize);
			}
			return add;
		}

	
		//mark for deletion, reset the content, update the user file system size
		public bool deleteFileSynchronized (string filename)
		{
			UserFile file = getFileSynchronized (filename);
			
			if (file == null) {
				throw new FileNotFoundException ("File not found :" + filename);
			}
			
			bool delete = file.markForDeletionSynchronized ();
			
			//reset the content. this will also increment the version number. Don't do it later
			long sizeDiff = file.SetFileContentSynchronized (new byte[0], file.getFileVersionNumberSynchronized () + 1); 
			incrementTotalFileSystemSize (sizeDiff);
			return delete;
			
		}
		
		public FileMetaData getFileMetaDataCloneSynchronized(string filename){	
			UserFile file = getFileSynchronized (filename);
			return file.getFileMetaDataCloneSynchronized();
		}
		
		private void incrementTotalFileSystemSize (long inc)
		{
			Logger.Debug ("Incrementing the total file system size by : " + inc);
			lock (this.privateLock) {
				this.metadata.totalFileSystemSizeBytes += inc;
			
				Logger.Debug ("Updated total file system size is : " + this.metadata.totalFileSystemSizeBytes);
				
				if (this.metadata.totalFileSystemSizeBytes < 0) {
					Logger.Warn ("The total file system size became negative : " + this.metadata.totalFileSystemSizeBytes + " FIX ME FIX ME");
				}
			}
			
		}
		
		/* Internal synchronized method to add file to the class map*/
		private void addFileToMapSynchronized (UserFile file)
		{
			lock (this.privateLock) {
				file.initializePrivateLock (); // this is because the files sent over the network do not have their private locks initialized
				this.filemap[file.filemetadata.filepath] =  file;
			}
		}

		public void SaveObjectSynchronized (UserFileSystem filesystem)
		{
			lock (privateLock) {
				
			}
		}

		public UserFileSystem CloneSynchronized ()
		{
			object obj;
			lock (privateLock) {
				MemoryStream ms = new MemoryStream ();
				BinaryFormatter bf = new BinaryFormatter ();
				bf.Serialize (ms, this);
				ms.Position = 0;
				obj = bf.Deserialize (ms);
				ms.Close ();
			}
			return obj as UserFileSystem;
		}

		public List<String> GetClonedFileListSynchronized ()
		{
			lock (privateLock) {
				return this.GetClonedFileList();
			}
		}

		public List<String> GetClonedFileList ()
		{
			List<string> retlist = new List<string>();
			foreach(KeyValuePair<string, UserFile> entry in  this.filemap){
				retlist.Add(entry.Key);
			}
			return retlist;
		}

		public UserMetaData GetMetadataSychronized ()
		{
			lock (this.privateLock) {
				return this.metadata;
			}
		}

		public void SetMetadataSychronized (UserMetaData metadata)
		{
			lock (this.privateLock) {
				this.metadata = metadata;
			}
		}

		
		/*	Synchronized method to add a shared file to the file system
		 */
		public bool addSharedFileSynchronized(SharedFile file){
			
			bool found = checkIfSharedFilePresentSynchronized(file);
			if( found){
				Logger.Debug("Shared file : " + file.filename + " already present in file system of the user " + this.metadata.clientId);
				return false;
			}else{
				lock( this.privateLock){//lock the shared file list and add this file
					Logger.Debug("Adding shared file :" + file.filename + " into the shared file list of user : " + this.metadata.clientId);
					this.sharedFiles.Add(file);
				}
				return true;
			}
		}

		public bool deleteSharedFileSynchronized (SharedFile file)
		{
			bool found = checkIfSharedFilePresentSynchronized (file);
			if (! found) {
				Logger.Debug ("Shared file : " + file.filename + " not present in file system of the user " + this.metadata.clientId);
				return false;
			} else {
				lock (this.privateLock) {//lock the shared file list and then make a new list removing this file
					Logger.Debug ("Size of the share file list before removing is : " + this.sharedFiles.Count); 
					List<SharedFile> newfilelist = new List<SharedFile> ();
					foreach (SharedFile f in this.sharedFiles) {	
						if (f.filename.Equals (file.filename) && f.owner.Equals (file.owner)) {
							
						} else {
							newfilelist.Add (f);
						}
					}
					this.sharedFiles = newfilelist;
				}
				Logger.Debug ("Size of the share file list after removing is : " + this.sharedFiles.Count); 
				return true;
			}
		}
		
		
		public bool removeSharedUserFromFileSynchronized (string filename, string sharedUser)
		{
			Logger.Debug ("Removing shared user from file : " + filename + " " + sharedUser);
			
			UserFile f = getFileSynchronized (filename);
			return f.deleteSharedUserSynchronized (sharedUser);
			
		}
		
		/* Synhronized method to checki if the file is already 
		 * shared with the user or not
		 */
		private bool checkIfSharedFilePresentSynchronized(SharedFile file){
			bool found = false;
			lock(this.privateLock){
				foreach(SharedFile f in this.sharedFiles){
					if( f.filename.Equals(file.filename) && f.owner.Equals(file.owner)){
						found = true;
						break;
					}
				}
			}
			return found;
		}
		
		
		
		public override string ToString ()
		{	
			string s =  this.metadata.ToString ();
			foreach (KeyValuePair<string, UserFile> entry in  this.filemap) {
				s += string.Format("[FileName: {0}, FileContent:{1}]", entry.Key, entry.Value.ToString());
			}
			return  s;

		}
	}
}

