/*
 * Author:      熊哲
 * CreateTime:  2/14/2017 3:58:53 PM
 * Description:
 * 
*/
using UnityEditor;

namespace EZUnityTools.EZEditor
{
    public class MenuItems
    {
        private const string ROOT_NAME = "EZUnityTools";

        [MenuItem(ROOT_NAME + "/EZRename")]
        private static void EZRename()
        {
            EditorWindow.GetWindow<EZRenameEditorWindow>("EZRename").Show();
        }

        [MenuItem(ROOT_NAME + "/EZScript")]
        private static void EZScript()
        {
            EditorWindow.GetWindow<EZScriptEditorWindow>("EZScript").Show();
        }

        [MenuItem(ROOT_NAME + "/EZBundle")]
        private static void EZBundle()
        {
            EditorWindow.GetWindow<EZBundleEditorWindow>("EZBundle").Show();
        }

        [MenuItem(ROOT_NAME + "/EZKeystore")]
        private static void EZKeystore()
        {
            EditorWindow.GetWindow<EZKeystoreEditorWindow>("EZKeystore").Show();
        }
    }
}