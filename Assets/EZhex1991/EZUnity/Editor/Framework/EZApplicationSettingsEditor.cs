/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-01-19 14:03:26
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.Framework
{
    [CustomEditor(typeof(EZApplicationSettings))]
    public class EZApplicationSettingsEditor : Editor
    {
        SerializedProperty m_PackageModeInEditor;
        SerializedProperty m_PackageModeInApp;

        SerializedProperty m_SleepTimeout;
        SerializedProperty m_TargetFrameRate;

        SerializedProperty m_AndroidServer;
        SerializedProperty m_IOSServer;
        SerializedProperty m_DefaultServer;
        SerializedProperty m_FileListName;
        SerializedProperty m_IgnorePrefix;
        SerializedProperty m_IgnoreSuffix;

        private void OnEnable()
        {
            m_PackageModeInEditor = serializedObject.FindProperty("m_PackageModeInEditor");
            m_PackageModeInApp = serializedObject.FindProperty("m_PackageModeInApp");

            m_SleepTimeout = serializedObject.FindProperty("m_SleepTimeout");
            m_TargetFrameRate = serializedObject.FindProperty("m_TargetFrameRate");

            m_AndroidServer = serializedObject.FindProperty("m_AndroidServer");
            m_IOSServer = serializedObject.FindProperty("m_IOSServer");
            m_DefaultServer = serializedObject.FindProperty("m_DefaultServer");
            m_FileListName = serializedObject.FindProperty("m_FileListName");
            m_IgnorePrefix = serializedObject.FindProperty("m_IgnorePrefix");
            m_IgnoreSuffix = serializedObject.FindProperty("m_IgnoreSuffix");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(target as ScriptableObject), typeof(MonoScript), false);
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mode", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_PackageModeInEditor);
            EditorGUILayout.PropertyField(m_PackageModeInApp);
            EditorGUILayout.HelpBox("Develop Mode is only available in Editor", MessageType.Info);
            if (m_PackageModeInApp.enumValueIndex == (int)PackageMode.Develop) m_PackageModeInApp.enumValueIndex = (int)PackageMode.Local;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Quality", EditorStyles.boldLabel);
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

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Update", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_AndroidServer);
            EditorGUILayout.PropertyField(m_IOSServer);
            EditorGUILayout.PropertyField(m_DefaultServer);
            EditorGUILayout.PropertyField(m_FileListName);
            EditorGUILayout.PropertyField(m_IgnorePrefix);
            EditorGUILayout.PropertyField(m_IgnoreSuffix);
            serializedObject.ApplyModifiedProperties();
        }
    }
}