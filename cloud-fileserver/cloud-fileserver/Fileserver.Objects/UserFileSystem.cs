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


		/* 	Synchronized method to add file to the file system.
			If the file is not present then the file is added. If the file
		 	is already present it is overwritten if the new file has a 
		 	higher version number
		*/
		public bool addFileSynchronized (UserFile file)
		{

			UserFile existingFile = null;
			lock (this.privateLock) {
				if (this.filemap.ContainsKey (file.filepath)) {
					existingFile = this.filemap [file.filepath];
				}
			}

			bool retval = true;

			if (existingFile != null) {
				if( file.versionNumber > existingFile.versionNumber){
					addFileToMapSynchronized(file);
				}else{
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

