/*
 * Author:      熊哲
 * CreateTime:  3/6/2017 2:13:28 PM
 * Description:
 * 
*/
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZUnityEditor
{
    public class EZBundleEditorWindow : EZEditorWindow
    {
        private EZBundleObject ezBundle;
        private SerializedObject so_EZBundle;
        private SerializedProperty bundleTarget;
        private SerializedProperty relativePath;
        private SerializedProperty removeOldFiles;
        private SerializedProperty bundleDirPath;
        private SerializedProperty bundleExtension;
        private SerializedProperty createListFile;
        private SerializedProperty listFileName;
        private ReorderableList copyList;
        private ReorderableList bundleList;

        private Vector2 scrollView;
        private bool copyListFoldout = true;
        private bool bundleListFoldout = true;
        private string saveName = "";

        protected override void OnFocus()
        {
            base.OnFocus();
            ezBundle = EZScriptableObject.Load<EZBundleObject>(EZBundleObject.AssetName);
            so_EZBundle = new SerializedObject(ezBundle);
            bundleTarget = so_EZBundle.FindProperty("bundleTarget");
            relativePath = so_EZBundle.FindProperty("relativePath");
            removeOldFiles = so_EZBundle.FindProperty("removeOldFiles");
            bundleDirPath = so_EZBundle.FindProperty("bundleDirPath");
            bundleExtension = so_EZBundle.FindProperty("bundleExtension");
            createListFile = so_EZBundle.FindProperty("createListFile");
            listFileName = so_EZBundle.FindProperty("listFileName");
            copyList = new ReorderableList(so_EZBundle, so_EZBundle.FindProperty("copyList"), true, true, true, true);
            bundleList = new ReorderableList(so_EZBundle, so_EZBundle.FindProperty("bundleList"), true, true, true, true);
            copyList.drawHeaderCallback = DrawCopyListHeader;
            copyList.drawElementCallback = DrawCopyListElement;
            bundleList.drawHeaderCallback = DrawBundleListHeader;
            bundleList.drawElementCallback = DrawBundleListElement;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            so_EZBundle.Update();
            DrawBaseInfo();
            scrollView = EditorGUILayout.BeginScrollView(scrollView);
            copyListFoldout = EditorGUILayout.Foldout(copyListFoldout, "Copy List");
            if (copyListFoldout) copyList.DoLayoutList();
            bundleListFoldout = EditorGUILayout.Foldout(bundleListFoldout, "Bundle List");
            if (bundleListFoldout) bundleList.DoLayoutList();
            EditorGUILayout.EndScrollView();
            DrawButton();
            so_EZBundle.ApplyModifiedProperties();
        }

        private void DrawBaseInfo()
        {
            EditorGUILayout.PropertyField(bundleTarget);
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(relativePath);
                EditorGUILayout.PropertyField(removeOldFiles);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Bundle Directory", new GUILayoutOption[] { GUILayout.Width(150), });
                if (ezBundle.relativePath) EditorGUILayout.LabelField("Assets/", new GUILayoutOption[] { GUILayout.Width(50), });
                EditorGUILayout.PropertyField(bundleDirPath, GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(bundleExtension);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(createListFile);
                if (ezBundle.createListFile) EditorGUILayout.PropertyField(listFileName);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();
        }

        private void DrawCopyListHeader(Rect rect)
        {
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 35, EditorGUIUtility.singleLineHeight), "NO.");
            EditorGUI.LabelField(new Rect(rect.x + 40, rect.y, 120, EditorGUIUtility.singleLineHeight), "Destination");
            EditorGUI.LabelField(new Rect(rect.x + 165, rect.y, 100, EditorGUIUtility.singleLineHeight), "File Pattern");
            EditorGUI.LabelField(new Rect(rect.x + 270, rect.y, 100, EditorGUIUtility.singleLineHeight), "Search Option");
            EditorGUI.LabelField(new Rect(rect.x + 375, rect.y, rect.width - 375, EditorGUIUtility.singleLineHeight), "Source");

        }
        private void DrawCopyListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            SerializedProperty copyInfo = copyList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight), index.ToString("00"));
            EditorGUI.PropertyField(new Rect(rect.x + 25, rect.y, 120, EditorGUIUtility.singleLineHeight), copyInfo.FindPropertyRelative("destDirPath"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 150, rect.y, 100, EditorGUIUtility.singleLineHeight), copyInfo.FindPropertyRelative("filePattern"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 255, rect.y, 100, EditorGUIUtility.singleLineHeight), copyInfo.FindPropertyRelative("searchOption"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 360, rect.y, rect.width - 360, EditorGUIUtility.singleLineHeight), copyInfo.FindPropertyRelative("sourDirPath"), GUIContent.none);
        }

        private void DrawBundleListHeader(Rect rect)
        {
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 35, EditorGUIUtility.singleLineHeight), "NO.");
            EditorGUI.LabelField(new Rect(rect.x + 40, rect.y, 120, EditorGUIUtility.singleLineHeight), "Bundle Name");
            EditorGUI.LabelField(new Rect(rect.x + 165, rect.y, 100, EditorGUIUtility.singleLineHeight), "File Pattern");
            EditorGUI.LabelField(new Rect(rect.x + 270, rect.y, 100, EditorGUIUtility.singleLineHeight), "Search Option");
            EditorGUI.LabelField(new Rect(rect.x + 375, rect.y, rect.width - 380, EditorGUIUtility.singleLineHeight), "Directory");
        }
        private void DrawBundleListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            SerializedProperty bundleInfo = bundleList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight), index.ToString("00"));
            EditorGUI.PropertyField(new Rect(rect.x + 25, rect.y, 120, EditorGUIUtility.singleLineHeight), bundleInfo.FindPropertyRelative("bundleName"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 150, rect.y, 100, EditorGUIUtility.singleLineHeight), bundleInfo.FindPropertyRelative("filePattern"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 255, rect.y, 100, EditorGUIUtility.singleLineHeight), bundleInfo.FindPropertyRelative("searchOption"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 360, rect.y, rect.width - 360, EditorGUIUtility.singleLineHeight), bundleInfo.FindPropertyRelative("dirPath"), GUIContent.none);
        }

        private void DrawButton()
        {
            if (GUILayout.Button("Build Bundle"))
            {
                EZBundleBuilder.BuildBundle(ezBundle);
            }
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save As"))
                {
                    if (saveName != "" && saveName != EZBundleObject.AssetName)
                        EZScriptableObject.Create(saveName, Object.Instantiate(ezBundle));
                }
                saveName = EditorGUILayout.TextField(saveName);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}