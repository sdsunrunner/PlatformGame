using System;
using UnityEngine;
using System.Collections;

public static class GameTime
{
    static int lastServerTime = 0;
    static float localTimeOnSetServerTime;
    static int serverDayStartTime = 0;
    static long deltaSecFromServerTime = 0;
    static long deltaMilSecFromServerTime = 0;
    public static bool sUseServerTime = false;

    public static long GetMilSecTime()
    {
        return Decimal.ToInt64(Decimal.Divide(DateTime.UtcNow.Ticks - 621355968000000000, 10000)) - deltaMilSecFromServerTime;
    }

    public static long GetLocalSecTime()
    {
        return Decimal.ToInt64(Decimal.Divide(DateTime.Now.Ticks - 621355968000000000, 10000000)) - deltaSecFromServerTime;
    }

    public static long GetSecTime()
    {
        return Decimal.ToInt64(Decimal.Divide(DateTime.UtcNow.Ticks - 621355968000000000, 10000000)) - deltaSecFromServerTime;
    }

    public static int GetHour()
    {
        return DateTime.Now.Hour;
    }

    public static int GetNowDayCode()
    {
        System.DateTime time = System.DateTime.Now;
        string str = string.Empty;

        time = time.ToLocalTime();
        str = time.ToString("yyyyMMdd");
        int timeValue = int.Parse(str);
        return timeValue;
    }


    public static long GetEscapedTime(long oldTime_MilSec)
    {
        long CurTime = GetMilSecTime();
        return CurTime - oldTime_MilSec;
    }



    public static int GetEscapedTimeSec(long oldTime_MilSec)
    {
        return (int)(GetEscapedTime(oldTime_MilSec) / 1000);
    }


    // Show "22m 22.351s"
    public static string MilSecToString(long _milSec)
    {
        int inSec = (int)(_milSec / 1000);

        int minute = inSec / 60;
        int second = inSec % 60;
        int msecond = (int)(_milSec % 1000);

        string result = " ";
        if (minute > 0)
            result = minute + "m";

        if (second != 0 || msecond != 0)
            result = result + " " + second + "." + msecond + "s";

        return result;
    }

    // Show "22d 22h" or "22m 22s" or "22h 22m"
    public static string SecondToString(int second)
    {
        // string dd = TextManager.instance.GetText(LText.DD);
        // string hh = TextManager.instance.GetText(LText.HH);
        // string mm = TextManager.instance.GetText(LText.MM);
        // string ss = TextManager.instance.GetText(LText.SS);
        string dd = "d";
        string hh = "h";
        string mm = "m";
        string ss = "s";

        string str = string.Empty;
        if (second == 0)
            return "0" + ss;

        int day = second / 86400;
        int hour = (second - day * 86400) / 3600;
        int minute = (second - day * 86400 - hour * 3600) / 60;
        second = second - day * 86400 - hour * 3600 - minute * 60;
        if (day > 0)
        {
            str = str + day + dd;
            if (hour > 0) str = str + " ";
        }
        if (hour > 0)
        {
            if (hour < 10)
                str = str + " " + hour + hh;
            else
                str = str + hour + hh;
            if (day == 0 && minute > 0) str = str + " ";
        }
        // max show two time segments 5d 20h
        if (day == 0)
        {
            if (minute > 0)
            {
                if (minute < 10)
                    str = str + " " + minute + mm;
                else
                    str = str + minute + mm;
                if (hour == 0 && second > 0) str = str + " ";
            }
            // max show two time segments 20h 43m
            if (hour == 0)
            {
                if (second > 0)
                {
                    if (second < 10)
                        str = str + " " + second + ss;
                    else
                        str = str + second + ss;
                }
            }
        }
        return str;
    }




    // Show "22d22h22m22s"
    public static string SecondToString3(int second)
    {
        string dd = "d";
        string hh = "h";
        string mm = "m";
        string ss = "s";
        string str = string.Empty;
        int day = second / (3600 * 24);
        int hour = (second - day * 3600 * 24) / 3600;
        int minute = (second - hour * 3600 - day * 3600 * 24) / 60;
        second = second - hour * 3600 - minute * 60 - day * 3600 * 24;

        if (day != 0)
        {
            if (day < 10)
                str = str + "0" + day + " d ";
            else
                str = str + day + " d ";
        }

        if (hour != 0)
        {
            if (hour < 10)
                str = str + "0" + hour + " h ";
            else
                str = str + hour + " h ";
        }
        if (minute < 10)
            str = str + "0" + minute + " m ";
        else
            str = str + minute + " m ";
        if (second < 10)
            str = str + "0" + second + " s ";
        else
            str = str + second + " s ";
        return str;
    }

