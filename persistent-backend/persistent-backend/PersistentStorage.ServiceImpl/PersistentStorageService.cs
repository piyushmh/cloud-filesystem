using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using ServiceStack;


namespace persistentbackend
{	
	[Route("/doCheckPoint", "POST")]
	public class DoCheckPoint
	{
		public CheckPointObject checkpoint  { get; set; }
	}
	public class PersistentStorageService : Service
	{
		/*Logger object*/
		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(PersistentStorageService));

		public void Post(DoCheckPoint request)
		{
			try{
				logger.Debug("API Request received for checkpointing");
				new CheckpointLogic().DoCheckPoint(request.checkpoint);

			} catch (Exception e) {
				throw e;
			}
		}

	}
}
