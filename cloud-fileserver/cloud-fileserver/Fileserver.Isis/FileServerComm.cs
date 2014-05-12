using Isis;
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Collections.Concurrent;
using System.Threading;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Text;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Serialization.Formatters.Binary;

namespace cloudfileserver
{	
	[Serializable]
	public class COobFetchFile
	{
		public string oobFileName  { get; set;}
	}

	delegate void handleBootStrappingCheckPoint(BootStrappingCheckPoint request);
	delegate void handleBootStrappingRequest(BootStrappingRequest request);
	delegate void handleBootStrappingResponse(BootStrappingResponse resp);


	delegate void handleFileOperation(FileMetaDataSync request);
	delegate void handleFileUserOperation(FileMetaDataSync request);
	delegate void handleFileUserMetaData(UserMetaDataSync request);
	delegate void handleOOBOperations(OOBTransaction request);

	delegate void handleShareWithUser(SyncSharedUser request);
	delegate void handleUnShareWithUser(SyncUnSharedUser request);

	delegate void myLhandler(string who);

	public class FileServerComm
	{
		public const int BootStrapRequest = 0;
		public const int BootStrapBegin = 1;
		public const int BootStrapContinue = 2;
		public const int BootStrapException = 3;
		public const int BootStrapEnd = 4;
		public const int BootStrapResponse = 5;
		public const int UpdateUserMetaData = 6;
		public const int UpdateFileMetaData = 7;
		public const int DeleteFileFromMemory = 8;
		public const int DeleteFile = 9;
		public const int SaveFileToMemory = 10;
		public const int UpdateUser = 11;
		public const int ShareWithUser = 12;
		public const int UnshareWithUser = 13;



		Group fileServerGroup { get; set;}
		OOBHandler oobhandler { get; set;}
		BootStrap bootstrap { get; set;}
		FileOperation fileHandler { get; set;}
		String groupName { get; set;}
		public TransactionManager transManager {get;set;}

		private static readonly log4net.ILog Logger = 
			log4net.LogManager.GetLogger(typeof(FileServerComm));

		public static FileServerComm instance = null;
		public static String fileServerGroupName;

		public FileServerComm(string fileServerGroupName)
		{
			groupName = fileServerGroupName;
			fileServerGroup = new Group (groupName);
			fileHandler = new FileOperation ();
			oobhandler = new OOBHandler ();
			bootstrap = new BootStrap ();
			transManager = new TransactionManager();
		}

			
		void ApplicationStartFileServerComm(bool isBootStrap, String fileServerGroupName)
		{
			Logger.Debug ("ApplicationStartFileServerComm ---->. Begin " + isBootStrap);
			Thread fileServerCommThread = new Thread (delegate() 
			{
				Logger.Debug ("ApplicationStartFileServerComm. Begin");
				registerBootStrapHandlers();
				registerFileOperationHandlers();

				Msg.RegisterType(typeof(BootStrappingCheckPoint), BootStrapBegin);
				Msg.RegisterType(typeof(BootStrappingCheckPoint), BootStrapContinue);
				Msg.RegisterType(typeof(BootStrappingCheckPoint), BootStrapEnd);
				Msg.RegisterType(typeof(BootStrappingRequest), BootStrapRequest);
				Msg.RegisterType(typeof(BootStrappingResponse), BootStrapResponse);

				Msg.RegisterType(typeof(UserMetaDataSync), UpdateUserMetaData);
				Msg.RegisterType(typeof(FileMetaDataSync), UpdateFileMetaData);
				Msg.RegisterType(typeof(FileMetaDataSync), DeleteFileFromMemory);
				Msg.RegisterType(typeof(FileMetaDataSync), DeleteFile);
				Msg.RegisterType(typeof(OOBTransaction), SaveFileToMemory);
				Msg.RegisterType(typeof(OOBTransaction), UpdateUser);

				Msg.RegisterType(typeof(SyncSharedUser), ShareWithUser);
				Msg.RegisterType(typeof(SyncUnSharedUser), UnshareWithUser);


				fileServerGroup.ViewHandlers += (Isis.ViewHandler)delegate(View v)
				{
					Logger.Debug ("myGroup got a new view event: " + v);
				};

				fileServerGroup.Join();

				if(isBootStrap)
				{
					Logger.Debug ("Need to do BootStrapping :), So I'll do BootStrapping");
					bootstrap.sendBootStrappingRequest(fileServerGroup,null);
				}
				else
				{
					Logger.Debug ("Do not Need to Do BootStrapping. Start the Application Server");
					bootstrap.state.currState = eBootStrapState.BootStrappingComplete;
					bootstrap.waitBootStrap.Release();
				}

				IsisSystem.WaitForever();
				Logger.Debug ("ApplicationStartFileServerComm. End");
			});

			fileServerCommThread.Start();
		}

