using System;
using System.Collections.Generic;

namespace cloudfileserver
{
	/*
	 * This class represents one file in the file system 
	 */
	public class UserFile
	{
		//This also acts as unique identifier for the file in the userfilesystem
		public string filepath { get; set;}

		public string owner { get; set;}

		public byte[] filecontent { get; set;}

		public List<string> sharedwithclients { get; set;}

		public long versionNumber {get;set;}

		public UserFile ()
		{	
			//Default constructor
		}

		public UserFile (string filepath, string owner)
		{	
			this.filepath = filepath;
			this.owner = owner;
		}

	}
}

