using System;

namespace persistentbackend
{
	public class StringUtils
	{
		public static byte[] getByteArrayFromString (string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy (str.ToCharArray (), 0, bytes, 0, bytes.Length);
			return bytes;
		}
		public static string getStringFromByteArray( byte[] b){
            char[] array = new char[b.Length];
            System.Buffer.BlockCopy(b,0,array,0,array.Length);
            return new string(array);
        } 
	}
}
