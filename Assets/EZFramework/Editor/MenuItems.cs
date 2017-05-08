/*
 * Author:      熊哲
 * CreateTime:  12/26/2016 5:25:19 PM
 * Description:
 * 
*/
using UnityEditor;

namespace EZFramework
{
    public class MenuItems
    {
        private const string ROOT_NAME = "EZFramework";

        [MenuItem(ROOT_NAME + "/EZSettings", priority = 0)]
        public static void EZSettings()
        {
            EditorWindow.GetWindow<EZSettingsEditorWindow>("EZSettings").Show();
        }

        [MenuItem(ROOT_NAME + "/Copy Lua To Text", priority = 20)]
        public static void LuaToTxt()
        {
            EZPathUtility.LuaToTxt();
        }
        [MenuItem(ROOT_NAME + "/Clear Lua Text", priority = 21)]
        public static void ClearLuaTxt()
        {
            EZPathUtility.ClearLuaTxt();
        }

        [MenuItem(ROOT_NAME + "/Clear Persistent Path", priority = 50)]
        public static void ClearPersistent()
        {
            EZPathUtility.ClearPersistent();
        }
        [MenuItem(ROOT_NAME + "/Clear StreamingAssets Path", priority = 51)]
        public static void ClearStreamingAssets()
        {
            EZPathUtility.ClearStreamingAssets();
        }
    }
}