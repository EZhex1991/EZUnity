/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-08-01 18:15:32
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnity.AssetProcessor
{
    public static class EZAssetGenerator
    {
        public static string defaultDirPath = "Assets/Resources/";
        public static string defaultMaterialName = "default.mat";
        public static string defaultTextAssetName = "default.txt";

        public static T GenerateAsset<T>(string path, Func<T> generator) where T : UnityEngine.Object
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null) asset = generator();
            return asset;
        }

        public static Material GenerateMaterial()
        {
            string path = defaultDirPath + defaultMaterialName;
            return GenerateAsset<Material>(path, delegate ()
            {
                Material material = new Material(Shader.Find("Standard"));
                AssetDatabase.CreateAsset(material, path);
                return material;
            });
        }
        public static TextAsset GenerateTextAsset()
        {
            string path = defaultDirPath + defaultTextAssetName;
            return GenerateAsset<TextAsset>(path, delegate ()
            {
                File.WriteAllText(path, "");
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            });
        }
    }
}