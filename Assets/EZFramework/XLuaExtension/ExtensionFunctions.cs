/*
 * Author:      熊哲
 * CreateTime:  4/1/2017 11:33:11 AM
 * Description:
 * 为了降低Lua和框架中其他逻辑的耦合性，部分方法使用扩展方式添加
*/
using System;
using UnityEngine;

namespace EZFramework.XLuaExtension
{
    public static class ExtensionFunctions
    {
        public static int GetInt(this EZDatabase instance, string dataName, string key, object value)
        {
            return Convert.ToInt32(instance.Get(dataName, key, value));
        }
        public static long GetLong(this EZDatabase instance, string dataName, string key, object value)
        {
            return Convert.ToInt64(instance.Get(dataName, key, value));
        }
        public static float GetSingle(this EZDatabase instance, string dataName, string key, object value)
        {
            return Convert.ToSingle(instance.Get(dataName, key, value));
        }
        public static double GetDouble(this EZDatabase instance, string dataName, string key, object value)
        {
            return Convert.ToDouble(instance.Get(dataName, key, value));
        }
        public static string GetString(this EZDatabase instance, string dataName, string key, object value)
        {
            return instance.Get(dataName, key, value).ToString();
        }
        public static bool GetBool(this EZDatabase instance, string dataName, string key, object value)
        {
            return Convert.ToBoolean(instance.Get(dataName, key, value));
        }

        public static Sprite LoadSprite(this EZResource instance, string bundleName, string assetName)
        {
            return instance.LoadAsset<Sprite>(bundleName, assetName);
        }
        public static AudioClip LoadAudioClip(this EZResource instance, string bundleName, string assetName)
        {
            return instance.LoadAsset<AudioClip>(bundleName, assetName);
        }
        public static GameObject LoadGameObject(this EZResource instance, string bundleName, string assetName)
        {
            return instance.LoadAsset<GameObject>(bundleName, assetName);
        }
        public static Texture LoadTextture(this EZResource instance, string bundleName, string assetName)
        {
            return instance.LoadAsset<Texture>(bundleName, assetName);
        }
    }
}