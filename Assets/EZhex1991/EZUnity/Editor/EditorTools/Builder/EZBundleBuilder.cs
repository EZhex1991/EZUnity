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

namespace EZhex1991.EZUnity.Builder
{
    [CreateAssetMenu(fileName = "EZBundleBuilder", menuName = "EZUnity/EZBundleBuilder", order = (int)EZAssetMenuOrder.EZBundleBuilder)]
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

        protected const char DELIMITER = '|';

        public BuildTarget buildTarget = BuildTarget.NoTarget;
        public BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.DeterministicAssetBundle;
        public string outputPath = "Assets/StreamingAssets";
        public string listFileName = "files.txt";
        public bool managerMode = false;
        public bool forceRebuild = false;

        public CopyInfo[] copyList;
        public BundleInfo[] bundleList;

        // view options
        public bool copyListFoldout = true;
        public bool bundleListFoldout = true;
        public AssetsViewOption showAssets = AssetsViewOption.Object;
        public BundleDependenciesViewOption showDependencies = BundleDependenciesViewOption.Recursive;

        public void Execute(BuildTarget buildTarget)
        {
            OnPreBuild();
            if (forceRebuild && Directory.Exists(outputPath)) Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);
            AssetDatabase.Refresh();
            CopyFiles();

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

        protected void CopyFiles()
        {
            for (int i = 0; i < copyList.Length; i++)
            {
                EditorUtility.DisplayProgressBar("Copying Files", "", (float)i / copyList.Length);
                string src = copyList[i].srcPath;
                string dst = copyList[i].dstPath;
                if (string.IsNullOrEmpty(src) || string.IsNullOrEmpty(dst)) continue;
                if (File.Exists(src))
                {
                    try
                    {
                        File.Copy(src, dst, true);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                    }
                }
                else if (Directory.Exists(src))
                {
                    Directory.CreateDirectory(dst);
                    string[] files = Directory.GetFiles(src);
                    foreach (string filePath in files)
                    {
                        try
                        {
                            if (filePath.EndsWith(".meta")) continue;
                            string newPath = dst + filePath.Substring(src.Length);
                            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                            File.Copy(filePath, newPath, true);
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning(e.Message);
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();
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