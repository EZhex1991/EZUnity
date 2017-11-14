/*
 * Author:      熊哲
 * CreateTime:  1/6/2017 10:44:41 AM
 * Description:
 * 
*/
using EZFramework;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZFrameworkEditor
{
    public static class EZFrameworkUtility
    {
        [EZUnityEditor.EZBundleBuilder.OnPreBuild]
        public static void LuaToTxt()
        {
            foreach (string dirPath in EZFrameworkSettings.Instance.luaDirList)
            {
                string luaDirPath = "Assets/" + dirPath + "/";
                string txtDirPath = "Assets/" + dirPath + "_txt/";
                if (!Directory.Exists(luaDirPath)) return;
                // if (Directory.Exists(luaTxtDirPath)) Directory.Delete(luaTxtDirPath, true);
                Directory.CreateDirectory(txtDirPath);
                string[] files = Directory.GetFiles(luaDirPath, "*.lua", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string newPath = txtDirPath + file.Replace(luaDirPath, "").Replace("/", "_").Replace("\\", "_") + ".txt";
                    File.Copy(file, newPath, true);
                }
                Debug.Log("Copy complete: " + txtDirPath);
            }
            AssetDatabase.Refresh();
        }
        public static void ClearLuaTxt()
        {
            foreach (string dirPath in EZFrameworkSettings.Instance.luaDirList)
            {
                string txtDirPath = "Assets/" + dirPath + "_txt/";
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

        public static void ClearStreamingAssets()
        {
            try
            {
                Directory.Delete(EZFacade.streamingDirPath, true);
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
                Directory.Delete(EZFacade.persistentDirPath, true);
                Debug.Log("Persistent Directory Cleared.");
                AssetDatabase.Refresh();
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