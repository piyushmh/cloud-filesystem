using System;

//author - piyush

namespace cloudfileserver
{
	public class UserMetaData
	{

		public string clientId { get; set;}

		public string password { get; set;}
		
		public UserMetaData (string clientId, string password)
		{
			this.clientId = clientId;
			this.password = password;
		}
	}
}

