using System;
using Isis;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Timers;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using ServiceStack;
using System.Threading;

namespace cloudfileserver
{
	[Serializable]
	[AutoMarshalled]
	public class BootStrappingCheckPoint
	{
		public string fileName;
		public int operationType;
		public Address nodeAddress;
		public int status;
		public int dataLength;

		public BootStrappingCheckPoint(string file, int operation, 
		                             Address addr, int _Status,
		                             int length)
		{
			fileName = file;
			operationType = operation;
			nodeAddress = addr;
			status = _Status;
			dataLength = length;
		}

		public BootStrappingCheckPoint ()
		{
		}
	}

	[Serializable]
	[AutoMarshalled]
	public class BootStrappingResponse
	{
		public string fileName;
		public Address nodeAddress;
		public int status;
		
		public BootStrappingResponse(string file, 
		                             Address addr, 
		                             int _Status)
		{
			fileName = file;
			nodeAddress = addr;
			status = _Status;
		}
		
		public BootStrappingResponse ()
		{
		}
	}

	/*
	 * This Class is Used to Initiate a Boot Strapping Request to a Remote Node
	*/
	[Serializable]
	[AutoMarshalled]
	public class BootStrappingRequest
	{
		public Address requestNodeAddress;
		public string RequestName;

		public BootStrappingRequest (Address addr, string requestName)
		{
			requestNodeAddress = addr;
			this.RequestName = requestName;
		}

		public BootStrappingRequest ()
		{
		}
	}

	[AutoMarshalled]
	public enum eBootStrapState
	{
		Initialized,
		SentBootStrappingBeginRequest,
		RecvBootStrappingBeginResponse,
		ReceivingData,
		BootStrappingComplete
	};

	public class BootStrappingState
	{
		public Address selectedNode { get; set; }
		private System.Timers.Timer bootStrappingTimer { get; set; }
		public BootStrap parent { get; set; }
		public Group group { get; set; }
		public eBootStrapState currState { get; set; }

		static int BOOTSTRAPWAIT_TIME = 60 * 1000;

		private static readonly log4net.ILog Logger = 
			log4net.LogManager.GetLogger(typeof(BootStrappingState));

		int numberOfRetries { get; set; }

		private static void BootStrappingTimedOutEvent(object source, ElapsedEventArgs e, BootStrappingState state)
		{
			Logger.Debug ("BootStrappingTimedOutEvent No Response Receieved :) Try Again :)");
			state.numberOfRetries = state.numberOfRetries + 1;

			state.parent.resetBootStrappingState ();
			state.parent.sendBootStrappingRequest(state.group, state.selectedNode);
		}

		public BootStrappingState ()
		{
			currState = eBootStrapState.Initialized;

			selectedNode = null;
			bootStrappingTimer = null;
			parent = null;
			group = null;
		}

		public void setBootStrappingTimer (Group _group, BootStrap _parent)
		{
			group = _group;
			parent = _parent;

			bootStrappingTimer = new System.Timers.Timer(BOOTSTRAPWAIT_TIME);
			bootStrappingTimer.Elapsed += new ElapsedEventHandler((sender, e) => BootStrappingTimedOutEvent(sender, e, this));
			bootStrappingTimer.AutoReset = false;
			bootStrappingTimer.Enabled = true;
		}

		public void cancelBootStrappingTimer()
		{
			if(null != bootStrappingTimer)
				bootStrappingTimer.Enabled = false;
		}

		public void reset()
		{
			cancelBootStrappingTimer();
			bootStrappingTimer = null;
			parent = null;
			group = null;
			selectedNode = null;

			currState = eBootStrapState.Initialized;
		}
	}

	public class BootStrap
	{
		int BatchSize = 50;

		static int FAILURE = 0;
		static int SUCCESS = 1;
		
		InMemoryFileSystem fileSystem = null;
		private static readonly log4net.ILog Logger = 
			log4net.LogManager.GetLogger(typeof(BootStrap));
		public BootStrappingState state = null;

		public Semaphore waitBootStrap = null;

		public BootStrap ()
		{
			Logger.Debug ("BootStrapping, This should be Fun");
			waitBootStrap = new Semaphore(0,1);
			state = new BootStrappingState();
			fileSystem = new InMemoryFileSystem (true);
		}

