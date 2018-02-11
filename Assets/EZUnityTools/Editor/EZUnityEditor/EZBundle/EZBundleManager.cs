/*
 * Author:      熊哲
 * CreateTime:  2/9/2018 12:05:23 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EZUnityEditor
{
    public class EZBundleManager : EZEditorWindow
    {
        public class BundleInfo
        {
            public string bundleName;
            public Object[] assets;
            public string[] dependencies;
            public string[] recursiveDependencies;
            public BundleInfo(string bundleName, Object[] assets, string[] dependencies, string[] recursiveDependencies)
            {
                this.bundleName = bundleName;
                this.assets = assets;
                this.dependencies = dependencies;
                this.recursiveDependencies = recursiveDependencies;
            }
        }
        private static List<BundleInfo> bundleList = new List<BundleInfo>();

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
        protected override void OnFocus()
        {
            base.OnFocus();
            Refresh();
            Repaint();
        }

        public static void DrawAssetBundleManager(BundleDependenciesShowOption showDependencies)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh"))
            {
                Refresh();
            }
            if (GUILayout.Button("Remove Unused AssetBundleNames"))
            {
                AssetDatabase.RemoveUnusedAssetBundleNames();
                Refresh();
            }
            EditorGUILayout.EndHorizontal();
            foreach (BundleInfo bundleInfo in bundleList)
            {
                EditorGUILayout.LabelField(bundleInfo.bundleName);
                EditorGUI.indentLevel++;
                DrawAssetCollection("", bundleInfo.assets);
                switch (showDependencies)
                {
                    case BundleDependenciesShowOption.DontShow:
                        break;
                    case BundleDependenciesShowOption.Direct:
                        DrawStringCollection("Dependencies", bundleInfo.dependencies);
                        break;
                    case BundleDependenciesShowOption.Recursive:
                        DrawStringCollection("Dependencies", bundleInfo.recursiveDependencies);
                        break;
                }
                EditorGUI.indentLevel--;
            }
        }
        public static void Refresh()
        {
            bundleList.Clear();
            foreach (string bundleName in AssetDatabase.GetAllAssetBundleNames())
            {
                Object[] assets = (from assetPath in AssetDatabase.GetAssetPathsFromAssetBundle(bundleName)
                                   where true
                                   select AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object))).ToArray();
                BundleInfo bundleInfo = new BundleInfo(bundleName, assets,
                    AssetDatabase.GetAssetBundleDependencies(bundleName, false),
                    AssetDatabase.GetAssetBundleDependencies(bundleName, true));
                bundleList.Add(bundleInfo);
            }
        }
        public static void DrawAssetCollection(string title, ICollection<Object> collection)
        {
            if (collection != null && collection.Count > 0)
            {
                if (!string.IsNullOrEmpty(title)) EditorGUILayout.LabelField(title);
                EditorGUI.indentLevel++;
                GUI.enabled = false;
                foreach (Object asset in collection)
                {
                    EditorGUILayout.ObjectField(asset, typeof(Object), false);
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