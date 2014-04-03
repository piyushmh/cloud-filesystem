using System;

namespace persistentbackend
{
	public class DateUtils
	{
		public static DateTime getDatefromString(string str){

			string[] split = str.Split('-');
			DateTime time = new DateTime(int.Parse(split[0].Trim()) , int.Parse(split[1].Trim()) , int.Parse(split[2].Trim()),
			                             int.Parse(split[3].Trim()) , int.Parse(split[4].Trim()) , 0);

			return time;
		}

		public static string getStringfromDateTime (DateTime time){
			return time.Year + "-" + time.Month + "-" + time.Day + "-" 
				+ time.Hour + "-" + time.Minute;
		}
	}

}

