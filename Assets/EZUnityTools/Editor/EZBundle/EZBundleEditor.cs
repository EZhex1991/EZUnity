/*
 * Author:      熊哲
 * CreateTime:  3/8/2017 7:42:27 PM
 * Description:
 * 
*/
using UnityEditor;
using UnityEngine;

namespace EZUnityTools.EZEditor
{
    [CustomEditor(typeof(EZBundleObject))]
    public class EZBundleEditor : Editor
    {
        public string saveName;
        EZBundleObject ezBundle;

        void OnEnable()
        {
            saveName = "";
            ezBundle = target as EZBundleObject;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Build Bundle"))
            {
                EZBundleBuilder.BuildBundle(ezBundle);
            }
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save As"))
                {
                    if (saveName == "")
                        EZScriptableObject.Create(EZBundleObject.AssetName, Object.Instantiate(ezBundle));
                    else
                        EZScriptableObject.Create(saveName, Object.Instantiate(ezBundle));
                }
                saveName = EditorGUILayout.TextField(saveName);
                EditorGUILayout.EndHorizontal();
            }
            base.OnInspectorGUI();
        }
    }
}