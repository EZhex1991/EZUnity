/*
 * Author:      熊哲
 * CreateTime:  3/8/2017 7:42:27 PM
 * Description:
 * 
*/
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZUnityEditor
{
    [CustomEditor(typeof(EZBundleObject))]
    public class EZBundleEditor : Editor
    {
        private SerializedProperty m_BundleTarget;
        private SerializedProperty m_RelativePath;
        private SerializedProperty m_RemoveOldFiles;
        private SerializedProperty m_BundleDirPath;
        private SerializedProperty m_BundleExtension;
        private SerializedProperty m_ListFileName;
        private SerializedProperty m_CopyList;
        private SerializedProperty m_BundleList;

        private ReorderableList copyList;
        private ReorderableList bundleList;
        private bool copyListFoldout = true;
        private bool bundleListFoldout = true;
        private string saveAsName;
        private Vector2 scrollPosition;

        float height = EditorGUIUtility.singleLineHeight;

        void OnEnable()
        {
            m_BundleTarget = serializedObject.FindProperty("bundleTarget");
            m_BundleDirPath = serializedObject.FindProperty("bundleDirPath");
            m_BundleExtension = serializedObject.FindProperty("bundleExtension");
            m_ListFileName = serializedObject.FindProperty("listFileName");
            m_RemoveOldFiles = serializedObject.FindProperty("removeOldFiles");
            m_CopyList = serializedObject.FindProperty("copyList");
            m_BundleList = serializedObject.FindProperty("bundleList");
            copyList = new ReorderableList(serializedObject, m_CopyList, true, true, true, true);
            copyList.drawHeaderCallback = DrawCopyListHeader;
            copyList.drawElementCallback = DrawCopyListElement;
            bundleList = new ReorderableList(serializedObject, m_BundleList, true, true, true, true);
            bundleList.drawHeaderCallback = DrawBundleListHeader;
            bundleList.drawElementCallback = DrawBundleListElement;
        }

        private void DrawCopyListHeader(Rect rect)
        {
            rect.x += 15; rect.y += 1; rect.width -= 15;
            float width = Mathf.Min(100, rect.width / 4); float residue = (rect.width - width * 4) / 10;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - 5, height), "Destination");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - 5, height), "File Pattern");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - 5, height), "Search Option");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - 5, height), "Source");
        }
        private void DrawCopyListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty m_CopyInfo = m_CopyList.GetArrayElementAtIndex(index);
            SerializedProperty m_DestDirPath = m_CopyInfo.FindPropertyRelative("destDirPath");
            SerializedProperty m_FilePattern = m_CopyInfo.FindPropertyRelative("filePattern");
            SerializedProperty m_SearchOption = m_CopyInfo.FindPropertyRelative("searchOption");
            SerializedProperty m_SrcDirPath = m_CopyInfo.FindPropertyRelative("sourDirPath");

            rect.y += 1; float width = Mathf.Min(100, rect.width / 4); float residue = (rect.width - width * 4) / 10;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - 5, height), m_DestDirPath, GUIContent.none);
            rect.x += width + residue;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - 5, height), m_FilePattern, GUIContent.none);
            rect.x += width + residue;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - 5, height), m_SearchOption, GUIContent.none);
            rect.x += width + residue;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue * 7 - 5, height), m_SrcDirPath, GUIContent.none);
        }

        private void DrawBundleListHeader(Rect rect)
        {
            rect.x += 15; rect.y += 1; rect.width -= 15;
            float width = Mathf.Min(100, rect.width / 4); float residue = (rect.width - width * 4) / 10;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue - 5, height), "Bundle Name");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue - 5, height), "File Pattern");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue - 5, height), "Search Option");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue * 7 - 5, height), "Source");
        }
        private void DrawBundleListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty m_BundleInfo = m_BundleList.GetArrayElementAtIndex(index);
            SerializedProperty m_BundleName = m_BundleInfo.FindPropertyRelative("bundleName");
            SerializedProperty m_FilePattern = m_BundleInfo.FindPropertyRelative("filePattern");
            SerializedProperty m_SearchOption = m_BundleInfo.FindPropertyRelative("searchOption");
            SerializedProperty m_SrcDirPath = m_BundleInfo.FindPropertyRelative("dirPath");

            rect.y += 1; float width = Mathf.Min(100, rect.width / 4); float residue = (rect.width - width * 4) / 10;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - 5, height), m_BundleName, GUIContent.none);
            rect.x += width + residue;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - 5, height), m_FilePattern, GUIContent.none);
            rect.x += width + residue;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - 5, height), m_SearchOption, GUIContent.none);
            rect.x += width + residue;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue * 7 - 5, height), m_SrcDirPath, GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(target as ScriptableObject), typeof(MonoScript), false);
            GUI.enabled = true;

            if (GUILayout.Button("Build Bundle"))
            {
                EZBundleBuilder.BuildBundle(target as EZBundleObject);
            }
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save As"))
                {
                    if (saveAsName == "")
                        EZScriptableObject.Create(EZBundleObject.AssetName, Object.Instantiate(target as EZBundleObject));
                    else
                        EZScriptableObject.Create(saveAsName, Object.Instantiate(target as EZBundleObject));
                }
                saveAsName = EditorGUILayout.TextField(saveAsName);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.PropertyField(m_BundleTarget);
            EditorGUILayout.PropertyField(m_BundleDirPath);
            EditorGUILayout.PropertyField(m_BundleExtension);
            EditorGUILayout.PropertyField(m_ListFileName);
            EditorGUILayout.PropertyField(m_RemoveOldFiles);

            EditorGUILayout.Space();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            copyListFoldout = EditorGUILayout.Foldout(copyListFoldout, string.Format("Copy List ({0})", copyList.count));
            if (copyListFoldout) copyList.DoLayoutList();
            bundleListFoldout = EditorGUILayout.Foldout(bundleListFoldout, string.Format("Bundle List ({0})", bundleList.count));
            if (bundleListFoldout) bundleList.DoLayoutList();
            EditorGUILayout.EndScrollView();

            serializedObject.ApplyModifiedProperties();
        }
    }
}