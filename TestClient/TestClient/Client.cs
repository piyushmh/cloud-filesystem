using System;
using System.Text;
using System.Collections.Generic;
using ServiceStack;
namespace TestClient{
	[Serializable]
	public class UpdateFile{
		public UserFile file {get; set;}
	}
	public class AddUser{

	}
	public class Client
	{
		public static void Main ()
		{

			JsonServiceClient client  = new JsonServiceClient("http://127.0.0.1:8080");
			//UpdateFile arg = new UpdateFile();
			//UserFile file = new UserFile ("x.txt", "piyush");
			//file.SetFileContent (getByteArrayFromString ("Filecontent"), 0);
			//arg.file = file;

			client.Post<Object>("/adduser/piyush/password", new AddUser());
		}

		public static byte[] getByteArrayFromString (string str)
		{
			byte[] y = Encoding.UTF8.GetBytes(str);
			return y;
		}

		public Client ()
		{
		}
	}
}

