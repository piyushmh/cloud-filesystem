using System;
using System.IO;
using System.Collections.Generic;
namespace cloudfileserver
{


	public class PersistentStoreInteraction
	{
	
		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(PersistentStoreInteraction));

		private string pathprefix = "/home/piyush/codes/checkpoint/";  //This works on linux. Will read this from config later
		private string metadatafilename = "metadata.txt";
		private string lastcheckpointfilename = "lastcheckpoint.txt";

		public PersistentStoreInteraction (){
		}

		public InMemoryFileSystem RestoreFromLastCheckPoint(){
			return null;
		}

		public void DoCheckPoint (InMemoryFileSystem filesystem)
		{
			try{
				string path = GenerateCheckpointPath ();
				logger.Debug ("Creating checkpointing path :" + path);
				Directory.CreateDirectory (path);
				string lastcheckpointfilepath = this.pathprefix + lastcheckpointfilename;
				List<string> userlist = filesystem.GetInMemoryUserList ();
		
				foreach (string user in userlist) {
					DoCheckPointForUser( user, path, filesystem);
				}

				//Now update the last check point file so that we can restore this checkpoint
				logger.Debug ("Writing to last checkpoint file at path : " + lastcheckpointfilepath);
				System.IO.File.WriteAllText(lastcheckpointfilepath, path);

			}catch ( Exception e){
					logger.Debug ("Exception :" + e);
				throw e;
			}

		}

		public void DoCheckPointForUser (
			string user,
			string path,
			InMemoryFileSystem filesystem)
		{

			logger.Debug ("Check pointing for user :" + user);

			string userpath = path + user + Path.DirectorySeparatorChar; //Appending '/' for linux and '\' on windows
			Directory.CreateDirectory (userpath);
			string metadatafilepath = userpath + this.metadatafilename;

			UserFileSystem userfilesystem = filesystem.clientToFileSystemMap [user].CloneSynchronized ();

			string metadata = userfilesystem.GetMetadata ().clientId + "\n" + userfilesystem.GetMetadata ().password
				+ "\n" + userfilesystem.GetMetadata ().versionNumber + "\n";
			List<String> files = userfilesystem.GetFileList ();
			foreach (string filename in files) {
				metadata += filename + "\n";
			}

			//Update the meta data file
			logger.Debug ("Writing meta file at path :" + metadatafilepath + " with content : " + metadata);
			System.IO.File.WriteAllText (metadatafilepath, metadata);

			//Now we write the file content on disk
			foreach (KeyValuePair<string, UserFile> entry in  userfilesystem.filemap) {
				string parentdir = GetParentDirectoryPath( entry.Key);
				string filepath = userpath + "files" + Path.DirectorySeparatorChar;
				Directory.CreateDirectory(filepath + parentdir);
				string completefilepath = filepath + entry.Key;
				File.WriteAllBytes( completefilepath, entry.Value.ReadFileContent());
			}
		}


		private string GetParentDirectoryPath( string fullpath){
			return Path.GetDirectoryName(fullpath) + Path.DirectorySeparatorChar;
		}

		private string GenerateCheckpointPath(){
			DateTime time = DateTime.Now;
			string path = time.Year + "-" + time.Month + "-" + time.Day + Path.DirectorySeparatorChar + 
				time.Hour + "-" + time.Minute + Path.DirectorySeparatorChar;
			return this.pathprefix + path;
		}
	}
}

