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
    public static class EZFrameworkEditorUtility
    {
        [EZUnityEditor.EZBundleBuilder.OnPreBuild]
        public static void LuaToTxt()
        {
            foreach (string dirPath in EZFrameworkSettings.Instance.luaDirList)
            {
                string luaDirPath = "Assets/" + dirPath + "/";
                string txtDirPath = "Assets/" + dirPath + "_txt/";
                if (!Directory.Exists(luaDirPath)) continue;
                Directory.CreateDirectory(txtDirPath);
                string[] files = Directory.GetFiles(luaDirPath, "*.lua", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string newPath = txtDirPath + file.Replace(luaDirPath, "").Replace("/", "__").Replace("\\", "__") + ".txt";
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

        public static void ClearPersistentData()
        {
            try
            {
                Directory.Delete(Application.persistentDataPath, true);
                Debug.Log("PersistentData Cleared.");
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