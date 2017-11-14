/*
 * Author:      熊哲
 * CreateTime:  3/16/2017 10:25:51 AM
 * Description:
 * 
*/
using UnityEditor;
using UnityEngine;

namespace EZUnityEditor
{
    [CustomEditor(typeof(EZKeystoreObject))]
    public class EZKeystoreEditor : Editor
    {
        private SerializedProperty m_KeystoreName;
        private SerializedProperty m_KeystorePass;
        private SerializedProperty m_KeyAliasName;
        private SerializedProperty m_KeyAliasPass;

        void OnEnable()
        {
            m_KeystoreName = serializedObject.FindProperty("m_KeystoreName");
            m_KeystorePass = serializedObject.FindProperty("m_KeystorePass");
            m_KeyAliasName = serializedObject.FindProperty("m_KeyAliasName");
            m_KeyAliasPass = serializedObject.FindProperty("m_KeyAliasPass");
        }

        void OnFocus()
        {
            EZKeystoreInitializer.LoadKeystore();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(target as ScriptableObject), typeof(MonoScript), false);
            GUI.enabled = true;
            EditorGUILayout.PropertyField(m_KeystoreName, new GUIContent("Keystore Path"));
            EditorGUILayout.PropertyField(m_KeystorePass, new GUIContent("Keystore Password"));
            EditorGUILayout.PropertyField(m_KeyAliasName, new GUIContent("Key Alias Name"));
            EditorGUILayout.PropertyField(m_KeyAliasPass, new GUIContent("Key Alias Password"));
            serializedObject.ApplyModifiedProperties();
            if (GUI.changed) EZKeystoreInitializer.SetKeystore(target as EZKeystoreObject);
        }
    }
}