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

		public UserFileSystem (){
			this.filemap = new Dictionary<string, UserFile>();
			this.privateLock = new object();
		}

		public UserFileSystem (UserMetaData metadata){	
			this.filemap = new Dictionary<string, UserFile>();
			this.privateLock = new object();
			this.metadata = metadata;

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
			Logger.Debug("Adding file with file name : " + file.filepath);

			UserFile existingFile = getFileSynchronized(file.filepath);

			bool retval = true;

			if (existingFile != null) {
				if( file.versionNumber > existingFile.versionNumber){
					addFileToMapSynchronized(file);
				}else{
					Logger.Debug("Existing file with higher version number found for file : " + file.filepath +
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
				this.filemap.Add (file.filepath, file);
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

