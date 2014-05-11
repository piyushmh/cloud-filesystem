using System;

//author - piyush

namespace cloudfileserver
{	
	[Serializable]
	public class UserMetaData
	{
		public string clientId { get; set;}

		public string password { get; set;}

		public long versionNumber { get; set;}
		
		public long totalFileSystemSizeBytes {get; set;} //in bytes
		
		public UserMetaData (string clientId, string password, long versionnumber, long totalFileSystemSizeBytes)
		{
			this.clientId = clientId;
			this.password = password;
			this.versionNumber = versionnumber;
			this.totalFileSystemSizeBytes = totalFileSystemSizeBytes;
		}

		public UserMetaData cloneMetaData(){
			return new UserMetaData( this.clientId, this.password, this.versionNumber, this.totalFileSystemSizeBytes);
		}
		
		public override string ToString ()
		{
			return string.Format ("[UserMetaData: clientId={0}, password={1}, versionNumber={2}, totalsizeinbytes={3}]", 
			                      clientId, password, versionNumber, totalFileSystemSizeBytes);
		}
	}
}

