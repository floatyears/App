using System.Collections;

public sealed class TimeHelper {

    private TimeHelper(){
    }

    /// <summary>
    /// Millions the seconds.
    /// </summary>
    /// <returns>The seconds.</returns>
    public static long MillionSecondsNow(){
        return System.DateTime.Now.ToBinary();
    }

    public static string FormattedTimeNow(){
        return System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }
}