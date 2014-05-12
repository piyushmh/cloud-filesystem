using System;
using Isis;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace cloudfileserver
{
	[Serializable]
	[AutoMarshalled]
	public class FileSynch
	{
		public string fileName;
		public string userName;
		public string transactionID;
		public Address initiatedSystemId;

		public FileSynch(string fileName, string userName, string transactionId, Address localAddress)
		{
			this.fileName = fileName;
			this.userName = userName;
			this.transactionID = transactionId;
			this.initiatedSystemId = localAddress;
		}

		public FileSynch ()
		{
		}
	}

	[Serializable]
	[AutoMarshalled]
	public class UserMetaDataSync
	{
		public string transactionID ;
		public Address initiatedSystemId ;
		public string clientId;
		public string password;
		public long versionNumber;

		public UserMetaDataSync (string transactionId, Address localAddress, UserMetaData data)
		{
			this.clientId = data.clientId;
			this.password = data.password;
			this.versionNumber = data.versionNumber;
			this.transactionID = transactionId;
			this.initiatedSystemId = localAddress;
		}
		public UserMetaDataSync ()
		{
		}

	}

	[Serializable]
	[AutoMarshalled]
	public class FileMetaDataSync
	{
		public string transactionID ;
		public Address initiatedSystemId ;
		public string filepath;
		public string owner;
		public long filesize;
		public long versionNumber;
		public string longSharedWithClients;
		
		public FileMetaDataSync (string transactionId, Address localAddress, FileMetaData filemetadata)
		{
			this.filepath = filemetadata.filepath;
			this.owner = filemetadata.owner;
			this.filesize = filemetadata.filesize;
			this.transactionID = transactionId;
			this.initiatedSystemId = localAddress;
			this.versionNumber = filemetadata.versionNumber;

			if (null != filemetadata) {
				int index = 0;
				foreach(string client in filemetadata.sharedwithclients) {
					if(index != filemetadata.sharedwithclients.Count - 1)
						longSharedWithClients += client + ",";
					else
						longSharedWithClients += client;
				}
			}
		}

		public FileMetaDataSync ()
		{
		}
	}

	[Serializable]
	[AutoMarshalled]
	public class SyncSharedUser
	{
		public string clientId;
		public string password;
		public string filename;
		public string sharedWithUser;
		public Address initiatedSystemId ;
		public string transactionID ;

		public SyncSharedUser (string transactionID,Address initiatedSystemId,
		                       string clientId, string password, 
		                       string filename, string sharedWithUser
		                       )
		{
			this.clientId = clientId;
			this.password = password;
			this.filename = filename;
			this.sharedWithUser = sharedWithUser;
			this.initiatedSystemId = initiatedSystemId;
			this.transactionID = transactionID;
		}

		public SyncSharedUser ()
		{

		}
	}

	[Serializable]
	[AutoMarshalled]
	public class SyncUnSharedUser
	{
		public string clientId;
		public string password;
		public string filename;
		public string sharedWithUser;
		public Address initiatedSystemId ;
		public string transactionID ;
		
		public SyncUnSharedUser (string transactionID,Address initiatedSystemId,
		                         string clientId, string password, 
		                         string filename, string sharedWithUser
		                         )
		{
			this.clientId = clientId;
			this.password = password;
			this.filename = filename;
			this.sharedWithUser = sharedWithUser;
			this.initiatedSystemId = initiatedSystemId;
			this.transactionID = transactionID;
		}
		
		public SyncUnSharedUser ()
		{
			
		}
	}

	[Serializable]
	[AutoMarshalled]
	public class OOBTransaction
	{
		public string transactionID ;
		public Address initiatedSystemId ;
		public int fileLength;

		public OOBTransaction (string transactionId, Address localAddress, int length)
		{
			this.transactionID = transactionId;
			this.initiatedSystemId = localAddress;
			this.fileLength = length;
		}

		public OOBTransaction ()
		{
		}
	}
	
	public class FileOperation
	{
		public InMemoryFileSystem filesystem {get;set;} 

		private static readonly log4net.ILog Logger = 
			log4net.LogManager.GetLogger(typeof(FileOperation));

		public FileOperation ()
		{
			Logger.Debug ("File Operations Synch :)");
		}

		/*
		 * This Function is called when a New User Is Added or the Meta Data of the User Needs to be Changed and Synched
		 */
		public bool sendsynchUserMetaData (UserMetaData data, OOBHandler handler, Group group, bool waitToFinish = true)
		{
			try {
				Logger.Debug ("File Operations Synch - sendsynchUserMetaData >> BEGIN");
				bool operationResult = false;

				if (waitToFinish) {
					string transactionId = FileServerComm.getInstance ().transManager.generateTransactionId (data.clientId);
					Transaction trans = new Transaction (transactionId);
					if (true == FileServerComm.getInstance ().transManager.insertTransaction (trans)) {
						UserMetaDataSync sync = new UserMetaDataSync (transactionId, IsisSystem.GetMyAddress (), data);
						group.OrderedSend (FileServerComm.UpdateUserMetaData, sync);
						Logger.Debug ("File Operations Synch - Making a Ordered Send");
						trans.waitTillSignalled ();
						FileServerComm.getInstance ().transManager.removeAndGetTransaction (transactionId);
						operationResult = !trans.isTimedOut;
					} else {
						Logger.Debug ("File Operations Synch - sendsynchUserMetaData >> Generation of Transaction ID Failed: " + transactionId);
					}
				} else {
					UserMetaDataSync sync = new UserMetaDataSync ("", IsisSystem.GetMyAddress (), data);
					group.OrderedSend (FileServerComm.UpdateUserMetaData, sync);
					operationResult = true;
				}
				Logger.Debug ("File Operations Synch - sendsynchUserMetaData >> END Operation Status " + operationResult);
				return operationResult;
			} catch (Exception e) {
				Logger.Debug("File Operations Synch - sendsynchUserMetaData threw an Exception " + e.ToString());
				return false;
			}
		}

		/* 
			This Function is called when a New File Needs to be Added into the memory
		 */
		public bool sendsynchaddFileToMemory (string UserName, UserFile file, OOBHandler oobhandle, Group group, List<Address> where)
		{
			try {
				Logger.Debug ("File Operations Synch - sendsynchaddFileToMemory >> BEGIN");
				bool operationResult = false;
				MemoryMappedFile transferFile = null;
				
				string fileName = FileServerComm.getInstance ().transManager.generateTransactionId (UserName + "_" + file.filemetadata.filepath);
				Transaction trans = new Transaction (fileName);
				
				if (true == FileServerComm.getInstance ().transManager.insertTransaction (trans)) {
					try {
						
						int writtenBytesLength = 0;
						transferFile = oobhandle.serializeIntoMemoryMappedFile (fileName, file,ref writtenBytesLength);

						oobhandle.sendOOBData (group, transferFile, fileName, where);

						trans.waitTillSignalled ();
						Logger.Debug ("File Operations Synch - sendsynchaddFileToMemory >> OOB Transfer of Data Complete for : " 
						              + file.filemetadata.filepath);

						operationResult = !trans.isTimedOut;

						OOBTransaction oobtrabs = new OOBTransaction (fileName, IsisSystem.GetMyAddress (), writtenBytesLength);
						
						if (operationResult) {
							group.OrderedSend (FileServerComm.SaveFileToMemory, oobtrabs);
							trans.waitTillSignalled ();
							Logger.Debug ("File Operations Synch - sendsynchaddFileToMemory >> Ordered Send Complete Complete for : " 
							              + file.filemetadata.filepath);
							operationResult = !trans.isTimedOut;
						}
						FileServerComm.getInstance ().transManager.removeAndGetTransaction (fileName);
						operationResult = !trans.isTimedOut;
					} catch (Exception e) {
						Logger.Debug ("Exception during File Operations Synch - sendsynchaddFileToMemory" + e.ToString ());
						FileServerComm.getInstance ().transManager.removeAndGetTransaction (fileName);
						operationResult = false;
					}
				} else {
					Logger.Debug ("File Operations Synch - sendsynchaddFileToMemory >> Generation of Transaction ID Failed: " + fileName);
				}
				return operationResult;
			} catch (Exception e) {
				Logger.Debug("Caught Exception " + e.ToString());
			}
			return false;
		}

		/*
		 * This Function Will be Called when the User Meta Data is Changed
		 * Typical Call is when a File is Shared or Unshared, the Meta Data Changes and we store this information
		 * and Synch it across all the File Server Nodes
		*/
		public bool sendsynchFileMetaData (FileMetaData filemetadata, OOBHandler handler, Group group, bool waitToFinish = true)
		{
			try {
				Logger.Debug ("File Operations Synch - sendsynchFileMetaData >> BEGIN");
				bool operationResult = false;

				if (waitToFinish) {
					string transactionId = FileServerComm.getInstance ().transManager.generateTransactionId (filemetadata.filepath);
					Transaction trans = new Transaction (transactionId);
					if (true == FileServerComm.getInstance ().transManager.insertTransaction (trans)) {
						FileMetaDataSync sync = new FileMetaDataSync (transactionId, IsisSystem.GetMyAddress (), filemetadata);
						group.OrderedSend (FileServerComm.UpdateFileMetaData, sync);
						trans.waitTillSignalled ();
						FileServerComm.getInstance ().transManager.removeAndGetTransaction (transactionId);
						operationResult = !trans.isTimedOut;
					} else {
						Logger.Debug ("File Operations Synch - sendsynchUserMetaData >> Generation of Transaction ID Failed: " + transactionId);
					}
				} else {
					FileMetaDataSync sync = new FileMetaDataSync ("", IsisSystem.GetMyAddress (), filemetadata);
					group.OrderedSend (FileServerComm.UpdateUserMetaData, sync);
					operationResult = true;
				}

				Logger.Debug ("File Operations Synch - sendsynchFileMetaData >> END Operation Status " + operationResult);
				return operationResult;
			} catch (Exception e) {
				Logger.Debug ("File Operations Synch - sendsynchFileMetaData encountered Exception " + e.ToString());
				return false;
			}

		}

		/*
			This Function is called when a File Needs to be Delete from the Memory
			This could be when the LRU replacement policy is chosen and we need to delete a File From memory
		*/
		public bool sendsynchdeleteFileFromMemory (FileMetaData filemetadata, OOBHandler handler, Group group, bool waitToFinish = true)
		{
			try {
				Logger.Debug ("File Operations Synch - sendsynchdeleteFileFromMemory >> BEGIN");
				bool operationResult = false;
				
				if (waitToFinish) {
					string transactionId = FileServerComm.getInstance ().transManager.generateTransactionId (filemetadata.filepath);
					Transaction trans = new Transaction (transactionId);
					if (true == FileServerComm.getInstance ().transManager.insertTransaction (trans)) {
						FileMetaDataSync sync = new FileMetaDataSync (transactionId, IsisSystem.GetMyAddress (), filemetadata);
						group.OrderedSend (FileServerComm.DeleteFileFromMemory, sync);
						trans.waitTillSignalled ();
						FileServerComm.getInstance ().transManager.removeAndGetTransaction (transactionId);
						operationResult = !trans.isTimedOut;
					} else {
						Logger.Debug ("File Operations Synch - sendsynchUserMetaData >> Generation of Transaction ID Failed: " + transactionId);
					}
				} else {
					FileMetaDataSync sync = new FileMetaDataSync ("", IsisSystem.GetMyAddress (), filemetadata);
					group.OrderedSend (FileServerComm.DeleteFileFromMemory, sync);
					operationResult = true;
				}
				
				Logger.Debug ("File Operations Synch - sendsynchdeleteFileFromMemory >> END Operation Status " + operationResult);
				return operationResult;
			} catch (Exception e) {
				Logger.Debug("File Operations Synch - sendsynchdeleteFileFromMemory Encountered Exception: " + e.ToString());
				return false;
			}
		}

		/*
		 * This Function is to be called when the File is Deleted from the Users List
		*/
		public bool sendsynchdeleteFile (FileMetaData filemetadata, OOBHandler handler, Group group, bool waitToFinish = true)
		{
			try {
				Logger.Debug ("File Operations Synch - sendsynchdeleteFile >> BEGIN");
				bool operationResult = false;

				if (waitToFinish) {
					string transactionId = FileServerComm.getInstance ().transManager.generateTransactionId (filemetadata.filepath);
					Transaction trans = new Transaction (transactionId);
					if (true == FileServerComm.getInstance ().transManager.insertTransaction (trans)) {
						FileMetaDataSync sync = new FileMetaDataSync (transactionId, IsisSystem.GetMyAddress (), filemetadata);
						group.OrderedSend (FileServerComm.DeleteFile, sync);
						trans.waitTillSignalled ();
						FileServerComm.getInstance ().transManager.removeAndGetTransaction (transactionId);
						operationResult = !trans.isTimedOut;
					} else {
						Logger.Debug ("File Operations Synch - sendsynchdeleteFile >> Generation of Transaction ID Failed: " + transactionId);
					}
				} else {
					FileMetaDataSync sync = new FileMetaDataSync ("", IsisSystem.GetMyAddress (), filemetadata);
					group.OrderedSend (FileServerComm.DeleteFile, sync);
					operationResult = true;
				}


				Logger.Debug ("File Operations Synch - sendsynchdeleteFileFromMemory >> END Operation Status " + operationResult);
				return operationResult;
			} catch (Exception e) {
				Logger.Debug("File Operations Synch - sendsynchdeleteFile encountered a Excetpion: " + e.ToString());
				return false;
			}
		}


		/*
		 * This Function is to be Called when a Entire User Needs to be Moved
		*/
		public bool sendsynchUser (UserFileSystem file, OOBHandler oobhandle, Group group, List<Address> where)
		{
			try {
				Logger.Debug ("File Operations Synch - sendsynchUser >> BEGIN");
				bool operationResult = false;
				string fileName = FileServerComm.getInstance ().transManager.generateTransactionId (file.metadata.clientId);

				Transaction trans = new Transaction (fileName);

				if (true == FileServerComm.getInstance ().transManager.insertTransaction (trans)) {
					try {
						MemoryMappedFile transferFile = null;
						int writtenBytesLength = 0;
						transferFile = oobhandle.serializeIntoMemoryMappedFile (fileName, file, ref writtenBytesLength);
						
						OOBTransaction oobtrabs = new OOBTransaction (fileName, IsisSystem.GetMyAddress (), writtenBytesLength);
						oobhandle.sendOOBData (group, transferFile, fileName, where);

						trans.waitTillSignalled ();
						operationResult = !trans.isTimedOut;

						if (operationResult) {
							group.OrderedSend (FileServerComm.UpdateUser, oobtrabs);
							trans.waitTillSignalled ();
							operationResult = !trans.isTimedOut;
						}
						FileServerComm.getInstance ().transManager.removeAndGetTransaction (fileName);
						operationResult = !trans.isTimedOut;
					} catch (Exception e) {
						Logger.Debug ("Exception during File Operations Synch - sendsynchUser" + e.ToString ());
					}
				}

				return operationResult;
			} catch (Exception e) {
				Logger.Debug ("File Operations Synch - sendsynchUser >> Encountered Exception : " + e.ToString());
				return false;
			}
		}

		public bool sendSynchShareRequest (ShareFileWithUser request, OOBHandler handler, Group group, bool waitToFinish = true)
		{
			try {
				Logger.Debug ("File Operations Synch - sendSynchShareRequest >> BEGIN");
				bool operationResult = false;
			
				if (waitToFinish) {
					string transactionId = FileServerComm.getInstance ().transManager.generateTransactionId (request.filename);
					Transaction trans = new Transaction (transactionId);
					if (true == FileServerComm.getInstance ().transManager.insertTransaction (trans)) {
						SyncSharedUser sync = new SyncSharedUser (transactionId, IsisSystem.GetMyAddress (), 
						                                          request.clientId,
						                                          request.password,
						                                          request.filename, request.sharedWithUser);
						group.OrderedSend (FileServerComm.ShareWithUser, sync);
						trans.waitTillSignalled ();
						FileServerComm.getInstance ().transManager.removeAndGetTransaction (transactionId);
						operationResult = !trans.isTimedOut;
					} else {
						Logger.Debug ("File Operations Synch - sendsynchdeleteFile >> Generation of Transaction ID Failed: " + transactionId);
					}
				} else {
					SyncSharedUser sync = new SyncSharedUser ("", IsisSystem.GetMyAddress (), request.clientId,
					                                          request.password, request.filename, request.sharedWithUser);
					group.OrderedSend (FileServerComm.ShareWithUser, sync);
					operationResult = true;
				}
				
				
				Logger.Debug ("File Operations Synch - sendsynchdeleteFileFromMemory >> END Operation Status " + operationResult);
				return operationResult;
			} catch (Exception e) {
				Logger.Debug ("File Server Creation Failed");
				return false;
			}
		}

		public bool sendSynchUnShareRequest (UnShareFileWithUser request, OOBHandler handler, Group group, bool waitToFinish = true)
		{
			try {
				Logger.Debug ("File Operations Synch - sendSynchUnShareRequest >> BEGIN");
				bool operationResult = false;
				
				if (waitToFinish) {
					string transactionId = FileServerComm.getInstance ().transManager.generateTransactionId (request.filename);
					Transaction trans = new Transaction (transactionId);
					if (true == FileServerComm.getInstance ().transManager.insertTransaction (trans)) {
						SyncUnSharedUser sync = new SyncUnSharedUser (transactionId, IsisSystem.GetMyAddress (), request.clientId,
						                                              request.password, request.filename, request.sharedWithUser);
						group.OrderedSend (FileServerComm.UnshareWithUser, sync);
						trans.waitTillSignalled ();
						FileServerComm.getInstance ().transManager.removeAndGetTransaction (transactionId);
						operationResult = !trans.isTimedOut;
					} else {
						Logger.Debug ("File Operations Synch - sendsynchdeleteFile >> Generation of Transaction ID Failed: " + transactionId);
					}
				} else {
					SyncUnSharedUser sync = new SyncUnSharedUser ("", IsisSystem.GetMyAddress (), request.clientId,
					                                            request.password, request.filename, request.sharedWithUser);
					group.OrderedSend (FileServerComm.UnshareWithUser, sync);
					operationResult = true;
				}
				
				
				Logger.Debug ("File Operations Synch - sendsynchdeleteFileFromMemory >> END Operation Status " + operationResult);
				return operationResult;
			} catch (Exception e) {
				Logger.Debug("File Operations Synch - sendSynchUnShareRequest Encountered a Exeption: " + e.ToString());
				return false;
			}
		}

		public void TryReleaseLock (Address addr, string transactionID)
		{
			if (addr.Equals (IsisSystem.GetMyAddress ())) {
				Transaction trans = FileServerComm.getInstance().transManager.removeAndGetTransaction(transactionID);
				if(null != trans){
					trans.signalTransactionEnd();
				}
			}
		}

		/*
		 * The Handler Function which handles the User Meta Data Change
		*/
		public bool handleUserMetaData (UserMetaDataSync request)
		{
			try {
				Logger.Debug ("handleFileUserMetaData - Begin");

				if (request.initiatedSystemId.Equals (IsisSystem.GetMyAddress ())) {
					TryReleaseLock (request.initiatedSystemId, request.transactionID);
					Logger.Debug ("handleFileUserMetaData - End");
					return true;
				}

				UserMetaData userdata = new UserMetaData (request.clientId, request.password, request.versionNumber, 0);

				try {
					filesystem.updateMetadataSynchronized (userdata);
					TryReleaseLock (request.initiatedSystemId, request.transactionID);
					Logger.Debug ("handleFileUserMetaData - End");
				} catch (UserNotLoadedInMemoryException e) {
					Logger.Debug ("Exception: " + e.ToString ());
					filesystem.addUserSynchronized (request.clientId, request.password);
					TryReleaseLock (request.initiatedSystemId, request.transactionID);
					Logger.Debug ("handleFileUserMetaData - End");
				}
				return true;
			} catch (Exception e) {
				Logger.Debug("handleUserMetaData Encountered an exception " + e.ToString());
				return false;
			}
		}

		/*
		 * Handler Function which Adds a File into Memory
        */
		public void handleAddFileToMemory (object arg)
		{
			try {
				OOBTransaction request = (OOBTransaction)arg;
				UserFile userfilesys = null;
				Logger.Debug ("Update addFileToMemory Data Synchronized - Begin");
				
				Group group = FileServerComm.getInstance ().getFileServerGroup ();
				OOBHandler oobhandle = FileServerComm.getInstance ().getOOBHandler ();
				
				MemoryMappedFile transferredFile = group.OOBFetch (request.transactionID);
				
				if (null != transferredFile) {
					Logger.Debug ("getUserFileInfo OOB Fetch Success :)");
					int index = 0;
					userfilesys = oobhandle.deserializeFromMemoryMappedFile (transferredFile, ref index, request.fileLength) as UserFile;
				} else {
					Logger.Debug ("getUserFileInfo Failed Reason: OOB Fetch Failed:)");
				}

				if (!request.initiatedSystemId.Equals (IsisSystem.GetMyAddress ())
					&& null != userfilesys) {
					filesystem.addFileSynchronized (userfilesys.filemetadata.owner, userfilesys);
				}
				group.OOBDelete (request.transactionID);
				
				TryReleaseLock (request.initiatedSystemId, request.transactionID);
				Logger.Debug ("Update addFileToMemory Data Synchronized - End");
			} catch (Exception e) {
				Logger.Debug("Update addFileToMemory Data Synchronized encountered an exception " + e.ToString());
			}
		}

		/*
		 * Handler Function which Deletes a File and Marks it for Deletion if Required
        */
		public bool handleDeleteFile (FileMetaDataSync request)
		{
			try {
				Logger.Debug ("handleDeleteFile - Begin");
				TryReleaseLock (request.initiatedSystemId, request.transactionID);

				if (!request.initiatedSystemId.Equals (IsisSystem.GetMyAddress ())) {
					bool delete = filesystem.deleteFileSynchronized (request.owner, request.filepath);
					
					if (!delete) {
						Logger.Debug ("The file : " + request.filepath + " was already marked for deletion, skipping");
					}
				}
				Logger.Debug ("handleDeleteFile - End");
				return true;
			} catch (Exception e) {
				Logger.Debug("handleDeleteFile Encountered an Exception " + e.ToString());
				return false;
			}
		}

		/*
		 * Handler Function which deltes a File From Memory if Required
		*/
		public bool handledeleteFileFromMemory (FileMetaDataSync request)
		{
			Logger.Debug ("deleteFileFromMemory - Begin");
			TryReleaseLock(request.initiatedSystemId,request.transactionID);
			Logger.Debug ("deleteFileFromMemory - End");
			return true;
		}

		/*
		 * Handler Function which handles Change in Meta Data of the File
		*/
		public bool handleFileMetaData (FileMetaDataSync request)
		{
			Logger.Debug ("handleFileMetaData - Begin");
			TryReleaseLock(request.initiatedSystemId,request.transactionID);
			Logger.Debug ("handleFileMetaData - End");
			return true;
		}

		public void handleAddUser (object arg)
		{
			try {
				Logger.Debug ("Update addUser To Memory Synchronized - Begin");
				OOBTransaction request = (OOBTransaction)arg;
				UserFileSystem fileSystem = null;

				Group group = FileServerComm.getInstance ().getFileServerGroup ();
				OOBHandler oobhandle = FileServerComm.getInstance ().getOOBHandler ();
				
				MemoryMappedFile transferredFile = group.OOBFetch (request.transactionID);
				
				if (null != transferredFile) {
					Logger.Debug ("AddUser for OOB Fetch Success :)");
					int index = 0;
					fileSystem = oobhandle.deserializeFromMemoryMappedFile (transferredFile, ref index, request.fileLength) as UserFileSystem;
					Logger.Debug ("Received User is " + fileSystem.ToString ());
				} else {
					Logger.Debug ("AddUser for OOB Fetch Success :(");
				}
				group.OOBDelete (request.transactionID);

				TryReleaseLock (request.initiatedSystemId, request.transactionID);
				Logger.Debug ("Update addUser To Memory Synchronized - End");
			} catch (Exception e) {
				Logger.Debug("Update addUser To Memory Synchronized encountered exception " + e.ToString());
			}
		}

		public bool handleShareRequest (SyncSharedUser request)
		{
			try {
				Logger.Debug ("handleShareRequest - Begin");
				if (request.initiatedSystemId.Equals (IsisSystem.GetMyAddress ())) {
					TryReleaseLock (request.initiatedSystemId, request.transactionID);
					Logger.Debug ("handleShareRequest - End");
					return true;
				}
				try {
					filesystem.shareFileWithUser (request.clientId, request.filename, request.sharedWithUser);
				} catch (Exception e) {
					Logger.Debug ("Exception : " + e.ToString ());
				}
				Logger.Debug ("handleShareRequest - End");
				return true;
			} catch (Exception e) {
				Logger.Debug("handleShareRequest Encountered an Exception " + e.ToString());
				return false;
			}
		}

		public bool handleUnshareRequest (SyncUnSharedUser request)
		{
			Logger.Debug ("handleUnshareRequest - Begin");
			if (request.initiatedSystemId.Equals (IsisSystem.GetMyAddress ())) {
				TryReleaseLock (request.initiatedSystemId, request.transactionID);
				Logger.Debug ("handleUnshareRequest - End");
				return true;
			}
			try {
				filesystem.unShareFileWithUser (request.clientId, request.filename, request.sharedWithUser);
			} catch (Exception e) {
				Logger.Debug ("Exception : " + e.ToString());
			}
			Logger.Debug ("handleUnshareRequest - End");
			return true;
		}
	}
}

