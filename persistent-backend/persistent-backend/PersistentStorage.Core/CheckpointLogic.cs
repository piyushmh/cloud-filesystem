using System;
using System.IO;
using System.Collections.Generic;


namespace persistentbackend
{
	public class CheckpointLogic
	{
		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(CheckpointLogic));

		//This works on linux. Will read this from config later and make this OS agnostic
		private string pathprefix = "/home/piyush/codes/checkpoint/"; 
		private string metadatafilename = "metadata.txt";
		private string lastcheckpointfilename = "lastcheckpoint.txt";

		public CheckpointLogic (){} //default contructor

		public void DoCheckPoint (CheckPointObject filesystem)
		{
			logger.Debug("Do check point method called for filesystem");
			try{
				string path = GenerateCheckpointPath ();
				logger.Debug ("Creating checkpointing path :" + path);
				Directory.CreateDirectory (path);
				string lastcheckpointfilepath = this.pathprefix + this.lastcheckpointfilename;
				foreach( UserFileSystem userfs in filesystem.userfilesystemlist){
					DoCheckPointForUser( userfs.metadata.clientId, path, userfs);
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
			UserFileSystem userfilesystem){

			logger.Debug ("Check pointing for user :" + user);

			string userpath = path + user + Path.DirectorySeparatorChar; //Appending '/' for linux and '\' on windows
			Directory.CreateDirectory (userpath);
			string metadatafilepath = userpath + this.metadatafilename;

			string metadata = userfilesystem.metadata.clientId + 
							"\n" + userfilesystem.metadata.password
							+ "\n" + userfilesystem.metadata.versionNumber + "\n";

			List<String> files = userfilesystem.GetFileList ();
			foreach (string filename in files) {
				metadata += filename + "\n";
			}

			//Update the meta data file
			logger.Debug ("Writing meta file at path :" + metadatafilepath + " with content :\n" + metadata);
			System.IO.File.WriteAllText (metadatafilepath, metadata);

			//Now we write the file content on disk
			foreach (KeyValuePair<string, UserFile> entry in  userfilesystem.filemap) {
				string parentdir = GetParentDirectoryPath( entry.Key);
				string filepath = userpath + "files" + Path.DirectorySeparatorChar;
				string metadatapath = userpath + "metadata" + Path.DirectorySeparatorChar;
				Directory.CreateDirectory(filepath + parentdir);
				Directory.CreateDirectory(metadatapath + parentdir);

				string completefilepath = filepath + entry.Key;
				string completemetadatafilepath = metadatapath + entry.Key + ".dat";
				System.IO.File.WriteAllText( completemetadatafilepath, entry.Value.GenerateMetaDataStringFromFile());
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

