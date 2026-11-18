/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-18
 * Organization:    #ORGANIZATION#
 * Description:     Menu entries for EZEditorTools general tools.
 */
using UnityEditor;

namespace EZhex1991.EZEditorTools
{
    internal static class EZGeneralMenuItems
    {
        private const string Root = "EZEditorTools/";

        [MenuItem(Root + "Renamer", false, (int)EZMenuItemOrder.Renamer)]
        private static void ShowRenamer() => EditorWindow.GetWindow<EZRenamerWindow>("Renamer").Show();

        [MenuItem(Root + "Guid Generator", false, (int)EZMenuItemOrder.GuidGenerator)]
        private static void ShowGuidGenerator() => EditorWindow.GetWindow<EZGuidGenerator>("Guid Generator").Show();

        [MenuItem(Root + "Regex Tester", false, (int)EZMenuItemOrder.RegexTester)]
        private static void ShowRegexTester() => EditorWindow.GetWindow<EZRegexTester>("Regex Tester").Show();

        [MenuItem(Root + "Color Blender", false, (int)EZMenuItemOrder.ColorBlender)]
        private static void ShowColorBlender() => EditorWindow.GetWindow<EZColorBlender>("Color Blender").Show();

        [MenuItem(Root + "PlayerPrefs Editor", false, (int)EZMenuItemOrder.PlayerPrefsEditor)]
        private static void ShowPlayerPrefsEditor() => EditorWindow.GetWindow<EZPlayerPrefsEditor>("PlayerPrefs Editor").Show();

        [MenuItem(Root + "Property Path Viewer", false, (int)EZMenuItemOrder.PropertyPathViewer)]
        private static void ShowPropertyPathViewer() => EditorWindow.GetWindow<EZPropertyPathViewer>("PropertyPathViewer").Show();

        [MenuItem(Root + "Asset Reference Viewer", false, (int)EZMenuItemOrder.AssetReferenceViewer)]
        private static void ShowAssetReferenceViewer() => EditorWindow.GetWindow<EZAssetReferenceViewer>("Asset Reference Viewer").Show();

        [MenuItem(Root + "Font Reference Viewer", false, (int)EZMenuItemOrder.FontReferenceViewer)]
        private static void ShowFontReferenceViewer() => EditorWindow.GetWindow<EZFontReferenceViewer>("Font Reference Viewer").Show();

        [MenuItem(Root + "Corresponding Object Viewer", false, (int)EZMenuItemOrder.CorrespondingObjectViewer)]
        private static void ShowCorrespondingObjectViewer() => EditorWindow.GetWindow<EZCorrespondingObjectViewer>("Corresponding Object Viewer").Show();

        [MenuItem(Root + "Hierarchy Diff Checker", false, (int)EZMenuItemOrder.HierachyDiffChecker)]
        private static void ShowHierarchyDiffChecker() => EditorWindow.GetWindow<EZHierachyDiffChecker>("Hierarchy Diff Checker").Show();

        [MenuItem(Root + "Type Reflection Helper", false, (int)EZMenuItemOrder.TypeReflectionHelper)]
        private static void ShowTypeReflectionHelper() => EditorWindow.GetWindow<EZTypeReflectionHelper>("Type Reflection Helper").Show();

        [MenuItem(Root + "Animator Controller Combiner", false, (int)EZMenuItemOrder.AnimatorControllerCombiner)]
        private static void ShowAnimatorControllerCombiner() => EditorWindow.GetWindow<EZAnimatorControllerCombiner>("AnimatorControllerCombiner").Show();
    }
}
