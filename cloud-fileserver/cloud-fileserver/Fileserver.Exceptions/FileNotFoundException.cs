using System;

namespace cloudfileserver
{
	[Serializable]
	public class FileNotFoundException : Exception
	{
		public FileNotFoundException() : base() { }
		public FileNotFoundException (string message) : base(message) {}
		public FileNotFoundException (string message, System.Exception inner) : base(message, inner) { }
	}
}

