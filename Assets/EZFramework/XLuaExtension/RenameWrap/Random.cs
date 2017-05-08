/*
 * Author:      熊哲
 * CreateTime:  4/5/2017 1:36:10 PM
 * Description:
 * 
*/
using XLua;

namespace EZFramework
{
    [LuaCallCSharp]
    public static class Random
    {
        public static int Range(int min, int max)
        {
            return RangeInt(min, max);
        }
        public static int RangeInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }
        public static float RangeFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}