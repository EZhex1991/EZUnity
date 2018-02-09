/*
 * Author:      熊哲
 * CreateTime:  2/9/2018 12:05:23 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZUnityEditor
{
    public class EZBundleManager : EZEditorWindow
    {
        private BundleDependenciesShowOption m_ShowDependencies = BundleDependenciesShowOption.Recursive;

        private Vector2 scrollPosition;

        protected override void OnGUI()
        {
            base.OnGUI();
            m_ShowDependencies = (BundleDependenciesShowOption)EditorGUILayout.EnumPopup("Show Dependencies", m_ShowDependencies);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DrawAssetBundleManager(m_ShowDependencies);
            EditorGUILayout.EndScrollView();
        }
        public static void DrawAssetBundleManager(BundleDependenciesShowOption showDependencies)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh"))
            {
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("Remove Unused AssetBundleNames"))
            {
                AssetDatabase.RemoveUnusedAssetBundleNames();
            }
            EditorGUILayout.EndHorizontal();
            foreach (string bundleName in AssetDatabase.GetAllAssetBundleNames())
            {
                EditorGUILayout.LabelField(bundleName);
                EditorGUI.indentLevel++;
                switch (showDependencies)
                {
                    case BundleDependenciesShowOption.DontShow:
                        DrawAssetCollection("", AssetDatabase.GetAssetPathsFromAssetBundle(bundleName));
                        break;
                    case BundleDependenciesShowOption.Direct:
                        DrawAssetCollection("Assets", AssetDatabase.GetAssetPathsFromAssetBundle(bundleName));
                        DrawStringCollection("Dependencies", AssetDatabase.GetAssetBundleDependencies(bundleName, false));
                        break;
                    case BundleDependenciesShowOption.Recursive:
                        DrawAssetCollection("Assets", AssetDatabase.GetAssetPathsFromAssetBundle(bundleName));
                        DrawStringCollection("Dependencies", AssetDatabase.GetAssetBundleDependencies(bundleName, true));
                        break;
                }
                EditorGUI.indentLevel--;
            }
        }
        public static void DrawAssetCollection(string title, ICollection<string> collection)
        {
            if (collection != null && collection.Count > 0)
            {
                if (!string.IsNullOrEmpty(title)) EditorGUILayout.LabelField(title);
                EditorGUI.indentLevel++;
                GUI.enabled = false;
                foreach (string assetPath in collection)
                {
                    Object assetObject = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
                    EditorGUILayout.ObjectField(assetObject, typeof(Object), false);
                }
                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }
        }
        public static void DrawStringCollection(string title, ICollection<string> collection)
        {
            if (collection != null && collection.Count > 0)
            {
                if (!string.IsNullOrEmpty(title)) EditorGUILayout.LabelField(title);
                EditorGUI.indentLevel++;
                foreach (string text in collection)
                {
                    EditorGUILayout.TextField(text);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}