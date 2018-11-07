/*
 * Author:      熊哲
 * CreateTime:  4/5/2017 12:57:17 PM
 * Description:
 * 
*/
using UnityEngine;
using XLua;

namespace EZhex1991.XLuaExample
{
    [LuaCallCSharp]
    public static class OverloadWrap
    {
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