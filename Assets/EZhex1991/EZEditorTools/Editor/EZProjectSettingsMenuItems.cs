/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-18
 * Organization:    #ORGANIZATION#
 * Description:     Menu and Project Settings entries for EZEditorTools.
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    internal static class EZProjectSettingsMenuItems
    {
        private const string Root = "EZEditorTools/";
        private const string SettingsRoot = "Project/EZProjectSettings";

        [MenuItem(Root + "Project Settings", false, (int)EZMenuItemOrder.ProjectSettings)]
        private static void OpenProjectSettings()
        {
            SettingsService.OpenProjectSettings(SettingsRoot);
        }

#if UNITY_2018_3_OR_NEWER
        private static SettingsProvider CreateProjectSettingsProvider<T>(string name, T instance) where T : EZProjectSettingsSingleton<T>
        {
            AssetSettingsProvider provider = AssetSettingsProvider.CreateProviderFromObject(SettingsRoot + "/" + name, instance);
            provider.guiHandler += searchContext =>
            {
                if (GUI.changed) instance.Save();
            };
            return provider;
        }

        [SettingsProvider]
        private static SettingsProvider CreateEZEditorSettingsProvider()
        {
            return CreateProjectSettingsProvider(nameof(EZEditorSettings), EZEditorSettings.Instance);
        }

        [SettingsProvider]
        private static SettingsProvider CreateEZScriptSettingsProvider()
        {
            return CreateProjectSettingsProvider(nameof(EZScriptSettings), EZScriptSettings.Instance);
        }

        [SettingsProvider]
        private static SettingsProvider CreateEZAssetImporterProvider()
        {
            return CreateProjectSettingsProvider(nameof(EZAssetImporterManager), EZAssetImporterManager.Instance);
        }

        [SettingsProvider]
        private static SettingsProvider CreateEZGraphicsSettingsProvider()
        {
            return new EZGraphicsSettings(SettingsRoot + "/" + nameof(EZGraphicsSettings), SettingsScope.Project);
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
