/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-04-01 11:33:11
 * Organization:    #ORGANIZATION#
 * Description:     为了降低Lua和框架中其他逻辑的耦合性，部分方法使用扩展方式添加
 */
#if XLUA
using EZhex1991.EZUnity.Framework;
using EZhex1991.EZUnity.XLuaExtension;
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity
{
    public static partial class EZExtensions
    {
        public static int GetInt(this EZDatabase instance, string dataName, string key, int value)
        {
            return instance.Get(dataName, key, value);
        }
        public static long GetLong(this EZDatabase instance, string dataName, string key, long value)
        {
            return instance.Get(dataName, key, value);
        }
        public static float GetSingle(this EZDatabase instance, string dataName, string key, float value)
        {
            return instance.Get(dataName, key, value);
        }
        public static double GetDouble(this EZDatabase instance, string dataName, string key, double value)
        {
            return instance.Get(dataName, key, value);
        }
        public static string GetString(this EZDatabase instance, string dataName, string key, string value)
        {
            return instance.Get(dataName, key, value);
        }
        public static bool GetBool(this EZDatabase instance, string dataName, string key, bool value)
        {
            return instance.Get(dataName, key, value);
        }

        public static Sprite LoadSprite(this EZResources instance, string bundleName, string assetName)
        {
            return instance.LoadAsset<Sprite>(bundleName, assetName);
        }
        public static AudioClip LoadAudioClip(this EZResources instance, string bundleName, string assetName)
        {
            return instance.LoadAsset<AudioClip>(bundleName, assetName);
        }
        public static GameObject LoadGameObject(this EZResources instance, string bundleName, string assetName)
        {
            return instance.LoadAsset<GameObject>(bundleName, assetName);
        }
        public static Texture LoadTextture(this EZResources instance, string bundleName, string assetName)
        {
            return instance.LoadAsset<Texture>(bundleName, assetName);
        }

        public static EZLuaBehaviour GetLuaBehaviour(this GameObject go, string moduleName)
        {
            EZLuaBehaviour[] behaviours = go.GetComponents<EZLuaBehaviour>();
            if (moduleName.Contains("."))
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i].moduleName == moduleName)
                    {
                        return behaviours[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    string shortName = behaviours[i].moduleName;
                    if (shortName.Contains(".")) shortName = shortName.Substring(shortName.LastIndexOf(".") + 1);
                    if (shortName == moduleName)
                    {
                        return behaviours[i];
                    }
                }
            }
            return null;
        }
        public static LuaTable GetLuaTable(this GameObject go, string moduleName)
        {
            EZLuaBehaviour behaviour = go.GetLuaBehaviour(moduleName);
            return behaviour ? behaviour.luaTable : null;
        }
    }
}
#endif
