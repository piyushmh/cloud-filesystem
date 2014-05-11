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

		public CheckpointLogic (){} 

		public CheckPointObject RestoreFileSystem (bool restoreFileContent)
		{
			logger.Debug ("Restoring file system with restore file content as : " + restoreFileContent);
			CheckPointObject checkObject = new CheckPointObject (); 
			string lastcheckpointfilepath = pathprefix + lastcheckpointfilename; 

			List<string> lastcheckpointfilecontent = FileUtils.ReadLinesFromFile (lastcheckpointfilepath);
			logger.Debug ("Last check point time stamp read :" + lastcheckpointfilecontent [0].Trim ());
			DateTime lastcheckpointtime = DateUtils.getDatefromString (lastcheckpointfilecontent [0].Trim ());

			logger.Debug ("Read last checkpoint time as :" + lastcheckpointtime);
			checkObject.lastcheckpoint = lastcheckpointtime;
			logger.Debug ("Poop : " + checkObject.lastcheckpoint);
			if (lastcheckpointfilecontent.Count < 2)
				throw new DiskuserMetaDataCorrupt ("Something wrong with the last check point file path, check!!");
			
			string latestcheckpointfolderpath = lastcheckpointfilecontent [1]; 
			logger.Debug ("Last check point path read as :" + latestcheckpointfolderpath);
			latestcheckpointfolderpath = latestcheckpointfolderpath.Trim (); //remove any leading or trailing whitespaces
			foreach (string userfolder in Directory.EnumerateDirectories(latestcheckpointfolderpath)) {
				checkObject.userfilesystemlist.Add (RestoreUserFileSystem (userfolder, restoreFileContent));	
			}
			
			logger.Debug ("Returning the check point object as :" + checkObject.userfilesystemlist.Capacity);
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
					userfilesystem.sharedFiles.Add (new SharedFile (contentlist [0].Trim (), contentlist [1].Trim ()));
				} else {
					logger.Warn ("File content for shared-info.txt is corrupt, check what happened " + sharefilecontent);
				}
			}
			
			//now restore the normal files
			List<string> userfilenames = FileUtils.ReadLinesFromFile (fileinfofileapth);
			logger.Debug ("# Owned file : " + userfilenames.Count);
			
			foreach (string filepath in userfilenames) {
				String relativeDiskFilePath = filepath.Trim ();
				if (relativeDiskFilePath.Length > 0){
					relativeDiskFilePath = FileUtils.getDiskPathFromMemoryPath(relativeDiskFilePath.Substring (1));
					UserFile file = RestoreUserFile(userdir, relativeDiskFilePath, restoreFileContent);
					userfilesystem.filemap.Add(FileUtils.getMemoryPathFromDiskPath(filepath.Trim()), file);
				}
			}
			return userfilesystem;
		}


		private UserFile RestoreUserFile (string userdir, string relativefilepath, bool restoreFileContent)
		{	
			logger.Debug ("Restoring user file for file path and flag :" + userdir + " " + relativefilepath + " " + restoreFileContent);
			string completefilepath = userdir + Path.DirectorySeparatorChar 
				+ "files" + Path.DirectorySeparatorChar + relativefilepath;
			string metadatafilepath = userdir + Path.DirectorySeparatorChar 
				+ "metadata" + Path.DirectorySeparatorChar + relativefilepath + ".dat";

			List<string> userfilemetadata = FileUtils.ReadLinesFromFile (metadatafilepath);
			
			if (userfilemetadata.Count < 2)
				throw new DiskuserMetaDataCorrupt ("File meta data corrupt for file  : " + relativefilepath);
			
			UserFile file = GetFileFromFileMetaData (FileUtils.getMemoryPathFromDiskPath(relativefilepath), userfilemetadata);
			
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
			if (oldcheckpointobject == null) {
				logger.Debug ("DAFUQ");
			}
			logger.Debug ("Poop  : " + oldcheckpointobject.ToString ());
			filesystem = mergeCheckPointObjects (filesystem, oldcheckpointobject);
			
			logger.Debug (filesystem);
			/*
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
			}*/

		}
		
		
		//Merge the check point objects
		private CheckPointObject mergeCheckPointObjects (CheckPointObject newimage, CheckPointObject oldimage)
		{
			logger.Debug ("Merge check point objects");
			logger.Debug ("Yo : " + newimage);
			logger.Debug ("Yo1 : " + oldimage);
			logger.Debug ("POOP2 : " + newimage.userfilesystemlist.Capacity);
			logger.Debug ("POOP3 : " + oldimage.userfilesystemlist.Capacity);
			CheckPointObject retObject = new CheckPointObject (); //ret object
			foreach (UserFileSystem oldfs in oldimage.userfilesystemlist) {
				foreach (UserFileSystem newfs in newimage.userfilesystemlist) {
					if (oldfs.metadata.clientId.Equals (newfs.metadata.clientId)) { //match based on user id
						retObject.userfilesystemlist.Add(mergeUserFileSystems (newfs, oldfs));				
					}
				}
			}
			return retObject;
			
		}
		
		//Merge the user file system
		private UserFileSystem mergeUserFileSystems (UserFileSystem newfs, UserFileSystem oldfs)
		{
			logger.Debug ("Merging user file systems for user id : " + newfs.metadata.clientId);
			
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
				logger.Warn ("The version number for the new user metadata is lower than the old, FIX ME FIX ME");
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
						logger.Debug ("File found marked for deleting, skipping it : " + filename);
					}
					
				} else { // case where there is old file and new file
					
					if (newfile.filemetadata.versionNumber >= oldfile.filemetadata.versionNumber) { //lets roll
						if (newfile.filemetadata.markedForDeletion == true) { //remove this file now
							oldfs.removeFromMap (filename); //this will decrement the size
						} else {
							long sizediff = newfile.filemetadata.filesize - oldfile.filemetadata.filesize;
							oldfs.filemap [filename] = newfile;
							oldfs.incrementTotalFileSystemSize (sizediff);
						}
					}
				}
				
			}
			
			return oldfs;
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

