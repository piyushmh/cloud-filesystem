using System;
using System.IO;
using System.Collections.Generic;
namespace cloudfileserver
{


	public class PersistentStoreInteraction
	{
	
		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(PersistentStoreInteraction));


		private string pathprefix = "/home/piyush/codes/checkpoint/";

		private string metadatafilename = "metadata.txt";

		private string lastcheckpointfilename = "lastcheckpoint.txt";

		public PersistentStoreInteraction ()
		{
		}

		public InMemoryFileSystem RestoreFromLastCheckPoint(){
			return null;
		}

		public void DoCheckPoint (InMemoryFileSystem filesystem)
		{
			string path = GenerateCheckpointPath ();
			logger.Debug ("Creating checkpointing path :" + path);
			Directory.CreateDirectory (path);
			string lastcheckpointfilepath = this.pathprefix + lastcheckpointfilename;
			List<string> userlist = filesystem.GetInMemoryUserList ();
			try{
				foreach (string user in userlist) {
					string userpath = path +user + '/';
					Directory.CreateDirectory(userpath);
					string metadatafilepath = userpath + this.metadatafilename;
					UserFileSystem userfilesystem = filesystem.clientToFileSystemMap[user].CloneSynchronized();
					string metadata = userfilesystem.GetMetadata().clientId + "\n" + userfilesystem.GetMetadata().password
						+ "\n" + userfilesystem.GetMetadata().versionNumber + "\n" ;
					List<String> files = userfilesystem.GetFileList();
					foreach( string filename in files){
						metadata  += filename + "\n" ;
					}

					logger.Debug("Writing meta file at path :" + metadatafilepath + " with content : " + metadata);
					System.IO.File.WriteAllText(metadatafilepath, metadata);
					logger.Debug ("Writing to last checkpoint file at path : " + lastcheckpointfilepath);
					System.IO.File.WriteAllText(lastcheckpointfilepath, path);
				}
			}catch ( Exception e){
					logger.Debug ("Exception :" + e);
				throw e;
			}

		}

		private string GenerateCheckpointPath(){
			DateTime time = DateTime.Now;
			string path = time.Year + "-" + time.Month + "-" + time.Day + "/" + time.Hour + "-" + time.Minute + "/";
			return this.pathprefix + path;
		}
	}
}

