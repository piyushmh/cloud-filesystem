using System;
using System.IO;
using System.Collections.Generic;
using ServiceStack;
namespace cloudfileserver
{
	[Serializable]
	public class DoCheckPoint{
		public CheckPointObject checkPointObject  { get; set; }
	}

	[Serializable]
	public class DoCheckPointResponse{}

	[Serializable]
	public class RestoreCheckPoint : IReturn<CheckPointObject>{}
	
	[Serializable]
	public class FlushFile{
		public UserFile file { get; set;}
	}

	
	public class PersistentStoreInteraction
	{
		public static string PERSISTENT_STORAGE_SERVICE_ENDPOINT = "http://128.84.216.57:8081";

		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(PersistentStoreInteraction));

		public PersistentStoreInteraction (){}

		
		public void DoCheckPoint (InMemoryFileSystem filesystem)
		{
			logger.Debug ("Request recieved for checkpointing userfile system");
			JsonServiceClient client = new JsonServiceClient (PERSISTENT_STORAGE_SERVICE_ENDPOINT);
			CheckPointObject checkPointObject = new CheckPointObject ();

			foreach (KeyValuePair<string, UserFileSystem> entry in filesystem.clientToFileSystemMap) {
				checkPointObject.userfilesystemlist.Add (entry.Value);
			}

			checkPointObject.lastcheckpoint = DateTime.Now;
			client.Post<Object> ("/doCheckPoint", new DoCheckPoint{checkPointObject = checkPointObject});
		}

		
		public void flushToDisk (UserFile file)
		{
			try {
				logger.Debug ("Flushing file to disk with filename and owner : " + file.filemetadata.filepath + " " + file.filemetadata.owner);
				JsonServiceClient client = new JsonServiceClient (PERSISTENT_STORAGE_SERVICE_ENDPOINT);
				client.Post<Object> ("/flushfile", new FlushFile{file = file});
			} catch (Exception e) {
				logger.Warn (e);
			}
		}
		
		public UserFile fetchFileFromDisk (string username, string filename)
		{
			try {
				logger.Debug ("Fetching from disk for username and filename : " + username + " " + filename);
				JsonServiceClient client = new JsonServiceClient (PERSISTENT_STORAGE_SERVICE_ENDPOINT);
				UserFile f = client.Get<UserFile> ("/fetchfile/" + username + "/" + filename);
			
				logger.Debug ("Received from persistent disk file " + f);
				return f;
			} catch (Exception e) {
				logger.Warn (e);
				return null;
			}
			
		}
		
		public InMemoryFileSystem RestoreCheckPoint ()
		{
			logger.Debug ("Request recieved for restoring userfile system");
			JsonServiceClient client = new JsonServiceClient (PERSISTENT_STORAGE_SERVICE_ENDPOINT);

			CheckPointObject obj = client.Get<CheckPointObject>("/restoreCheckPoint");
			InMemoryFileSystem filesystem = new InMemoryFileSystem (false);
			filesystem.lastcheckpoint = obj.lastcheckpoint;
			foreach (UserFileSystem fs in obj.userfilesystemlist) {
				filesystem.clientToFileSystemMap.Add( fs.metadata.clientId, fs);
			}

			return filesystem;
		}
	}
}

