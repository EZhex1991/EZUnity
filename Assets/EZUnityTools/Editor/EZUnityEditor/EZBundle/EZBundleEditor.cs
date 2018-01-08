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

        float space = EZEditorGUIUtility.space;
        float lineHeight = EditorGUIUtility.singleLineHeight;

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
            rect.y += 1;
            rect = EZEditorGUIUtility.DrawReorderableListHeaderIndex(rect);
            float width = rect.width / 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - space, lineHeight), "Destination");
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - space, lineHeight), "Source");
        }
        private void DrawCopyListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_CopyList, index);

            SerializedProperty m_CopyInfo = m_CopyList.GetArrayElementAtIndex(index);
            SerializedProperty m_DestDirPath = m_CopyInfo.FindPropertyRelative("destDirPath");
            SerializedProperty m_SrcDirPath = m_CopyInfo.FindPropertyRelative("sourDirPath");

            float width = rect.width / 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - space, lineHeight), m_DestDirPath, GUIContent.none);
            rect.x += width;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - space, lineHeight), m_SrcDirPath, GUIContent.none);
        }

        private void DrawBundleListHeader(Rect rect)
        {
            rect.y += 1;
            rect = EZEditorGUIUtility.DrawReorderableListHeaderIndex(rect);
            float width = Mathf.Min(100, rect.width / 4); float residue = (rect.width - width * 4) / 10;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue - space, lineHeight), "Bundle Name");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue - space, lineHeight), "File Pattern");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue - space, lineHeight), "Search Option");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue * 7 - space, lineHeight), "Source");
        }
        private void DrawBundleListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_BundleList, index);

            SerializedProperty m_BundleInfo = m_BundleList.GetArrayElementAtIndex(index);
            SerializedProperty m_BundleName = m_BundleInfo.FindPropertyRelative("bundleName");
            SerializedProperty m_FilePattern = m_BundleInfo.FindPropertyRelative("filePattern");
            SerializedProperty m_SearchOption = m_BundleInfo.FindPropertyRelative("searchOption");
            SerializedProperty m_SrcDirPath = m_BundleInfo.FindPropertyRelative("dirPath");

            float width = Mathf.Min(100, rect.width / 4); float residue = (rect.width - width * 4) / 10;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - space, lineHeight), m_BundleName, GUIContent.none);
            rect.x += width + residue;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - space, lineHeight), m_FilePattern, GUIContent.none);
            rect.x += width + residue;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - space, lineHeight), m_SearchOption, GUIContent.none);
            rect.x += width + residue;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue * 7 - space, lineHeight), m_SrcDirPath, GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptTitle(target);

            if (GUILayout.Button("Build Bundle"))
            {
                EditorApplication.delayCall += delegate () { EZBundleBuilder.BuildBundle(target as EZBundleObject); };
            }
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save As"))
                {
                    if (!string.IsNullOrEmpty(saveAsName))
                        EZScriptableObject.Create(saveAsName, Instantiate(target as EZBundleObject));
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
            copyListFoldout = EditorGUILayout.Foldout(copyListFoldout, string.Format("Copy List ({0})", copyList.count));
            if (copyListFoldout) copyList.DoLayoutList();
            bundleListFoldout = EditorGUILayout.Foldout(bundleListFoldout, string.Format("Bundle List ({0})", bundleList.count));
            if (bundleListFoldout) bundleList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}