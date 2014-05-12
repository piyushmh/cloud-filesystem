using System;
using System.IO;
using System.Collections.Generic;

namespace persistentbackend
{
	public class FileOperations
	{
		public FileOperations ()
		{
		}
		
		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger (typeof(FileOperations));
		
		
		public void FlushFile (UserFile file)
		{
			logger.Debug ("Flushing file name for user : " + file.filemetadata.owner + " " + file.filemetadata.filepath);
			
			try {
				string lastcheckpointfilepath = CheckpointLogic.pathprefix + CheckpointLogic.lastcheckpointfilename; 
				List<string> lastcheckpointfilecontent = FileUtils.ReadLinesFromFile (lastcheckpointfilepath);
				logger.Debug ("Last check point time stamp read :" + lastcheckpointfilecontent [0].Trim ());
				DateTime lastcheckpointtime = DateUtils.getDatefromString (lastcheckpointfilecontent [0].Trim ());

				logger.Debug ("Read last checkpoint time as :" + lastcheckpointtime);
				
				if (lastcheckpointfilecontent.Count < 2)
					throw new DiskuserMetaDataCorrupt ("Something wrong with the last check point file path, check!!");
			
				string latestcheckpointfolderpath = lastcheckpointfilecontent [1]; 
				logger.Debug ("Last check point path read as :" + latestcheckpointfolderpath);
				latestcheckpointfolderpath = latestcheckpointfolderpath.Trim (); 
				string userpath = latestcheckpointfolderpath + file.filemetadata.owner + Path.DirectorySeparatorChar;
				userpath = FileUtils.getDiskPathFromMemoryPath (userpath);
				logger.Debug ("User path constructed is :" + userpath);
				
				string filepath = userpath + "files" + Path.DirectorySeparatorChar;
				string metadatapath = userpath + "metadata" + Path.DirectorySeparatorChar;
				logger.Debug ("User path constructed is :" + filepath);
				
				string completefilepath = filepath + FileUtils.getDiskPathFromMemoryPath (file.filemetadata.filepath);
				string completemetadatafilepath = metadatapath + FileUtils.getDiskPathFromMemoryPath (file.filemetadata.filepath)
					+ ".dat";
				
				logger.Debug ("Final file path  : " + completefilepath);
				logger.Debug ("Final metadata file path : " + completemetadatafilepath);
				
				System.IO.File.WriteAllText (completemetadatafilepath, file.GenerateMetaDataStringFromFile ());
				File.WriteAllBytes (completefilepath, file.ReadFileContent ());
				
			} catch (Exception e) {
				//logger.Warn ("Exception occured while restoring the file system : " + e);
				throw e;
			}
			
		}
		
		public UserFile fetchFile (string username, string filename)
		{
			logger.Debug ("Fetching file for user and file name : " + username + " " + filename);
			
			try {
				string lastcheckpointfilepath = CheckpointLogic.pathprefix + CheckpointLogic.lastcheckpointfilename; 
				List<string> lastcheckpointfilecontent = FileUtils.ReadLinesFromFile (lastcheckpointfilepath);
				logger.Debug ("Last check point time stamp read :" + lastcheckpointfilecontent [0].Trim ());
				DateTime lastcheckpointtime = DateUtils.getDatefromString (lastcheckpointfilecontent [0].Trim ());

				logger.Debug ("Read last checkpoint time as :" + lastcheckpointtime);
				
				if (lastcheckpointfilecontent.Count < 2)
					throw new DiskuserMetaDataCorrupt ("Something wrong with the last check point file path, check!!");
			
				string latestcheckpointfolderpath = lastcheckpointfilecontent [1]; 
				logger.Debug ("Last check point path read as :" + latestcheckpointfolderpath);
				latestcheckpointfolderpath = latestcheckpointfolderpath.Trim (); 
				string userpath = latestcheckpointfolderpath + Path.DirectorySeparatorChar + username;
				
				logger.Debug ("User path constructed is :" + userpath);
				UserFile file = new CheckpointLogic ().RestoreUserFile (
					userpath, FileUtils.getDiskPathFromMemoryPath (filename), true);
				
				logger.Debug ("File read of disk is : " + file.ToString ());
				return file;
				
			} catch (Exception e) {
				//logger.Warn ("Exception occured while restoring the file system : " + e);
				throw e;
			}
		}
	}
}

