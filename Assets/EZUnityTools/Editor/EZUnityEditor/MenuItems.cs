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

        [MenuItem(ROOT_NAME + "/EZRename", false, 1)]
        private static void EZRename()
        {
            EditorWindow.GetWindow<EZRenameEditorWindow>("EZRename").Show();
        }

        [MenuItem(ROOT_NAME + "/EZPlayerPrefsEditor", false, 2)]
        private static void PlayerPrefsEditor()
        {
            EditorWindow.GetWindow<EZPlayerPrefsEditor>("EZPlayerPrefsEditor").Show();
        }

        [MenuItem(ROOT_NAME + "/EZScriptTemplate", false, 3)]
        private static void EZScriptTemplate()
        {
            EditorWindow.GetWindow<EZScriptTemplateEditorWindow>("EZScriptTemplate").Show();
        }

        [MenuItem(ROOT_NAME + "/EZProjectSettings/EZKeystore", false, 100)]
        private static void EZKeystore()
        {
            Selection.activeObject = EZScriptableObject.Load<EZKeystoreObject>(EZKeystoreObject.AssetName);
        }
        [MenuItem(ROOT_NAME + "/EZProjectSettings/Include Built-in Shaders", false, 101)]
        private static void IncludeBuiltinShaders()
        {
            EZGraphicsSettings.IncludeBuiltinShaders();
        }

        [MenuItem(ROOT_NAME + "/EZAssetGenerator/Material", false, 110)]
        private static void GenerateMaterial()
        {
            Selection.activeObject = EZAssetGenerator.GenerateMaterial();
        }
        [MenuItem(ROOT_NAME + "/EZAssetGenerator/TextAsset", false, 111)]
        private static void GenerateTextAsset()
        {
            Selection.activeObject = EZAssetGenerator.GenerateTextAsset();
        }

        [MenuItem(ROOT_NAME + "/EZBundle/EZBundle", false, 120)]
        private static void EZBundle()
        {
            Selection.activeObject = EZScriptableObject.Load<EZBundleObject>(EZBundleObject.AssetName);
        }
        [MenuItem(ROOT_NAME + "/EZBundle/EZBundleManager", false, 121)]
        private static void EZBundleManager()
        {
            EditorWindow.GetWindow<EZBundleManager>("EZBundleManager").Show();
        }
        [MenuItem(ROOT_NAME + "/EZBundle/EZBundleViewer", false, 122)]
        private static void EZBundleViewer()
        {
            EditorWindow.GetWindow<EZBundleViewer>("EZBundleViewer").Show();
        }
    }
}