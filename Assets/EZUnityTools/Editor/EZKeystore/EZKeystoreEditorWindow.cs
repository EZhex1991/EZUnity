/*
 * Author:      熊哲
 * CreateTime:  3/16/2017 11:00:36 AM
 * Description:
 * 
*/
using UnityEditor;
using UnityEngine;

namespace EZUnityTools.EZEditor
{
    public class EZKeystoreEditorWindow : EZEditorWindow
    {
        private EZKeystoreObject ezKeystore;

        protected override void OnFocus()
        {
            base.OnEnable();
            ezKeystore = EZScriptableObject.Load<EZKeystoreObject>(EZKeystoreObject.AssetName, false);
            if (ezKeystore == null)
            {
                ezKeystore = EZKeystoreInitializer.CreateKeystore();
            }
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            if (GUILayout.Button("Load"))
            {
                EZKeystoreInitializer.CreateKeystore();
            }
            ezKeystore.keystoreFilePath = EditorGUILayout.TextField("Keystore File Path", ezKeystore.keystoreFilePath);
            ezKeystore.keystorePassword = EditorGUILayout.PasswordField("Keystore Password", ezKeystore.keystorePassword);
            ezKeystore.keyAliasName = EditorGUILayout.TextField("Key Alias Name", ezKeystore.keyAliasName);
            ezKeystore.keyAliasPassword = EditorGUILayout.PasswordField("Key Alias Password", ezKeystore.keyAliasPassword);
            if (GUI.changed)
            {
                EZKeystoreInitializer.SetKeystore(ezKeystore);
                EditorUtility.SetDirty(ezKeystore);
            }
        }
    }
}