using System;
using System.Collections.Generic;

//Author - piyush

namespace cloudfileserver
{
	public class InMemoryFileSystem
	{
		public Dictionary<string, UserFileSystem> clientToFileSystemMap {get;set;}

		public DateTime lastcheckpoint { get; set; }

		/*This will lock the whole file system, use it carefully
		 For now this is only being used when the client to file system map
		is being touched */

		private object privateLock = new object();

		private static readonly log4net.ILog Logger = 
			log4net.LogManager.GetLogger(typeof(InMemoryFileSystem));

		//private PersistentStoreInteraction persistentstoreinteraction;

		public InMemoryFileSystem ()
		{

			this.clientToFileSystemMap = new Dictionary<string, UserFileSystem>();
			try {
				Logger.Debug ("Starting InMemoryFileSystemconstructor");

				
				//this.persistentstoreinteraction = new PersistentStoreInteraction ();
				//InMemoryFileSystem fs = this.persistentstoreinteraction.RestoreCheckPoint();
				
				//this.clientToFileSystemMap = fs.clientToFileSystemMap;
				//this.lastcheckpoint = fs.lastcheckpoint;
				//Logger.Debug(this.ToString());
				//Logger.Debug("XXX : " + Utils.getStringFromByteArray(this.clientToFileSystemMap["piyush"].filemap["x.txt"].filecontent));

			} catch (Exception e) {
				Logger.Debug("Exception caught :"  + e);
				throw e;
			}
		}

		public InMemoryFileSystem (bool dummyarg)
		{
			this.clientToFileSystemMap = new Dictionary<string, UserFileSystem>();
		}

		/*
		 * This method is used to authenticate a user 
		 */
		public Boolean AuthenticateUser( string clientId, string password){
			return true;
		}
		
		
		public UserFileSystemMetaData FetchUserFileSystemMetadata (string clientid)
		{
			Logger.Debug ("Fetching user file system metadata for client id : " + clientid);
			UserFileSystem fs = getUserFSFromMapSynchronized (clientid);
			if (fs == null) {
				throw new UserNotLoadedInMemoryException ("User not loaded in memory : " + clientid);
			}
			UserFileSystemMetaData mdReturn = new UserFileSystemMetaData ();
			mdReturn.userMetaData = fs.GetMetadataSychronized ();
			mdReturn.fileMDList = fs.getFileMetaDataListCopySynchronized ();
			mdReturn.sharedFileList = fs.getSharedMetaDataListCopySynchronized ();
			
			return mdReturn;
		}

		public UserFile FetchFile (
			string clientId, 
			string filename, 
			string fileowner)
		{

			Logger.Debug ("Fetching file : " + filename + " for client id :" + clientId +  
			              " with  owner " + fileowner);

			UserFile file = null;
			UserFileSystem fs = getUserFSFromMapSynchronized(clientId);

			//Now there no need for taking any more locks of this class. Used
			// synchronized methods of the file system class

			if (fs != null) {
				if (this.clientToFileSystemMap [fileowner].filemap.ContainsKey (filename)) {
					file = fs.filemap [filename];
				} else {
					throw new FileNotFoundException ("File with name :" + filename + 
						" not found for owner : " + fileowner);
				}
			} else {
				throw new UserNotLoadedInMemoryException ("Client not found in memory : " + clientId);
			}

			if (! fileowner.Equals (clientId)) {
				bool access = false;
				foreach (string shareduser in file.filemetadata.sharedwithclients) {
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

		/*This method returns a list of all file name for a client id
		 This method is synchronized*/
		public List<String> FetchFileList(string clientId)
		{

			UserFileSystem userfilesystem = getUserFSFromMapSynchronized(clientId);
			List<String> filelist = new List<string>();

			if ( userfilesystem != null) {
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


		/*	Synchronized method to add user. Returns false if the user is already present
			Otherwise creates an empty file system and adds that user with that empty file system.
		 */
		public bool addUserSynchronized (string clientid, string password, long versionNumber)
		{
		
			Logger.Debug("Adding user with client id : " + clientid + " and password : " + 
			             password + " and version number :" + versionNumber);
			UserFileSystem fs = getUserFSFromMapSynchronized(clientid);

			if ( fs != null) {
				return false;

			} else {
				UserMetaData md = new UserMetaData(clientid, password, versionNumber);
				UserFileSystem filesystem = new UserFileSystem(md);
				addFSToMapSynchronized(filesystem, clientid);
			}
			return true;
		}


		/*	Synchronized method to add user. Returns false if the user is already present
			Otherwise creates an empty file system and adds that user with that empty file system.
			This adds user metadata with version number 0.
		 */
		
		public bool addUserSynchronized (string clientid, string password){

			Logger.Debug("Adding user with client id : " + clientid + " and password : " + password);
			return addUserSynchronized( clientid, password, 0);
		}


		public bool updateMetadataSynchronized (UserMetaData md)
		{
			Logger.Debug("updating user meta data for user id :" + md.clientId);

			UserFileSystem fs = getUserFSFromMapSynchronized (md.clientId);
			if (fs != null) {
				fs.SetMetadataSychronized(md);	
			} else {
				throw new UserNotLoadedInMemoryException("Update meta data operation failed for user id : " + md.clientId);
			}
			return true;
		}


		/* Synchronized method to add file for given client id 
		   Throws exception if the user for present 
		 */
		public bool addFileSynchronized (string clientid, UserFile file)
		{
			Logger.Debug("Adding file : " + file.filemetadata.filepath + " for client : " + clientid);
			UserFileSystem fs = getUserFSFromMapSynchronized (clientid);
			if (fs != null) {
				return fs.addFileSynchronized(file);

			} else {
				throw new UserNotLoadedInMemoryException("Add file failed for user :" 
				                                         + clientid + " and file name :" + file.filemetadata.filepath);
			}
		}

		public bool deleteFileSynchronized (string clientid, string filename)
		{
			return true;
		}




		private UserFileSystem getUserFSFromMapSynchronized (string clientid)
		{
			Logger.Debug("Synchronized fetch file system from map for user :" + clientid);
			lock (this.privateLock) {
				if( this.clientToFileSystemMap.ContainsKey(clientid)){
					return this.clientToFileSystemMap[clientid];
				}else{
					return null;
				}
			}
		}


		private bool addFSToMapSynchronized (UserFileSystem fs, string clientid)
		{
			Logger.Debug ("Synchronized adding file system in map for client id  : " + clientid);

			lock (this.privateLock) {
				this.clientToFileSystemMap.Add(clientid, fs);
				return true;
			}
		}


		public override string ToString ()
		{
			string s = "[InMemoryFileSystem: [lastcheckpoint :" + this.lastcheckpoint + "] ,";
			foreach (KeyValuePair<string, UserFileSystem> entry in this.clientToFileSystemMap) {
				s += " [" + entry.Key + "," + entry.Value.ToString() + "]";
			}
			return s;
		}

	}
}
