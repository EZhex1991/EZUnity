/* Author:          熊哲
 * CreateTime:      2016-11-28 12:02:02
 * Orgnization:     #ORGNIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnityEditor
{
    public class EZScriptableObject : ScriptableObject
    {
        // 加载一个ScriptableAsset
        public static T Load<T>(string assetName, bool createDefault = true) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(GetPath(assetName));
            if (asset == null && createDefault)
            {
                asset = Create<T>(assetName);
            }
            return asset;
        }
        // 创建一个ScriptableAsset
        public static T Create<T>(string assetName, T obj = null) where T : ScriptableObject
        {
            if (obj == null) obj = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(obj, GetPath(assetName));
            return obj;
        }
        // 根据Asset的名称得到其的路径（相对于工程路径）
        public static string GetPath(string assetName)
        {
            return EZEditorUtility.assetDirPath + assetName + ".asset";
        }
    }
}