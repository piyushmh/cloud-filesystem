using System;
using System.Text;
using System.Collections.Generic;
using ServiceStack;
namespace TestClient{
	
	[Serializable]
	public class UpdateFile{
		public UserFile file {get; set;}
	}
	
	public class AddUser{}	
	
	public class GetUserFileSystemInfo{	}
	
	public class ShareFileWithUser{}
	
	public class DeleteFile{}
	
	[Serializable]
	public class DoCheckPoint{
		public CheckPointObject checkPointObject { get; set;}
	}
	
	[Serializable]
	public class FlushFile{
		public UserFile file { get; set;}
	}
	
	//[Route("/doPersistentCheckPoint/{clientId}/{password}", "POST")]
	public class DoPersistentCheckPoint{}
	
	public class Client
	{
		public static void Main ()
		{

			JsonServiceClient client = new JsonServiceClient ("http://128.84.216.57:8080");
			//UpdateFile arg = new UpdateFile();
			UserFile file = new UserFile ("x.txt", "piyush");
			file.filecontent = new byte[0];//getByteArrayFromString ("Bitches this is the new file content");
			file.filemetadata.versionNumber = 3;
			file.filemetadata.filesize = file.filecontent.Length;
			client.Post<Object> ("/adduser/piyush/password", new AddUser());
			//client.Post<Object>("/updatefile/piyush/password", new UpdateFile{file= file});
			//UserFile f = client.Get<UserFile> ("/file/piyush/password/z_x.txt/piyush");
			//Console.WriteLine (f);
			client.Post<object> ("/doPersistentCheckPoint/piyush/password", new DoPersistentCheckPoint ());
			
			//arg.file = file;
			//UserFile file = client.Get<UserFile> ("/fetchfile/piyush/x_z.txt");
			//UserFile file1 = client.Get<UserFile> ("/fetchfile/piyush/z_x.txt");
			//client.Get<UserFile> ("/fetchfile/piyush/z_z.txt");
			//Console.WriteLine (file);
			//Console.WriteLine (file1);
			/*CheckPointObject obj = new CheckPointObject ();
			obj.lastcheckpoint = DateTime.Now;
			Console.WriteLine (obj.lastcheckpoint);
			UserFileSystem fs = new UserFileSystem ();
			UserFile file = new UserFile ("x_z.txt", "piyush");
			file.filecontent = Encoding.UTF8.GetBytes ("this");
			file.filemetadata.filesize = file.filecontent.Length;
			file.filemetadata.versionNumber = 1;
			if (fs.filemap == null) {
				Console.WriteLine ("File System is null");
			}
			
			Console.WriteLine (file);
			Console.WriteLine (fs.filemap.Count);
			Console.WriteLine (file.filemetadata);
			Console.WriteLine (file.filemetadata.filepath);
			fs.filemap [file.filemetadata.filepath] = file;
			//fs.filemap.Add (file.filemetadata.filepath, file);
			fs.metadata = new UserMetaData ("piyush", "password", 2, 0);
			fs.metadata.totalFileSystemSizeBytes = 37 + file.filemetadata.filesize;
			fs.sharedFiles.Add (new SharedFile ("laxman", "x_y.txt"));		
			obj.userfilesystemlist.Add (fs);
			client.Post<Object> ("/doCheckPoint", new DoCheckPoint{ checkPointObject = obj});
			//Console.WriteLine (obj.lastcheckpoint);
			//Console.WriteLine (obj.ToJson());
			
			
			//UserFile file1 = new UserFile ("y_z.txt", "piyush");
			//file1.filecontent = Encoding.UTF8.GetBytes ("this");
			//UserFile file = new UserFile ("y_x.txt", "piyush");
			///file.filemetadata.sharedwithclients.Add ("garima");
			//file.filecontent = Encoding.UTF8.GetBytes ("this");
			//file.filemetadata.filesize = file.filecontent.Length;
			//file.filemetadata.versionNumber = 1;
			//Console.WriteLine ("Poop " + Encoding.UTF8.GetString (file.filecontent));
			//client.Post<Object> ("/adduser/piyush/password", new AddUser ());
			//client.Post<Object> ("/adduser/laxman/password", new AddUser ());
			//client.Post<Object> ("/updatefile/piyush/password", new UpdateFile{ file = file});
			//client.Post<Object> ("/updatefile/piyush/password", new UpdateFile{ file = file1});
			//UserFileSystemMetaData md = client.Get<UserFileSystemMetaData> ("/getUserFileSystemInfo/piyush/password");
			//Console.WriteLine (md.userMetaData.clientId + " " + md.userMetaData.password);
			
			//client.Post<Object> ("/shareFile/piyush/password/y_x.txt/laxman", new ShareFileWithUser ()); 
			//UserFile f = client.Get<UserFile> ("/file/laxman/password/y_x.txt/piyush");
			//Console.Write (f);
			//client.Post<Object> ("/shareFile/piyush/password/y_z.txt/laxman", new ShareFileWithUser ()); 
			//client.Post<Object> ("/unShareFile/piyush/password/y_x.txt/laxman", new ShareFileWithUser ()); 
			//UserFile f = client.Get<UserFile> ("/file/piyush/password/y_x.txt/piyush");
			//Console.Write (f);
			//client.Post<Object> ("/deletefile/piyush/password/y_x.txt", new DeleteFile());
			*/
			
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

