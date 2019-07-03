/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-08-06 10:27:30
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZImageCapture))]
    public class EZImageCaptureEditor : Editor
    {
        private EZImageCapture capturer { get { return target as EZImageCapture; } }

        private Camera camera;
        private SerializedProperty m_Resolution;
        private SerializedProperty m_TextureFormat;
        private SerializedProperty m_FilePath;

        private void OnEnable()
        {
            m_Resolution = serializedObject.FindProperty("resolution");
            m_TextureFormat = serializedObject.FindProperty("textureFormat");
            m_FilePath = serializedObject.FindProperty("filePath");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, !serializedObject.isEditingMultipleObjects);

            EditorGUILayout.PropertyField(m_FilePath);
            if (GUILayout.Button("Open Folder"))
            {
                if (!string.IsNullOrEmpty(m_FilePath.stringValue)) Directory.CreateDirectory(m_FilePath.stringValue);
                string projectPath = Application.dataPath.Substring(0, Application.dataPath.Length - 7);
                Application.OpenURL(string.Format("file://{0}/{1}", projectPath, m_FilePath.stringValue));
            }

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Game View Capture is only available when playing", MessageType.Info);
            GUI.enabled = EditorApplication.isPlaying;
            if (GUILayout.Button("Game View Capture"))
            {
                if (!string.IsNullOrEmpty(m_FilePath.stringValue)) Directory.CreateDirectory(m_FilePath.stringValue);
                capturer.ScreenCapture(GetPath());
            }
            GUI.enabled = true;

            EditorGUILayout.Space();
            camera = (Camera)EditorGUILayout.ObjectField("Target Camera", camera, typeof(Camera), true);
            EditorGUILayout.PropertyField(m_Resolution);
            EditorGUILayout.PropertyField(m_TextureFormat);
            if (camera == null) GUI.enabled = false;
            if (GUILayout.Button("Camera Capture"))
            {
                if (!string.IsNullOrEmpty(m_FilePath.stringValue)) Directory.CreateDirectory(m_FilePath.stringValue);
                capturer.CameraCapture(camera, GetPath());
            }
            GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }

        private string GetPath()
        {
            string folderPath = m_FilePath.stringValue;
            if (!string.IsNullOrEmpty(folderPath)) Directory.CreateDirectory(folderPath);
            string fileName = string.Format("screenshot-{0}", DateTime.Now.ToString("yyyyMMdd-HHmmss"));
            return string.Format("{0}/{1}.{2}", folderPath, fileName, "png");
        }
    }
}
