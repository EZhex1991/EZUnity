/*
 * Author:      熊哲
 * CreateTime:  9/4/2017 6:41:45 PM
 * Description:
 * 
*/
using System;

namespace EZFramework.LuaInjector
{
    [Serializable]
    public class Injection
    {
        public string key;
        public UnityEngine.Object value;
        public string typeName;
    }
}