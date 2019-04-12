/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-03-06 14:13:28
 * Organization:    #ORGANIZATION#
 * Description:     
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

namespace EZUnity
{
    public enum AssetsViewOption
    {
        Object = 0,
        Path = 1,
        PathAndObject = 2,
    }
    public enum BundleDependenciesViewOption
    {
        DontShow = 0,
        Direct = 1,
        Recursive = 2,
    }

    [CreateAssetMenu(fileName = "EZBundleBuilder", menuName = "EZUnity/EZBundleBuilder", order = EZUnityMenuOrder.EZBundleBuilder)]
    public class EZBundleBuilder : ScriptableObject
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
        public class CopyInfo
        {
            public string destDirPath = "";
            public string sourDirPath = "";
        }
        [Serializable]
        public class BundleInfo
        {
            public string bundleName = "";
            public string filePattern = "*.*";
            public SearchOption searchOption = SearchOption.AllDirectories;
            public string dirPath = "";
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

        public BuildTarget buildTarget = BuildTarget.Android;
        public string outputPath = "Assets/StreamingAssets";
        public string listFileName = "files.txt";
        public bool managerMode = false;
        public bool forceRebuild = false;

        public List<CopyInfo> copyList = new List<CopyInfo>();
        public List<BundleInfo> bundleList = new List<BundleInfo>();

        // view options
        public bool copyListFoldout = true;
        public bool bundleListFoldout = true;
        public AssetsViewOption showAssets = AssetsViewOption.Object;
        public BundleDependenciesViewOption showDependencies = BundleDependenciesViewOption.Recursive;

        public void Execute()
        {
            Execute(buildTarget);
        }
        public void Execute(BuildTarget buildTarget)
        {
            OnPreBuild();
            if (forceRebuild && Directory.Exists(outputPath)) Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);
            AssetDatabase.Refresh();
            CopyDirectories();

            AssetDatabase.Refresh();
            AssetBundleManifest manifest;
            if (managerMode)
            {
                manifest = BuildPipeline.BuildAssetBundles(outputPath, buildOptions, buildTarget);
            }
            else
            {
                manifest = BuildPipeline.BuildAssetBundles(outputPath, GetBuildList(), buildOptions, buildTarget);
            }
            OnPostBuild(manifest);
            if (!string.IsNullOrEmpty(listFileName)) CreateFileList(outputPath, listFileName);
            AssetDatabase.Refresh();

            Debug.Log("build complete.");
        }

        protected void OnPreBuild()
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
        protected void OnPostBuild(AssetBundleManifest manifest)
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

        protected void CopyDirectories()
        {
            foreach (CopyInfo copyInfo in copyList)
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
        protected AssetBundleBuild[] GetBuildList()
        {
            List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
            foreach (BundleInfo bundleInfo in bundleList)
            {
                if (bundleInfo.bundleName == "") continue;
                if (bundleInfo.filePattern == "") bundleInfo.filePattern = "*.*";
                string[] files = Directory.GetFiles(bundleInfo.dirPath, bundleInfo.filePattern, bundleInfo.searchOption);
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = files[i].Replace('\\', '/');
                }
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = bundleInfo.bundleName;
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