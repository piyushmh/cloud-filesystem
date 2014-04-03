using System;
using System.IO;
using System.Collections.Generic;
using ServiceStack;
namespace cloudfileserver
{
	public class DoCheckPoint
	{
		public CheckPointObject checkpoint  { get; set; }
	}

	public class DoCheckPointResponse{

	}
	public class PersistentStoreInteraction
	{
	
		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(PersistentStoreInteraction));

		public PersistentStoreInteraction (){
		}

		public InMemoryFileSystem RestoreFromLastCheckPoint(){
			return null;
		}

		public void DoCheckPoint (InMemoryFileSystem filesystem)
		{
			try{

				logger.Debug("Request recieved for checkpointing userfile system");
				JsonServiceClient client  = new JsonServiceClient("http://127.0.0.1:8081");
				CheckPointObject checkPointObject = new CheckPointObject();
				List<UserFileSystem> userfslist = new List<UserFileSystem>();

				foreach( KeyValuePair<string, UserFileSystem> entry in filesystem.clientToFileSystemMap){
					userfslist.Add( entry.Value);
				}

				checkPointObject.userfilesystemlist = userfslist;
				DoCheckPoint arg = new DoCheckPoint();
				arg.checkpoint = checkPointObject;
				client.Post<DoCheckPointResponse>("/doCheckPoint", arg);

			}catch ( Exception e){
					logger.Debug ("Exception :" + e);
				throw e;
			}

		}
	}
}

