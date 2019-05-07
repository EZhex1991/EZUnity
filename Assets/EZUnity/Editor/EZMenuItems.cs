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
    public static class EZMenuItems
    {
        private enum Order
        {
            _Section_1 = 12000,
            ApplicationSettings,

            _Section_2 = 13000,
            SaveAssets,
            RefreshAssetDatabase,
            ClearPersistentFolder,

            _Section_3 = 14000,
            Renamer,
            GuidGenerator,
            AssetBundleManager,
            AssetReferenceViewer,
            CorrespondingObjectViewer,
            FontReferenceViewer,
            PlayerPrefsEditor,
            ColorBlender,

            _Section_4 = 15000,
            ShaderKeywordManager,
        }

        private const string ROOT_NAME = "EZUnity";

        [MenuItem(ROOT_NAME + "/Application Settings", false, (int)Order.ApplicationSettings)]
        private static void ApplicationSettings()
        {
            Selection.activeObject = EZApplicationSettings.Instance;
        }

        [MenuItem(ROOT_NAME + "/Save Assets", false, (int)Order.SaveAssets)]
        private static void SaveAssets()
        {
            AssetDatabase.SaveAssets();
        }
        [MenuItem(ROOT_NAME + "/Refresh AssetDatabase", false, (int)Order.RefreshAssetDatabase)]
        private static void RefreshAssetDatabase()
        {
            AssetDatabase.Refresh();
        }
        [MenuItem(ROOT_NAME + "/Clear Persistent Folder", false, (int)Order.ClearPersistentFolder)]
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

        [MenuItem(ROOT_NAME + "/Renamer", false, (int)Order.Renamer)]
        private static void Renamer()
        {
            EditorWindow.GetWindow<EZRenamer>("Renamer").Show();
        }
        [MenuItem(ROOT_NAME + "/Guid Generator", false, (int)Order.GuidGenerator)]
        private static void GuidGenerator()
        {
            EditorWindow.GetWindow<EZGuidGenerator>("Guid Generator").Show();
        }
        [MenuItem(ROOT_NAME + "/Asset Bundle Manager", false, (int)Order.AssetBundleManager)]
        private static void BundleManager()
        {
            EditorWindow.GetWindow<EZBundleManager>("Asset Bundle Manager").Show();
        }
        [MenuItem(ROOT_NAME + "/Asset Reference Viewer", false, (int)Order.AssetReferenceViewer)]
        private static void AssetReferenceViewer()
        {
            EditorWindow.GetWindow<EZAssetReferenceViewer>("Asset Reference Viewer").Show();
        }
        [MenuItem(ROOT_NAME + "/Font Reference Viewer", false, (int)Order.FontReferenceViewer)]
        private static void FontReferenceViewer()
        {
            EditorWindow.GetWindow<EZFontReferenceViewer>("Font Reference Viewer").Show();
        }
        [MenuItem(ROOT_NAME + "/PlayerPrefs Editor", false, (int)Order.PlayerPrefsEditor)]
        private static void PlayerPrefsEditor()
        {
            EditorWindow.GetWindow<EZPlayerPrefsEditor>("PlayerPrefs Editor").Show();
        }
        [MenuItem(ROOT_NAME + "/Color Blender", false, (int)Order.ColorBlender)]
        private static void ColorBlender()
        {
            EditorWindow.GetWindow<EZColorBlender>("Color Blender").Show();
        }
        [MenuItem(ROOT_NAME + "/Corresponding Object Viewer", false, (int)Order.CorrespondingObjectViewer)]
        private static void CorrespondingObjectViewer()
        {
            EditorWindow.GetWindow<EZCorrespondingObjectViewer>("Corresponding Object Viewer").Show();
        }
        [MenuItem(ROOT_NAME + "/Experimental/Shader Keyword Manager", false, (int)Order.ShaderKeywordManager)]
        private static void ShaderKeywordManager()
        {
            EditorWindow.GetWindow<EZShaderKeywordManager>("Keyword Manager").Show();
        }

#if UNITY_2018_3_OR_NEWER
        [SettingsProvider]
        private static SettingsProvider CreateEZScriptSettingsProvider()
        {
            AssetSettingsProvider provider = AssetSettingsProvider.CreateProviderFromObject("Project/" + ROOT_NAME + "/EZScriptSettings", EZScriptSettings.Instance);
            provider.guiHandler += (searchContext) =>
            {
                if (GUI.changed) EZScriptSettings.Instance.Save();
            };
            return provider;
        }
        [SettingsProvider]
        private static SettingsProvider CreateEZEditorSettingsProvider()
        {
            AssetSettingsProvider provider = AssetSettingsProvider.CreateProviderFromObject("Project/" + ROOT_NAME + "/EZEditorSettings", EZEditorSettings.Instance);
            provider.guiHandler += (searchContext) =>
            {
                if (GUI.changed) EZEditorSettings.Instance.Save();
            };
            return provider;
        }
        [SettingsProvider]
        private static SettingsProvider CreateEZGrapicSettingsProvider()
        {
            EZGraphicsSettings provider = new EZGraphicsSettings("Project/" + ROOT_NAME + "/EZGraphicsSettings", SettingsScope.Project);
            return provider;
        }
#endif
    }
}