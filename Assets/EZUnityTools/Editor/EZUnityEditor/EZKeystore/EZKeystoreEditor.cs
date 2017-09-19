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
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUI.changed)
            {
                EZKeystoreInitializer.SetKeystore(target as EZKeystoreObject);
                EditorUtility.SetDirty(target);
            }
        }
    }
}