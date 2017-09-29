/*
 * Author:      熊哲
 * CreateTime:  9/26/2017 1:02:58 PM
 * Description:
 * 
*/
using System.Collections;
using UnityEngine;

namespace EZFramework.XLuaExtension
{
    public static class LuaUtility
    {
        // 两位整数的平台ID，可以用来代替RuntimePlatform，如果有自定义宏也可以在这里加
        public static int PlatformID
        {
            get
            {
#if UNITY_EDITOR_WIN
                return 1;
#elif UNITY_EDITOR_OSX
                return 2;
#elif UNITY_EDITOR
                return 10;
#elif UNITY_STANDALONE_WIN
                return 11;
#elif UNITY_STANDALONE_OSX
                return 12;
#elif UNITY_STANDALONE_LINUX
                return 13;
#elif UNITY_STANDALONE
                return 20;
#elif UNITY_ANDROID
                return 30;
#elif UNITY_IOS
                return 40;
#endif
            }
        }

        // lua时间精确度不太可靠
        public static long CurrentTime
        {
            get
            {
                return System.DateTime.UtcNow.Ticks;
            }
        }
        public static long DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond = 0)
        {
            return new System.DateTime(year, month, day, hour, minute, second, millisecond).Ticks;
        }
        public static double TimeSpanInMilliseconds(long ticks1, long ticks2)
        {
            return new System.TimeSpan(ticks2 - ticks1).TotalMilliseconds;
        }
        public static double RelativeTime()
        {
            return RelativeTime(CurrentTime);
        }
        public static double RelativeTime(long ticks)
        {
            return TimeSpanInMilliseconds(DateTime(1970, 1, 1, 0, 0, 0), ticks);
        }

        // 小数位的保留
        public static double Round(double number, int digits = 2)
        {
            return System.Math.Round(number, digits);
        }

        // System.String只导出部分方法
        public static string FormatString(string format, params object[] args)
        {
            return System.String.Format(format, args);
        }

        // 变量是否为空的判断由C#来做比较保险
        public static bool IsNull(UnityEngine.Object o)
        {
            return o == null;
        }

        // 词典不能使用string和object索引，getter可使用TryGetValue，setter用该方法代替
        public static void SetItem(IDictionary dict, object key, object value)
        {
            dict[key] = value;
        }

        // float和int参数造成UnityEngine.Random.Range重载调用不明确
        public static int RandomInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }
        public static float RandomFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        // out和ref参数造成UnityEngine.Physics.Raycast重载调用不明确
        public static bool Raycast(Ray ray, out RaycastHit hitInfo)
        {
            return UnityEngine.Physics.Raycast(ray, out hitInfo);
        }
        public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
        {
            return UnityEngine.Physics.Raycast(ray, out hitInfo, maxDistance);
        }
        public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask)
        {
            return UnityEngine.Physics.Raycast(ray, out hitInfo, maxDistance, layerMask);
        }
    }
}