/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-01-19 14:03:26
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity.Framework
{
    [CustomEditor(typeof(EZApplicationSettings))]
    public class EZApplicationSettingsEditor : Editor
    {
        SerializedProperty m_RunModeInEditor;
        SerializedProperty m_RunModeInApp;

        SerializedProperty m_SleepTimeout;
        SerializedProperty m_TargetFrameRate;

        SerializedProperty m_AndroidServer;
        SerializedProperty m_IOSServer;
        SerializedProperty m_DefaultServer;
        SerializedProperty m_FileListName;
        SerializedProperty m_IgnorePrefix;
        SerializedProperty m_IgnoreSuffix;

        SerializedProperty m_LuaFolders;
        SerializedProperty m_LuaBundles;
        ReorderableList luaFolderList;
        ReorderableList luaBundleList;

        float height = EditorGUIUtility.singleLineHeight;

        void OnEnable()
        {
            m_RunModeInEditor = serializedObject.FindProperty("m_RunModeInEditor");
            m_RunModeInApp = serializedObject.FindProperty("m_RunModeInApp");

            m_SleepTimeout = serializedObject.FindProperty("m_SleepTimeout");
            m_TargetFrameRate = serializedObject.FindProperty("m_TargetFrameRate");

            m_AndroidServer = serializedObject.FindProperty("m_AndroidServer");
            m_IOSServer = serializedObject.FindProperty("m_IOSServer");
            m_DefaultServer = serializedObject.FindProperty("m_DefaultServer");
            m_FileListName = serializedObject.FindProperty("m_FileListName");
            m_IgnorePrefix = serializedObject.FindProperty("m_IgnorePrefix");
            m_IgnoreSuffix = serializedObject.FindProperty("m_IgnoreSuffix");

            m_LuaFolders = serializedObject.FindProperty("m_LuaFolders");
            m_LuaBundles = serializedObject.FindProperty("m_LuaBundles");
            luaFolderList = new ReorderableList(serializedObject, m_LuaFolders, true, true, true, true)
            {
                drawHeaderCallback = DrawLuaFolderListHeader,
                drawElementCallback = DrawLuaFolderListElement
            };
            luaBundleList = new ReorderableList(serializedObject, m_LuaBundles, true, true, true, true)
            {
                drawHeaderCallback = DrawLuaBundleListHeader,
                drawElementCallback = DrawLuaBundleListElement
            };
        }

        private void DrawLuaFolderListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Lua Folders");
        }
        private void DrawLuaFolderListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_LuaFolders, index);
            SerializedProperty dir = m_LuaFolders.GetArrayElementAtIndex(index);
            rect.y += 1;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, height), dir, GUIContent.none);
        }

        private void DrawLuaBundleListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Lua Bundles");
        }
        private void DrawLuaBundleListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_LuaBundles, index);
            SerializedProperty bundle = m_LuaBundles.GetArrayElementAtIndex(index);
            rect.y += 1;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, height), bundle, GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(target as ScriptableObject), typeof(MonoScript), false);
            GUI.enabled = true;

            EditorGUILayout.Space(); EditorGUILayout.LabelField("Mode", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_RunModeInEditor);
            EditorGUILayout.PropertyField(m_RunModeInApp);
            EditorGUILayout.HelpBox("Develop Mode is only available in Editor", MessageType.Info);
            if (m_RunModeInApp.enumValueIndex == (int)RunMode.Develop) m_RunModeInApp.enumValueIndex = (int)RunMode.Package;

            EditorGUILayout.Space(); EditorGUILayout.LabelField("Quality", EditorStyles.boldLabel);
            m_SleepTimeout.intValue = EditorGUILayout.IntPopup("Sleep Timeout", Screen.sleepTimeout,
                new string[] { "Never Sleep", "System Setting" },
                new int[] { SleepTimeout.NeverSleep, SleepTimeout.SystemSetting }
                );
            PlayerSettings.runInBackground = EditorGUILayout.Toggle("Run In Background", Application.runInBackground);
            QualitySettings.vSyncCount = EditorGUILayout.IntSlider("V Sync Count", QualitySettings.vSyncCount, 0, 4);
            if (QualitySettings.vSyncCount == 0)
            {
                EditorGUILayout.PropertyField(m_TargetFrameRate);
            }
            else
            {
                EditorGUILayout.HelpBox("If the 'QualitySettings.vSyncCount' property is set, the 'Application.targetFrameRate' will be ignored", MessageType.Info);
            }

            EditorGUILayout.Space(); EditorGUILayout.LabelField("Update", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_AndroidServer);
            EditorGUILayout.PropertyField(m_IOSServer);
            EditorGUILayout.PropertyField(m_DefaultServer);
            EditorGUILayout.PropertyField(m_FileListName);
            EditorGUILayout.PropertyField(m_IgnorePrefix);
            EditorGUILayout.PropertyField(m_IgnoreSuffix);
#if XLUA
            EditorGUILayout.Space(); EditorGUILayout.LabelField("Lua", EditorStyles.boldLabel);
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 30;
            luaFolderList.DoLayoutList();
            luaBundleList.DoLayoutList();
            EditorGUIUtility.labelWidth = labelWidth;
#endif
            serializedObject.ApplyModifiedProperties();
        }
    }
}