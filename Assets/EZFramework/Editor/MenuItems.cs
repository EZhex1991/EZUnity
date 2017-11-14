/*
 * Author:      熊哲
 * CreateTime:  12/26/2016 5:25:19 PM
 * Description:
 * 
*/
using EZFramework;
using UnityEditor;

namespace EZFrameworkEditor
{
    public class MenuItems
    {
        private const string ROOT_NAME = "EZFramework";

        [MenuItem(ROOT_NAME + "/Settings", priority = 0)]
        public static void EZSettings()
        {
            Selection.activeObject = EZFrameworkSettings.Instance;
        }

        [MenuItem(ROOT_NAME + "/Copy Lua To Text", priority = 20)]
        public static void LuaToTxt()
        {
            EZFrameworkUtility.LuaToTxt();
        }
        [MenuItem(ROOT_NAME + "/Clear Lua Text", priority = 21)]
        public static void ClearLuaTxt()
        {
            EZFrameworkUtility.ClearLuaTxt();
        }

        [MenuItem(ROOT_NAME + "/Clear Persistent Path", priority = 50)]
        public static void ClearPersistent()
        {
            EZFrameworkUtility.ClearPersistent();
        }
        [MenuItem(ROOT_NAME + "/Clear StreamingAssets Path", priority = 51)]
        public static void ClearStreamingAssets()
        {
            EZFrameworkUtility.ClearStreamingAssets();
        }
        [MenuItem(ROOT_NAME + "/Clear PlayerPrefs", priority = 52)]
        public static void ClearPlayerPrefs()
        {
            EZFrameworkUtility.ClearPlayerPrefs();
        }
    }
}