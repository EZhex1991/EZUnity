/*
 * Author:      熊哲
 * CreateTime:  3/8/2017 6:30:08 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EZUnity.Bundle
{
    public class EZBundleBuilder
    {
        [AttributeUsage(AttributeTargets.Method)]
        public class OnPreBuildAttribute : Attribute
        {
            public int priority;
        }
        [AttributeUsage(AttributeTargets.Method)]
        public class OnPostBuildAttribute : Attribute
        {
            public int priority;
        }
        [Serializable]
        public class FileInfo
        {
            public string fileName;
            public string md5;
            public int size;
        }

        protected const char DELIMITER = '|';
        protected static BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.DeterministicAssetBundle;

        public static void BuildBundle(EZBundleObject ezBundle, bool managerMode)
        {
            OnPreBuild();
            if (ezBundle.forceRebuild && Directory.Exists(ezBundle.outputPath)) Directory.Delete(ezBundle.outputPath, true);
            Directory.CreateDirectory(ezBundle.outputPath);
            AssetDatabase.Refresh();
            CopyDirectories(ezBundle);

            AssetDatabase.Refresh();
            if (managerMode)
            {
                OnPostBuild(BuildPipeline.BuildAssetBundles(ezBundle.outputPath, buildOptions, ezBundle.buildTarget));
            }
            else
            {
                OnPostBuild(BuildPipeline.BuildAssetBundles(ezBundle.outputPath, GetBuildList(ezBundle), buildOptions, ezBundle.buildTarget));
            }
            if (!string.IsNullOrEmpty(ezBundle.listFileName)) CreateFileList(ezBundle.outputPath, ezBundle.listFileName);
            AssetDatabase.Refresh();

            Debug.Log("build complete.");
        }
        protected static void OnPreBuild()
        {
            foreach (Type type in (from type in EZEditorUtility.GetAllTypes()
                                   where type.IsClass
                                   select type))
            {
                foreach (MethodInfo methods in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (methods.IsDefined(typeof(OnPreBuildAttribute), false))
                    {
                        methods.Invoke(null, null);
                    }
                }
            }
        }
        protected static void OnPostBuild(AssetBundleManifest manifest)
        {
            foreach (Type type in (from type in EZEditorUtility.GetAllTypes()
                                   where type.IsClass
                                   select type))
            {
                foreach (MethodInfo methods in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (methods.IsDefined(typeof(OnPostBuildAttribute), false))
                    {
                        methods.Invoke(null, new object[] { manifest });
                    }
                }
            }
        }

        protected static void CopyDirectories(EZBundleObject ezBundle)
        {
            foreach (EZBundleObject.CopyInfo copyInfo in ezBundle.copyList)
            {
                string sour = copyInfo.sourDirPath;
                string dest = copyInfo.destDirPath;
                if (string.IsNullOrEmpty(sour) || string.IsNullOrEmpty(dest)) continue;
                if (!Directory.Exists(sour)) return;
                Directory.CreateDirectory(dest);
                string[] files = Directory.GetFiles(sour);
                foreach (string filePath in files)
                {
                    if (filePath.EndsWith(".meta")) continue;
                    string newPath = dest + filePath.Replace(sour, "");
                    Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                    File.Copy(filePath, newPath, true);
                }
            }
        }
        protected static AssetBundleBuild[] GetBuildList(EZBundleObject ezBundle)
        {
            List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
            foreach (EZBundleObject.BundleInfo ezBundleInfo in ezBundle.bundleList)
            {
                if (ezBundleInfo.bundleName == "") continue;
                if (ezBundleInfo.filePattern == "") ezBundleInfo.filePattern = "*.*";
                string[] files = Directory.GetFiles(ezBundleInfo.dirPath, ezBundleInfo.filePattern, ezBundleInfo.searchOption);
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = files[i].Replace('\\', '/');
                }
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = ezBundleInfo.bundleName;
                build.assetNames = files;
                buildList.Add(build);
            }
            return buildList.ToArray();
        }
        protected static void CreateFileList(string dirPath, string listFileName)
        {
            string listFilePath = Path.Combine(dirPath, listFileName);
            if (File.Exists(listFilePath)) File.Delete(listFilePath);

            List<string> fileList = new List<string>();
            DirTraverse(dirPath, fileList);

            StreamWriter streamWriter = new StreamWriter(File.Create(listFilePath));
            foreach (string filePath in fileList)
            {
                if (filePath.EndsWith(".meta") || filePath.EndsWith(".manifest")) continue;
                FileInfo info = new FileInfo()
                {
                    fileName = filePath.Substring(dirPath.Length + 1),
                    md5 = GetFileMD5(filePath),
                    size = Mathf.CeilToInt(new System.IO.FileInfo(filePath).Length >> 10)
                };
                streamWriter.WriteLine(JsonUtility.ToJson(info));
            }
            streamWriter.Close();
            File.Copy(listFilePath, Path.Combine("Assets/Resources", listFileName), true);
        }

        protected static void DirTraverse(string dirPath, List<string> fileList)
        {
            if (!Directory.Exists(dirPath)) return;
            string[] files = Directory.GetFiles(dirPath);
            string[] dirs = Directory.GetDirectories(dirPath);
            foreach (string file in files)
            {
                if (file.EndsWith(".meta")) continue;
                fileList.Add(file.Replace('\\', '/'));
            }
            foreach (string dir in dirs)
            {
                DirTraverse(dir, fileList);
            }
        }
        protected static string GetFileMD5(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] md5Data = md5Hasher.ComputeHash(fs);
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < md5Data.Length; i++)
                {
                    sBuilder.Append(md5Data[i].ToString("x2"));
                }
                fs.Close();
                return sBuilder.ToString();
            }
            catch (Exception ex)
            {
                Debug.Log("MD5File() fail, error: " + ex.Message);
                return null;
            }
        }
    }
}