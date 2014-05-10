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
		private string pathprefix = "/home/piyush/codes/checkpoint/"; 

		/* Name of the meta data file name for the user present on disk*/
		private string metadatafilename = "metadata.txt";

		/*Name of file which stores the last check point path*/
		private string lastcheckpointfilename = "lastcheckpoint.txt";

		public CheckpointLogic (){} //default contructor

		public CheckPointObject RestoreFileSystem ()
		{
			logger.Debug ("Restoring file system");
			CheckPointObject checkObject = new CheckPointObject();
			string lastcheckpointfilepath = pathprefix + lastcheckpointfilename;

			List<string> lastcheckpointfilecontent = FileUtils.ReadLinesFromFile (lastcheckpointfilepath);
			logger.Debug("Last check point time stamp read :" + lastcheckpointfilecontent[0].Trim());
			DateTime lastcheckpointtime = DateUtils.getDatefromString(lastcheckpointfilecontent[0].Trim());

			checkObject.lastcheckpoint = lastcheckpointtime;
			logger.Debug("Poop : " + checkObject.lastcheckpoint);
			string latestcheckpointfolderpath = lastcheckpointfilecontent [1]; 
			logger.Debug("Last check point path read as :" + latestcheckpointfolderpath);
			latestcheckpointfolderpath = latestcheckpointfolderpath.Trim (); //remove any leading or trailing whitespaces
			foreach (string userfolder in Directory.EnumerateDirectories(latestcheckpointfolderpath)) {
				checkObject.userfilesystemlist.Add(RestoreUserFileSystem(userfolder));	
			}

			return checkObject;
		}

		private UserFileSystem RestoreUserFileSystem (string userdir)
		{
			logger.Debug("Restoring user file system for user dir path :" + userdir);
			UserFileSystem userfilesystem = new UserFileSystem ();
			string user = new DirectoryInfo (userdir).Name;
			string usermetadatafilepath = userdir + Path.DirectorySeparatorChar + metadatafilename;
			List<String> metadatafilecontent = FileUtils.ReadLinesFromFile (usermetadatafilepath);
			if (metadatafilecontent.Count < 3) {
				throw new DiskuserMetaDataCorrupt ("Disk meta data corrupt for user: " + user);
			}

			UserMetaData metadataobj = new UserMetaData (metadatafilecontent[0].Trim(), 
			                                         metadatafilecontent[1].Trim(), int.Parse(metadatafilecontent[2].Trim()));
			userfilesystem.metadata = metadataobj;

			metadatafilecontent.RemoveRange (0, 3); //remove the meta data rows and we are left with file names
			foreach (string filepath in metadatafilecontent) {
				UserFile file = RestoreUserFile(userdir, filepath.Trim());
				userfilesystem.filemap.Add(filepath.Trim(), file);
			}
			return userfilesystem;
		}


		private UserFile RestoreUserFile (string userdir, string relativefilepath)
		{	
			logger.Debug("Restoring user file for file path :" + relativefilepath);
			string completefilepath = userdir + Path.DirectorySeparatorChar 
				+ "files" + Path.DirectorySeparatorChar + relativefilepath;
			string metadatafilepath = userdir + Path.DirectorySeparatorChar 
				+ "metadata" + Path.DirectorySeparatorChar + relativefilepath + ".dat";

			List<string> userfilemetadata = FileUtils.ReadLinesFromFile(metadatafilepath);
			UserFile file = GetFileFromFileMetaData(relativefilepath, userfilemetadata);
			file.filecontent = File.ReadAllBytes(completefilepath);
			return file;
		}

		private UserFile GetFileFromFileMetaData (string filepath, List<string> metadata)
		{
			UserFile file = new UserFile (filepath, metadata [0].Trim ());
			file.filepath = filepath;
			file.filesize = int.Parse (metadata [1].Trim ());
			file.versionNumber = int.Parse (metadata [2].Trim ());

			if (metadata.Count > 3) { //this is because this file might be shared with no one
				string [] clients = metadata [3].Trim ().Split (',');
			
				foreach (string client in clients) {
					if (client.Trim ().Length > 0) {
						file.sharedwithclients.Add (client.Trim ());
					}
				}
			}
			return file;
		}

		/*Check point entry */
		public void DoCheckPoint (CheckPointObject filesystem)
		{
			logger.Debug("Check point method called for filesystem");
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

		
		public void DoCheckPointForUser (
			string user,
			string path,
			UserFileSystem userfilesystem){

			logger.Debug ("Check pointing for user :" + user);

			string userpath = path + user + Path.DirectorySeparatorChar; //Appending '/' for linux and '\' on windows
			Directory.CreateDirectory (userpath);
			string metadatafilepath = userpath + this.metadatafilename;

			string metadata = userfilesystem.metadata.clientId + 
							"\n" + userfilesystem.metadata.password
							+ "\n" + userfilesystem.metadata.versionNumber + "\n";

			List<String> files = userfilesystem.GetFileList ();
			foreach (string filename in files) {
				metadata += filename + "\n";
			}

			//Update the meta data file
			logger.Debug ("Writing meta file at path :" + metadatafilepath + " with content :\n" + metadata);
			System.IO.File.WriteAllText (metadatafilepath, metadata);

			//Now we write the file content on disk
			foreach (KeyValuePair<string, UserFile> entry in  userfilesystem.filemap) {
				string parentdir = GetParentDirectoryPath( entry.Key);
				string filepath = userpath + "files" + Path.DirectorySeparatorChar;
				string metadatapath = userpath + "metadata" + Path.DirectorySeparatorChar;
				Directory.CreateDirectory(filepath + parentdir);
				Directory.CreateDirectory(metadatapath + parentdir);

				string completefilepath = filepath + entry.Key;
				string completemetadatafilepath = metadatapath + entry.Key + ".dat";
				System.IO.File.WriteAllText( completemetadatafilepath, entry.Value.GenerateMetaDataStringFromFile());
				File.WriteAllBytes( completefilepath, entry.Value.ReadFileContent());
			}
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

