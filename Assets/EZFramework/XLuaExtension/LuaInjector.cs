/*
 * Author:      熊哲
 * CreateTime:  9/4/2017 4:48:11 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using Object = UnityEngine.Object;

namespace EZFramework.XLuaExtension
{
    public class LuaInjector : MonoBehaviour
    {
        [Serializable]
        public class Injection
        {
            public string key;
            public Object value;
            public string typeName;
        }

        public Injection[] injections;

        protected Dictionary<string, Object> dict = new Dictionary<string, Object>();
        void Awake()
        {
            for (int i = 0; i < injections.Length; i++)
            {
                Injection pair = injections[i];
                dict.Add(pair.key, pair.value);
            }
        }
        public Object Get(string key)
        {
            Object value;
            if (dict.TryGetValue(key, out value))
            {
                return value;
            }
            return null;
        }
        public Object Get(int index)
        {
            return index < injections.Length ? injections[index].value : null;
        }

        public void Inject(LuaTable self)
        {
            for (int i = 0; i < injections.Length; i++)
            {
                Injection pair = injections[i];
                if (pair.key.Contains("."))
                {
                    self.SetInPath(pair.key, pair.value);   // SetInPath必须保证Path不为nil
                }
                else if (pair.key.Contains("#"))
                {
                    string[] info = pair.key.Split('#');
                    string tableName = info[0];
                    int index = System.Convert.ToInt32(info[1]);
                    LuaTable table = self.Get<LuaTable>(tableName); // 与SetInPath规则保持一致，此处table不能为nil
                    table.Set(index, pair.value);
                }
                else
                {
                    self.Set(pair.key, pair.value);
                }
            }
        }
    }
}