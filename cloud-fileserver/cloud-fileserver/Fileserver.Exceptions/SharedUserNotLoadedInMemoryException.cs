using System;
namespace cloudfileserver
{
	[Serializable]
	public class SharedUserNotLoadedInMemoryException : Exception
	{
		public SharedUserNotLoadedInMemoryException() : base() { }
		public SharedUserNotLoadedInMemoryException (string message) : base(message) {}
		public SharedUserNotLoadedInMemoryException (string message, System.Exception inner) : base(message, inner) { }
	}
}

