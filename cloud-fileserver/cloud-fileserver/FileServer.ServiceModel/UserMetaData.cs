using System;

//author - piyush

namespace cloudfileserver
{	[Serializable]
	public class UserMetaData
	{
		public string clientId { get; set;}

		public string password { get; set;}

		public long versionNumber { get; set;}
		
		public UserMetaData (string clientId, string password, long versionnumber)
		{
			this.clientId = clientId;
			this.password = password;
			this.versionNumber = versionnumber;
		}
	}
}

