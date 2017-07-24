/*
 * Author:      熊哲
 * CreateTime:  1/19/2017 2:03:26 PM
 * Description:
 * 
*/
using UnityEditor;
using UnityEngine;

namespace EZFramework
{
    public class EZSettingsEditorWindow : EditorWindow
    {
        private EZSettings ezSettings;
        private GUIStyle titleStyle = new GUIStyle();
        private GUIStyle subtitleStyle = new GUIStyle();

        void OnEnable()
        {
            titleStyle.fontSize = 12;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;
            subtitleStyle.fontStyle = FontStyle.Bold;
            ezSettings = AssetDatabase.LoadAssetAtPath<EZSettings>(EZSettings.AssetDirPath + EZSettings.AssetName + ".asset");
            if (ezSettings == null)
            {
                ezSettings = CreateInstance<EZSettings>();
                System.IO.Directory.CreateDirectory(EZSettings.AssetDirPath);
                AssetDatabase.CreateAsset(ezSettings, EZSettings.AssetDirPath + EZSettings.AssetName + ".asset");
            }
        }

        void OnGUI()
        {
            EditorGUILayout.Space(); EditorGUILayout.LabelField(titleContent.text, titleStyle);

            EditorGUILayout.Space(); EditorGUILayout.LabelField("Mode", subtitleStyle);
            ezSettings.runMode = (EZSettings.RunMode)EditorGUILayout.EnumPopup("Run Mode In Editor", ezSettings.runMode);

            EditorGUILayout.Space(); EditorGUILayout.LabelField("Quality", subtitleStyle);
            ezSettings.sleepTimeout = (EZSettings.SleepTimeout)EditorGUILayout.EnumPopup("Sleep Timeout", ezSettings.sleepTimeout);
            ezSettings.runInBackground = EditorGUILayout.Toggle("Run In Background", ezSettings.runInBackground);
            ezSettings.targetFrameRate = EditorGUILayout.IntSlider("Frame Rate", ezSettings.targetFrameRate, 30, 120);

            EditorGUILayout.Space(); EditorGUILayout.LabelField("Network", subtitleStyle);
            ezSettings.updateServer = EditorGUILayout.TextField("Update Server", ezSettings.updateServer);

            EditorGUILayout.Space(); EditorGUILayout.LabelField("Asset Bundle", subtitleStyle);
            ezSettings.bundleExtension = EditorGUILayout.TextField("Extension", ezSettings.bundleExtension);

            EditorGUILayout.Space(); EditorGUILayout.LabelField("Lua", subtitleStyle);
            ezSettings.luaDirName = EditorGUILayout.TextField("Dir Name", ezSettings.luaDirName);

            if (GUI.changed) EditorUtility.SetDirty(ezSettings);
        }
    }
}