		void registerBootStrapHandlers()
		{
			fileServerGroup.Handlers[BootStrapBegin] += (handleBootStrappingCheckPoint) delegate (BootStrappingCheckPoint response)
			{
				Logger.Debug("Received a Boot Strapping BootStrapBegin Request");
				bootstrap.handleBootStrappingResponse(response,fileServerGroup,oobhandler);
			};

			fileServerGroup.Handlers[BootStrapContinue] += (handleBootStrappingCheckPoint) delegate (BootStrappingCheckPoint response)
			{
				Logger.Debug("Received a Boot Strapping BootStrapContinue Request");
				bootstrap.handleBootStrappingResponse(response,fileServerGroup,oobhandler);
			};

			fileServerGroup.Handlers[BootStrapEnd] += (handleBootStrappingCheckPoint) delegate (BootStrappingCheckPoint response)
			{
				Logger.Debug("Received a Boot Strapping BootStrapEnd Request");
				bootstrap.handleBootStrappingResponse(response,fileServerGroup,oobhandler);
			};

			fileServerGroup.Handlers[BootStrapRequest] += (handleBootStrappingRequest) delegate (BootStrappingRequest request)
			{
				Logger.Debug("Received a Boot Strapping Request");
				ThreadPool.QueueUserWorkItem(new WaitCallback(bootstrap.handleBootStrappingRequestPlaceHolder),request);
			};

			fileServerGroup.Handlers[BootStrapResponse] += (handleBootStrappingResponse) delegate (BootStrappingResponse response)
			{
				Logger.Debug("Received a Boot Strapping BootStrapResponse Request");
				bootstrap.handleBootStarppingResponse(response);
			};
		}

		void registerFileOperationHandlers()
		{
			fileServerGroup.Handlers[UpdateUserMetaData] += (handleFileUserMetaData)delegate(UserMetaDataSync usermeta)
			{
				fileHandler.handleUserMetaData(usermeta);
			};

			fileServerGroup.Handlers[UpdateFileMetaData] += (handleFileUserOperation)delegate(FileMetaDataSync file)
			{
				fileHandler.handleFileMetaData(file);
			};

			fileServerGroup.Handlers[SaveFileToMemory] += (handleOOBOperations)delegate(OOBTransaction request)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(fileHandler.handleAddFileToMemory),request);
			};

			fileServerGroup.Handlers[UpdateUser] += (handleFileUserMetaData)delegate(UserMetaDataSync request)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(fileHandler.handleAddUser),request);
			};

			fileServerGroup.Handlers[DeleteFileFromMemory] += (handleFileUserOperation)delegate(FileMetaDataSync file)
			{
				fileHandler.handledeleteFileFromMemory(file);
			};

			fileServerGroup.Handlers[DeleteFile] += (handleFileUserOperation)delegate(FileMetaDataSync file)
			{
				fileHandler.handleDeleteFile(file);
			};

			fileServerGroup.Handlers[ShareWithUser] += (handleShareWithUser)delegate(SyncSharedUser request)
			{
				fileHandler.handleShareRequest(request);
			};

			fileServerGroup.Handlers[UnshareWithUser] += (handleUnShareWithUser)delegate(SyncUnSharedUser request)
			{
				fileHandler.handleUnshareRequest(request);
			};
		}

		public void ApplicationStartup (bool isBootStrap, String fileServerGroupName)
		{
			Console.WriteLine ("ApplicationStartup. Begin");

			//Start Communicating between Same Groups
			ApplicationStartFileServerComm (isBootStrap, fileServerGroupName);

			if (true == isBootStrap) {
				//Wait for BootStrapping to Complete
				Console.WriteLine("ApplicationStartup. About to Wait");
				bootstrap.waitBootStrap.WaitOne ();

				Console.WriteLine("ApplicationStartup. Wait Finished");
			}
			Console.WriteLine("ApplicationStartup. End");
		}

		public static FileServerComm getInstance ()
		{
			if (null == instance) {
				instance =  new FileServerComm(fileServerGroupName);
			}
			return instance;
		}

		public FileOperation getFileHandler ()
		{
			return fileHandler;
		}

		public OOBHandler getOOBHandler ()
		{
			return oobhandler;
		}

		public Group getFileServerGroup ()
		{
			return fileServerGroup;
		}
	}
}