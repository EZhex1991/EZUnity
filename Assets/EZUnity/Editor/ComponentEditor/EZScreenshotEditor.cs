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
        private SerializedProperty m_VideoRecord;
        private SerializedProperty m_VideoPath;
        private SerializedProperty m_FrameSkip;

        private void OnEnable()
        {
            m_Resolution = serializedObject.FindProperty("resolution");
            m_Format = serializedObject.FindProperty("format");
            m_CapturePath = serializedObject.FindProperty("capturePath");
            m_VideoRecord = serializedObject.FindProperty("videoRecord");
            m_VideoPath = serializedObject.FindProperty("videoPath");
            m_FrameSkip = serializedObject.FindProperty("frameSkip");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("Camera Capture", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_Resolution);
            EditorGUILayout.PropertyField(m_Format);
            EditorGUILayout.PropertyField(m_CapturePath);
            if (GUILayout.Button("Capture"))
            {
                Capture();
            }
            EditorGUILayout.LabelField("Video Recorder", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_FrameSkip);
            EditorGUILayout.LabelField("FrameCount", capturer.frameCount.ToString());
            EditorGUILayout.PropertyField(m_VideoPath);
            if (GUILayout.Button(m_VideoRecord.boolValue ? "Recording(While Game Playing)" : "Start Record"))
            {
                m_VideoRecord.boolValue = !m_VideoRecord.boolValue;
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
