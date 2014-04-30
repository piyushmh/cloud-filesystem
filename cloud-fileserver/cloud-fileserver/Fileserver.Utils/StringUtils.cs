using System;
using System.Text;

namespace cloudfileserver
{
	public class Utils
	{
		public static byte[] getByteArrayFromString (string str)
		{
			if ( str == null)
				return null;

			byte[] y = Encoding.UTF8.GetBytes(str);
			return y;
		}

		public static string getStringFromByteArray( byte[] b){
            
			if ( b == null)
				return null;
			string s = Encoding.UTF8.GetString(b);
			return s;
        }
	}
}

