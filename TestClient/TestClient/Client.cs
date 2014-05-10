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
	
	public class GetUserFileSystemInfo
	{
		
	}
	
	public class Client
	{
		public static void Main ()
		{

			/*JsonServiceClient client = new JsonServiceClient ("http://127.0.0.1:8080");
			//UpdateFile arg = new UpdateFile();
			//UserFile file = new UserFile ("x.txt", "piyush");
			//file.SetFileContent (getByteArrayFromString ("Filecontent"), 0);
			//arg.file = file;
			UserFile file = new UserFile ("x.txt", "piyush");
			file.filemetadata.sharedwithclients.Add ("garima");
			file.filecontent = Encoding.UTF8.GetBytes ("this is the file content");
			//client.Post<Object> ("/adduser/piyush/password", new AddUser ());
			client.Post<Object> ("/adduser/laxman/password", new AddUser ());
			client.Post<object> ("/updatefile/piyush/password", new UpdateFile{ file = file});
			UserFileSystemMetaData md = client.Get<UserFileSystemMetaData> ("/getUserFileSystemInfo/piyush/password");
			Console.WriteLine (md.userMetaData.clientId + " " + md.userMetaData.password);
			*/
			
			string s = "x_y_z.txt";
			Console.WriteLine(s.Replace("_", "/"));
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

