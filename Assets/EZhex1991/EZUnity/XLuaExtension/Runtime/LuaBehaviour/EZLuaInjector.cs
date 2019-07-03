/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-09-04 16:48:11
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity.XLuaExtension
{
    public class EZLuaInjector : EZPropertyList
    {
        public void Inject(LuaTable table)
        {
            if (isList)
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    Set(table, i + 1, elements[i]);
                }
            }
            else
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    string key = elements[i].key;
                    SetInPath(table, key, elements[i]);
                }
            }
        }
        private void Set<T>(LuaTable table, T key, EZProperty injection)
        {
            if (injection.objectValue.GetType() == typeof(EZLuaInjector))
            {
                LuaTable child;
                table.Get(key, out child);
                if (child == null) child = table.NewTable(key);
                (injection.objectValue as EZLuaInjector).Inject(child);
            }
            else if (injection.typeName == typeof(int).FullName) { table.Set(key, injection.intValue); }
            else if (injection.typeName == typeof(long).FullName) { table.Set(key, injection.longValue); }
            else if (injection.typeName == typeof(bool).FullName) { table.Set(key, injection.boolValue); }
            else if (injection.typeName == typeof(float).FullName) { table.Set(key, injection.floatValue); }
            else if (injection.typeName == typeof(double).FullName) { table.Set(key, injection.doubleValue); }
            else if (injection.typeName == typeof(string).FullName) { table.Set(key, injection.stringValue); }
            else if (injection.typeName == typeof(Color).FullName) { table.Set(key, injection.colorValue); }
            else if (injection.typeName == typeof(AnimationCurve).FullName) { table.Set(key, injection.animationCurveValue); }
            else if (injection.typeName == typeof(Vector2).FullName) { table.Set(key, injection.vector2Value); }
            else if (injection.typeName == typeof(Vector3).FullName) { table.Set(key, injection.vector3Value); }
            else if (injection.typeName == typeof(Vector2Int).FullName) { table.Set(key, injection.vector2IntValue); }
            else if (injection.typeName == typeof(Vector3Int).FullName) { table.Set(key, injection.vector3IntValue); }
            else { table.Set(key, injection.objectValue); }
        }
        private void SetInPath(LuaTable table, string key, EZProperty injection)
        {
            if (injection.objectValue.GetType() == typeof(EZLuaInjector))
            {
                LuaTable child = table.GetInPath<LuaTable>(key);
                if (child == null) child = table.NewTable(key);
                (injection.objectValue as EZLuaInjector).Inject(child);
            }
            else if (injection.typeName == typeof(int).FullName) { table.SetInPath(key, injection.intValue); }
            else if (injection.typeName == typeof(long).FullName) { table.SetInPath(key, injection.longValue); }
            else if (injection.typeName == typeof(bool).FullName) { table.SetInPath(key, injection.boolValue); }
            else if (injection.typeName == typeof(float).FullName) { table.SetInPath(key, injection.floatValue); }
            else if (injection.typeName == typeof(double).FullName) { table.SetInPath(key, injection.doubleValue); }
            else if (injection.typeName == typeof(string).FullName) { table.SetInPath(key, injection.stringValue); }
            else if (injection.typeName == typeof(Color).FullName) { table.SetInPath(key, injection.colorValue); }
            else if (injection.typeName == typeof(AnimationCurve).FullName) { table.SetInPath(key, injection.animationCurveValue); }
            else if (injection.typeName == typeof(Vector2).FullName) { table.SetInPath(key, injection.vector2Value); }
            else if (injection.typeName == typeof(Vector3).FullName) { table.SetInPath(key, injection.vector3Value); }
            else if (injection.typeName == typeof(Vector2Int).FullName) { table.SetInPath(key, injection.vector2IntValue); }
            else if (injection.typeName == typeof(Vector3Int).FullName) { table.SetInPath(key, injection.vector3IntValue); }
            else { table.SetInPath(key, injection.objectValue); }
        }
    }
}
#endif
