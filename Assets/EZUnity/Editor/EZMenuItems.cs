/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-02-14 15:58:53
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZUnity.Framework;
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

        [MenuItem(ROOT_NAME + "/Save Assets", false, PRIORITY + 1000)]
        private static void SaveAssets()
        {
            AssetDatabase.SaveAssets();
        }
        [MenuItem(ROOT_NAME + "/Refresh AssetDatabase", false, PRIORITY + 1001)]
        private static void RefreshAssetDatabase()
        {
            AssetDatabase.Refresh();
        }
        [MenuItem(ROOT_NAME + "/Clear Persistent Folder", false, PRIORITY + 1002)]
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

        [MenuItem(ROOT_NAME + "/Renamer", false, PRIORITY + 1101)]
        private static void Renamer()
        {
            EditorWindow.GetWindow<EZRenamer>("Renamer").Show();
        }
        [MenuItem(ROOT_NAME + "/Guid Generator", false, PRIORITY + 1102)]
        private static void GuidGenerator()
        {
            EditorWindow.GetWindow<EZGuidGenerator>("Guid Generator").Show();
        }
        [MenuItem(ROOT_NAME + "/Asset Bundle Manager", false, PRIORITY + 1103)]
        private static void BundleManager()
        {
            EditorWindow.GetWindow<EZBundleManager>("Asset Bundle Manager").Show();
        }
        [MenuItem(ROOT_NAME + "/Asset Reference Viewer", false, PRIORITY + 1104)]
        private static void AssetReferenceViewer()
        {
            EditorWindow.GetWindow<EZAssetReferenceViewer>("Asset Reference Viewer").Show();
        }
        [MenuItem(ROOT_NAME + "/Font Reference Viewer", false, PRIORITY + 1105)]
        private static void FontReferenceViewer()
        {
            EditorWindow.GetWindow<EZFontReferenceViewer>("Font Reference Viewer").Show();
        }
        [MenuItem(ROOT_NAME + "/PlayerPrefs Editor", false, PRIORITY + 1106)]
        private static void PlayerPrefsEditor()
        {
            EditorWindow.GetWindow<EZPlayerPrefsEditor>("PlayerPrefs Editor").Show();
        }

        [MenuItem(ROOT_NAME + "/Experimental/Shader Keyword Manager", false, PRIORITY + 5001)]
        private static void ShaderKeywordManager()
        {
            EditorWindow.GetWindow<EZShaderKeywordManager>("Keyword Manager").Show();
        }


        [SettingsProvider]
        private static SettingsProvider CreateEZScriptSettingsProvider()
        {
            AssetSettingsProvider provider = AssetSettingsProvider.CreateProviderFromObject("Project/EZUnity/EZScriptSettings", EZScriptSettings.Instance);
            return provider;
        }
        [SettingsProvider]
        private static SettingsProvider CreateEZEditorSettingsProvider()
        {
            AssetSettingsProvider provider = AssetSettingsProvider.CreateProviderFromObject("Project/EZUnity/EZEditorSettings", EZEditorSettings.Instance);
            return provider;
        }
        [SettingsProvider]
        private static SettingsProvider CreateEZGrapicSettingsProvider()
        {
            EZGraphicsSettings provider = new EZGraphicsSettings("Project/EZUnity/EZGraphicsSettings", SettingsScope.Project);
            return provider;
        }
    }
}