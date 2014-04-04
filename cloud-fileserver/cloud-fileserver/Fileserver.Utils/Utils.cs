using System;
using System.Text;

namespace cloudfileserver
{
	public class Utils
	{
		public static byte[] getByteArrayFromString (string str)
		{
			byte[] y = Encoding.UTF8.GetBytes(str);
			return y;
		}

		public static string getStringFromByteArray( byte[] b){
            string s = Encoding.UTF8.GetString(b);
			return s;
        }
	}
}

