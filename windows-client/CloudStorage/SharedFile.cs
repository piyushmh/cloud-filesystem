using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage
{
	[Serializable]
	public class SharedFile
	{
		public string owner { get; set; }

		public string filename { get; set; }

		public SharedFile cloneObject ()
		{
			SharedFile sf = new SharedFile ();
			sf.owner = this.owner;
			sf.filename = this.filename;
			return sf;
		}
		
		public SharedFile (string owner, string filename)
		{
			this.owner = owner;
			this.filename = filename;
		}
		
		public SharedFile ()
		{
		}
	}
}