		public void resetBootStrappingState ()
		{
			Logger.Debug ("BootStrapping, resetBootStrappingState()");

			fileSystem = null;
			state.reset();

			fileSystem = new InMemoryFileSystem ();
		}

		void handleBootStrappingRequest (Group group, OOBHandler oobhandle, InMemoryFileSystem fileSystem, string requestName, Address recvdFrom)
		{
			if (state.currState == eBootStrapState.BootStrappingComplete) {

				Logger.Debug("handleBootStrappingRequest , Received from " + recvdFrom.ToStringVerboseFormat());

				BootStrappingCheckPoint initialStage = null;
				initialStage = new BootStrappingCheckPoint ("Boot Strapping Begin", FileServerComm.BootStrapBegin, 
				                                          IsisSystem.GetMyAddress (), SUCCESS,
				                                          0);

				Logger.Debug("Sending a BootStrapping Begin , Response to " + recvdFrom.ToStringVerboseFormat());
				group.RawP2PSend (recvdFrom, FileServerComm.BootStrapBegin, initialStage);

				MemoryMappedFile transferFile = null;
				int currentUserIndex = 0;
				int numberOfUsersCurrentBatch = 0;

				List<string> users = fileSystem.GetInMemoryUserList ();
				InMemoryFileSystem tempFileSystem = new InMemoryFileSystem (false);

				//Yayy Lets Begin Doing Some Boot Strapping
				try {
					Logger.Debug("Number of Users to BootStrap and Send " + users.Count);
					while (currentUserIndex < users.Count) {

						UserFileSystem userfilesys = fileSystem.GetClonedInMemoryUserFileSystem (users [currentUserIndex]);
						numberOfUsersCurrentBatch++;

						Logger.Debug("Adding User to the BootStrap : " + users[currentUserIndex]);

						tempFileSystem.addFSToMapSynchronized (userfilesys, users [currentUserIndex]);
						currentUserIndex++;
						
						if (numberOfUsersCurrentBatch == BatchSize) {
							//Let's Make a OOB File and Transfer the Data
							string currentFileName = FileServerComm.getInstance ().transManager.generateTransactionId ();

							bool operationResult = false;

							numberOfUsersCurrentBatch = 0;

							Transaction trans = new Transaction (currentFileName);

							FileServerComm.getInstance ().transManager.insertTransaction (trans);

							int writtenBytesLength = 0;
							transferFile = oobhandle.serializeIntoMemoryMappedFile (currentFileName, tempFileSystem, ref writtenBytesLength);

							BootStrappingCheckPoint continueBootStrap = null;
							continueBootStrap = new BootStrappingCheckPoint (currentFileName, FileServerComm.BootStrapContinue, 
							                                               IsisSystem.GetMyAddress (), SUCCESS,
							                                               writtenBytesLength);
							List<Address> where = new List<Address>();
							where.Add(recvdFrom);
							where.Add (IsisSystem.GetMyAddress ());
							oobhandle.sendOOBData (group, transferFile, currentFileName, where);

							trans.waitTillSignalled ();
							operationResult = !trans.isTimedOut;

							if (operationResult) {
								group.RawP2PSend (recvdFrom, FileServerComm.BootStrapContinue, continueBootStrap);
								trans.waitTillSignalled ();
								operationResult = !trans.isTimedOut;
							} else {
								Logger.Debug ("Sending BootStraping Request Timed Out, Quit Out of BootStrapping partcicipation");
								return;
							}
							tempFileSystem = new InMemoryFileSystem ();
						}
					}

					//Lets Throw out the Remaining Users
					if (numberOfUsersCurrentBatch != 0) {
						string currentFileName = FileServerComm.getInstance ().transManager.generateTransactionId ();

						Transaction trans = new Transaction (currentFileName);
						FileServerComm.getInstance ().transManager.insertTransaction (trans);

						bool currentOperationResult = false;

						int writtenBytesLength = 0;
						transferFile = oobhandle.serializeIntoMemoryMappedFile (currentFileName, tempFileSystem, ref writtenBytesLength);
						BootStrappingCheckPoint _continue = null;
						_continue = new BootStrappingCheckPoint (currentFileName, FileServerComm.BootStrapContinue, 
						                                         IsisSystem.GetMyAddress (), SUCCESS,
						                                         writtenBytesLength);

						List<Address> where = new List<Address>();
						where.Add(recvdFrom);
						where.Add (IsisSystem.GetMyAddress ());

						oobhandle.sendOOBData (group, transferFile, currentFileName, where);

						trans.waitTillSignalled ();
						currentOperationResult = !trans.isTimedOut;

						if (currentOperationResult) {
							group.RawP2PSend (recvdFrom, FileServerComm.BootStrapContinue, _continue);

							trans.waitTillSignalled ();
							currentOperationResult = !trans.isTimedOut;
						} else {
							Logger.Debug ("Sending BootStraping Request Timed Out, Quit Out of BootStrapping partcicipation");
							return;
						}

						tempFileSystem = new InMemoryFileSystem ();
						numberOfUsersCurrentBatch = 0;
					}

					Logger.Debug("Sending a Boot Strap End Response to " + recvdFrom.ToString());

					BootStrappingCheckPoint finalStage = null;
					finalStage = new BootStrappingCheckPoint ("Yayyy Finally Done", FileServerComm.BootStrapEnd, 
					                                        IsisSystem.GetMyAddress (), SUCCESS,
					                                        0);

					group.RawP2PSend (recvdFrom, FileServerComm.BootStrapEnd, finalStage);

					return;
				} catch (Exception e) {
					Logger.Debug("Caught an Exception : " + e.ToString());
					BootStrappingCheckPoint _exception = null;
					_exception = new BootStrappingCheckPoint (e.ToString (), FileServerComm.BootStrapException, 
					                                          IsisSystem.GetMyAddress (), FAILURE,
					                                        0);
					group.P2PSend (recvdFrom, FileServerComm.BootStrapException, _exception);
					return;
				}
			}
			else {
				Logger.Debug("Node is Not in the State of Boot Strapping, Ignore this Request and Keep Calm");
			}
		}

