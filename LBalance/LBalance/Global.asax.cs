
namespace LBalance
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Web;
	using System.Web.SessionState;
	using System.Collections.Generic;
	using System.Collections.Concurrent;
	using System.Threading;
	using Isis;
	using ServiceStack;
	using ServiceStack.Configuration;
	using ServiceStack.Text;

	public class FileServerGroupObject
	{
		static internal Int32 NoOfServers = 5;

		List<Address> fileServerList;
		Int32 lastUsedIndex;

		public FileServerGroupObject()
		{
			fileServerList = new List<Address> ();
			lastUsedIndex = 0;
		}

		public void addToFileServerList(Address fsaddress)
		{
			if (fileServerList.Exists (element => element == fsaddress)) {
				Isis.WriteLine ("The Address " + fsaddress.ToStringVerboseFormat () + "already exists ");
			} 
			else {
				fileServerList.Add (fsaddress);
			}
		}

		public void removeFromFileServerList(Address fsaddress)
		{
			fileServerList.Remove (fsaddress);
		}

		public List<Address> getAddressList()
		{
			List <Address> addList = new List<Address> ();
			if(fileServerList.Count != 0)
			{
				lastUsedIndex = (lastUsedIndex + 1) % (fileServerList.Count);
				int numberOfEntriesToGive = NoOfServers;
				int startIndex = lastUsedIndex;
				while (startIndex != fileServerList.Count && numberOfEntriesToGive != 0) 
				{
					addList.Add (fileServerList[startIndex]);
					startIndex = (startIndex + 1) % fileServerList.Count;
					numberOfEntriesToGive = numberOfEntriesToGive - 1;
					if (lastUsedIndex == startIndex) {
						break;
					}
				}
			}
			return addList;
		}
	}

	public class Global : System.Web.HttpApplication
	{
		[Route("/loadbalance")]
		[Route("/loadbalance/{loginName}")]
		public class loadbalance
		{
			public string loginName { get; set; }
		}
			
		public class urlendpoint
		{
			public List<string> endpoints { get; set; }
		}
			
		public class AppHost : AppHostBase
		{
			public AppHost() : base("LoadBalancing Service", typeof(loadBalanceService).Assembly) { }
			public override void Configure(Funq.Container container)
			{
				this.Config.DefaultContentType = "Json";
			}
		}

		public static ConcurrentDictionary<string,FileServerGroupObject> fileServerGroupList = new ConcurrentDictionary<string, FileServerGroupObject>();

		public static string getFileServerGroup(string userName)
		{
			string groupName = "FS1";
			string logicalGroupName = "lbalancer-" + groupName;
			return logicalGroupName;
		}

		public static List<string> getServerAddresses(string userName)
		{
			string groupName = getFileServerGroup (userName);
			List<Address> addrList = new List<Address> ();
			List<string> temp = new List<string> ();

			if (!groupName.IsEmpty ()) {
				if (null != fileServerGroupList [groupName]) {
					addrList = fileServerGroupList [groupName].getAddressList ();
				}
			}
			foreach(Address addr in addrList)
			{
				temp.Add ("http://"+addr.home.ToString()+":8080");
			}
			return temp;
		}

		public class loadBalanceService : Service
		{
			public urlendpoint Get(loadbalance request)
			{
				Isis.WriteLine ("The Login Name is " + request.loginName);
				return new urlendpoint {endpoints =  getServerAddresses(request.loginName)};
			}
		} 

		protected void Application_Start (Object sender, EventArgs e)
		{
			Semaphore sem = new Semaphore(0, 1);
			Thread lbFileServerGroupThread = new Thread (delegate() 
			{
				//Start the ISIS System
				IsisSystem.Start();

				string groupName = "FS1";
				string logicalGroupName = "lbalancer-" + groupName;

				fileServerGroupList.TryAdd (logicalGroupName, new FileServerGroupObject());

				Group lbfsgroup = new Group(logicalGroupName);

				lbfsgroup.ViewHandlers += (ViewHandler)delegate(View v) 
				{ 
					Console.WriteLine("myGroup got a new view event: " + v); 
					
					Address [] addList = v.GetLiveMembers();
					for(Int32 index = 0; index < addList.Length; index++)
					{
						Isis.WriteLine ("The Add List Address " + addList[index].ToStringVerboseFormat ());
						fileServerGroupList[logicalGroupName].addToFileServerList(addList[index]);
					}

					Address [] removeList = v.GetFailedMembers();
					for(Int32 index = 0; index < removeList.Length; index++)
					{
						fileServerGroupList[logicalGroupName].removeFromFileServerList(removeList[index]);
					}
				}; 

				lbfsgroup.Join();
				sem.Release();
				IsisSystem.WaitForever();
			});

			lbFileServerGroupThread.Start ();
			sem.WaitOne ();
			Isis.WriteLine ("Starting Web Service on the LoadBalancer");

			new AppHost().Init();
		}

		protected void Session_Start (Object sender, EventArgs e)
		{
		}

		protected void Application_BeginRequest (Object sender, EventArgs e)
		{
		}

		protected void Application_EndRequest (Object sender, EventArgs e)
		{
		}

		protected void Application_AuthenticateRequest (Object sender, EventArgs e)
		{
		}

		protected void Application_Error (Object sender, EventArgs e)
		{
		}

		protected void Session_End (Object sender, EventArgs e)
		{
		}

		protected void Application_End (Object sender, EventArgs e)
		{
		}
	}
}

