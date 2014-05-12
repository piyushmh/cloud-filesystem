using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using ServiceStack;

//author - piyush
namespace persistentbackend
{	
	[Route("/doCheckPoint", "POST")]
	public class DoCheckPoint
	{
		public CheckPointObject checkPointObject  { get; set; }
	}

	[Route("/restoreCheckPoint", "GET")]
	public class RestoreCheckPoint : IReturn<CheckPointObject>{

	}

	[Route("/fetchfile/{username}/{filename}" , "GET")]
	public class FetchFile{
		public string username { get; set;}
		public string filename { get; set;}
	}
	
	[Route("/flushfile", "POST")]
	public class FlushFile{
		public UserFile file { get; set;}
	}
	
	public class PersistentStorageService : Service
	{
		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(PersistentStorageService));

		public object Get (RestoreCheckPoint request)
		{
			try{
				logger.Debug("API call for restoring the check point");
				CheckPointObject obj =  new CheckpointLogic().RestoreFileSystem(false);
				return obj;
			} catch (Exception e) {
				logger.Debug(e);
				throw e;
			}
		}

		public void Post (DoCheckPoint request)
		{
			try {
				logger.Debug ("API Request received for checkpointing ");
				new CheckpointLogic ().DoCheckPointAllUsers (request.checkPointObject);

			} catch (Exception e) {
				logger.Debug(e);
				throw e;
			}
		}

		public UserFile Get (FetchFile request)
		{
			try {
				logger.Info ("Request received for fetching file for user : " + request.filename + " " + request.username);
				UserFile file = new FileOperations ().fetchFile (request.username, request.filename);
				return file;
				
			} catch (Exception e) {
				logger.Warn (e);
				throw e;
			}
		}

		
		public void Post (FlushFile request)
		{
			try {
				logger.Info ("Request received for file name and user  : " + request.file.filemetadata.owner + " " + request.file.filemetadata.filepath); 
				new FileOperations ().FlushFile (request.file);
			} catch (Exception e) {
				logger.Warn (e);
				throw e;
			}
		}
	}
}
