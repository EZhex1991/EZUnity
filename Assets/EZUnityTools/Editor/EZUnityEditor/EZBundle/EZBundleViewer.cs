/*
 * Author:      熊哲
 * CreateTime:  2/7/2018 6:41:39 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using YamlDotNet.Serialization;
using UObject = UnityEngine.Object;

namespace EZUnityEditor
{
    public class EZBundleViewer : EZEditorWindow
    {
        public class Bundle
        {
            public string bundleName;
            public bool foldout = true;
            public Manifest manifest;
            public Bundle(string bundleName, Manifest manifest)
            {
                this.bundleName = bundleName;
                this.manifest = manifest;
            }
        }

        Deserializer deserializer = new DeserializerBuilder().Build();
        Dictionary<string, Bundle> bundleDict = new Dictionary<string, Bundle>();
        Vector2 scrollPosition;
        bool showDependencies = true;
        bool forceLoad = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            Refresh();
        }
        protected override void OnSelectionChange()
        {
            base.OnSelectionChange();
            Refresh();
        }
        private void Refresh()
        {
            bundleDict.Clear();
            foreach (UObject obj in Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets))
            {
                string objPath = AssetDatabase.GetAssetPath(obj);
                if (File.Exists(objPath + ".manifest")) objPath += ".manifest";
                if (objPath.EndsWith(".manifest"))
                {
                    StreamReader input = File.OpenText(objPath);
                    Manifest manifest = deserializer.Deserialize<Manifest>(input);
                    input.Close();
                    string bundleName = objPath.Replace(".manifest", "");
                    if (!bundleDict.ContainsKey(bundleName))
                        bundleDict.Add(bundleName, new Bundle(bundleName, manifest));
                }
                else if (forceLoad)
                {
                    AssetBundle bundle = AssetBundle.LoadFromFile(objPath);
                    if (bundle != null)
                    {
                        Manifest manifest = new Manifest() { Assets = new List<string>() };
                        manifest.Assets.AddRange(bundle.GetAllAssetNames());
                        manifest.Assets.AddRange(bundle.GetAllScenePaths());
                        bundle.Unload(false);
                        if (!bundleDict.ContainsKey(objPath))
                            bundleDict.Add(objPath, new Bundle(objPath, manifest));
                    }
                }
            }
            Repaint();
        }

        private void SetFoldout(bool foldout)
        {
            foreach (Bundle bundle in bundleDict.Values)
            {
                bundle.foldout = foldout;
            }
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            DrawOptionControllers();
            DrawCollapseControllers();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (Bundle bundle in bundleDict.Values)
            {
                if (bundle.foldout = EditorGUILayout.Foldout(bundle.foldout, bundle.bundleName))
                {
                    EditorGUI.indentLevel++;
                    EZBundleManager.DrawStringCollection(showDependencies ? "Assets" : "", bundle.manifest.Assets);
                    if (showDependencies) EZBundleManager.DrawStringCollection("Dependencies", bundle.manifest.Dependencies);
                    if (bundle.manifest.AssetBundleManifest != null && bundle.manifest.AssetBundleManifest.AssetBundleInfos.Count > 0)
                    {
                        foreach (AssetBundleInfo abInfo in bundle.manifest.AssetBundleManifest.AssetBundleInfos.Values)
                        {
                            EditorGUILayout.LabelField(abInfo.Name);
                            if (showDependencies) EZBundleManager.DrawStringCollection("", abInfo.Dependencies.Values);
                        }
                    }
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndScrollView();
        }
        private void DrawCollapseControllers()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Collapse"))
            {
                SetFoldout(false);
            }
            if (GUILayout.Button("Expand"))
            {
                SetFoldout(true);
            }
            EditorGUILayout.EndHorizontal();
        }
        private void DrawOptionControllers()
        {
            EditorGUILayout.BeginHorizontal();
            forceLoad = EditorGUILayout.ToggleLeft("Force Load", forceLoad);
            showDependencies = EditorGUILayout.ToggleLeft("Show Dependencies", showDependencies);
            EditorGUILayout.EndHorizontal();
        }

        // Structure of bundle.manifest
        public class Manifest
        {
            public uint ManifestFileVersion { get; set; }
            public long CRC { get; set; }
            // bundle
            public HashesInfo Hashes { get; set; }
            public uint HashAppended { get; set; }
            public List<ClassTypeInfo> ClassTypes { get; set; }
            public List<string> Assets { get; set; }
            public List<string> Dependencies { get; set; }
            // Main Manifest
            public MainManifestInfo AssetBundleManifest { get; set; }
        }
        public class HashesInfo
        {
            public HashInfo AssetFileHash { get; set; }
            public HashInfo TypeTreeHash { get; set; }
        }
        public class HashInfo
        {
            [YamlMember(Alias = "serializedVersion", ApplyNamingConventions = false)]
            public uint SerializedVersion { get; set; }
            public string Hash { get; set; }
        }
        public class ClassTypeInfo
        {
            public int Class { get; set; }
            public ScriptInfo Script { get; set; }
        }
        public class ScriptInfo
        {
            public long instanceID { get; set; }
            public long fileID { get; set; }
            public string guid { get; set; }
            public uint type { get; set; }
        }
        public class MainManifestInfo
        {
            public Dictionary<string, AssetBundleInfo> AssetBundleInfos { get; set; }
        }
        public class AssetBundleInfo
        {
            public string Name { get; set; }
            public Dictionary<string, string> Dependencies { get; set; }
        }
    }
}