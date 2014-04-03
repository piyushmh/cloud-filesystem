using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using ServiceStack;

namespace persistentbackend
{
	public class Global : System.Web.HttpApplication
	{
		    public class AppHost : AppHostBase
		    {	
			 	public AppHost() : base("Persistent Back End Storage Service",
			                        typeof(PersistentStorageService).Assembly) { }

		        
				public override void Configure(Funq.Container container){
					Plugins.Add(new RequestLogsFeature());
					this.Config.DefaultContentType = "Json";	
					//container.RegisterAutoWired<InMemoryFileSystem>();
					//container.Register<InMemoryFileSystem>(c => new InMemoryFileSystem());
		        }
			 }

	    //Initialize your application singleton
	    protected void Application_Start(object sender, EventArgs e)
	    {
	        new AppHost().Init();
	    }

	}
}

