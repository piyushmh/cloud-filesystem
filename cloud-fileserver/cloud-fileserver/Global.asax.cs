using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using ServiceStack;

namespace cloudfileserver
{
	public class Global : System.Web.HttpApplication
	{
	
		    public class AppHost : AppHostBase
		    {	
		        //Tell Service Stack the name of your application and where to find your web services
		        public AppHost() : base("Hello Web Services",typeof(CloudFileService).Assembly) { }

		        public override void Configure(Funq.Container container){
					Plugins.Add(new RequestLogsFeature());
					this.Config.DefaultContentType = "Json";					
					container.RegisterAutoWired<InMemoryFileSystem>();
		        }
			 }

	    //Initialize your application singleton
	    protected void Application_Start(object sender, EventArgs e)
	    {
	        new AppHost().Init();
	    }

	}
}

