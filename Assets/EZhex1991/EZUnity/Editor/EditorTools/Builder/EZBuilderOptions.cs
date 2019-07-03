/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-24 14:30:01
 * Organization:    ezhex1991@outlook.com
 * Description:     
 */
using System;
using System.IO;

namespace EZhex1991.EZUnity.Builder
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

    [Serializable]
    public class CopyInfo
    {
        [UnityEngine.Serialization.FormerlySerializedAs("destDirPath")]
        public string dstPath = "";
        [UnityEngine.Serialization.FormerlySerializedAs("sourDirPath")]
        public string srcPath = "";
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
}
