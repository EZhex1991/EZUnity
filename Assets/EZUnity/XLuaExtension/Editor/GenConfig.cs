/*
 * Author:      熊哲
 * CreateTime:  3/13/2017 11:23:11 AM
 * Description:
 * 
*/
#if XLUA
using CSObjectWrapEditor;
using UnityEngine;

namespace EZUnity.XLuaExtension
{
    public static class GenConfig
    {
        [GenPath]
        public static string GenPath = Application.dataPath + "/XLua/Gen/";

        [GenCodeMenu]
        public static void OnGenCode()
        {

        }
    }
}
#endif
