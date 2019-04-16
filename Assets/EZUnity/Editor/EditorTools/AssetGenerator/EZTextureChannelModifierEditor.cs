/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 16:13:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    [CustomEditor(typeof(EZTextureChannelModifier))]
    public class EZTextureChannelModifierEditor : EZTextureGeneratorEditor
    {
        private SerializedProperty m_ReferenceTexture;

        private SerializedProperty m_ChannelR;
        private SerializedProperty m_CurveR;
        private SerializedProperty m_ChannelG;
        private SerializedProperty m_CurveG;
        private SerializedProperty m_ChannelB;
        private SerializedProperty m_CurveB;
        private SerializedProperty m_ChannelA;
        private SerializedProperty m_CurveA;

        protected override void GetProperties()
        {
            m_ReferenceTexture = serializedObject.FindProperty("referenceTexture");
            m_ChannelR = serializedObject.FindProperty("channelR");
            m_CurveR = serializedObject.FindProperty("curveR");
            m_ChannelG = serializedObject.FindProperty("channelG");
            m_CurveG = serializedObject.FindProperty("curveG");
            m_ChannelB = serializedObject.FindProperty("channelB");
            m_CurveB = serializedObject.FindProperty("curveB");
            m_ChannelA = serializedObject.FindProperty("channelA");
            m_CurveA = serializedObject.FindProperty("curveA");
        }

        protected override void DrawTextureSettings()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_ReferenceTexture);
            if (EditorGUI.EndChangeCheck())
            {
                Texture2D texture = m_ReferenceTexture.objectReferenceValue as Texture2D;
                if (texture != null) resolution.vector2IntValue = new Vector2Int(texture.width, texture.height);
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_ChannelR);
                EditorGUILayout.PropertyField(m_CurveR, GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_ChannelG);
                EditorGUILayout.PropertyField(m_CurveG, GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_ChannelB);
                EditorGUILayout.PropertyField(m_CurveB, GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_ChannelA);
                EditorGUILayout.PropertyField(m_CurveA, GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }
        }
        protected override void DrawGenerateButton()
        {
            base.DrawGenerateButton();
            EditorGUILayout.Space();
            if (GUILayout.Button("Open Modifier Window (Batch Mode)"))
            {
                var window = EditorWindow.GetWindow<EZTextureChannelModifierWindow>("Texture Channel Modifier");
                window.modifier = target as EZTextureChannelModifier;
                window.Show();
            }
        }
    }
}
