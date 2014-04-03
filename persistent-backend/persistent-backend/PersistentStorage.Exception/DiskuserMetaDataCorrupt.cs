using System;

namespace persistentbackend
{
	public class DiskuserMetaDataCorrupt : Exception
	{
		public DiskuserMetaDataCorrupt() : base() { }
        public DiskuserMetaDataCorrupt (string message) : base(message) {}
 		public DiskuserMetaDataCorrupt (string message, System.Exception inner) : base(message , inner) { }

	}
}

