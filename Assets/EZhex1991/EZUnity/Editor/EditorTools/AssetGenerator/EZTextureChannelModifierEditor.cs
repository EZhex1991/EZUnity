/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 16:13:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.AssetGenerator
{
    [CustomEditor(typeof(EZTextureChannelModifier))]
    public class EZTextureChannelModifierEditor : _EZTextureGeneratorEditor
    {
        private SerializedProperty m_ReferenceTexture;

        private SerializedProperty m_OverrideTextureR;
        private SerializedProperty m_ChannelR;
        private SerializedProperty m_CurveR;
        private SerializedProperty m_OverrideTextureG;
        private SerializedProperty m_ChannelG;
        private SerializedProperty m_CurveG;
        private SerializedProperty m_OverrideTextureB;
        private SerializedProperty m_ChannelB;
        private SerializedProperty m_CurveB;
        private SerializedProperty m_OverrideTextureA;
        private SerializedProperty m_ChannelA;
        private SerializedProperty m_CurveA;

        protected override void GetProperties()
        {
            m_ReferenceTexture = serializedObject.FindProperty("referenceTexture");

            m_OverrideTextureR = serializedObject.FindProperty("overrideTextureR");
            m_ChannelR = serializedObject.FindProperty("channelR");
            m_CurveR = serializedObject.FindProperty("curveR");

            m_OverrideTextureG = serializedObject.FindProperty("overrideTextureG");
            m_ChannelG = serializedObject.FindProperty("channelG");
            m_CurveG = serializedObject.FindProperty("curveG");

            m_OverrideTextureB = serializedObject.FindProperty("overrideTextureB");
            m_ChannelB = serializedObject.FindProperty("channelB");
            m_CurveB = serializedObject.FindProperty("curveB");

            m_OverrideTextureA = serializedObject.FindProperty("overrideTextureA");
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
            DrawChannelSettings(m_OverrideTextureR, m_ChannelR, m_CurveR);
            DrawChannelSettings(m_OverrideTextureG, m_ChannelG, m_CurveG);
            DrawChannelSettings(m_OverrideTextureB, m_ChannelB, m_CurveB);
            DrawChannelSettings(m_OverrideTextureA, m_ChannelA, m_CurveA);
        }
        private void DrawChannelSettings(SerializedProperty overrideTexture, SerializedProperty channel, SerializedProperty curve)
        {
            EditorGUILayout.BeginHorizontal();
            float fieldWidth = (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth) * 0.1f;
            EditorGUILayout.PropertyField(overrideTexture, new GUIContent(channel.displayName));
            EditorGUILayout.PropertyField(channel, GUIContent.none, GUILayout.MaxWidth(fieldWidth));
            EditorGUILayout.PropertyField(curve, GUIContent.none, GUILayout.MaxWidth(fieldWidth * 3));
            EditorGUILayout.EndHorizontal();
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
