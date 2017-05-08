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

namespace EZUnityTools.EZEditor
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

        protected const char DELIMITER = '|';

        public static void BuildBundle(EZBundleObject ezBundle)
        {
            OnPreBuild();
            string bundleDirPath = ezBundle.relativePath ? "Assets/" + ezBundle.bundleDirPath : ezBundle.bundleDirPath;
            if (!bundleDirPath.EndsWith("/")) bundleDirPath = bundleDirPath + "/";
            if (ezBundle.removeOldFiles && Directory.Exists(bundleDirPath)) Directory.Delete(bundleDirPath, true);
            Directory.CreateDirectory(bundleDirPath);

            AssetDatabase.Refresh();
            foreach (EZBundleObject.CopyInfo copyInfo in ezBundle.copyList)
            {
                if (copyInfo.sourDirPath == "") continue;
                if (copyInfo.filePattern == "") copyInfo.filePattern = "*.*";
                DirCopy("Assets/" + copyInfo.sourDirPath, "Assets/" + copyInfo.destDirPath + "/" + copyInfo.sourDirPath, copyInfo.filePattern, copyInfo.searchOption);
            }
            AssetDatabase.Refresh();
            BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle;
            BuildPipeline.BuildAssetBundles(bundleDirPath, GetBuildList(ezBundle).ToArray(), options, ezBundle.bundleTarget);
            if (ezBundle.createListFile) BuildFileIndex(bundleDirPath, ezBundle.listFileName);
            AssetDatabase.Refresh();
            OnPostBuild();
        }
        protected static void OnPreBuild()
        {
            foreach (Type type in (from type in EZUtility.GetAllTypes()
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
        protected static void OnPostBuild()
        {
            foreach (Type type in (from type in EZUtility.GetAllTypes()
                                   where type.IsClass
                                   select type))
            {
                foreach (MethodInfo methods in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (methods.IsDefined(typeof(OnPostBuildAttribute), false))
                    {
                        methods.Invoke(null, null);
                    }
                }
            }
        }
        protected static void DirCopy(string source, string destination, string pattern, SearchOption searchOption)
        {
            if (!Directory.Exists(source)) return;
            Directory.CreateDirectory(destination);
            string[] files = Directory.GetFiles(source, pattern, searchOption);
            foreach (string filePath in files)
            {
                if (filePath.EndsWith(".meta")) continue;
                string newPath = destination + filePath.Replace(source, "");
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                File.Copy(filePath, newPath, true);
            }
        }
        protected static List<AssetBundleBuild> GetBuildList(EZBundleObject ezBundle)
        {
            List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
            foreach (EZBundleObject.BundleInfo ezBundleInfo in ezBundle.bundleList)
            {
                if (ezBundleInfo.bundleName == "" || ezBundleInfo.dirPath == "") continue;
                if (ezBundleInfo.filePattern == "") ezBundleInfo.filePattern = "*.*";
                string[] files = Directory.GetFiles("Assets/" + ezBundleInfo.dirPath, ezBundleInfo.filePattern, ezBundleInfo.searchOption);
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = files[i].Replace('\\', '/');
                }
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = ezBundleInfo.bundleName + ezBundle.bundleExtension;
                build.assetNames = files;
                buildList.Add(build);
            }
            return buildList;
        }
        protected static void BuildFileIndex(string dirPath, string listFileName)
        {
            string listFilePath = dirPath + listFileName;
            if (File.Exists(listFilePath)) File.Delete(listFilePath);

            List<string> fileList = new List<string>();
            DirTraverse(dirPath, fileList);

            FileStream fileStream = new FileStream(listFilePath, FileMode.CreateNew);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            foreach (string filePath in fileList)
            {
                if (filePath.EndsWith(".meta")) continue;
                string md5Data = GetFileMD5(filePath);
                string relativePath = filePath.Replace(dirPath, string.Empty);
                streamWriter.WriteLine(relativePath + DELIMITER + md5Data);
            }
            streamWriter.Close();
            fileStream.Close();
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