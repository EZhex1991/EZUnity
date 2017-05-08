/*
 * Author:      熊哲
 * CreateTime:  11/28/2016 12:02:02 PM
 * Description:
 * 
*/
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnityTools.EZEditor
{
    public abstract class EZScriptableObject : ScriptableObject
    {
        // 自定义Asset的存放路径（相对于工程路径）
        private static string AssetsDirPath = "Assets/EZAssets/";

        // 加载一个ScriptableAsset
        public static T Load<T>(string adbFileName, bool createDefault = true) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(GetPath(adbFileName));
            if (asset == null && createDefault)
            {
                asset = Create<T>(adbFileName);
            }
            return asset;
        }
        // 创建一个ScriptableAsset
        public static T Create<T>(string adbFileName, T obj = null) where T : ScriptableObject
        {
            if (obj == null) obj = ScriptableObject.CreateInstance<T>();
            if (!Directory.Exists(AssetsDirPath)) { Directory.CreateDirectory(AssetsDirPath); }
            AssetDatabase.CreateAsset(obj, GetPath(adbFileName));
            return obj;
        }
        // 根据Asset的名称得到其的路径（相对于工程路径）
        public static string GetPath(string adbFileName)
        {
            return AssetsDirPath + adbFileName + ".asset";
        }
    }
}