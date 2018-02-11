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
        private SerializedProperty m_BuildTarget;
        private SerializedProperty m_OutputPath;
        private SerializedProperty m_FileExtension;
        private SerializedProperty m_ListFileName;
        private SerializedProperty m_ManagerMode;
        private SerializedProperty m_ForceRebuild;
        private SerializedProperty m_CopyList;
        private SerializedProperty m_BundleList;

        private SerializedProperty m_CopyListFoldout;
        private SerializedProperty m_BundleListFoldout;
        private SerializedProperty m_ShowDependencies;

        private ReorderableList copyList;
        private ReorderableList bundleList;
        private string saveAsName;

        float space = EZEditorGUIUtility.space;
        float lineHeight = EditorGUIUtility.singleLineHeight;

        void OnEnable()
        {
            m_BuildTarget = serializedObject.FindProperty("buildTarget");
            m_OutputPath = serializedObject.FindProperty("outputPath");
            m_FileExtension = serializedObject.FindProperty("fileExtension");
            m_ListFileName = serializedObject.FindProperty("listFileName");
            m_ManagerMode = serializedObject.FindProperty("managerMode");
            m_ForceRebuild = serializedObject.FindProperty("forceRebuild");
            m_CopyList = serializedObject.FindProperty("copyList");
            m_BundleList = serializedObject.FindProperty("bundleList");
            m_CopyListFoldout = serializedObject.FindProperty("copyListFoldout");
            m_BundleListFoldout = serializedObject.FindProperty("bundleListFoldout");
            m_ShowDependencies = serializedObject.FindProperty("showDependencies");
            copyList = new ReorderableList(serializedObject, m_CopyList, true, true, true, true);
            copyList.drawHeaderCallback = DrawCopyListHeader;
            copyList.drawElementCallback = DrawCopyListElement;
            bundleList = new ReorderableList(serializedObject, m_BundleList, true, true, true, true);
            bundleList.drawHeaderCallback = DrawBundleListHeader;
            bundleList.drawElementCallback = DrawBundleListElement;
        }
        void OnFocus()
        {
            EZBundleManager.Refresh();
            Repaint();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptTitle(target);

            DrawFunctionButtons();
            DrawBaseProperties();

            EditorGUILayout.Space();
            if (m_CopyListFoldout.boolValue = EditorGUILayout.Foldout(m_CopyListFoldout.boolValue, string.Format("Copy List ({0})", copyList.count)))
            {
                copyList.DoLayoutList();
            }

            if (m_ManagerMode.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                string label = string.Format("Bundle List ({0})", AssetDatabase.GetAllAssetBundleNames().Length);
                m_BundleListFoldout.boolValue = EditorGUILayout.Foldout(m_BundleListFoldout.boolValue, label);
                EditorGUILayout.EndHorizontal();
                if (m_BundleListFoldout.boolValue)
                    DrawAssetBundleManager();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                string label = string.Format("Bundle List ({0})", bundleList.count);
                m_BundleListFoldout.boolValue = EditorGUILayout.Foldout(m_BundleListFoldout.boolValue, label);
                EditorGUILayout.EndHorizontal();
                if (m_BundleListFoldout.boolValue)
                    bundleList.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawFunctionButtons()
        {
            if (GUILayout.Button("Build Bundle"))
            {
                EditorApplication.delayCall += delegate () { EZBundleBuilder.BuildBundle(target as EZBundleObject, m_ManagerMode.boolValue); };
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
        }
        private void DrawBaseProperties()
        {
            EditorGUILayout.PropertyField(m_BuildTarget);
            EditorGUILayout.PropertyField(m_OutputPath);
            EditorGUILayout.PropertyField(m_FileExtension);
            EditorGUILayout.PropertyField(m_ListFileName);
            EditorGUILayout.PropertyField(m_ForceRebuild);
            EditorGUILayout.PropertyField(m_ManagerMode);
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

        private void DrawAssetBundleManager()
        {
            EditorGUILayout.PropertyField(m_ShowDependencies);
            EZBundleManager.DrawAssetBundleManager((BundleDependenciesShowOption)m_ShowDependencies.enumValueIndex);
        }
    }
}