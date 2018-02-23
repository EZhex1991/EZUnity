/*
 * Author:      熊哲
 * CreateTime:  2/14/2017 3:58:53 PM
 * Description:
 * 
*/
using UnityEditor;

namespace EZUnityEditor
{
    public class MenuItems
    {
        private const string ROOT_NAME = "EZUnityEditor";
        private const int PRIORITY = 12000;

        [MenuItem(ROOT_NAME + "/Refresh AssetDatabase", false, PRIORITY + 1)]
        private static void RefreshAssetDatabase()
        {
            AssetDatabase.Refresh();
        }

        [MenuItem(ROOT_NAME + "/EZRename", false, PRIORITY + 51)]
        private static void EZRename()
        {
            EditorWindow.GetWindow<EZRenameEditorWindow>("Rename").Show();
        }
        [MenuItem(ROOT_NAME + "/EZPlayerPrefsEditor", false, PRIORITY + 52)]
        private static void PlayerPrefsEditor()
        {
            EditorWindow.GetWindow<EZPlayerPrefsEditor>("PlayerPrefs Editor").Show();
        }

        [MenuItem(ROOT_NAME + "/EZProjectSettings/EZKeystore", false, PRIORITY + 101)]
        private static void EZKeystore()
        {
            Selection.activeObject = EZScriptableObject.Load<EZKeystoreObject>(EZKeystoreObject.AssetName);
        }
        [MenuItem(ROOT_NAME + "/EZProjectSettings/Include Built-in Shaders", false, PRIORITY + 102)]
        private static void IncludeBuiltinShaders()
        {
            EZGraphicsSettings.IncludeBuiltinShaders();
        }

        [MenuItem(ROOT_NAME + "/EZAssetGenerator/Material", false, PRIORITY + 111)]
        private static void GenerateMaterial()
        {
            Selection.activeObject = EZAssetGenerator.GenerateMaterial();
        }
        [MenuItem(ROOT_NAME + "/EZAssetGenerator/TextAsset", false, PRIORITY + 112)]
        private static void GenerateTextAsset()
        {
            Selection.activeObject = EZAssetGenerator.GenerateTextAsset();
        }

        [MenuItem(ROOT_NAME + "/EZBundle/EZBundle", false, PRIORITY + 121)]
        private static void EZBundle()
        {
            Selection.activeObject = EZScriptableObject.Load<EZBundleObject>(EZBundleObject.AssetName);
        }
        [MenuItem(ROOT_NAME + "/EZBundle/Bundle Manager", false, PRIORITY + 122)]
        private static void EZBundleManager()
        {
            EditorWindow.GetWindow<EZBundleManager>("Bundle Manager").Show();
        }
        [MenuItem(ROOT_NAME + "/EZBundle/Bundle Viewer", false, PRIORITY + 123)]
        private static void EZBundleViewer()
        {
            EditorWindow.GetWindow<EZBundleViewer>("Bundle Viewer").Show();
        }

        [MenuItem(ROOT_NAME + "/EZScriptTemplate/Templates Manager", false, PRIORITY + 131)]
        private static void EZScriptTemplate()
        {
            EditorWindow.GetWindow<EZScriptTemplateManager>("Templates Manager").Show();
        }
        [MenuItem(ROOT_NAME + "/EZScriptTemplate/Script Statistics", false, PRIORITY + 132)]
        private static void EZScriptStatistics()
        {
            Selection.activeObject = EZScriptableObject.Load<EZScriptStatisticsObject>(EZScriptStatisticsObject.AssetName);
        }
    }
}