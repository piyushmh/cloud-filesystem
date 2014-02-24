using System;
using System;
using Isis;
using System.Collections.Generic;

namespace LBox
{
	[AutoMarshalled]
	public class FileServerInfo
	{
		public Address fileServerAddress;
		public int rank;
		public string fileServerName;

		public FileServerInfo()
		{
		}
		public FileServerInfo(Address address, int r,string serverGroupName)
		{
			fileServerAddress = address;
			rank = r;
			fileServerName = serverGroupName;
		}
	}
}

