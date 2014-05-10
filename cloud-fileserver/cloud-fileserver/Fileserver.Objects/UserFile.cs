using System;
using System.Collections.Generic;

namespace cloudfileserver
{
	/*
	 * This class represents one file in the file system 
	 * Author - piyush
	 */
	[Serializable]
	public class UserFile	{

		//file content
		public byte[] filecontent { get; set;}

		//metadata
		public FileMetaData filemetadata { get; set; }
		
		private object privateLock = new object();

		private static readonly log4net.ILog logger = 
			log4net.LogManager.GetLogger(typeof(UserFile));


		public FileMetaData getFileMetaDataSynchronized(){
			lock( this.privateLock){
				return this.filemetadata.cloneMetaDataObject();
			}
		}
		
		
		public UserFile (string filepath, string owner)
		{	
			this.filemetadata = new FileMetaData (filepath, owner);
			this.filecontent = new byte[0];
		}

		public byte[] ReadFileContentSynchronized ()
		{
			lock (privateLock) {
				return ReadFileContent();
			}
		}

		public byte[] ReadFileContent ()
		{
			byte[] b = new byte[this.filecontent.Length];
			Buffer.BlockCopy (this.filecontent, 0, b, 0, this.filecontent.Length);
			return b;
		}

		public bool SetFileContentSynchronized (UserFile newfile)
		{
			return SetFileContentSynchronized( newfile.filecontent, newfile.filemetadata.versionNumber);
		}

		/* Change the file contents only if new version number is greater than the current 
		 * version number. Also update the size and the version number accordingly
		 */
		public bool SetFileContentSynchronized( byte[] newcontent, long newversionNumber){

			lock (privateLock) {
				return SetFileContent( newcontent, newversionNumber);
			}
		}

		public bool SetFileContent( byte[] newcontent, long newversionNumber){
			logger.Debug("Set file content called on file with path :" + this.filemetadata.filepath);
			if( this.filemetadata.versionNumber < newversionNumber){ //only if the file is of a newer version
				this.filecontent = new byte[newcontent.Length];
				System.Array.Copy(newcontent, this.filecontent, newcontent.Length);
				this.filemetadata.versionNumber = newversionNumber;
				this.filemetadata.filesize = newcontent.Length;
				return true;
			}else{
				logger.Debug("File over write attempt with smaller version number, ignoring");
				return false;
			}	
		}

		//This will be used in checkpointing
		public string GenerateMetaDataStringFromFile ()
		{
			string r = this.filemetadata.owner + "\n" + this.filemetadata.filesize.ToString() + "\n" + this.filemetadata.versionNumber.ToString() + "\n";
			string joined = string.Join(",", this.filemetadata.sharedwithclients.ToArray());
			return  r + joined;
		}

		public override string ToString ()
		{
			try {
				return string.Format ("[UserFile: filepath={0}, owner={1}, filecontent={2}, filesize={3}, " +
					"sharedwithclients={4}, versionNumber={5}]", filemetadata.filepath, filemetadata.owner, Utils.getStringFromByteArray (filecontent), 
			                      filemetadata.filesize, filemetadata.sharedwithclients, filemetadata.versionNumber);
			} catch (Exception e) {
				logger.Debug(e);
				throw e;
			}
		}

	}
}

