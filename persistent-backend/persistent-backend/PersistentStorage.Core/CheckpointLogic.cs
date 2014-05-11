using System;
using System.IO;
using System.Collections.Generic;

/*Author - piyush*/
namespace persistentbackend
{
	/*Core check pointing logic lives here*/
	public class CheckpointLogic
	{
		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(CheckpointLogic));

		//This works on linux. Will read this from config later and make this OS agnostic
		private string pathprefix = "../../persistent-disk/"; 

		private string USERINFOFILENAME = "user-info.txt";
		
		private string FILEINFOFILENAME = "file-info.txt";
		
		private string SHAREDFILEINFONAME = "shared-info.txt";

		/*Name of file which stores the last check point path*/
		private string lastcheckpointfilename = "lastcheckpoint.txt";  //name of the file contains the last checkpoint time stamp, this is just next to the path prefix folder

		public CheckpointLogic (){} /

		public CheckPointObject RestoreFileSystem (bool restoreFileContent)
		{
			logger.Debug ("Restoring file system with restore file content as : " + restoreFileContent);
			CheckPointObject checkObject = new CheckPointObject (); 
			string lastcheckpointfilepath = pathprefix + lastcheckpointfilename; 

			List<string> lastcheckpointfilecontent = FileUtils.ReadLinesFromFile (lastcheckpointfilepath);
			logger.Debug ("Last check point time stamp read :" + lastcheckpointfilecontent [0].Trim ());
			DateTime lastcheckpointtime = DateUtils.getDatefromString (lastcheckpointfilecontent [0].Trim ());

			checkObject.lastcheckpoint = lastcheckpointtime;
			logger.Debug ("Poop : " + checkObject.lastcheckpoint);
			if (lastcheckpointfilecontent.Count < 2)
				throw new DiskuserMetaDataCorrupt ("Something wrong with the last check point file path, check!!");
			
			string latestcheckpointfolderpath = lastcheckpointfilecontent [1]; 
			logger.Debug("Last check point path read as :" + latestcheckpointfolderpath);
			latestcheckpointfolderpath = latestcheckpointfolderpath.Trim (); //remove any leading or trailing whitespaces
			foreach (string userfolder in Directory.EnumerateDirectories(latestcheckpointfolderpath)) {
				checkObject.userfilesystemlist.Add(RestoreUserFileSystem(userfolder, restoreFileContent));	
			}

			return checkObject;
		}
		
		// Method used for restoring the user file system
		private UserFileSystem RestoreUserFileSystem (string userdir, bool restoreFileContent)
		{
			logger.Debug ("Restoring user file system for user dir path and restore file content :" + userdir + " " + restoreFileContent);
			UserFileSystem userfilesystem = new UserFileSystem (); //this is what we return from this method
			string user = new DirectoryInfo (userdir).Name; //fetch the user name from directory name
			
			string userinfofilepath = userdir + Path.DirectorySeparatorChar + this.USERINFOFILENAME;
			string fileinfofileapth = userdir + Path.DirectorySeparatorChar + this.FILEINFOFILENAME;
			string sharedfileinfofilepath = userdir + Path.DirectorySeparatorChar + this.SHAREDFILEINFONAME;
			
			//First restore the user info
			List<String> metadatafilecontent = FileUtils.ReadLinesFromFile (userinfofilepath);
			if (metadatafilecontent.Count < 3) {
				throw new DiskuserMetaDataCorrupt ("Disk meta data corrupt for user: " + user);
			}

			//add the user meta data into the file system
			userfilesystem.metadata = new UserMetaData (
				metadatafilecontent [0].Trim (), 
				metadatafilecontent [1].Trim (),
				int.Parse (metadatafilecontent [2].Trim ())
			);
			
			//now restore the shared files
			List<string> sharedFileNames = FileUtils.ReadLinesFromFile (sharedfileinfofilepath);
			logger.Debug ("# shared file : " + sharedFileNames.Count);
			foreach (string sharefilecontent in sharedFileNames) {
				string[] contentlist = sharefilecontent.Split (' ');
				if (contentlist.Length > 1) {
					userfilesystem.sharedFiles.Add (new SharedFile (contentlist [1].Trim (), contentlist [2].Trim ()));
				} else {
					logger.Warn ("File content for shared-info.txt is corrupt, check what happened " + sharefilecontent);
				}
			}
			
			//now restore the normal files
			List<string> userfilenames = FileUtils.ReadLinesFromFile (fileinfofileapth);
			logger.Debug ("# Owned file : " + userfilenames.Count);
			
			foreach (string filepath in userfilenames) {
				UserFile file = RestoreUserFile(userdir, filepath.Trim(), restoreFileContent);
				userfilesystem.filemap.Add(filepath.Trim(), file);
			}
			return userfilesystem;
		}


