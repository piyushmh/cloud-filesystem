using System;

namespace cloudfileserver
{	
	[Serializable()]
	public class UserNotLoadedInMemoryException : Exception
	{
		public UserNotLoadedInMemoryException() : base() { }
		public UserNotLoadedInMemoryException (string message) : base(message) {}
		public UserNotLoadedInMemoryException (string message, System.Exception inner) : base(message, inner) { }
	}
}

