using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * Author - piyush
 */

namespace cloudfileserver
{
	public class UserFileSystem
	{
		private object privateLock = new object();//this object is used to synchronize access over this object

		public UserMetaData metadata {
			get {
				lock (privateLock) {
					return this.metadata;
				}
			}
			set{
				lock(this.privateLock){
					this.metadata = value;
				}
			}
		}

		public Dictionary<string, UserFile> filemap {get; set;}


		public UserFileSystem (){
			filemap = new Dictionary<string, UserFile>();
		}

		public UserFileSystem (UserMetaData metadata){	
			filemap = new Dictionary<string, UserFile>();
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

	}
}

