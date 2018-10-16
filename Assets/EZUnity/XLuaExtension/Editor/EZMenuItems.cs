/*
 * Author:      熊哲
 * CreateTime:  1/6/2017 10:44:41 AM
 * Description:
 * 
*/
#if XLUA
using EZUnity.Framework;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    public static partial class EZMenuItems
    {
        [Bundle.EZBundleBuilder.OnPreBuild]
        [MenuItem(ROOT_NAME + "/XLuaExtension/'.lua' To '.txt'", false, PRIORITY + 5001)]
        public static void LuaToTxt()
        {
            foreach (string dirPath in EZApplicationSettings.Instance.luaFolders)
            {
                string luaFolderPath = Path.Combine(Application.dataPath, dirPath);
                string txtFolderPath = luaFolderPath + "_txt/";
                if (!Directory.Exists(luaFolderPath)) continue;
                Directory.CreateDirectory(txtFolderPath);
                string[] files = Directory.GetFiles(luaFolderPath, "*.lua", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string newPath = txtFolderPath + file.Substring(luaFolderPath.Length + 1).Replace("/", "__").Replace("\\", "__") + ".txt";
                    File.Copy(file, newPath, true);
                }
                Debug.Log("Copy complete: " + txtFolderPath);
            }
            AssetDatabase.Refresh();
        }
        [MenuItem(ROOT_NAME + "/XLuaExtension/Clear Lua Text", false, PRIORITY + 5002)]
        public static void ClearLuaTxt()
        {
            foreach (string dirPath in EZApplicationSettings.Instance.luaFolders)
            {
                string luaFolderPath = Path.Combine(Application.dataPath, dirPath);
                string txtDirPath = luaFolderPath + "_txt/";
                try
                {
                    Directory.Delete(txtDirPath, true);
                    Debug.Log("Delete complete: " + txtDirPath);
                    AssetDatabase.Refresh();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
        }
    }
}
#endif
