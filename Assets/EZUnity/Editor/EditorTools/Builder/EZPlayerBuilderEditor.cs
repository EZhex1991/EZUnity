/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-04 16:14:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZUnity.Builder
{
    [CustomEditor(typeof(EZPlayerBuilder)), CanEditMultipleObjects]
    public class EZPlayerBuilderEditor : Editor
    {
        protected EZPlayerBuilder playerBuilder;

        protected SerializedProperty m_ConfigButDontBuild;

        protected SerializedProperty m_BundleBuilder;

        protected SerializedProperty m_LocationPathName;
        protected SerializedProperty m_Scenes;

        protected SerializedProperty m_CompanyName;
        protected SerializedProperty m_ProductName;
        protected SerializedProperty m_BundleIdentifier;
        protected SerializedProperty m_BundleVersion;
        protected SerializedProperty m_BuildNumber;
        protected SerializedProperty m_Icon;

        private SerializedProperty m_CopyList;
        private ReorderableList copyList;

        protected void OnEnable()
        {
            playerBuilder = target as EZPlayerBuilder;
            m_ConfigButDontBuild = serializedObject.FindProperty("configButDontBuild");
            m_BundleBuilder = serializedObject.FindProperty("bundleBuilder");
            m_LocationPathName = serializedObject.FindProperty("locationPathName");
            m_Scenes = serializedObject.FindProperty("scenes");
            m_CompanyName = serializedObject.FindProperty("companyName");
            m_ProductName = serializedObject.FindProperty("productName");
            m_BundleIdentifier = serializedObject.FindProperty("bundleIdentifier");
            m_BundleVersion = serializedObject.FindProperty("bundleVersion");
            m_BuildNumber = serializedObject.FindProperty("buildNumber");
            m_Icon = serializedObject.FindProperty("icon");

            m_CopyList = serializedObject.FindProperty("copyList");
            copyList = new ReorderableList(serializedObject, m_CopyList, true, true, true, true);
            copyList.drawHeaderCallback = DrawCopyListHeader;
            copyList.drawElementCallback = DrawCopyListElement;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, !serializedObject.isEditingMultipleObjects);

            if (!serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.LabelField("Build", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_ConfigButDontBuild);
                DrawBuildButtons();
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField("Build Options", EditorStyles.boldLabel);
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_LocationPathName);
                if (GUILayout.Button("...", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(30) }))
                {
                    string path = EditorUtility.SaveFolderPanel("Choose Output Folder", "", "");
                    if (!string.IsNullOrEmpty(path)) m_LocationPathName.stringValue = path;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.PropertyField(m_BundleBuilder);
            EditorGUILayout.PropertyField(m_Scenes, true);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Player Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_CompanyName);
            EditorGUILayout.PropertyField(m_ProductName);
            EditorGUILayout.PropertyField(m_BundleIdentifier);
            EditorGUILayout.PropertyField(m_BundleVersion);
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_BuildNumber);
                if (GUILayout.Button("+", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(30) }))
                {
                    m_BuildNumber.intValue++;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.PropertyField(m_Icon);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Copy List", EditorStyles.boldLabel);
            copyList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
        public void DrawBuildButtons()
        {
            if (GUILayout.Button("Android"))
            {
                playerBuilder.Execute(BuildTargetGroup.Android, BuildTarget.Android);
                GUIUtility.ExitGUI();
            }
            if (GUILayout.Button("iOS"))
            {
                playerBuilder.Execute(BuildTargetGroup.iOS, BuildTarget.iOS);
                GUIUtility.ExitGUI();
            }
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Windows"))
                {
                    playerBuilder.Execute(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
                    GUIUtility.ExitGUI();
                }
                if (GUILayout.Button("Windows64"))
                {
                    playerBuilder.Execute(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
                    GUIUtility.ExitGUI();
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("OSX"))
            {
                playerBuilder.Execute(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
                GUIUtility.ExitGUI();
            }
        }

        private void DrawCopyListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.DrawReorderableListHeaderIndex(rect);
            rect.y += 1;
            float width = rect.width / 2; float margin = 5;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - margin, rect.height), "Destination");
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - margin, rect.height), "Source");
        }
        private void DrawCopyListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_CopyList, index);
            rect.y += 1; rect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty m_CopyInfo = m_CopyList.GetArrayElementAtIndex(index);
            SerializedProperty m_DestDirPath = m_CopyInfo.FindPropertyRelative("dstPath");
            SerializedProperty m_SrcDirPath = m_CopyInfo.FindPropertyRelative("srcPath");

            float width = rect.width / 2; float margin = 5;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - margin, rect.height), m_DestDirPath, GUIContent.none);
            rect.x += width;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - margin, rect.height), m_SrcDirPath, GUIContent.none);
        }
    }
}
