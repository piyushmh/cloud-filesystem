using System;
using System.IO;
using System.Collections.Generic;
using ServiceStack;
namespace cloudfileserver
{
	public class DoCheckPoint{
		public CheckPointObject checkpoint  { get; set; }
	}

	public class DoCheckPointResponse{}
	public class RestoreCheckPoint : IReturn<CheckPointObject>{}


	public class PersistentStoreInteraction
	{
		public static string PERSISTENT_STORAGE_SERVICE_ENDPOINT = "http://127.0.0.1:8081";

		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(PersistentStoreInteraction));

		public PersistentStoreInteraction (){}

		public void DoCheckPoint (InMemoryFileSystem filesystem)
		{
			logger.Debug("Request recieved for checkpointing userfile system");
			JsonServiceClient client  = new JsonServiceClient(PERSISTENT_STORAGE_SERVICE_ENDPOINT);
			CheckPointObject checkPointObject = new CheckPointObject();

			foreach( KeyValuePair<string, UserFileSystem> entry in filesystem.clientToFileSystemMap){
				checkPointObject.userfilesystemlist.Add( entry.Value);
			}

			checkPointObject.lastcheckpoint = new DateTime();
			DoCheckPoint arg = new DoCheckPoint();
			arg.checkpoint = checkPointObject;
			client.Post<DoCheckPointResponse>("/doCheckPoint", arg);
		}

		public InMemoryFileSystem RestoreCheckPoint ()
		{
			logger.Debug ("Request recieved for restoring userfile system");
			JsonServiceClient client = new JsonServiceClient (PERSISTENT_STORAGE_SERVICE_ENDPOINT);

			RestoreCheckPoint arg = new RestoreCheckPoint ();
			CheckPointObject obj = client.Post<CheckPointObject> ("/restoreCheckPoint", arg);
			InMemoryFileSystem filesystem = new InMemoryFileSystem (false);
			foreach (UserFileSystem fs in obj.userfilesystemlist) {
				filesystem.clientToFileSystemMap.Add( fs.metadata.clientId, fs);
			}
			return filesystem;
		}
	}
}

