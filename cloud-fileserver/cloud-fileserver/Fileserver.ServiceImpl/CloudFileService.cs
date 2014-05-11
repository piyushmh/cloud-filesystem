
using System;
using System.Text;
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

	[Route("/getUserFileSystemInfo/{clientId}/{password}", "GET")]
	public class GetUserFileSystemInfo : IReturn<UserFileSystemMetaData>
	{
		public string clientId  { get; set; }
		public string password  { get; set; }
	}

	[Route("/updatefile/{clientId}/{password}","POST")]
    public class UpdateFile
    {
		public string clientId {get; set;}
		public string password {get; set;}
    	public UserFile file   {get; set; }
    }

	[Route("/shareFile/{clientId}/{password}/{filename}/{sharedWithUser}", "POST")]
	public class ShareFileWithUser{

		public string clientId       {get; set;}
		public string password 		 {get; set;}
		public string filename 		 {get; set;}
		public string sharedWithUser {get; set;}
	}

	[Route("/unShareFile/{clientId}/{password}/{filename}/{sharedWithUser}", "POST")]
	public class UnShareFileWithUser{
		
		public string clientId 		{get; set;}
		public string password 		{get; set;}
		public string filename 		{get; set;}
		public string sharedWithUser{get; set;}
	}
	
	
	[Route("/deletefile/{clientId}/{password}/{filepath}", "POST")]
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
			logger.Info ("****Request received for getting file : " + request.filename + " client id : " + 
				request.clientId + " and fileowner : " + request.fileowner
			);
			
			if (!filesystem.AuthenticateUser (request.clientId, request.password)) {
				throw new AuthenticationException ("Authentication failed");
			}

			try {
				UserFile file = 
					filesystem.FetchFile (request.clientId, request.filename, request.fileowner);
				logger.Debug ("Returning file with size : " + file.filemetadata.filesize);
				return file;
				
			} catch (Exception e) {
				logger.Debug("Exception occured while doing getfile for client : " + request.clientId
				             +" for filename :" + request.filename , e);
				throw e;
			}
		}

		public object Get (GetUserFileSystemInfo request)
		{
			logger.Info ("****Request received for get user file system info for client id : " + request.clientId);
			
			if (!filesystem.AuthenticateUser (request.clientId, request.password)) {
				throw new AuthenticationException ("Authentication failed");
			}
			UserFileSystemMetaData md = null;
			try {
				md = filesystem.FetchUserFileSystemMetadata (request.clientId);
			} catch (Exception e) {
				logger.Debug ("Exception occured while gettin' user file system info for client id " + e);
				throw e;
			}
			return md;
		}

		public void Post (UpdateFile request)
		{

			logger.Info ("****Request received for updating user file for client id  : " + request.clientId
				+ " and file name : " + request.file.filemetadata.filepath
			);
		
			if (!filesystem.AuthenticateUser (request.clientId, request.password)) {
				throw new AuthenticationException ("Authentication failed");
			}
			try {
				filesystem.addFileSynchronized (request.clientId, request.file);
			} catch (Exception e) {
				logger.Debug ("Exception occured while updating user file for client id  : " + request.clientId
					+ " and file name : " + request.file.filemetadata.filepath, e);
				throw e;
			}

		}


		public void Post (AddUser request)
		{
			logger.Info ("****Request received for adding user with client id :" + request.clientId 
				+ " and password : " + request.password
			);

			try {
				bool ispresent = 
				filesystem.addUserSynchronized (request.clientId, request.password);
				
				if (! ispresent) {
					logger.Debug ("User is already present in inmemory map, throwing back exception");
					throw new Exception ("User is already present in memory");
				} else {
					logger.Debug ("User added succesfully");
				}	
				
			} catch (Exception e) {
				logger.Debug (e);
				throw e;
			}	

		}
		
		public void Post (ShareFileWithUser request)
		{
			
			logger.Info ("****Request received for sharing file owned by user : " + request.sharedWithUser + " by user : " 
				+ request.clientId + " for file name : " + request.filename
			); 	
			try {
				if (!filesystem.AuthenticateUser (request.clientId, request.password)) {
					throw new AuthenticationException ("Authentication failed");
				}
				filesystem.shareFileWithUser (request.clientId, request.filename, request.sharedWithUser);
			} catch (Exception e) {
				logger.Warn (e);
				throw e;
			}
		}
	
		public void Post (UnShareFileWithUser request)
		{
			
			logger.Info ("****Request received for un-sharing file owned by user : " + request.sharedWithUser + " by user : " 
				+ request.clientId + " for file name : " + request.filename
			); 	
			try {
				if (!filesystem.AuthenticateUser (request.clientId, request.password)) {
					throw new AuthenticationException ("Authentication failed");
				}
				filesystem.unShareFileWithUser (request.clientId, request.filename, request.sharedWithUser);
			} catch (Exception e) {
				logger.Warn (e);
				throw e;
			}
		}
	
		public void Post (DeleteFile request)
		{
			logger.Info ("****Request received for deleting file : " + request.filepath + " owned by user : " + request.clientId);
			
			try {
				
				if (!filesystem.AuthenticateUser (request.clientId, request.password)) {
					throw new AuthenticationException ("Authentication failed");
				}
				
				bool delete = filesystem.deleteFileSynchronized (request.clientId, request.filepath);
				
				if (!delete) {
					logger.Debug ("The file : " + request.filepath + " was already marked for deletion, skipping");
				}
				
			} catch (Exception e) {
				logger.Warn (e);
				throw e;
			}
		}
		
		
	}
}
