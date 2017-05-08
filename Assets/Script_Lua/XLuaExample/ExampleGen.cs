/*
 * Author:      熊哲
 * CreateTime:  4/18/2017 5:40:45 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public static class ExampleGen
{
    static ExampleGen()
    {
        Debug.Log(typeof(System.Collections.Generic.Dictionary<string, Vector3>));
    }
    [XLua.LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(System.Collections.Generic.Dictionary<int, object>),
        typeof(System.Collections.Generic.Dictionary<string, string>),
        typeof(System.Collections.Generic.Dictionary<string, Vector3>),
    };
}