/*
 * Author:      熊哲
 * CreateTime:  4/1/2017 11:33:11 AM
 * Description:
 * 
*/
using System;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XLua;
using Object = UnityEngine.Object;

// 为了降低Lua和其他Manager的耦合性，用到Lua的成员方法需要通过扩展去添加
namespace EZFramework
{
    [LuaCallCSharp]
    public static class LuaExtensions
    {
        public static int GetInt(this EZDatabase instance, string dataName, string key, object value)
        {
            return Convert.ToInt32(instance.Get(dataName, key, value));
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

        public static WWWTask Download(this EZUpdate instance, string resourceName, Action<LuaTable, WWWTask, bool> action, LuaTable self)
        {
            return instance.Download(resourceName, delegate (WWWTask task, bool succeed)
            {
                if (action != null) action(self, task, succeed);
            });
        }

        public static WWWTask NewTask(this EZNetwork instance, string url, string data, Action<LuaTable, WWWTask, bool> action, LuaTable self)
        {
            return instance.NewTask(url, Encoding.UTF8.GetBytes(data), delegate (WWWTask task, bool succeed)
            {
                if (action != null) action(self, task, succeed);
            });
        }

        public static void LoadAssetAsync(this EZResource instance, string bundleName, string assetName, Action<LuaTable, Object> action, LuaTable self)
        {
            instance.LoadAssetAsync(bundleName, assetName, delegate (Object obj)
            {
                if (action != null) action(self, obj);
            });
        }
        public static void LoadAssetAsync(this EZResource instance, string bundleName, string assetName, Type type, Action<LuaTable, Object> action, LuaTable self)
        {
            instance.LoadAssetAsync(bundleName, assetName, type, delegate (Object obj)
            {
                if (action != null) action(self, obj);
            });
        }
        public static void LoadSceneAsync(this EZResource instance, string bundleName, string sceneName, LoadSceneMode mode, Action<LuaTable> action, LuaTable self)
        {
            instance.LoadSceneAsync(bundleName, sceneName, mode, delegate ()
            {
                if (action != null) action(self);
            });
        }

        public static void AddListener(this Button button, Action<LuaTable, Button> action, LuaTable self)
        {
            if (action == null) return;
            button.onClick.AddListener(delegate { action(self, button); });
        }
        public static void AddListener(this Slider slider, Action<LuaTable, Slider> action, LuaTable self)
        {
            if (action == null) return;
            slider.onValueChanged.AddListener(delegate { action(self, slider); });
        }
        public static void AddListener(this Toggle toggle, Action<LuaTable, Toggle> action, LuaTable self)
        {
            if (action == null) return;
            toggle.onValueChanged.AddListener(delegate { action(self, toggle); });
        }
    }
}