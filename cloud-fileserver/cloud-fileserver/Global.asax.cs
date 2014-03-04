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
		        public AppHost() : base("Hello Web Services",typeof(HelloService).Assembly) { }

		        public override void Configure(Funq.Container container)
		        {
					//use this for adding dependencies. 
					// Isis initialization code might go here

		            //register any dependencies your services use, e.g:
		            //container.Register<ICacheClient>(new MemoryCacheClient());
		        }
			 }

	    //Initialize your application singleton
	    protected void Application_Start(object sender, EventArgs e)
	    {
	        new AppHost().Init();
	    }

	}

	[Route("/hello")]
	[Route("/hello/{Name}")]
	public class Hello
	{
	    public string Name { get; set; }
	}

	public class HelloResponse
	{
		public string Result { get; set; }
	}

	public class HelloService : Service
	{
	    public object Get(Hello request)
	    {
	        return new HelloResponse { Result = "Hello, " + request.Name };
	    }
	} 
}

