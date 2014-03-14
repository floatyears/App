using System.Collections;
using System;

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
//
//	public static string UnixTimeToLocalTime(uint unixTime){
//		return System.DateTime.Now.ToString()
//	System.DateTime dtDateTime = new System.DateTime(1970,1,1,0,0,0,0).AddSeconds(1394713370).ToLocalTime();
//	Debug.LogError("Local time : " + (System.DateTime.Now - dtDateTime).Minutes.ToString());

//	}

//	public static string DeltaTimeLastLoginToNow(uint unixTime){
//		System.DateTime dtLastLoginTo1970 = new DateTime(1970,1,1).AddSeconds(unixTime).ToLocalTime;
//		System.DateTime dtNowToLas
//	}
}