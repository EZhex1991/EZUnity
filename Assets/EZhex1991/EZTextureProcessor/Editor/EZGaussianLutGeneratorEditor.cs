/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-27 17:29:17
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CustomEditor(typeof(EZGaussianLutGenerator))]
    public class EZGaussianLutGeneratorEditor : EZTextureProcessorEditor
    {
        protected SerializedProperty m_TextureType;
        protected SerializedProperty m_RangeX;
        protected SerializedProperty m_SigmaX;
        protected SerializedProperty m_RangeY;
        protected SerializedProperty m_SigmaY;

        protected override void GetInputProperties()
        {
            base.GetInputProperties();
            m_TextureType = serializedObject.FindProperty("textureType");
            m_RangeX = serializedObject.FindProperty("rangeX");
            m_SigmaX = serializedObject.FindProperty("sigmaX");
            m_RangeY = serializedObject.FindProperty("rangeY");
            m_SigmaY = serializedObject.FindProperty("sigmaY");
        }
        protected override void DrawInputSettings()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(m_Shader);
            GUI.enabled = true;
            EditorGUILayout.PropertyField(m_TextureType);
            switch (m_TextureType.intValue)
            {
                default:
                    EditorGUILayout.PropertyField(m_RangeX);
                    EditorGUILayout.PropertyField(m_SigmaX);
                    EditorGUILayout.PropertyField(m_RangeY);
                    EditorGUILayout.PropertyField(m_SigmaY);
                    break;
                case (int)EZGaussianLutGenerator.TextureType.Wave:
                    EditorGUILayout.PropertyField(m_RangeX);
                    EditorGUILayout.PropertyField(m_SigmaX);
                    break;
                case (int)EZGaussianLutGenerator.TextureType.Lut1D:
                    EditorGUILayout.PropertyField(m_RangeX);
                    EditorGUILayout.PropertyField(m_SigmaX);
                    break;
                case (int)EZGaussianLutGenerator.TextureType.Lut2D:
                    EditorGUILayout.PropertyField(m_RangeX);
                    EditorGUILayout.PropertyField(m_SigmaX);
                    EditorGUILayout.PropertyField(m_RangeY);
                    EditorGUILayout.PropertyField(m_SigmaY);
                    break;
            }
        }
    }
}
