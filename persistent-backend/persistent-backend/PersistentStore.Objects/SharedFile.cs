using System;

namespace persistentbackend
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
