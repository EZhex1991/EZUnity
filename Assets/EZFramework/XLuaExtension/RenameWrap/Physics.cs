/*
 * Author:      熊哲
 * CreateTime:  4/5/2017 12:57:17 PM
 * Description:
 * 
*/
using UnityEngine;
using XLua;

// Lua的弱类型特性会造成重载调用的混淆，需要通过异名方法去调用（这里命名空间不同，类名和方法名可以保持不变）
namespace EZFramework
{
    [LuaCallCSharp]
    public static class Physics
    {
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