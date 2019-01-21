using System;

public class TimeUtils {

    readonly static DateTime DateTime_1970_01_01_08_00_00 = new DateTime(1970, 1, 1, 8, 0, 0);

    /// <summary>
    /// 当前毫秒时间戳
    /// </summary>
    /// <returns></returns>
    public static double GetTotalMillisecondsSince1970()
    {
        DateTime nowtime = DateTime.Now.ToLocalTime();
        return nowtime.Subtract(DateTime_1970_01_01_08_00_00).TotalMilliseconds;
    }

    /// <summary>
    /// 当前秒时间戳
    /// </summary>
    /// <returns></returns>
    public static double GetTotalSecondsSince1970()
    {
        DateTime nowtime = DateTime.Now.ToLocalTime();
        return nowtime.Subtract(DateTime_1970_01_01_08_00_00).TotalSeconds;
    }


    /// <summary>
    /// 时间戳转DateTime
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime TimeStampToDate(double timeStamp) {

        return DateTime_1970_01_01_08_00_00.AddSeconds(timeStamp);
    }

    /// <summary>
    /// 日期转时间戳
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static double DateToTimeStamp(DateTime time)
    {
        return time.Subtract(DateTime_1970_01_01_08_00_00).TotalSeconds; ;
    }

    /// <summary>
    /// DateTime转字符串显示
    /// </summary>
    /// <param name="time"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string DateToString(DateTime time,string format) {
        return "";
    }

    /// <summary>
    /// 时间戳转字符串显示
    /// </summary>
    /// <param name="time"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string TimeStampToString(DateTime time, string format) {
        return "";
    }

}