		public void handleBootStrappingResponse(BootStrappingCheckPoint response, Group group, OOBHandler oobhandle)
		{
			switch (response.operationType) 
			{
				case FileServerComm.BootStrapBegin:
					handleBootStrappingBegin (response);
					break;

				case FileServerComm.BootStrapContinue:
					handleBootStrappingContinue (response,group,oobhandle);
					break;

				case FileServerComm.BootStrapEnd:
					handleBootStrappingEnd (response);
					break;
			}
		}
			
		void handleBootStrappingBegin (BootStrappingCheckPoint request)
		{
			Logger.Debug("handleBootStrappingBegin , Response received from " + request.nodeAddress.ToStringVerboseFormat());
			if (null != state.selectedNode && request.nodeAddress.Equals (state.selectedNode)) {
				if(request.status == SUCCESS){
					state.currState = eBootStrapState.RecvBootStrappingBeginResponse;
					Logger.Debug ("handleBootStrappingBegin Success Start Now:)");
				}
				else{
					Logger.Debug ("handleBootStrappingBegin Failure, Must be a Node which is BootStrapping :)");
					state.cancelBootStrappingTimer();
					sendBootStrappingRequest(state.group,state.selectedNode);
				}
			} else {
				Logger.Debug ("handleBootStrappingBegin, Receieved from Another Node, Some Problem. Ignore this Plz :)");
			}
		}

		void handleBootStrappingEnd (BootStrappingCheckPoint request)
		{
			Logger.Debug("handleBootStrappingEnd , Response received from " + request.nodeAddress.ToStringVerboseFormat());
			if (null != state.selectedNode && request.nodeAddress.Equals (state.selectedNode)) {
				Logger.Debug ("handleBootStrappingEnd :)");
				state.currState = eBootStrapState.BootStrappingComplete;
				FileServerComm fileSrvComm = FileServerComm.getInstance ();
				doDiff(fileSrvComm.getFileHandler ().filesystem);
				state.cancelBootStrappingTimer ();
				waitBootStrap.Release();
				Logger.Debug ("handleBootStrappingEnd :) Finally Done");
			}else {
				Logger.Debug ("handleBootStrappingEnd, Receieved from Another Node, Some Problem. Ignore this Plz :)");
			}
		}

