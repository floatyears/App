using System.Collections;
using System;

namespace Utility{
	public sealed class TimeHelper {
		
		private TimeHelper(){
		}
		
		
		/// <summary>
		/// Millions the seconds.
		/// </summary>
		/// <returns>The seconds.</returns>
		public static long MillionSecondsNow(){
			DateTime dt1970 = new DateTime(1970,1,1);
			TimeSpan ts = DateTime.Now - dt1970;
			return (long)ts.TotalMilliseconds;
		}
		
		public static string FormattedTimeNow(){
			return System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
		}
		
		public static string GetLatestPlayTime(uint unixTime){
			string text = string.Empty;
			
			System.DateTime latestPlayTime = new System.DateTime(1970,1,1).AddSeconds(unixTime).ToLocalTime();
			int days = (System.DateTime.Now - latestPlayTime).Days;
			int hours = (System.DateTime.Now - latestPlayTime).Hours;
			int minutes = (System.DateTime.Now - latestPlayTime).Minutes;
			
			if(days > 0){
				text = string.Format(TextCenter.GetText("Last_LoginTimeD"), days);
				return text;
			}
			else if(hours > 0){
				text = string.Format(TextCenter.GetText("Last_LoginTimeH"), hours);
				return text;
			}
			else if(minutes >= 1){
				text = string.Format(TextCenter.GetText("Last_LoginTimeM"), minutes);      
				return text;
			}
			else{
				text = TextCenter.GetText("Less_OneMin");
				return text;
			}
		}
		
		
	}
}
