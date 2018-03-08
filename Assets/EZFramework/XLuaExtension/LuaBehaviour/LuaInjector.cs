/*
 * Author:      熊哲
 * CreateTime:  9/4/2017 4:48:11 PM
 * Description:
 * 
*/
using UnityEngine;
using XLua;

namespace EZFramework.XLuaExtension
{
    public class LuaInjector : MonoBehaviour
    {
        public Injection[] injections;

        public void Inject(LuaTable self)
        {
            for (int i = 0; i < injections.Length; i++)
            {
                Injection injection = injections[i];
                string[] path = injection.key.Split('.');
                LuaTable toBeSet = self;
                for (int j = 0; j < path.Length - 1; j++)
                {
                    toBeSet = toBeSet.Get<LuaTable>(path[j]);
                }
                string key = path[path.Length - 1];
                if (key.Contains("#"))
                {
                    string[] listKey = key.Split('#');
                    int index = System.Convert.ToInt32(listKey[1]);
                    toBeSet = toBeSet.Get<LuaTable>(listKey[0]);
                    SetByType(toBeSet, index, injection);
                }
                else
                {
                    SetByType(toBeSet, key, injection);
                }
            }
        }
        private void SetByType<T>(LuaTable table, T key, Injection injection)
        {
            if (injection.typeName == typeof(int).FullName)
            {
                table.Set(key, injection.nonObjectValue.intValue);
            }
            else if (injection.typeName == typeof(float).FullName)
            {
                table.Set(key, injection.nonObjectValue.floatValue);
            }
            else if (injection.typeName == typeof(bool).FullName)
            {
                table.Set(key, injection.nonObjectValue.boolValue);
            }
            else if (injection.typeName == typeof(string).FullName)
            {
                table.Set(key, injection.nonObjectValue.stringValue);
            }
            else if (injection.typeName == typeof(Vector2).FullName)
            {
                table.Set(key, injection.nonObjectValue.v2Value);
            }
            else if (injection.typeName == typeof(Vector3).FullName)
            {
                table.Set(key, injection.nonObjectValue.v3Value);
            }
            else if (injection.typeName == typeof(Vector4).FullName)
            {
                table.Set(key, injection.nonObjectValue.v4Value);
            }
            else if (injection.typeName == typeof(AnimationCurve).FullName)
            {
                table.Set(key, injection.nonObjectValue.animationCurveValue);
            }
            else
            {
                table.Set(key, injection.value);
            }
        }
    }
}