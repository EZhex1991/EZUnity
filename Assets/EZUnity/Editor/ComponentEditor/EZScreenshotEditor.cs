/* Author:          熊哲
 * CreateTime:      2018-08-06 10:27:30
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    [CustomEditor(typeof(EZScreenshot))]
    public class EZScreenshotEditor : Editor
    {
        private EZScreenshot capturer { get { return target as EZScreenshot; } }

        private SerializedProperty m_Resolution;
        private SerializedProperty m_Format;
        private SerializedProperty m_CapturePath;

        private void OnEnable()
        {
            m_Resolution = serializedObject.FindProperty("resolution");
            m_Format = serializedObject.FindProperty("format");
            m_CapturePath = serializedObject.FindProperty("capturePath");
        }
        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptTitle(target);
            serializedObject.Update();
            EditorGUILayout.LabelField("Camera Capture", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_Resolution);
            EditorGUILayout.PropertyField(m_Format);
            EditorGUILayout.PropertyField(m_CapturePath);
            if (GUILayout.Button("Capture"))
            {
                if (!string.IsNullOrEmpty(m_CapturePath.stringValue)) Directory.CreateDirectory(m_CapturePath.stringValue);
                Capture();
            }
            if (GUILayout.Button("Open Folder"))
            {
                if (!string.IsNullOrEmpty(m_CapturePath.stringValue)) Directory.CreateDirectory(m_CapturePath.stringValue);
                string projectPath = Application.dataPath.Substring(0, Application.dataPath.Length - 7);
                Application.OpenURL(string.Format("file://{0}/{1}", projectPath, m_CapturePath.stringValue));
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void Capture()
        {
            string folderPath = m_CapturePath.stringValue;
            if (!string.IsNullOrEmpty(folderPath)) Directory.CreateDirectory(folderPath);
            string fileName = EZScreenshot.GetFileName();
            string path = string.Format("{0}/{1}.{2}", folderPath, fileName, "png");
            capturer.CameraCapture(path);
            AssetDatabase.Refresh();
        }
    }
}
