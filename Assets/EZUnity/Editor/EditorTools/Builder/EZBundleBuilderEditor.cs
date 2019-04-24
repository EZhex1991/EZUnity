/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-03-08 19:42:27
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZUnity
{
    [CustomEditor(typeof(EZBundleBuilder))]
    public class EZBundleBuilderEditor : Editor
    {
        private EZBundleBuilder bundleBuilder;

        private SerializedProperty m_OutputPath;
        private SerializedProperty m_ListFileName;
        private SerializedProperty m_ManagerMode;
        private SerializedProperty m_ForceRebuild;
        private SerializedProperty m_CopyList;
        private SerializedProperty m_BundleList;

        private SerializedProperty m_CopyListFoldout;
        private SerializedProperty m_BundleListFoldout;
        private SerializedProperty m_ShowAssets;
        private SerializedProperty m_ShowDependencies;

        private ReorderableList copyList;
        private ReorderableList bundleList;

        float space = EZEditorGUIUtility.space;
        float lineHeight = EditorGUIUtility.singleLineHeight;

        void OnEnable()
        {
            bundleBuilder = target as EZBundleBuilder;
            m_OutputPath = serializedObject.FindProperty("outputPath");
            m_ListFileName = serializedObject.FindProperty("listFileName");
            m_ManagerMode = serializedObject.FindProperty("managerMode");
            m_ForceRebuild = serializedObject.FindProperty("forceRebuild");
            m_CopyList = serializedObject.FindProperty("copyList");
            m_BundleList = serializedObject.FindProperty("bundleList");
            m_CopyListFoldout = serializedObject.FindProperty("copyListFoldout");
            m_BundleListFoldout = serializedObject.FindProperty("bundleListFoldout");
            m_ShowAssets = serializedObject.FindProperty("showAssets");
            m_ShowDependencies = serializedObject.FindProperty("showDependencies");
            copyList = new ReorderableList(serializedObject, m_CopyList, true, true, true, true);
            copyList.drawHeaderCallback = DrawCopyListHeader;
            copyList.drawElementCallback = DrawCopyListElement;
            bundleList = new ReorderableList(serializedObject, m_BundleList, true, true, true, true);
            bundleList.drawHeaderCallback = DrawBundleListHeader;
            bundleList.drawElementCallback = DrawBundleListElement;
            EZBundleManager.Refresh();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, !serializedObject.isEditingMultipleObjects);

            if (!serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.LabelField("Build", EditorStyles.boldLabel);
                DrawBuildButtons();
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField("Build Options", EditorStyles.boldLabel);
            DrawBaseProperties();

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

        private void DrawBuildButtons()
        {
            if (GUILayout.Button("Android"))
            {
                EditorApplication.delayCall += delegate () { bundleBuilder.Execute(BuildTarget.Android); };
            }
            if (GUILayout.Button("iOS"))
            {
                EditorApplication.delayCall += delegate () { bundleBuilder.Execute(BuildTarget.iOS); };
            }
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Windows"))
                {
                    EditorApplication.delayCall += delegate () { bundleBuilder.Execute(BuildTarget.StandaloneWindows); };
                }
                if (GUILayout.Button("Windows64"))
                {
                    EditorApplication.delayCall += delegate () { bundleBuilder.Execute(BuildTarget.StandaloneWindows64); };
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("OSX"))
            {
                EditorApplication.delayCall += delegate () { bundleBuilder.Execute(BuildTarget.StandaloneOSX); };
            }
        }

        private void DrawBaseProperties()
        {
            EditorGUILayout.PropertyField(m_OutputPath);
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
            EditorGUILayout.PropertyField(m_ShowAssets);
            EditorGUILayout.PropertyField(m_ShowDependencies);
            EZBundleManager.DrawAssetBundleManager((AssetsViewOption)m_ShowAssets.enumValueIndex, (BundleDependenciesViewOption)m_ShowDependencies.enumValueIndex);
        }
    }
}