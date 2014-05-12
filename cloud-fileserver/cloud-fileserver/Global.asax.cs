using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using ServiceStack;
using System.Configuration;
using Isis;

namespace cloudfileserver
{
	public class Global : System.Web.HttpApplication
	{
	
		    public class AppHost : AppHostBase
		    {	
		        //Tell Service Stack the name of your application and where to find your web services
		        public AppHost() : base("File Server",typeof(CloudFileService).Assembly) { }

		        public override void Configure (Funq.Container container)
				{
					Plugins.Add (new RequestLogsFeature ());
					this.Config.DefaultContentType = "Json";	
					//container.RegisterAutoWired<InMemoryFileSystem>();

					InMemoryFileSystem fileSystem = new InMemoryFileSystem ();
					container.Register<InMemoryFileSystem> (fileSystem);

					Console.WriteLine ("Application_Start ---->. Begin");
					
					//Start the ISIS System
					IsisSystem.Start ();
				
					Console.WriteLine ("ISIS Started :)");
					
					FileServerComm.fileServerGroupName = "FileServer";
					FileServerComm fileSrvComm = FileServerComm.getInstance ();

					fileSrvComm.getFileHandler ().filesystem = fileSystem;

					System.IO.StreamReader file = new System.IO.StreamReader ("bootstrap.txt");
					string line = file.ReadLine ();
					Console.WriteLine (line);

					bool isBootStrap = false;
					if (line.Equals ("1")) {
						isBootStrap = true;
					}
					
					fileSrvComm.ApplicationStartup(isBootStrap,FileServerComm.fileServerGroupName);
				
					Console.WriteLine("Application_Start. End"); 
		        }
			 }

	    //Initialize your application singleton
	    protected void Application_Start (object sender, EventArgs e)
		{
			new AppHost ().Init ();
	    }

	}
}

