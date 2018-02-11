/*
 * Author:      熊哲
 * CreateTime:  12/26/2016 5:25:19 PM
 * Description:
 * 
*/
using EZFramework;
using UnityEditor;
using UnityEngine;

namespace EZFrameworkEditor
{
    public class MenuItems
    {
        private const string ROOT_NAME = "EZFramework";
        private const int PRIORITY = 11000;

        [MenuItem(ROOT_NAME + "/Settings", false, PRIORITY + 1)]
        public static void EZSettings()
        {
            string assetPath = EZFrameworkSettings.AssetDirPath + EZFrameworkSettings.AssetName + ".asset";
            EZFrameworkSettings target = AssetDatabase.LoadAssetAtPath<EZFrameworkSettings>(assetPath);
            if (target == null)
            {
                target = ScriptableObject.CreateInstance<EZFrameworkSettings>();
                AssetDatabase.CreateAsset(target, assetPath);
                AssetDatabase.Refresh();
            }
            Selection.activeObject = target;
        }

        [MenuItem(ROOT_NAME + "/Copy Lua To Text", false, PRIORITY + 101)]
        public static void LuaToTxt()
        {
            EZFrameworkEditorUtility.LuaToTxt();
        }
        [MenuItem(ROOT_NAME + "/Clear Lua Text", false, PRIORITY + 102)]
        public static void ClearLuaTxt()
        {
            EZFrameworkEditorUtility.ClearLuaTxt();
        }

        [MenuItem(ROOT_NAME + "/Clear Persistent Path", false, PRIORITY + 201)]
        public static void ClearPersistent()
        {
            EZFrameworkEditorUtility.ClearPersistent();
        }
        [MenuItem(ROOT_NAME + "/Clear StreamingAssets Path", false, PRIORITY + 202)]
        public static void ClearStreamingAssets()
        {
            EZFrameworkEditorUtility.ClearStreamingAssets();
        }
        [MenuItem(ROOT_NAME + "/Clear PlayerPrefs", false, PRIORITY + 203)]
        public static void ClearPlayerPrefs()
        {
            EZFrameworkEditorUtility.ClearPlayerPrefs();
        }
    }
}