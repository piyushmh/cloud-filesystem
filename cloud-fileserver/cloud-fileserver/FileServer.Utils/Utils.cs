using System;

namespace cloudfileserver
{
	public class Utils
	{
		public static byte[] getByteArrayFromString( string str){
			byte[] bytes = new byte[str.Length * sizeof(char)];
    		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
    		return bytes;
		}
	}
}

