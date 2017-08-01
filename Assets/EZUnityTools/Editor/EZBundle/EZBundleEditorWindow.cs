/*
 * Author:      熊哲
 * CreateTime:  3/6/2017 2:13:28 PM
 * Description:
 * 
*/
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnityTools.EZEditor
{
    public class EZBundleEditorWindow : EZEditorWindow
    {
        private EZBundleObject ezBundle;
        private Vector2 scrollView;
        private int bundleListSize = 0;
        private int copyListSize = 0;
        private string saveName = "";

        protected override void OnFocus()
        {
            base.OnFocus();
            ezBundle = EZScriptableObject.Load<EZBundleObject>(EZBundleObject.AssetName);
            bundleListSize = ezBundle.bundleList.Count;
            copyListSize = ezBundle.copyList.Count;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            GUI_BaseInfo();
            scrollView = EditorGUILayout.BeginScrollView(scrollView);
            GUI_CopyList();
            GUI_BundelList();
            EditorGUILayout.EndScrollView();
            GUI_Button();
            if (GUI.changed) EditorUtility.SetDirty(ezBundle);
        }

        private void GUI_BaseInfo()
        {
            ezBundle.bundleTarget = (BuildTarget)EditorGUILayout.EnumPopup("Bundle Target", ezBundle.bundleTarget);
            {
                EditorGUILayout.BeginHorizontal();
                ezBundle.relativePath = EditorGUILayout.Toggle("Relative Path", ezBundle.relativePath);
                ezBundle.removeOldFiles = EditorGUILayout.Toggle("Remove Old Files", ezBundle.removeOldFiles);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Bundle Directory", new GUILayoutOption[] { GUILayout.Width(150), });
                if (ezBundle.relativePath) EditorGUILayout.LabelField("Assets/", new GUILayoutOption[] { GUILayout.Width(50), });
                ezBundle.bundleDirPath = EditorGUILayout.TextField(ezBundle.bundleDirPath);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                ezBundle.bundleExtension = EditorGUILayout.TextField("Bundle Extension", ezBundle.bundleExtension);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                ezBundle.createListFile = EditorGUILayout.Toggle("Create List File", ezBundle.createListFile);
                if (ezBundle.createListFile) ezBundle.listFileName = EditorGUILayout.TextField("List File Name", ezBundle.listFileName);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();
        }
        private void GUI_CopyList()
        {
            EditorGUILayout.Space();
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Copy List", subtitleStyle, new GUILayoutOption[] { GUILayout.Width(100), });
                EditorGUILayout.LabelField("Size", new GUILayoutOption[] { GUILayout.Width(40), });
                copyListSize = EditorGUILayout.DelayedIntField(copyListSize); copyListSize = copyListSize < 0 ? 0 : copyListSize;
                EditorGUILayout.EndHorizontal();
            }
            while (copyListSize != ezBundle.copyList.Count)
            {
                if (copyListSize < ezBundle.copyList.Count) ezBundle.copyList.RemoveAt(copyListSize);
                else ezBundle.copyList.Add(new EZBundleObject.CopyInfo());
            }
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("", new GUILayoutOption[] { GUILayout.Width(20), });
                EditorGUILayout.LabelField("Destination", new GUILayoutOption[] { GUILayout.Width(120), });
                EditorGUILayout.LabelField("File Pattern", new GUILayoutOption[] { GUILayout.Width(100), });
                EditorGUILayout.LabelField("Search Option", new GUILayoutOption[] { GUILayout.Width(100), });
                EditorGUILayout.LabelField("Source Dir");
                EditorGUILayout.EndHorizontal();
            }
            for (int i = 0; i < ezBundle.copyList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(i.ToString(), new GUILayoutOption[] { GUILayout.Width(20), });
                ezBundle.copyList[i].destDirPath = EditorGUILayout.TextField(ezBundle.copyList[i].destDirPath, new GUILayoutOption[] { GUILayout.Width(120), });
                ezBundle.copyList[i].filePattern = EditorGUILayout.TextField(ezBundle.copyList[i].filePattern, new GUILayoutOption[] { GUILayout.Width(100), });
                ezBundle.copyList[i].searchOption = (SearchOption)EditorGUILayout.EnumPopup(ezBundle.copyList[i].searchOption, new GUILayoutOption[] { GUILayout.Width(100), });
                EditorGUILayout.LabelField("Assets/", new GUILayoutOption[] { GUILayout.Width(50), });
                ezBundle.copyList[i].sourDirPath = EditorGUILayout.TextField(ezBundle.copyList[i].sourDirPath);
                EditorGUILayout.EndHorizontal();
            }
        }
        private void GUI_BundelList()
        {
            EditorGUILayout.Space();
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Build List", subtitleStyle, new GUILayoutOption[] { GUILayout.Width(100), });
                EditorGUILayout.LabelField("Size", new GUILayoutOption[] { GUILayout.Width(40), });
                bundleListSize = EditorGUILayout.DelayedIntField(bundleListSize, new GUILayoutOption[] { GUILayout.Width(40), }); bundleListSize = bundleListSize < 0 ? 0 : bundleListSize;
                EditorGUILayout.EndHorizontal();
            }
            while (bundleListSize != ezBundle.bundleList.Count)
            {
                if (bundleListSize < ezBundle.bundleList.Count) ezBundle.bundleList.RemoveAt(bundleListSize);
                else ezBundle.bundleList.Add(new EZBundleObject.BundleInfo());
            }
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("", new GUILayoutOption[] { GUILayout.Width(20), });
                EditorGUILayout.LabelField("Bundle Name", new GUILayoutOption[] { GUILayout.Width(120), });
                EditorGUILayout.LabelField("File Pattern", new GUILayoutOption[] { GUILayout.Width(100), });
                EditorGUILayout.LabelField("Search Option", new GUILayoutOption[] { GUILayout.Width(100), });
                EditorGUILayout.LabelField("Directory Path");
                EditorGUILayout.EndHorizontal();
            }
            for (int i = 0; i < ezBundle.bundleList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(i.ToString(), new GUILayoutOption[] { GUILayout.Width(20), });
                ezBundle.bundleList[i].bundleName = EditorGUILayout.TextField(ezBundle.bundleList[i].bundleName, new GUILayoutOption[] { GUILayout.Width(120), }).ToLower();
                ezBundle.bundleList[i].filePattern = EditorGUILayout.TextField(ezBundle.bundleList[i].filePattern, new GUILayoutOption[] { GUILayout.Width(100), });
                ezBundle.bundleList[i].searchOption = (SearchOption)EditorGUILayout.EnumPopup(ezBundle.bundleList[i].searchOption, new GUILayoutOption[] { GUILayout.Width(100), });
                EditorGUILayout.LabelField("Assets/", new GUILayoutOption[] { GUILayout.Width(50), });
                ezBundle.bundleList[i].dirPath = EditorGUILayout.TextField(ezBundle.bundleList[i].dirPath);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();
        }
        private void GUI_Button()
        {
            if (GUILayout.Button("Build Bundle"))
            {
                EZBundleBuilder.BuildBundle(ezBundle);
            }
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save As"))
                {
                    if (saveName == "")
                        EZScriptableObject.Create(EZBundleObject.AssetName, Object.Instantiate(ezBundle));
                    else
                        EZScriptableObject.Create(saveName, Object.Instantiate(ezBundle));
                }
                saveName = EditorGUILayout.TextField(saveName);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}