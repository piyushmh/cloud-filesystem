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
			
		public List<FileMetaData> getFileMetaDataListCopySynchronized ()
		{
			Logger.Debug ("Getting file meta data list for user : " + this.metadata.clientId);
			
			List<FileMetaData> returnList = new List<FileMetaData> ();
			lock (this.privateLock) {
				foreach (KeyValuePair<string, UserFile> entry in  this.filemap) {
					returnList.Add (entry.Value.getFileMetaDataSynchronized ().cloneMetaDataObject ());
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
		 	Use this to read from the map
		 */
		private UserFile getFileSynchronized (string filename)
		{
			UserFile existingFile = null;
			lock (this.privateLock) {
				if (this.filemap.ContainsKey (filename)) {
					existingFile = this.filemap [filename];
				}
			}
			return existingFile;
		}

		/* 	Synchronized method to add file to the file system.
			If the file is not present then the file is added. If the file
		 	is already present it is overwritten if the new file has a 
		 	higher version number
		*/
		public bool addFileSynchronized (UserFile file)
		{
			Logger.Debug("Adding file with file name : " + file.filemetadata.filepath);

			UserFile existingFile = getFileSynchronized(file.filemetadata.filepath);

			bool retval = true;

			if (existingFile != null) {
				if( file.filemetadata.versionNumber > existingFile.filemetadata.versionNumber){
					addFileToMapSynchronized(file);
				}else{
					Logger.Debug("Existing file with higher version number found for file : " + file.filemetadata.filepath +
					             " , skipping updation");
					retval = false;
				}
			} else {
				addFileToMapSynchronized(file);			
			}

			return retval;
		}

	
		/* Internal synchronized method to add file to the class map*/
		private void addFileToMapSynchronized (UserFile file)
		{
			lock (this.privateLock) {
				this.filemap.Add (file.filemetadata.filepath, file);
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

		public List<String> GetFileListSynchronized ()
		{
			lock (privateLock) {
				return this.GetFileList();
			}
		}

		public List<String> GetFileList ()
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
		
		/* Synchronized method to delete a shared file. If the owner passed is null in the argument, 
		 * then all the files with the given filename will be deleted
		 */
		public bool deleteSharedFileSynchronized(SharedFile file){
			
			Logger.Debug("Deleting shared file : " + file.filename);
			bool deleted  = false;
			List<SharedFile> replaceList = new List<SharedFile>();
			lock( this.privateLock){
				foreach(SharedFile f in this.sharedFiles){
					if( f.filename.Equals(file.filename)){
						if( file.owner!=null && !file.owner.Equals(f.owner)){
							replaceList.Add(f);
						}else{
							//this means that this file has been deleted
							deleted = true;
						}
					}else{ // if the file name does not match, keep the file anyway
						replaceList.Add(f);
					}
				}	
				this.sharedFiles = replaceList;
			}
			
			return deleted;
		}
		
		/*	Synchronized method to add a shared file to the file system
		 */
		public bool addSharedFileSynchronized(SharedFile file){
			
			bool found = checkIfSharedFilePresentSynchronized(file);
			if( found){
				return false;
			}else{
				lock( this.privateLock){//lock the shared file list and add this file
					this.sharedFiles.Add(file);
				}
				return true;
			}
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

