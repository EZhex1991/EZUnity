/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 16:13:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CustomEditor(typeof(EZTextureChannelModifier))]
    public class EZTextureChannelModifierEditor : EZTextureGeneratorEditor
    {
        private SerializedProperty m_InputTexture;
        private SerializedProperty m_OutputCurve;

        private SerializedProperty m_OverrideTextureR;
        private SerializedProperty m_OverrideChannelR;
        private SerializedProperty m_OverrideCurveR;
        private SerializedProperty m_OverrideTextureG;
        private SerializedProperty m_OverrideChannelG;
        private SerializedProperty m_OverrideCurveG;
        private SerializedProperty m_OverrideTextureB;
        private SerializedProperty m_OverrideChannelB;
        private SerializedProperty m_OverrideCurveB;
        private SerializedProperty m_OverrideTextureA;
        private SerializedProperty m_OverrideChannelA;
        private SerializedProperty m_OverrideCurveA;

        protected override void GetInputProperties()
        {
            m_InputTexture = serializedObject.FindProperty("m_InputTexture");
            m_OutputCurve = serializedObject.FindProperty("outputCurve");

            m_OverrideTextureR = serializedObject.FindProperty("overrideTextureR");
            m_OverrideChannelR = serializedObject.FindProperty("overrideChannelR");
            m_OverrideCurveR = serializedObject.FindProperty("overrideCurveR");

            m_OverrideTextureG = serializedObject.FindProperty("overrideTextureG");
            m_OverrideChannelG = serializedObject.FindProperty("overrideChannelG");
            m_OverrideCurveG = serializedObject.FindProperty("overrideCurveG");

            m_OverrideTextureB = serializedObject.FindProperty("overrideTextureB");
            m_OverrideChannelB = serializedObject.FindProperty("overrideChannelB");
            m_OverrideCurveB = serializedObject.FindProperty("overrideCurveB");

            m_OverrideTextureA = serializedObject.FindProperty("overrideTextureA");
            m_OverrideChannelA = serializedObject.FindProperty("overrideChannelA");
            m_OverrideCurveA = serializedObject.FindProperty("overrideCurveA");
        }
        protected override void DrawInputSettings()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            float fieldWidth = (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth) * 0.1f;
            EditorGUILayout.PropertyField(m_InputTexture, new GUIContent(m_InputTexture.displayName));
            EditorGUILayout.PropertyField(m_OutputCurve, GUIContent.none, GUILayout.MaxWidth(fieldWidth * 3));
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                Texture2D texture = m_InputTexture.objectReferenceValue as Texture2D;
                if (texture != null) m_OutputResolution.vector2IntValue = new Vector2Int(texture.width, texture.height);
            }

            DrawChannelSettings("Override R", m_OverrideTextureR, m_OverrideChannelR, m_OverrideCurveR);
            DrawChannelSettings("Override G", m_OverrideTextureG, m_OverrideChannelG, m_OverrideCurveG);
            DrawChannelSettings("Override B", m_OverrideTextureB, m_OverrideChannelB, m_OverrideCurveB);
            DrawChannelSettings("Override A", m_OverrideTextureA, m_OverrideChannelA, m_OverrideCurveA);
        }
        private void DrawChannelSettings(string label, SerializedProperty texture, SerializedProperty channel, SerializedProperty curve)
        {
            EditorGUILayout.BeginHorizontal();
            float fieldWidth = (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth) * 0.1f;
            EditorGUILayout.PropertyField(texture, new GUIContent(label));
            EditorGUILayout.PropertyField(channel, GUIContent.none, GUILayout.MaxWidth(fieldWidth));
            EditorGUILayout.PropertyField(curve, GUIContent.none, GUILayout.MaxWidth(fieldWidth * 3));
            EditorGUILayout.EndHorizontal();
        }
        protected override void DrawGenerateSettings()
        {
            base.DrawGenerateSettings();
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
