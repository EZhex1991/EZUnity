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
        SerializedProperty m_RunMode;
        SerializedProperty m_SleepTimeout;
        SerializedProperty m_RunInBackground;
        SerializedProperty m_TargetFrameRate;
        SerializedProperty m_UpdateServer;
        SerializedProperty m_BundleExtension;
        SerializedProperty m_LuaDirList;
        SerializedProperty m_LuaBundleList;
        ReorderableList luaDirList;
        ReorderableList luaBundleList;

        GUIStyle subtitleStyle;
        float height = EditorGUIUtility.singleLineHeight;

        void OnEnable()
        {
            subtitleStyle = new GUIStyle(GUIStyle.none) { fontStyle = FontStyle.Bold };
            m_RunMode = serializedObject.FindProperty("m_RunMode");
            m_SleepTimeout = serializedObject.FindProperty("m_SleepTimeout");
            m_RunInBackground = serializedObject.FindProperty("m_RunInBackground");
            m_TargetFrameRate = serializedObject.FindProperty("m_TargetFrameRate");
            m_UpdateServer = serializedObject.FindProperty("m_UpdateServer");
            m_BundleExtension = serializedObject.FindProperty("m_BundleExtension");
            m_LuaDirList = serializedObject.FindProperty("m_LuaDirList");
            m_LuaBundleList = serializedObject.FindProperty("m_LuaBundleList");
            luaDirList = new ReorderableList(serializedObject, m_LuaDirList, true, true, true, true);
            luaDirList.drawHeaderCallback = DrawLuaDirListHeader;
            luaDirList.drawElementCallback = DrawLuaDirListElement;
            luaBundleList = new ReorderableList(serializedObject, m_LuaBundleList, true, true, true, true);
            luaBundleList.drawHeaderCallback = DrawLuaBundleListHeader;
            luaBundleList.drawElementCallback = DrawLuaBundleListElement;
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
            EditorGUILayout.Space(); EditorGUILayout.LabelField("Mode", subtitleStyle);
            EditorGUILayout.PropertyField(m_RunMode, new GUIContent("Run Mode In Editor"));
            EditorGUILayout.PropertyField(m_SleepTimeout);
            EditorGUILayout.PropertyField(m_RunInBackground);
            EditorGUILayout.Space(); EditorGUILayout.LabelField("Quality", subtitleStyle);
            EditorGUILayout.PropertyField(m_TargetFrameRate);
            EditorGUILayout.Space(); EditorGUILayout.LabelField("Network", subtitleStyle);
            EditorGUILayout.PropertyField(m_UpdateServer);
            EditorGUILayout.Space(); EditorGUILayout.LabelField("Asset Bundle", subtitleStyle);
            EditorGUILayout.PropertyField(m_BundleExtension);
            EditorGUILayout.Space(); EditorGUILayout.LabelField("Lua", subtitleStyle);
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 30;
            luaDirList.DoLayoutList();
            luaBundleList.DoLayoutList();
            EditorGUIUtility.labelWidth = labelWidth;
            serializedObject.ApplyModifiedProperties();
        }
    }
}