		void handleBootStrappingContinue (BootStrappingCheckPoint request, Group group, OOBHandler oobhandle)
		{
			Logger.Debug("handleBootStrappingContinue , Response received from " + request.nodeAddress.ToStringVerboseFormat());
			int bootStrapStatus = SUCCESS;
			int numberOfUsersAdded = 0;

			if (null != state.selectedNode && request.nodeAddress.Equals (state.selectedNode)) {
				Logger.Debug ("handleBootStrapping Continue :)");

				state.currState = eBootStrapState.ReceivingData;
				MemoryMappedFile transferredFile = group.OOBFetch (request.fileName);

				if (transferredFile != null) {
					Logger.Debug ("File" + request.fileName + "request Transfer Successfull!");
					try 
					{
						int index = 0;
						InMemoryFileSystem tempFileSystem = null;
						tempFileSystem = oobhandle.deserializeFromMemoryMappedFile(transferredFile, ref index, request.dataLength) as InMemoryFileSystem;

						List<string> users = tempFileSystem.GetInMemoryUserList ();
						numberOfUsersAdded = users.Count;

						foreach(string user in users) {
							UserFileSystem userfilesys = tempFileSystem.GetClonedInMemoryUserFileSystem (user);
							if(null != userfilesys) {
								fileSystem.addFSToMapSynchronized(userfilesys,user);
							}
						}
					} 
					catch (Exception e) 
					{
						Logger.Debug("Exception Encountered : " + e.ToString());
						bootStrapStatus = FAILURE;
					}
				} else {
					Logger.Debug ("File" + request.fileName + "was Deleted!");
					bootStrapStatus = FAILURE;
				}

				Logger.Debug ("handleBootStrapping Added :)" + numberOfUsersAdded + " Users");
				group.OOBDelete (request.fileName);

				BootStrappingResponse response = null;
				response = new BootStrappingResponse(request.fileName, IsisSystem.GetMyAddress (), bootStrapStatus);

				group.P2PSend (request.nodeAddress, FileServerComm.BootStrapResponse, response);

			} else {
				Logger.Debug ("handleBootStrappingContinue, Receieved from Another Node, Some Problem. Ignore this Plz :)");
			}
		}

		public void sendBootStrappingRequest (Group group, Address lastSelectedNode)
		{
			Address [] liveMembers = group.getLiveMembers ();
			Address myAddress = IsisSystem.GetMyAddress ();

			if (liveMembers.Length > 1) {
				Random rnd = new Random ();

				int sendNodeIndex = 0;
				int numTries = 0;

				do {
					sendNodeIndex = rnd.Next (0, liveMembers.Length);
					if (liveMembers [sendNodeIndex].Equals (myAddress)) {
						continue;
					}
					Logger.Debug ("Stuck in a Infinite Loop : Length: " + liveMembers.Length);
					numTries = numTries + 1;
					break;
				} while(true);

				if (numTries != liveMembers.Length) {
					Logger.Debug ("sendBootStrappingRequest Succeeded :). Sending to Address " + liveMembers [sendNodeIndex].ToStringVerboseFormat ());
					Logger.Debug ("My Address is :) " + IsisSystem.GetMyAddress ().ToStringVerboseFormat ());

					BootStrappingRequest request = new BootStrappingRequest (IsisSystem.GetMyAddress (),
															FileServerComm.getInstance ().transManager.generateTransactionId ());
					state.selectedNode = liveMembers [sendNodeIndex];
					state.currState = eBootStrapState.SentBootStrappingBeginRequest;

					group.RawP2PSend (liveMembers [sendNodeIndex], FileServerComm.BootStrapRequest, request);
				} else {
					Logger.Debug ("sendBootStrappingRequest Failed :) Try After Backup");
					state.selectedNode = lastSelectedNode;
					state.currState = eBootStrapState.Initialized;
					state.setBootStrappingTimer (group, this);
				}
			} else {
				Logger.Debug("There are No Live Members Available, Just Continue :  " + liveMembers.Length);
				state.currState = eBootStrapState.BootStrappingComplete;
				waitBootStrap.Release();
			}
		}

		public void handleBootStrappingRequestPlaceHolder (object arg)
		{
			Logger.Debug("Begin handleBootStrappingRequestPlaceHolder ()");
			BootStrappingRequest request = (BootStrappingRequest) arg;
			handleBootStrappingRequest(FileServerComm.getInstance().getFileServerGroup(),
			                           FileServerComm.getInstance().getOOBHandler(),
			                           FileServerComm.getInstance().getFileHandler().filesystem,
			                           request.RequestName,
			                           request.requestNodeAddress);

		}

