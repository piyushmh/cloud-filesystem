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
		public static string pathprefix = "../../persistent-disk/"; 

		public static string USERINFOFILENAME = "user-info.txt";
		
		public static string FILEINFOFILENAME = "file-info.txt";
		
		public static string SHAREDFILEINFONAME = "shared-info.txt";

		/*Name of file which stores the last check point path*/
		public static string lastcheckpointfilename = "lastcheckpoint.txt";

		public CheckpointLogic (){} 

		public CheckPointObject RestoreFileSystem (bool restoreFileContent)
		{
			logger.Debug ("Restoring file system with restore file content as : " + restoreFileContent);
			CheckPointObject checkObject = new CheckPointObject (); 
			string lastcheckpointfilepath = pathprefix + lastcheckpointfilename; 

			try {
				List<string> lastcheckpointfilecontent = FileUtils.ReadLinesFromFile (lastcheckpointfilepath);
				logger.Debug ("Last check point time stamp read :" + lastcheckpointfilecontent [0].Trim ());
				DateTime lastcheckpointtime = DateUtils.getDatefromString (lastcheckpointfilecontent [0].Trim ());

				logger.Debug ("Read last checkpoint time as :" + lastcheckpointtime);
				checkObject.lastcheckpoint = lastcheckpointtime;
				if (lastcheckpointfilecontent.Count < 2)
					throw new DiskuserMetaDataCorrupt ("Something wrong with the last check point file path, check!!");
			
				string latestcheckpointfolderpath = lastcheckpointfilecontent [1]; 
				logger.Debug ("Last check point path read as :" + latestcheckpointfolderpath);
				latestcheckpointfolderpath = latestcheckpointfolderpath.Trim (); //remove any leading or trailing whitespaces
				foreach (string userfolder in Directory.EnumerateDirectories(latestcheckpointfolderpath)) {
					checkObject.userfilesystemlist.Add (RestoreUserFileSystem (userfolder, restoreFileContent));	
				}
				logger.Debug ("Returning the check point object as :" + checkObject.userfilesystemlist.Count);
			
			} catch (Exception e) {
				logger.Warn ("Exception occured while restoring the file system : " + e);
			}
			return checkObject;
		}
		
		// Method used for restoring the user file system
		private UserFileSystem RestoreUserFileSystem (string userdir, bool restoreFileContent)
		{
			logger.Debug ("Restoring user file system for user dir path and restore file content :" + userdir + " " + restoreFileContent);
			UserFileSystem userfilesystem = new UserFileSystem (); //this is what we return from this method
			string user = new DirectoryInfo (userdir).Name; //fetch the user name from directory name
			
			string userinfofilepath = userdir + Path.DirectorySeparatorChar + CheckpointLogic.USERINFOFILENAME;
			string fileinfofileapth = userdir + Path.DirectorySeparatorChar + CheckpointLogic.FILEINFOFILENAME;
			string sharedfileinfofilepath = userdir + Path.DirectorySeparatorChar + CheckpointLogic.SHAREDFILEINFONAME;
			
			//First restore the user info
			List<String> metadatafilecontent = FileUtils.ReadLinesFromFile (userinfofilepath);
			if (metadatafilecontent.Count < 4) {
				throw new DiskuserMetaDataCorrupt ("Disk meta data corrupt for user: " + user);
			}

			//add the user meta data into the file system
			userfilesystem.metadata = new UserMetaData (
				metadatafilecontent [0].Trim (), 
				metadatafilecontent [1].Trim (),
				int.Parse (metadatafilecontent [2].Trim ()),
				long.Parse (metadatafilecontent [3].Trim ())
			);
			
			//logger.Fatal ("SEE : " + long.Parse (metadatafilecontent [3].Trim ()));	
			//logger.Fatal ("SEE : " + userfilesystem.metadata.ToString());			
			
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
				if (relativeDiskFilePath.Length > 0) {	
					if (relativeDiskFilePath [0] == '_')
						relativeDiskFilePath = relativeDiskFilePath.Substring (1);
					relativeDiskFilePath = FileUtils.getDiskPathFromMemoryPath(relativeDiskFilePath);
					UserFile file = RestoreUserFile(userdir, relativeDiskFilePath, restoreFileContent);
					userfilesystem.filemap.Add(FileUtils.getMemoryPathFromDiskPath(filepath.Trim()), file);
				}
			}
			return userfilesystem;
		}

		
		public UserFile RestoreUserFile (string userdir, string relativefilepath, bool restoreFileContent)
		{	
			logger.Debug ("Restoring user file for file path and flag :" + userdir + " " + relativefilepath + " " + restoreFileContent);
			string completefilepath = userdir + Path.DirectorySeparatorChar 
				+ "files" + Path.DirectorySeparatorChar + relativefilepath;
			string metadatafilepath = userdir + Path.DirectorySeparatorChar 
				+ "metadata" + Path.DirectorySeparatorChar + relativefilepath + ".dat";

			List<string> userfilemetadata = FileUtils.ReadLinesFromFile (metadatafilepath);
			
			if (userfilemetadata.Count < 2)
				throw new DiskuserMetaDataCorrupt ("File meta data corrupt for file  : " + relativefilepath);
			
			UserFile file = GetFileFromFileMetaData (FileUtils.getMemoryPathFromDiskPath(relativefilepath),
			                                         userfilemetadata);
			
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
			logger.Debug ("Recovered metadata as : " + file.filemetadata);
			return file;
		}
	
		
		
		/*Check point entry */
		public void DoCheckPointAllUsers (CheckPointObject filesystem)
		{
			logger.Debug ("Do Check point method called for filesystem for timestamp : " + filesystem.lastcheckpoint);
			CheckPointObject oldcheckpointobject = RestoreFileSystem (true); //load the old file in memory to merege
		
			//logger.Debug ("POOP1  : " + oldcheckpointobject.userfilesystemlist[0].metadata.totalFileSystemSizeBytes);
			//logger.Debug ("POOP2  : " + filesystem.userfilesystemlist[0].metadata.totalFileSystemSizeBytes);
			DateTime newCheckPointTime = filesystem.lastcheckpoint;
			filesystem = mergeCheckPointObjects (filesystem, oldcheckpointobject);
			filesystem.lastcheckpoint = newCheckPointTime;
			//logger.Debug (filesystem);
			
			try {
				string path = GenerateCheckpointPath (filesystem.lastcheckpoint);
				logger.Debug ("Creating checkpointing path :" + path);
				Directory.CreateDirectory (path);
				string lastcheckpointfilepath = CheckpointLogic.pathprefix + CheckpointLogic.lastcheckpointfilename;
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
		
		
		public void DoCheckPointForUser (
			string user,
			string path,
			UserFileSystem userfilesystem)
		{

			logger.Debug ("Check pointing for user and path :" + user + " " + path);

			string userpath = path + user + Path.DirectorySeparatorChar; //Appending '/' for linux and '\' on windows
			Directory.CreateDirectory (userpath);
			string usermetadatafilepath = userpath + CheckpointLogic.USERINFOFILENAME;
			string sharedfilemetadatapath = userpath + CheckpointLogic.SHAREDFILEINFONAME;
			string filemetadatafilepath = userpath + CheckpointLogic.FILEINFOFILENAME;
			
			string usermetadata = userfilesystem.metadata.clientId + 
				"\n" + userfilesystem.metadata.password +
				"\n" + userfilesystem.metadata.versionNumber + 
				"\n" + userfilesystem.metadata.totalFileSystemSizeBytes;
			
			string sharefilecontent = "";
			foreach (SharedFile sharedfile in userfilesystem.sharedFiles) {
				sharefilecontent += sharedfile.owner + " " + sharedfile.filename + "\n";
			}
			
			string filelistcontent = "";
			foreach (KeyValuePair<string, UserFile> entry in  userfilesystem.filemap) {
				filelistcontent += entry.Key + "\n";
			}
			
			filelistcontent = filelistcontent.Trim ();
			logger.Debug ("Writing filelist file at path :" + filemetadatafilepath + " with content :\n" + filelistcontent);
			System.IO.File.WriteAllText (filemetadatafilepath, filelistcontent);
			
			sharefilecontent = sharefilecontent.Trim ();
			logger.Debug ("Writing sharedfilelist file at path :" + sharedfilemetadatapath + " with content :\n" + sharefilecontent);
			System.IO.File.WriteAllText (sharedfilemetadatapath, sharefilecontent);

			//Update the meta data file
			logger.Debug ("Writing meta file at path :" + usermetadatafilepath + " with content :\n" + usermetadata);
			System.IO.File.WriteAllText (usermetadatafilepath, usermetadata);

			
			
			//Now we write the file content on disk
			foreach (KeyValuePair<string, UserFile> entry in  userfilesystem.filemap) {
				string parentdir = FileUtils.getDiskPathFromMemoryPath (entry.Key);
				parentdir = GetParentDirectoryPath (parentdir);
				string filepath = userpath + "files" + Path.DirectorySeparatorChar;
				string metadatapath = userpath + "metadata" + Path.DirectorySeparatorChar;
				Directory.CreateDirectory (FileUtils.getDiskPathFromMemoryPath (filepath + parentdir));
				Directory.CreateDirectory (FileUtils.getDiskPathFromMemoryPath (metadatapath + parentdir));

				string completefilepath = filepath + FileUtils.getDiskPathFromMemoryPath (entry.Key);
				string completemetadatafilepath = metadatapath + FileUtils.getDiskPathFromMemoryPath(entry.Key)
					+ ".dat";
					
				System.IO.File.WriteAllText( completemetadatafilepath, entry.Value.GenerateMetaDataStringFromFile());
				File.WriteAllBytes( completefilepath, entry.Value.ReadFileContent());
			}
		}

		
		
		//Merge the check point objects
		private CheckPointObject mergeCheckPointObjects (CheckPointObject newimage, CheckPointObject oldimage)
		{
			logger.Debug ("Merge check point objects");
			CheckPointObject retObject = new CheckPointObject (); //ret object
			
			foreach (UserFileSystem newfs in newimage.userfilesystemlist) {
				bool found = false;
				foreach (UserFileSystem oldfs in oldimage.userfilesystemlist) {
					if (oldfs.metadata.clientId.Equals (newfs.metadata.clientId)) { //match based on user id
						found = true;
						retObject.userfilesystemlist.Add (mergeUserFileSystems (newfs, oldfs));				
					}
				}
				if (! found) {
					retObject.userfilesystemlist.Add (newfs);
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
			
			if (newfs.metadata.versionNumber > oldfs.metadata.versionNumber) {
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
					
					if (newfile.filemetadata.versionNumber > oldfile.filemetadata.versionNumber) { //lets roll
						if (newfile.filemetadata.markedForDeletion == true) { //remove this file now
							logger.Debug ("File marked for deletion, removing : " + filename);
							oldfs.removeFromMap (filename); //this will decrement the size
						} else {
							//long sizediff = newfile.filemetadata.filesize - oldfile.filemetadata.filesize;
							oldfs.filemap [filename] = newfile;
							//oldfs.incrementTotalFileSystemSize (sizediff);
						}
					} else {
						logger.Debug ("Version number of new content is not greater, skipping : " + newfile.filemetadata.versionNumber + " " 
						              + oldfile.filemetadata.versionNumber);
					}
				}
				
			}
			
			return oldfs;
		}
		
		
		private string GetParentDirectoryPath( string fullpath){
			return Path.GetDirectoryName(fullpath) + Path.DirectorySeparatorChar;
		}

		private string GenerateCheckpointPath (DateTime checkpointtime)
		{
			DateTime time = checkpointtime;
			string path = time.Year + "-" + time.Month + "-" + time.Day + Path.DirectorySeparatorChar + 
				time.Hour + "-" + time.Minute + Path.DirectorySeparatorChar;
			return CheckpointLogic.pathprefix + path;
		}
	}
}