		private UserFile RestoreUserFile (string userdir, string relativefilepath, bool restoreFileContent)
		{	
			logger.Debug ("Restoring user file for file path and flag :" + userdir + relativefilepath + " " + restoreFileContent);
			string completefilepath = userdir + Path.DirectorySeparatorChar 
				+ "files" + Path.DirectorySeparatorChar + relativefilepath;
			string metadatafilepath = userdir + Path.DirectorySeparatorChar 
				+ "metadata" + Path.DirectorySeparatorChar + relativefilepath + ".dat";

			List<string> userfilemetadata = FileUtils.ReadLinesFromFile (metadatafilepath);
			
			if (userfilemetadata.Count < 2)
				throw new DiskuserMetaDataCorrupt ("File meta data corrupt for file  : " + relativefilepath);
			
			UserFile file = GetFileFromFileMetaData (relativefilepath, userfilemetadata);
			
			//this makes sure that in case of checkpointing this stuff, we don't load the file content, I know it's pretty neat :)
			if (restoreFileContent) 
				file.filecontent = File.ReadAllBytes(completefilepath);
			
			return file;
		}

		private UserFile GetFileFromFileMetaData (string filepath, List<string> metadata)
		{
			UserFile file = new UserFile (filepath, metadata [0].Trim ());
			file.filemetadata.filepath = filepath;
			
			file.filemetadata.filesize = int.Parse (metadata [1].Trim ());
			file.filemetadata.versionNumber = int.Parse (metadata [2].Trim ());

			if (metadata.Count > 3) { //this is because this file might be shared with no one
				string [] clients = metadata [3].Trim ().Split (',');
			
				foreach (string client in clients) {
					if (client.Trim ().Length > 0) {
						file.filemetadata.sharedwithclients.Add (client.Trim ());
					}
				}
			}
			return file;
		}
	
		
		
		/*Check point entry */
		public void DoCheckPointAllUsers (CheckPointObject filesystem)
		{
			logger.Debug ("Do Check point method called for filesystem");
			
			CheckPointObject oldcheckpointobject = RestoreFileSystem (true); //load the old file in memory to merege
			filesystem = mergeCheckPointObjects (filesystem, oldcheckpointobject);
			
			try{
				string path = GenerateCheckpointPath (filesystem.lastcheckpoint);
				logger.Debug ("Creating checkpointing path :" + path);
				Directory.CreateDirectory (path);
				string lastcheckpointfilepath = this.pathprefix + this.lastcheckpointfilename;
				foreach( UserFileSystem userfs in filesystem.userfilesystemlist){
					DoCheckPointForUser( userfs.metadata.clientId, path, userfs);
				}
			
				//Now update the last check point file so that we can restore this checkpoint
				logger.Debug ("Writing to last checkpoint file at path : " + lastcheckpointfilepath);
				System.IO.File.WriteAllText(lastcheckpointfilepath, 
				          DateUtils.getStringfromDateTime(filesystem.lastcheckpoint) + "\n" + path);

			}catch ( Exception e){
					logger.Debug ("Exception :" + e);
				throw e;
			}

		}
		
		
		//Merge the check point objects
		private CheckPointObject mergeCheckPointObjects (CheckPointObject newimage, CheckPointObject oldimage)
		{
			logger.Debug("Merge check point objects");
			CheckPointObject retObject = new CheckPointObject (); //ret object
			foreach (UserFileSystem oldfs in oldimage) {
				foreach (UserFileSystem newfs in newimage) {
					if (oldfs.metadata.clientId.Equals (newfs.metadata.clientId)) { //match based on user id
						retObject.userfilesystemlist = mergeUserFileSystems (newfs, oldfs);				
					}
				}
			}
			return retObject;
			
		}
		
		//Merge the user file system
		private UserFileSystem mergeUserFileSystems (UserFileSystem newfs, UserFileSystem oldfs)
		{
			logger.Debug ("Merging user file systems for user id : " + newfs.metadata.clientId);
			
			
		}
		
		private string GetParentDirectoryPath( string fullpath){
			return Path.GetDirectoryName(fullpath) + Path.DirectorySeparatorChar;
		}

		private string GenerateCheckpointPath(DateTime checkpointtime){
			DateTime time = checkpointtime;
			string path = time.Year + "-" + time.Month + "-" + time.Day + Path.DirectorySeparatorChar + 
				time.Hour + "-" + time.Minute + Path.DirectorySeparatorChar;
			return this.pathprefix + path;
		}
	}
}

