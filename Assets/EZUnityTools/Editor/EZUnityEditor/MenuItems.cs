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

        [MenuItem(ROOT_NAME + "/EZScriptTemplate", priority = 1)]
        private static void EZScriptTemplate()
        {
            EditorWindow.GetWindow<EZScriptTemplateEditorWindow>("EZScriptTemplate").Show();
        }

        [MenuItem(ROOT_NAME + "/EZKeystore", priority = 2)]
        private static void EZKeystore()
        {
            Selection.activeObject = EZScriptableObject.Load<EZKeystoreObject>(EZKeystoreObject.AssetName);
        }

        [MenuItem(ROOT_NAME + "/EZBundle", priority = 3)]
        private static void EZBundle()
        {
            Selection.activeObject = EZScriptableObject.Load<EZBundleObject>(EZBundleObject.AssetName);
        }

        [MenuItem(ROOT_NAME + "/EZRename", priority = 4)]
        private static void EZRename()
        {
            EditorWindow.GetWindow<EZRenameEditorWindow>("EZRename").Show();
        }

        [MenuItem(ROOT_NAME + "/EZAssetGenerator/Material", priority = 6)]
        private static void GenerateMaterial()
        {
            Selection.activeObject = EZAssetGenerator.GenerateMaterial();
        }
        [MenuItem(ROOT_NAME + "/EZAssetGenerator/TextAsset", priority = 7)]
        private static void GenerateTextAsset()
        {
            Selection.activeObject = EZAssetGenerator.GenerateTextAsset();
        }

        [MenuItem(ROOT_NAME + "/EZProjectSettings/Include Built-in Shaders", priority = 9)]
        private static void IncludeBuiltinShaders()
        {
            EZGraphicsSettings.IncludeBuiltinShaders();
        }
    }
}