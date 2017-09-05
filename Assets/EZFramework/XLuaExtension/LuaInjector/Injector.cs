/*
 * Author:      熊哲
 * CreateTime:  9/4/2017 4:48:11 PM
 * Description:
 * 
*/
using UnityEngine;
using XLua;

namespace EZFramework.LuaInjector
{
    [LuaCallCSharp]
    public class Injector : MonoBehaviour
    {
        public Injection[] injections;

        public void Inject(LuaTable self)
        {
            for (int i = 0; i < injections.Length; i++)
            {
                Injection pair = injections[i];
                if (pair.key.Contains("."))
                {
                    self.SetInPath(pair.key, pair.value);
                }
                else
                {
                    self.Set(pair.key, pair.value);
                }
            }
        }
    }
}