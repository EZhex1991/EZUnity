/*
 * Author:      熊哲
 * CreateTime:  1/6/2017 10:44:41 AM
 * Description:
 * 
*/
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZFramework
{
    public static class EZPathUtility
    {
        [EZUnityTools.EZEditor.EZBundleBuilder.OnPreBuild]
        public static void LuaToTxt()
        {
            string luaDirPath = "Assets/" + EZSettings.Instance.luaDirName + "/";
            string luaTxtDirPath = "Assets/" + EZSettings.Instance.luaDirName + "_txt/";
            if (!Directory.Exists(luaDirPath)) return;
            if (Directory.Exists(luaTxtDirPath)) Directory.Delete(luaTxtDirPath, true);
            Directory.CreateDirectory(luaTxtDirPath);
            string[] files = Directory.GetFiles(luaDirPath, "*.lua", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string newPath = luaTxtDirPath + file.Replace(luaDirPath, "").Replace("/", "_").Replace("\\", "_") + ".txt";
                File.Copy(file, newPath, true);
            }
            AssetDatabase.Refresh();
        }
        public static void ClearLuaTxt()
        {
            string luaTxtDirPath = "Assets/" + EZSettings.Instance.luaDirName + "_txt/";
            try
            {
                Directory.Delete(luaTxtDirPath, true);
                Debug.Log("Lua Text Directory Cleared.");
                AssetDatabase.Refresh();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        public static void ClearStreamingAssets()
        {
            try
            {
                Directory.Delete(EZUtility.streamingDirPath, true);
                Debug.Log("Streaming Directory Cleared.");
                AssetDatabase.Refresh();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        public static void ClearPersistent()
        {
            try
            {
                Directory.Delete(EZUtility.persistentDirPath, true);
                Debug.Log("Persistent Directory Cleared.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}