using System;

namespace cloudfileserver
{
	[Serializable]
	public class SharedFile
	{
		public string owner { get; set; }

		public string filename { get; set; }

		public SharedFile ()
		{
		}
	}
}
