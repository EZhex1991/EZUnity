/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-01-06 10:44:41
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
using EZhex1991.EZUnity.Builder;
using EZhex1991.EZUnity.Framework;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.XLuaExtension
{
    public static class EZMenuItems
    {
        private const string ROOT_NAME = "EZUnity/XLuaExtension";

        [EZBundleBuilder.OnPreBuild]
        [MenuItem(ROOT_NAME + "/'.lua' To '.txt'", false, (int)EZMenuItemOrder.LuaToTxt)]
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
        [MenuItem(ROOT_NAME + "/Clear Lua Text", false, (int)EZMenuItemOrder.ClearLuaTextFolder)]
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
