/*
 * Author:      熊哲
 * CreateTime:  5/23/2017 6:20:26 PM
 * Description:
 * 
*/
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using XLua;

namespace EZhex1991.XLuaExample
{
    [LuaCallCSharp]
    public class LuaManager : CustomLoader  // 不懂CustomLoader的看第一个例子
    {
        private static LuaManager instance;    // 自己写单例
        public static LuaManager Instance { get { return instance; } }
        void Awake()
        {
            instance = this;
        }

        public void Yield(object cor, Action callback)  // 这里的两个方法是用来在lua中写携程的
        {
            StartCoroutine(Cor(cor, callback));
        }
        public IEnumerator Cor(object cor, Action callback)
        {
            if (cor is IEnumerator) yield return StartCoroutine((IEnumerator)cor);
            else yield return cor;
            callback();
        }
    }
}