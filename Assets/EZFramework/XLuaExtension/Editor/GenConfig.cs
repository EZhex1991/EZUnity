/*
 * Author:      熊哲
 * CreateTime:  3/13/2017 11:23:11 AM
 * Description:
 * 
*/
using CSObjectWrapEditor;
using UnityEngine;

namespace EZFramework.XLuaConfig
{
    public static class GenConfig
    {
        [GenPath]
        public static string GenPath = Application.dataPath + "/EZFramework/3rdParty/XLua/Gen/";

        [GenCodeMenu]
        public static void OnGenCode()
        {
            
        }
    }
}