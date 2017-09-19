/*
 * Author:      熊哲
 * CreateTime:  3/16/2017 11:00:36 AM
 * Description:
 * 
*/
using UnityEditor;
using UnityEngine;

namespace EZUnityEditor
{
    public class EZKeystoreEditorWindow : EZEditorWindow
    {
        private EZKeystoreObject ezKeystore;
        private SerializedObject so_EZKeystore;
        private SerializedProperty keystoreFilePath;
        private SerializedProperty keystorePassword;
        private SerializedProperty keyAliasName;
        private SerializedProperty keyAliasPassword;

        protected override void OnFocus()
        {
            base.OnFocus();
            ezKeystore = EZScriptableObject.Load<EZKeystoreObject>(EZKeystoreObject.AssetName, false);
            if (ezKeystore == null)
            {
                ezKeystore = EZKeystoreInitializer.CreateKeystore();
            }
            so_EZKeystore = new SerializedObject(ezKeystore);
            keystoreFilePath = so_EZKeystore.FindProperty("keystoreFilePath");
            keystorePassword = so_EZKeystore.FindProperty("keystorePassword");
            keyAliasName = so_EZKeystore.FindProperty("keyAliasName");
            keyAliasPassword = so_EZKeystore.FindProperty("keyAliasPassword");
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            so_EZKeystore.Update();
            if (GUILayout.Button("Load"))
            {
                EZKeystoreInitializer.CreateKeystore();
            }
            EditorGUILayout.PropertyField(keystoreFilePath);
            EditorGUILayout.PropertyField(keystorePassword);
            EditorGUILayout.PropertyField(keyAliasName);
            EditorGUILayout.PropertyField(keyAliasPassword);
            so_EZKeystore.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EZKeystoreInitializer.SetKeystore(ezKeystore);
                EditorUtility.SetDirty(ezKeystore);
            }
        }
    }
}