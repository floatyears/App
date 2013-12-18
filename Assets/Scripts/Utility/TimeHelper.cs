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
