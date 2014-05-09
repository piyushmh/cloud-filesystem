
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using ServiceStack;


namespace cloudfileserver
{	
	[Route("/file/{clientId}/{password}/{filename}/{fileowner}", "GET")]
	public class GetFile : IReturn<UserFile>
	{
		public string clientId  { get; set; }
		public string password  { get; set; }
		public string filename  { get; set; }
		public string fileowner { get; set; }
	}

	[Route("/filelist/{clientId}/{password}", "GET")]
	public class GetUserFileList : IReturn<List<String>>
	{
		public string clientId  { get; set; }
		public string password  { get; set; }
	}

	[Route("/updatefile/{clientId}/{password}","POST")]
    public class UpdateFile
    {
		public string clientId {get; set;}
		public string password {get; set;}
    	public UserFile file { get; set; }
    }

	[Route("/savefilefile/{clientId}/{password}", "POST")]
	public class SaveFile{

		public string clientId {get; set;}
		public string password {get; set;}
		public UserFile file {get; set;}
	}

	[Route("/deletefile/{clientId}/{password}/{filename}", "POST")]
	public class DeleteFile{

		public string clientId {get; set;}
		public string password {get; set;}
		public string filepath {get; set;}
	}

	[Route("/adduser/{clientId}/{password}", "POST")]
	public class AddUser{
		public string clientId {get; set;}
		public string password {get; set;}
	}



	public class CloudFileService : Service
	{
		public InMemoryFileSystem filesystem {get;set;} //Injected by IOC, hopefully :)

		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(CloudFileService));

		public object Get (GetFile request)
		{
			if (!filesystem.AuthenticateUser (request.clientId, request.password)) {
				throw new AuthenticationException ("Authentication failed");
			}

			try {
				UserFile file = 
					filesystem.FetchFile (request.clientId, request.filename, request.fileowner);
				return file;
			} catch (Exception e) {
				logger.Debug("Exception occured while doing getfile for client : " + request.clientId
				             +" for filename :" + request.filename , e);
				throw e;
			}
		}

		public object Get(GetUserFileList request){
			if (!filesystem.AuthenticateUser (request.clientId, request.password)) {
				throw new AuthenticationException("Authentication failed");
			}

			List<string> filelist = 
				filesystem.FetchFileList(request.clientId);
			return filelist;
		}

		public void Post( UpdateFile request){

			logger.Debug("Request received for updating user file for client id  : " + request.clientId
			             + " and file name : + " + request.file.filepath);
		
			if (!filesystem.AuthenticateUser (request.clientId, request.password)) {
				throw new AuthenticationException("Authentication failed");
			}

			filesystem.addFileSynchronized(request.clientId, request.file);

		}


		public void Post (AddUser request)
		{
			logger.Debug ("Request received for adding user with client id :" + request.clientId 
				+ " and password : " + request.password
			);

			bool ispresent = 
				filesystem.addUserSynchronized (request.clientId, request.password);

			if (! ispresent) {
				logger.Debug ("User is already present in inmemory map, throwing back exception");
				throw new Exception ("User is already present in memory");
			} else {
				logger.Debug ("User added succesfully");
			}

		}
	}
}