		public void handleBootStarppingResponse (BootStrappingResponse response)
		{
			FileServerComm.getInstance().getFileHandler().TryReleaseLock(IsisSystem.GetMyAddress(), response.fileName);
		}


		public void doDiff (InMemoryFileSystem currentFileSystem)
		{
			try {
				List<UserFileSystem> needInsertions = new List<UserFileSystem>();

				foreach (KeyValuePair<string, UserFileSystem> entry in  fileSystem.clientToFileSystemMap) {
					UserFileSystem newFileSystem = entry.Value;
					UserFileSystem oldFileSystem = currentFileSystem.getUserFSFromMapSynchronized (entry.Key);

					if (null != oldFileSystem) {
						mergeUserFileSystems (newFileSystem, oldFileSystem);
					}
					else {
						needInsertions.Add(newFileSystem);
					}
				}
				foreach(UserFileSystem tempFileSystem in needInsertions) {
					Logger.Debug("New Insertions to the FIle System " + tempFileSystem.metadata.clientId);
					currentFileSystem.addFSToMapSynchronized(tempFileSystem,tempFileSystem.metadata.clientId);
				}
			} catch (Exception e) {
				Logger.Debug("Received an Exception: " + e.ToString());
			}
		}
		//Merge the user file system
		private UserFileSystem mergeUserFileSystems (UserFileSystem newfs, UserFileSystem oldfs)
		{
			Logger.Debug ("Merging user file systems for user id : " + newfs.metadata.clientId);
			
			/*
			 * Rule of Fight Club ( UserFile system merge) 
			 * 
			 * 1) User meta data- The one with higher version number wins, although it will be mostly the 
			 * newer file system object since user meta data is always maintained in memory
			 * 
			 * 2) SharedFileList - This will be picked from the newer file system object since we don't have versioning for it
			 * and it is maintained in the memory always
			 * 
			 * 3) File map - 
			 * 		a) If there is a new file which is not present in the old file system
			 * 				If its marked for deletion  - Don't add it
			 * 				If its not marked for deletion  - Add it
			 * 		b) If there is file present in the new file system which is also present in the old file system
			 * 				If the version number of the new file is higher
			 * 						If its marked for deletion in the new file system delete that file
			 * 						If its not marked for deletion, replace the file
			 * 				If its version number is lower
			 * 						TOO BAD
			 * 		c) If there are files which are not present in the new file system which are present in old file system
			 * 				Ideally this should not happen since the all file names will always remain in memory. In any case, take the file on disk.
			 * 				
			 * 
			 */
			
			if (newfs.metadata.versionNumber >= oldfs.metadata.versionNumber) {
				oldfs.metadata = newfs.metadata; //replace
			} else {
				Logger.Warn ("The version number for the new user metadata is lower than the old, FIX ME FIX ME");
			}
			
			oldfs.sharedFiles = newfs.sharedFiles; //replace the shared file list
			
			//now iterate over the file map, don't fuck up man
			foreach (KeyValuePair<string, UserFile> entry in newfs.filemap) {
				
				UserFile newfile = entry.Value;
				UserFile oldfile = null;
				string filename = entry.Key;
				
				if (oldfs.filemap.ContainsKey (filename)) {
					oldfile = oldfs.filemap [filename];
				}
				
				if (oldfile == null) {  //case when there is new file and NO old file
					
					if (newfile.filemetadata.markedForDeletion == false) {
						oldfs.addFileSynchronized (newfile);
					} else {
						Logger.Debug ("File found marked for deleting, skipping it : " + filename);
					}
					
				} else { // case where there is old file and new file
					
					if (newfile.filemetadata.versionNumber >= oldfile.filemetadata.versionNumber) { //lets roll
						if (newfile.filemetadata.markedForDeletion == true) { //remove this file now
							Logger.Debug ("File marked for deletion, removing : " + filename);
							oldfs.removeFromMap (filename); //this will decrement the size
						} else {
							//long sizediff = newfile.filemetadata.filesize - oldfile.filemetadata.filesize;
							oldfs.filemap [filename] = newfile;
							//oldfs.incrementTotalFileSystemSize (sizediff);
						}
					}
				}
				
			}
			
			return oldfs;
		}
	}
}
	