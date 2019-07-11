/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-02-14 15:58:53
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity.Framework;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class EZMenuItems
    {
        private const string ROOT_NAME = "EZUnity";

        [MenuItem(ROOT_NAME + "/EZTimePanel", false, (int)EZMenuItemOrder.TimePanel)]
        private static void ShowTimePanel()
        {
            EditorWindow.GetWindow<EZTimePanel>("EZTimePanel").Show();
        }

        [MenuItem(ROOT_NAME + "/Application Settings", false, (int)EZMenuItemOrder.ApplicationSettings)]
        private static void ApplicationSettings()
        {
            Selection.activeObject = EZApplicationSettings.Instance;
        }

        [MenuItem(ROOT_NAME + "/Save Assets", false, (int)EZMenuItemOrder.SaveAssets)]
        private static void SaveAssets()
        {
            AssetDatabase.SaveAssets();
        }

        [MenuItem(ROOT_NAME + "/Refresh AssetDatabase", false, (int)EZMenuItemOrder.RefreshAssetDatabase)]
        private static void RefreshAssetDatabase()
        {
            AssetDatabase.Refresh();
        }

        [MenuItem(ROOT_NAME + "/Open Persistent Folder", false, (int)EZMenuItemOrder.OpenPersistentFolder)]
        private static void OpenPersistentFolder()
        {
            try
            {
                Application.OpenURL("file://" + Application.persistentDataPath);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        [MenuItem(ROOT_NAME + "/Clear Persistent Folder", false, (int)EZMenuItemOrder.ClearPersistentFolder)]
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

        [MenuItem(ROOT_NAME + "/Renamer", false, (int)EZMenuItemOrder.Renamer)]
        private static void Renamer()
        {
            EditorWindow.GetWindow<EZRenamerWindow>("Renamer").Show();
        }

        [MenuItem(ROOT_NAME + "/Guid Generator", false, (int)EZMenuItemOrder.GuidGenerator)]
        private static void GuidGenerator()
        {
            EditorWindow.GetWindow<EZGuidGenerator>("Guid Generator").Show();
        }

        [MenuItem(ROOT_NAME + "/Asset Bundle Manager", false, (int)EZMenuItemOrder.AssetBundleManager)]
        private static void BundleManager()
        {
            EditorWindow.GetWindow<Builder.EZBundleManager>("Asset Bundle Manager").Show();
        }

        [MenuItem(ROOT_NAME + "/Asset Reference Viewer", false, (int)EZMenuItemOrder.AssetReferenceViewer)]
        private static void AssetReferenceViewer()
        {
            EditorWindow.GetWindow<EZAssetReferenceViewer>("Asset Reference Viewer").Show();
        }

        [MenuItem(ROOT_NAME + "/Font Reference Viewer", false, (int)EZMenuItemOrder.FontReferenceViewer)]
        private static void FontReferenceViewer()
        {
            EditorWindow.GetWindow<EZFontReferenceViewer>("Font Reference Viewer").Show();
        }

        [MenuItem(ROOT_NAME + "/PlayerPrefs Editor", false, (int)EZMenuItemOrder.PlayerPrefsEditor)]
        private static void PlayerPrefsEditor()
        {
            EditorWindow.GetWindow<EZPlayerPrefsEditor>("PlayerPrefs Editor").Show();
        }

        [MenuItem(ROOT_NAME + "/Color Blender", false, (int)EZMenuItemOrder.ColorBlender)]
        private static void ColorBlender()
        {
            EditorWindow.GetWindow<EZColorBlender>("Color Blender").Show();
        }

        [MenuItem(ROOT_NAME + "/Corresponding Object Viewer", false, (int)EZMenuItemOrder.CorrespondingObjectViewer)]
        private static void CorrespondingObjectViewer()
        {
            EditorWindow.GetWindow<EZCorrespondingObjectViewer>("Corresponding Object Viewer").Show();
        }

        [MenuItem(ROOT_NAME + "/Experimental/Shader Keyword Manager", false, (int)EZMenuItemOrder.ShaderKeywordManager)]
        private static void ShaderKeywordManager()
        {
            EditorWindow.GetWindow<EZShaderKeywordManager>("Keyword Manager").Show();
        }

        [MenuItem(ROOT_NAME + "/Regex Tester", false, (int)EZMenuItemOrder.RegexTester)]
        private static void RegexTester()
        {
            EditorWindow.GetWindow<EZRegexTester>("Regex Tester").Show();
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
#else
        [PreferenceItem("EZScriptSettings")]
        private static void CreateEZScriptSettingsProvider()
        {
            Editor.CreateEditor(EZScriptSettings.Instance).OnInspectorGUI();
        }
        [PreferenceItem("EZEditorSettings")]
        private static void CreateEZEditorSettingsProvider()
        {
            Editor.CreateEditor(EZEditorSettings.Instance).OnInspectorGUI();
        }
#endif
    }
}