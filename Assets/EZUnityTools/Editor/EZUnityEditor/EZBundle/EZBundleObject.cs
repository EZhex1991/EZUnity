/*
 * Author:      熊哲
 * CreateTime:  3/6/2017 2:13:28 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace EZUnityEditor
{
    public class EZBundleObject : EZScriptableObject
    {
        public const string AssetName = "EZBundle";

        public BuildTarget bundleTarget = BuildTarget.Android;
        public string bundleDirPath = "Assets/StreamingAssets";
        public string bundleExtension = ".unity3d";
        public string listFileName = "files.txt";
        public bool removeOldFiles = false;

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
        public List<CopyInfo> copyList = new List<CopyInfo>();
        public List<BundleInfo> bundleList = new List<BundleInfo>();
    }
}