    // Show "22:22:22"
    public static string SecondToString2(int second)
    {
        string str = string.Empty;
        int hour = second / 3600;
        int minute = (second - hour * 3600) / 60;
        second = second - hour * 3600 - minute * 60;
        if (hour != 0)
        {
            if (hour < 10)
                str = str + "0" + hour + ":";
            else
                str = str + hour + ":";
        }
        if (minute < 10)
            str = str + "0" + minute + ":";
        else
            str = str + minute + ":";
        if (second < 10)
            str = str + "0" + second;
        else
            str = str + second;
        return str;
    }



    public static string SecondToDate(long second, bool bLongDate)
    {
        DateTime dt = new DateTime(Decimal.ToInt64(Decimal.Multiply(second, 10000000) + 621355968000000000));
        if (bLongDate)
        {
#if UNITY_METRO
			return dt.GetDateTimeFormats('D')[0];
#else
            return dt.ToLongDateString();
#endif
        }
        else
        {
#if UNITY_METRO
			return dt.GetDateTimeFormats('d')[0];
#else
            return dt.ToShortDateString();
#endif
        }
    }


    public static string SecondToStringYMDLocal(int second)
    {
        System.DateTime time = new System.DateTime((long)second * 10000000 + 621355968000000000);
        string str = string.Empty;

        //str = time.ToString("yyyy/MM/dd HH:mm");
        //long curTime = DateTime.UtcNow.Ticks - 621355968000000000;
        //string str2 = curTime.ToString("yyyy/MM/dd HH:mm");
        //string str3 = GameTime.SecondToDate((long)second,true);

        time = time.ToLocalTime();
        str = time.ToString("yyyy/MM/dd HH:mm");
        return str;
    }

    public static int[] SecondToHMS(int second)
    {
        int h = second / 3600;

        int hmin = h * 3600;
        int m = (second - hmin) / 60;
        int s = second - hmin - m * 60;

        return new[] { h, m, s };
    }

    public static string SecondToHM(int second)
    {
        int h = second / 3600;

        int hmin = h * 3600;
        int m = (second - hmin) / 60;

        string t = "";
        if (h > 0)
        {
            //t = string.Format(TextManager.instance.GetText(LText.Time_remain_hour), h, m + 1);
        }
        else
        {
            //t = string.Format(TextManager.instance.GetText(LText.Time_remain_min), m + 1);
        }
        return t;
    }


    public static void SetServerTime(int _sec)
    {
        if (_sec <= lastServerTime) { return; }

        lastServerTime = _sec;
        localTimeOnSetServerTime = Time.timeSinceLevelLoad;
        deltaSecFromServerTime = Decimal.ToInt64(Decimal.Divide(DateTime.UtcNow.Ticks - 621355968000000000, 10000000)) - lastServerTime;
        deltaMilSecFromServerTime = SecondToMilSec((int)deltaSecFromServerTime);
        sUseServerTime = true;
    }


    public static void SetServerDayStartTime(int _sec)
    {
        serverDayStartTime = _sec;
    }

    public static int GetServerDayStartTime()
    {
        return serverDayStartTime;
    }

    public static long GetServerDayStartTimeMilSec()
    {
        return SecondToMilSec(serverDayStartTime);
    }

    public static long SecondToMilSec(int _sec)
    {
        return Decimal.ToInt64(Decimal.Multiply(_sec, 1000));
    }


    public static int GetServerTimeSec(float curLocTime = -1)
    {
        float curTime = curLocTime;
        if (curTime < 0)
        {
            curTime = Time.timeSinceLevelLoad;
        }
        return lastServerTime + (int)(curTime - localTimeOnSetServerTime);
    }


    public static int GetTimeSpan(int startTime, int curTime = -1, bool checkError = false)
    {
        int currentTime = curTime;
        if (currentTime < 0)
        {
            currentTime = GetServerTimeSec();
        }

        if (checkError)
        {
            if (currentTime < 1400000000)
            {
                TimeSpan span = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0);
                currentTime = (int)span.TotalSeconds;
            }

            int timeLen = currentTime.ToString().Length;

            string timeStr = startTime.ToString();
            if (timeStr.Length < timeLen)
            {
                return 0;
            }

            string timeSub = timeStr.Substring(0, timeLen);
            return currentTime - Convert.ToInt32(timeSub);
        }
        return currentTime - startTime;
    }

    public static float SpanToLastSetTime()
    {
        return Time.timeSinceLevelLoad - localTimeOnSetServerTime;
    }

    //microSecond!!!

    public static long NANOSECOND_GetTimeSinceStart()
    {
        return (long)(Time.realtimeSinceStartup * 1000000000);
    }

    public static long NANOSECOND_GetEscapeTime(long oldTime)
    {
        long CurTime = NANOSECOND_GetTimeSinceStart();
        return CurTime - oldTime;
    }

    public static int DaySeconds
    {
        get { return 86400; }
    }

    public static int NowHour()
    {
        return DateTime.Now.Hour;
    }
}