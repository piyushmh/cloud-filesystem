using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage
{
    public class ClientInfo
    {
        public string ClientId { get; set;}
        public string Password { get; set; }
        public List<string> filename { get; set; }

        public ClientInfo(string clientId, string password)
		{
            this.ClientId = clientId;
            this.Password = password;	
		    this.filename = new List<string>();
		}
    }     
}
