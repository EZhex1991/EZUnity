/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-02-01 17:26:47
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.IO;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static partial class EZUtility
    {
#if UNITY_ANDROID
        private static AndroidJavaClass m_UnityPlayerClass;
        public static AndroidJavaClass UnityPlayerClass
        {
            get
            {
                if (m_UnityPlayerClass == null)
                {
                    m_UnityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                }
                return m_UnityPlayerClass;
            }
        }

        private static AndroidJavaClass m_ContextClass;
        public static AndroidJavaClass ContextClass
        {
            get
            {
                if (m_ContextClass == null)
                {
                    m_ContextClass = new AndroidJavaClass("android.content.Context");
                }
                return m_ContextClass;
            }
        }

        private static AndroidJavaObject m_CurrentActivity;
        public static AndroidJavaObject CurrentActivity
        {
            get
            {
                if (m_CurrentActivity == null)
                {
                    m_CurrentActivity = UnityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                }
                return m_CurrentActivity;
            }
        }

        private static AndroidJavaObject m_ContentResolver;
        public static AndroidJavaObject ContentResolver
        {
            get
            {
                if (m_ContentResolver == null)
                {
                    m_ContentResolver = CurrentActivity.Call<AndroidJavaObject>("getContentResolver");
                }
                return m_ContentResolver;
            }
        }

        public static string GetDeviceId() // IMEI: International Mobile Equipment Identity
        {
#if UNITY_EDITOR
            return "123456789012345";
#else
            string KEY = ContextClass.GetStatic<string>("TELEPHONY_SERVICE");
            AndroidJavaObject telephonyService = CurrentActivity.Call<AndroidJavaObject>("getSystemService", KEY);
            try
            {
                return telephonyService.Call<string>("getDeviceId");
            }
            catch (Exception e)
            {
                Debug.LogError("GetDeviceId failed! " + e.Message);
                return GetAndoidId();
            }
#endif
        }
        public static string GetAndoidId()
        {
#if UNITY_EDITOR
            return "123456789abcdef0";
#else
            AndroidJavaClass settingsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
            string KEY = settingsSecure.GetStatic<string>("ANDROID_ID");
            string androidId = settingsSecure.CallStatic<string>("getString", ContentResolver, KEY);
            return string.IsNullOrEmpty(androidId) ? GetMacAddress() : androidId;
#endif
        }
        public static string GetMacAddress()
        {
#if UNITY_EDITOR
            return "00000000000000000000000000000000";
#else
            string mac = "00000000000000000000000000000000";
            try
            {
                StreamReader reader = new StreamReader("/sys/class/net/wlan0/address");
                mac = reader.ReadLine();
                reader.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("GetMacAddress failed! " + e.Message);
            }
            return mac.Replace(":", "");
#endif
        }
#endif
    }
}