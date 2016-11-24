using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
#if !UNITY_METRO && !UNITY_WP8
using System.Security.Cryptography;
#else
using UnityEngine.Windows;
#endif
using System.Net.NetworkInformation;

public class DebugUtils// : MonoBehaviour 
{
    public static void LogNetwork(string _log)
    {
        if (Constants.LOG_NETWORK_ENABLE)
        {
            Log(_log);
        }
    }

    public static void Log(string _log)
    {
        if (Constants.LOG_ENABLE)
        {
            Debug.Log(_log);
        }
    }

    public static void LogTouch(string _log)
    {
        if (Constants.LOG_TOUCH_ENABLE)
        {
            Debug.Log("TOUCH : " + _log);
        }
    }

    public static void Warning(string _log)
    {
        if (Constants.LOG_ENABLE)
        {
            Debug.LogWarning(_log);
        }
    }

    public static void Error(string _log)
    {
        if (Constants.LOG_ENABLE)
        {
            Debug.LogError(_log);
        }
    }

    public static void Assert(bool condition)
    {
#if DEBUG
        if (!condition) throw new UnityException();
#endif
    }
    public static void Assert(bool condition, string msg)
    {
#if DEBUG
        if (!condition) throw new UnityException(msg);
#endif
    }    
}

public class Utils
{

