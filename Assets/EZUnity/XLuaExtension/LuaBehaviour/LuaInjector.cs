/*
 * Author:      熊哲
 * CreateTime:  9/4/2017 4:48:11 PM
 * Description:
 * 
*/
#if XLUA
using UnityEngine;
using XLua;

namespace EZUnity.XLuaExtension
{
    public class LuaInjector : EZDictionary
    {
        public void Inject(LuaTable self)
        {
            foreach (var pair in dictionary)
            {
                string[] path = pair.Key.Split('.');
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
                    SetByType(toBeSet, index, pair.Value);
                }
                else
                {
                    SetByType(toBeSet, key, pair.Value);
                }
            }
        }
        private void SetByType<T>(LuaTable table, T key, Element injection)
        {
            if (injection.typeName == typeof(int).FullName)
            {
                table.Set(key, injection.intValue);
            }
            else if (injection.typeName == typeof(float).FullName)
            {
                table.Set(key, injection.floatValue);
            }
            else if (injection.typeName == typeof(bool).FullName)
            {
                table.Set(key, injection.boolValue);
            }
            else if (injection.typeName == typeof(string).FullName)
            {
                table.Set(key, injection.stringValue);
            }
            else if (injection.typeName == typeof(Vector2).FullName)
            {
                table.Set(key, injection.vector2Value);
            }
            else if (injection.typeName == typeof(Vector3).FullName)
            {
                table.Set(key, injection.vector3Value);
            }
            else if (injection.typeName == typeof(Vector4).FullName)
            {
                table.Set(key, injection.vector4Value);
            }
            else if (injection.typeName == typeof(AnimationCurve).FullName)
            {
                table.Set(key, injection.animationCurveValue);
            }
            else
            {
                table.Set(key, injection.objectValue);
            }
        }
    }
}
#endif
