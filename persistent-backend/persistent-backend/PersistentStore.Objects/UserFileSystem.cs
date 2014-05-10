using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace persistentbackend
{
	[Serializable]
	public class UserFileSystem
	{
		private object privateLock;//this object is used to synchronize access over this object

		//private static readonly log4net.ILog Logger = 
		//	log4net.LogManager.GetLogger(typeof(UserFileSystem));

		public UserMetaData metadata {get; set;}

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

		public void addFile(UserFile file){
			this.filemap.Add(file.filepath, file);
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

		public UserMetaData GetMetadataSynchronized ()
		{
			lock (this.privateLock) {
				return this.metadata;
			}
		}

		public void SetMetadataSynchronized (UserMetaData metadata)
		{
			lock (this.privateLock) {
				this.metadata = metadata;
			}
		}

		public override string ToString ()
		{	
			return  (this.metadata.clientId + " " + this.metadata.password + "\n" + 
				string.Format ("[UserFileSystem: filemap={0}]", filemap));

		}
	}
}