    public static string GetUniqueIdentifier()
    {   
        //return SystemInfo.deviceUniqueIdentifier + "1231231";//+ "aaa";  
        return SystemInfo.deviceUniqueIdentifier;//+ "aaa";  
        // very very important
        // don't change this 1231231 !!!!!
    }
    public static string GetUniqueIdentifier2()
    {
#if UNITY_EDITOR
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        return nics[0].GetPhysicalAddress().ToString();
        //#elif UNITY_IPHONE
        //        return iPhone.advertisingIdentifier;
#else
        return GetUniqueIdentifier();
#endif
    }
    public static int GetPlatformType()
    {
        int type = 0;
#if UNITY_IPHONE 
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			UnityEngine.iOS.DeviceGeneration gen = UnityEngine.iOS.Device.generation;
			type = (int)gen;
		}
#endif
        return type;
    }

    public static string GetDeviceNameByDeviceType(int dtype)
    {
#if UNITY_IPHONE
		switch (dtype)
		{
		case (int)UnityEngine.iOS.DeviceGeneration.iPhone:
			return "iphone";
		case (int)UnityEngine.iOS.DeviceGeneration.iPhone3G:
			return "iphone3g";
		case (int)UnityEngine.iOS.DeviceGeneration.iPhone3GS:
			return "iphone3gs";
		case (int)UnityEngine.iOS.DeviceGeneration.iPhone4:
			return "iphone4";
		case (int)UnityEngine.iOS.DeviceGeneration.iPhone4S:
			return "iphone4s";
		case (int)UnityEngine.iOS.DeviceGeneration.iPhone5:
			return "iphone5";
		case (int)UnityEngine.iOS.DeviceGeneration.iPhoneUnknown:
			return "iphoneunknown";
		case (int)UnityEngine.iOS.DeviceGeneration.iPad1Gen:
			return "ipad1";
		case (int)UnityEngine.iOS.DeviceGeneration.iPad2Gen:
			return "ipad2";
		case (int)UnityEngine.iOS.DeviceGeneration.iPad3Gen:
			return "ipad3";
		case (int)UnityEngine.iOS.DeviceGeneration.iPad4Gen:
			return "ipad4";
		case (int)UnityEngine.iOS.DeviceGeneration.iPadMini1Gen:
			return "ipadmini1";
		case (int)UnityEngine.iOS.DeviceGeneration.iPadUnknown:
			return "ipadunknown";
		case (int)UnityEngine.iOS.DeviceGeneration.iPodTouch1Gen:
			return "ipodtouch1";
		case (int)UnityEngine.iOS.DeviceGeneration.iPodTouch2Gen:
			return "ipodtouch2";
		case (int)UnityEngine.iOS.DeviceGeneration.iPodTouch3Gen:
			return "ipodtouch3";
		case (int)UnityEngine.iOS.DeviceGeneration.iPodTouch4Gen:
			return "ipodtouch4";
		case (int)UnityEngine.iOS.DeviceGeneration.iPodTouch5Gen:
			return "ipodtouch5";
		case (int)UnityEngine.iOS.DeviceGeneration.iPodTouchUnknown:
			return "ipodtouchunknown";
		default :
			return "iosUnknown";
		}
#elif UNITY_ANDROID
		return "android";
#else
        return "unknown";
#endif
    }

    public static string GetString(byte[] _data)
    {
        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < _data.Length; i++)
        {
            sBuilder.Append(_data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }


    public static string GetMd5Hash(byte[] _data)
    {
#if !UNITY_METRO && !UNITY_WP8
        // Create a new instance of the MD5CryptoServiceProvider object.
        MD5 md5Hasher = MD5.Create();

        byte[] data = md5Hasher.ComputeHash(_data);
#else		
		byte[] data = Crypto.ComputeMD5Hash(_data);
#endif

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    public static string GetMd5Hash(string input)
    {
#if !UNITY_METRO && !UNITY_WP8
        // Create a new instance of the MD5CryptoServiceProvider object.
        MD5 md5Hasher = MD5.Create();

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
#else		
		byte[] data = Crypto.ComputeMD5Hash(Encoding.UTF8.GetBytes(input));
#endif

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    private static string sSavePath = null;

    public static string SAVE_PATH
    {
        get
        {
            if (sSavePath == null)
                sSavePath = GetPath();
            return sSavePath;
        }
    }

    private static string GetPath()
    {
        string pathTemp = string.Empty;
#if UNITY_EDITOR || !UNITY_METRO
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //string path = Application.dataPath.Substring(0,Application.dataPath.Length - 5);
            //path = path.Substring(0,path.LastIndexOf('/'));

            //pathTemp = path + "/Documents/";
            pathTemp = Application.persistentDataPath;
            if (!pathTemp.EndsWith("/"))
                pathTemp += "/";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            pathTemp = path + "/Saves";
            if ((pathTemp.Length > 0) && (!System.IO.Directory.Exists(pathTemp)))
            {
                System.IO.Directory.CreateDirectory(pathTemp);
            }
        }
        else
        {
            pathTemp = Application.dataPath + "/Saves";
            if ((pathTemp.Length > 0) && (!System.IO.Directory.Exists(pathTemp)))
            {
                System.IO.Directory.CreateDirectory(pathTemp);
            }
        }
#endif
        return pathTemp;
    }

#if !UNITY_METRO && !UNITY_WP8
    private void PreLinkSomeLibraries()
    {
        // Application crashes when using System.Security.Cryptography.MD5 with managed code stripping
        object obj = new MD5CryptoServiceProvider();
        if (obj != null) { }
    }
#endif

    public static string GetFormatStringFromInt(int val)
    {
        /*if (val >= 0)
        {
            if (val < 1000)
            {
                return val.ToString();
            }
            else if (val < 1000000)
            {
                int val1 = val % 1000;
                int val2 = val / 1000;
                string bet1 = (val1 < 10)? " 00": ((val1 < 100)? " 0": " ");
                return val2 + bet1 + val1;
            }
            else if (val < 1000000000)
            {
                int val1 = val % 1000;
                int val2 = val / 1000 % 1000;
                int val3 = val / 1000000;
                string bet2 = (val2 < 10)? " 00": ((val2 < 100)? " 0": " ");
                string bet1 = (val1 < 10)? " 00": ((val1 < 100)? " 0": " ");
                return val3 + bet2 + val2 + bet1 + val1;
            }
        }
		
        return val.ToString("n0").Replace(",", " ");*/
        //return val.ToString();
        if (val <= 0)
            return "0";
        else
            return val.ToString("### ### ### ### ###").Trim();
    }

    public static bool IsKindleDevice()
    {
        if (SystemInfo.deviceModel == "Amazon KFOT" || SystemInfo.deviceModel == "Amazon KFTT" || SystemInfo.deviceModel == "Amazon KFJWA" || SystemInfo.deviceModel == "Amazon KFJWI"
            || SystemInfo.deviceModel == "Amazon KFSOWI" || SystemInfo.deviceModel == "Amazon KFTHWA" || SystemInfo.deviceModel == "Amazon KFHWI" || SystemInfo.deviceModel == "Amazon KFAPWA"
            || SystemInfo.deviceModel == "Amazon KFAPWI")
            return true;
        else
            return false;
    }
}
