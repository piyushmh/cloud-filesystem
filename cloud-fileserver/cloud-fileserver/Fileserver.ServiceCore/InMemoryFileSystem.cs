using System;
using System.Collections.Generic;

//Author - piyush

namespace cloudfileserver
{
	public class InMemoryFileSystem
	{
		private Dictionary<string, UserFileSystem> clientToFileSystemMap;

		public DateTime lastcheckpoint { get; set; }

		public InMemoryFileSystem ()
		{

		}

		/*
		 * This methos is used to authenticate a user 
		 */
		public Boolean AuthenticateUser( string clientId, string password){
			return true;
		}

		public UserFile FetchFile(
			string clientId, 
			string filename, 
			string fileowner){
	
			UserFile f = new UserFile();
			f.filepath = "/piyush/x.txt";
			f.owner = "piyush";
			f.versionNumber = 1;
			List<string> shared = new List<string>();
			shared.Add ("laxman");
			f.sharedwithclients = shared;

			return f;
		}
	}
}
