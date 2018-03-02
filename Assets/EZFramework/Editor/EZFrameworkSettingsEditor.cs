/*
 * Author:      熊哲
 * CreateTime:  1/19/2017 2:03:26 PM
 * Description:
 * 
*/
using EZFramework;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZFrameworkEditor
{
    [CustomEditor(typeof(EZFrameworkSettings))]
    public class EZFrameworkSettingsEditor : Editor
    {
        SerializedProperty m_RunModeInEditor;
        SerializedProperty m_RunModeInApp;
        SerializedProperty m_SleepTimeout;
        SerializedProperty m_RunInBackground;
        SerializedProperty m_TargetFrameRate;
        SerializedProperty m_UpdateServer;
        SerializedProperty m_LuaDirList;
        SerializedProperty m_LuaBundleList;
        ReorderableList luaDirList;
        ReorderableList luaBundleList;

        float height = EditorGUIUtility.singleLineHeight;

        void OnEnable()
        {
            m_RunModeInEditor = serializedObject.FindProperty("m_RunModeInEditor");
            m_RunModeInApp = serializedObject.FindProperty("m_RunModeInApp");
            m_SleepTimeout = serializedObject.FindProperty("m_SleepTimeout");
            m_RunInBackground = serializedObject.FindProperty("m_RunInBackground");
            m_TargetFrameRate = serializedObject.FindProperty("m_TargetFrameRate");
            m_UpdateServer = serializedObject.FindProperty("m_UpdateServer");
            m_LuaDirList = serializedObject.FindProperty("m_LuaDirList");
            m_LuaBundleList = serializedObject.FindProperty("m_LuaBundleList");
            luaDirList = new ReorderableList(serializedObject, m_LuaDirList, true, true, true, true)
            {
                drawHeaderCallback = DrawLuaDirListHeader,
                drawElementCallback = DrawLuaDirListElement
            };
            luaBundleList = new ReorderableList(serializedObject, m_LuaBundleList, true, true, true, true)
            {
                drawHeaderCallback = DrawLuaBundleListHeader,
                drawElementCallback = DrawLuaBundleListElement
            };
        }

        private void DrawLuaDirListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Search Dir");
        }
        private void DrawLuaDirListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty dir = m_LuaDirList.GetArrayElementAtIndex(index);
            rect.y += 1;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, height), dir, new GUIContent(index.ToString()));

        }

        private void DrawLuaBundleListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Search Bundle");
        }
        private void DrawLuaBundleListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty bundle = m_LuaBundleList.GetArrayElementAtIndex(index);
            rect.y += 1;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, height), bundle, new GUIContent(index.ToString()));
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
            if (m_RunModeInApp.enumValueIndex == (int)RunMode.Develop) m_RunModeInApp.enumValueIndex = (int)RunMode.Local;
            EditorGUILayout.Space(); EditorGUILayout.LabelField("Quality", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_SleepTimeout);
            EditorGUILayout.PropertyField(m_RunInBackground);
            EditorGUILayout.PropertyField(m_TargetFrameRate);
            EditorGUILayout.Space(); EditorGUILayout.LabelField("Network", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_UpdateServer);
            EditorGUILayout.Space(); EditorGUILayout.LabelField("Lua", EditorStyles.boldLabel);
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 30;
            luaDirList.DoLayoutList();
            luaBundleList.DoLayoutList();
            EditorGUIUtility.labelWidth = labelWidth;
            serializedObject.ApplyModifiedProperties();
        }
    }
}