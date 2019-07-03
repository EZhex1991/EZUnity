/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-02-09 12:05:23
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.Builder
{
    public class EZBundleManager : EditorWindow
    {
        public class BundleInfo
        {
            public string bundleName;
            public string[] assetPaths;
            public Object[] assetObjects;
            public string[] dependencies;
            public string[] recursiveDependencies;
            public BundleInfo(string bundleName, string[] assetPaths, string[] dependencies, string[] recursiveDependencies)
            {
                this.bundleName = bundleName;
                this.assetPaths = assetPaths;
                this.assetObjects = new Object[assetPaths.Length];
                for (int i = 0; i < assetPaths.Length; i++)
                {
                    this.assetObjects[i] = AssetDatabase.LoadAssetAtPath(assetPaths[i], typeof(Object));
                }
                this.dependencies = dependencies;
                this.recursiveDependencies = recursiveDependencies;
            }
        }
        private static List<BundleInfo> bundleList = new List<BundleInfo>();

        private AssetsViewOption showAssets = AssetsViewOption.Object;
        private BundleDependenciesViewOption showDependencies = BundleDependenciesViewOption.Recursive;
        private Vector2 scrollPosition;

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);
            showAssets = (AssetsViewOption)EditorGUILayout.EnumPopup("Show Assets", showAssets);
            showDependencies = (BundleDependenciesViewOption)EditorGUILayout.EnumPopup("Show Dependencies", showDependencies);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DrawAssetBundleManager(showAssets, showDependencies);
            EditorGUILayout.EndScrollView();
        }
        protected void OnFocus()
        {
            Refresh();
            Repaint();
        }

        public static void DrawAssetBundleManager(AssetsViewOption showAssets, BundleDependenciesViewOption showDependencies)
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
                switch (showAssets)
                {
                    case AssetsViewOption.Object:
                        DrawAssetCollection("", bundleInfo.assetObjects);
                        break;
                    case AssetsViewOption.Path:
                        DrawStringCollection("", bundleInfo.assetPaths);
                        break;
                    case AssetsViewOption.PathAndObject:
                        DrawAssetList("", bundleInfo.assetPaths, bundleInfo.assetObjects);
                        break;
                }
                switch (showDependencies)
                {
                    case BundleDependenciesViewOption.DontShow:
                        break;
                    case BundleDependenciesViewOption.Direct:
                        DrawStringCollection("Dependencies", bundleInfo.dependencies);
                        break;
                    case BundleDependenciesViewOption.Recursive:
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
                BundleInfo bundleInfo = new BundleInfo(bundleName, AssetDatabase.GetAssetPathsFromAssetBundle(bundleName),
                    AssetDatabase.GetAssetBundleDependencies(bundleName, false),
                    AssetDatabase.GetAssetBundleDependencies(bundleName, true));
                bundleList.Add(bundleInfo);
            }
        }
        public static void DrawAssetList(string title, string[] stringList, Object[] objectList)
        {
            if (stringList != null && stringList.Length > 0)
            {
                if (!string.IsNullOrEmpty(title)) EditorGUILayout.LabelField(title);
                EditorGUI.indentLevel++;
                GUI.enabled = false;
                for (int i = 0; i < stringList.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.TextField(stringList[i]);
                    EditorGUILayout.ObjectField(objectList[i], typeof(Object), false);
                    EditorGUILayout.EndHorizontal();
                }
                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }
        }
        public static void DrawAssetCollection(string title, ICollection<Object> objectCollection)
        {
            if (objectCollection != null && objectCollection.Count > 0)
            {
                if (!string.IsNullOrEmpty(title)) EditorGUILayout.LabelField(title);
                EditorGUI.indentLevel++;
                GUI.enabled = false;
                foreach (Object asset in objectCollection)
                {
                    EditorGUILayout.ObjectField(asset, typeof(Object), false);
                }
                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }
        }
        public static void DrawStringCollection(string title, ICollection<string> stringCollection)
        {
            if (stringCollection != null && stringCollection.Count > 0)
            {
                if (!string.IsNullOrEmpty(title)) EditorGUILayout.LabelField(title);
                EditorGUI.indentLevel++;
                foreach (string text in stringCollection)
                {
                    EditorGUILayout.TextField(text);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}