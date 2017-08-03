/*
 * Author:      熊哲
 * CreateTime:  2/14/2017 3:58:53 PM
 * Description:
 * 
*/
using UnityEditor;
using UnityEngine;

namespace EZUnityTools.EZEditor
{
    public class MenuItems
    {
        private const string ROOT_NAME = "EZUnityTools";

        [MenuItem(ROOT_NAME + "/EZScriptTemplate", priority = 1)]
        private static void EZScriptTemplate()
        {
            EditorWindow.GetWindow<EZScriptTemplateEditorWindow>("EZScriptTemplate").Show();
        }

        [MenuItem(ROOT_NAME + "/EZKeystore", priority = 2)]
        private static void EZKeystore()
        {
            EditorWindow.GetWindow<EZKeystoreEditorWindow>("EZKeystore").Show();
        }

        [MenuItem(ROOT_NAME + "/EZBundle", priority = 3)]
        private static void EZBundle()
        {
            EditorWindow.GetWindow<EZBundleEditorWindow>("EZBundle").Show();
        }

        [MenuItem(ROOT_NAME + "/EZRename", priority = 4)]
        private static void EZRename()
        {
            EditorWindow.GetWindow<EZRenameEditorWindow>("EZRename").Show();
        }
        
        [MenuItem(ROOT_NAME + "/EZAsset Generator/Material", priority = 6)]
        private static void GenerateMaterial()
        {
            EZAssetGenerator.GenerateMaterial();
        }
        [MenuItem(ROOT_NAME + "/EZAsset Generator/TextAsset", priority = 7)]
        private static void GenerateTextAsset()
        {
            EZAssetGenerator.GenerateTextAsset();
        }
    }
}