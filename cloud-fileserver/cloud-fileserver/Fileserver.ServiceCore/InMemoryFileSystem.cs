using System;
using System.Collections.Generic;

//Author - piyush

namespace cloudfileserver
{
	public class InMemoryFileSystem
	{
		public Dictionary<string, UserFileSystem> clientToFileSystemMap {get;set;}

		public DateTime lastcheckpoint { get; set; }

		private static readonly log4net.ILog Logger = 
			log4net.LogManager.GetLogger(typeof(InMemoryFileSystem));

		public InMemoryFileSystem()
		{
			//This is just dummy, later this will be filled by ISIS bootstrapping
			clientToFileSystemMap = new Dictionary<string, UserFileSystem>();
			UserFileSystem filesystem = new UserFileSystem();
			filesystem.metadata = new UserMetaData("piyush", "password", 1);
			UserFile file = new UserFile("x.txt","piyush");
			file.SetFileContent(Utils.getByteArrayFromString("Filecontent"), 0);
			filesystem.filemap.Add("x.txt", file);
			filesystem.filemap.Add("/x/y.txt", file);
			filesystem.filemap.Add("/x/z/y/.txt", file);
			this.clientToFileSystemMap.Add("piyush", filesystem);
		}

		/*
		 * This method is used to authenticate a user 
		 */
		public Boolean AuthenticateUser( string clientId, string password){
			return true;
		}

		public UserFile FetchFile (
			string clientId, 
			string filename, 
			string fileowner)
		{
			Logger.Debug(filename);
			UserFile file = null;
			if (this.clientToFileSystemMap.ContainsKey (fileowner)) {
				if (this.clientToFileSystemMap [fileowner].filemap.ContainsKey (filename)) {
					file = this.clientToFileSystemMap [fileowner].filemap [filename];
				} else {
					throw new FileNotFoundException ("File with name :" + filename + 
						" not found for owner : " + fileowner);
				}
			} else {
				throw new UserNotLoadedInMemoryException ("Client not found in memory : " + clientId);
			}
			if (! fileowner.Equals (clientId)) {
				bool access = false;
				foreach (string shareduser in file.sharedwithclients) {
					if (shareduser.Equals (clientId)) {
						access = true;
						break;
					}
				}
				if (!access) {
					throw new AccessViolationException ("File : " + filename + " owned by " + 
						fileowner + "is not shared with " + clientId
					);
				}
			}
			return file;
		}

		/*This method returns a list of all file name for a client id*/
		public List<String> FetchFileList(string clientId)
		{
			List<String> filelist = new List<string>();
			if (this.clientToFileSystemMap.ContainsKey (clientId)) {
				UserFileSystem userfilesystem = this.clientToFileSystemMap[clientId];
				foreach(KeyValuePair<string, UserFile> entry in  userfilesystem.filemap){
					filelist.Add( entry.Key);
				}
			} else {
				throw new UserNotLoadedInMemoryException("Client id not loaded in memory :" + clientId);
			}
			return filelist;
		}

		/* Fetch the list of all users in memory
		 * To be used for bootstrapping
		 */
		public List<String> GetInMemoryUserList ()
		{
			List<String> userList = new List<string> ();
			foreach (KeyValuePair<string, UserFileSystem> entry in  this.clientToFileSystemMap) {
				userList.Add(entry.Key);
			}
			return userList;
		}

		/**/
		public UserFileSystem GetClonedInMemoryUserFileSystem (string clientId)
		{

			if (this.clientToFileSystemMap.ContainsKey(clientId)) {
				UserFileSystem filesystem = this.clientToFileSystemMap[clientId];
				return filesystem.CloneSynchronized();
			} else {
				throw new UserNotLoadedInMemoryException("Client not found in memory :" + clientId);
			}
		}

		
		//getinmemoryuserobject(synchronized)
		//saveuserobjecttomemory(synchronized), take care of versioning he
		
		
			
	}
}
