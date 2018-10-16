/*
 * Author:      熊哲
 * CreateTime:  2/14/2017 3:58:53 PM
 * Description:
 * 
*/
using EZUnity.AssetProcessor;
using EZUnity.Bundle;
using EZUnity.Framework;
using EZUnity.ProjectSettings;
using EZUnity.Scripting;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    public static partial class EZMenuItems
    {
        private const string ROOT_NAME = "EZUnity";
        private const int PRIORITY = 12000;

        [MenuItem(ROOT_NAME + "/Application Settings", false, PRIORITY + 1)]
        private static void ApplicationSettings()
        {
            Selection.activeObject = EZApplicationSettings.Instance;
        }

        [MenuItem(ROOT_NAME + "/Save Assets", false, PRIORITY + 1001)]
        private static void SaveAssets()
        {
            AssetDatabase.SaveAssets();
        }
        [MenuItem(ROOT_NAME + "/Refresh AssetDatabase", false, PRIORITY + 1002)]
        private static void RefreshAssetDatabase()
        {
            AssetDatabase.Refresh();
        }
        [MenuItem(ROOT_NAME + "/Clear Persistent Folder", false, PRIORITY + 1003)]
        private static void ClearPersistentFolder()
        {
            try
            {
                Directory.Delete(Application.persistentDataPath, true);
                Debug.LogFormat("{0} Cleared", Application.persistentDataPath);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        [MenuItem(ROOT_NAME + "/Experimental/Include Built-in Shaders", false, PRIORITY + 1014)]
        private static void IncludeBuiltinShaders()
        {
            EZGraphicsSettings.IncludeBuiltinShaders();
        }

        [MenuItem(ROOT_NAME + "/Script Template Manager", false, PRIORITY + 1101)]
        private static void ScriptTemplate()
        {
            EditorWindow.GetWindow<EZScriptTemplateManager>("Script Template Manager").Show();
        }
        [MenuItem(ROOT_NAME + "/Renamer", false, PRIORITY + 1102)]
        private static void Renamer()
        {
            EditorWindow.GetWindow<EZRenamer>("Renamer").Show();
        }
        [MenuItem(ROOT_NAME + "/Guid Generator", false, PRIORITY + 1103)]
        private static void GuidGenerator()
        {
            EditorWindow.GetWindow<EZGuidGenerator>("Guid Generator").Show();
        }
        [MenuItem(ROOT_NAME + "/Asset Bundle Manager", false, PRIORITY + 1104)]
        private static void BundleManager()
        {
            EditorWindow.GetWindow<EZBundleManager>("Asset Bundle Manager").Show();
        }
        [MenuItem(ROOT_NAME + "/Asset Reference Viewer", false, PRIORITY + 1105)]
        private static void AssetReferenceViewer()
        {
            EditorWindow.GetWindow<EZAssetReferenceViewer>("Asset Reference Viewer").Show();
        }
        [MenuItem(ROOT_NAME + "/Font Reference Viewer", false, PRIORITY + 1106)]
        private static void FontReferenceViewer()
        {
            EditorWindow.GetWindow<EZFontReferenceViewer>("Font Reference Viewer").Show();
        }
        [MenuItem(ROOT_NAME + "/PlayerPrefs Editor", false, PRIORITY + 1107)]
        private static void PlayerPrefsEditor()
        {
            EditorWindow.GetWindow<EZPlayerPrefsEditor>("PlayerPrefs Editor").Show();
        }

        [MenuItem(ROOT_NAME + "/Generator Material", false, PRIORITY + 1301)]
        private static void GenerateMaterial()
        {
            Selection.activeObject = EZAssetGenerator.GenerateMaterial();
        }
        [MenuItem(ROOT_NAME + "/Generator TextAsset", false, PRIORITY + 1302)]
        private static void GenerateTextAsset()
        {
            Selection.activeObject = EZAssetGenerator.GenerateTextAsset();
        }
    }
}