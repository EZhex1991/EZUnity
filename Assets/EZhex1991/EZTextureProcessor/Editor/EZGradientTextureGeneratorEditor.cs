/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-19 19:05:35
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;

namespace EZhex1991.EZTextureProcessor
{
    [CustomEditor(typeof(EZGradient2DTextureGenerator))]
    public class EZGradient2DTextureGeneratorEditor : EZTextureGeneratorEditor
    {
        protected SerializedProperty m_Gradient;
        protected SerializedProperty m_GradientCurve;
        protected SerializedProperty m_CoordinateMode;
        protected SerializedProperty m_CoordinateCurveU;
        protected SerializedProperty m_CoordinateCurveV;
        protected SerializedProperty m_Rotation;

        protected override void GetInputProperties()
        {
            m_Gradient = serializedObject.FindProperty("gradient");
            m_GradientCurve = serializedObject.FindProperty("gradientCurve");
            m_CoordinateMode = serializedObject.FindProperty("coordinateMode");
            m_CoordinateCurveU = serializedObject.FindProperty("coordinateCurveU");
            m_CoordinateCurveV = serializedObject.FindProperty("coordinateCurveV");
            m_Rotation = serializedObject.FindProperty("rotation");
        }
        protected override void DrawInputSettings()
        {
            EditorGUILayout.PropertyField(m_Gradient);
            EditorGUILayout.PropertyField(m_GradientCurve);
            EditorGUILayout.PropertyField(m_CoordinateMode);
            switch (m_CoordinateMode.intValue)
            {
                default:
                    EditorGUILayout.PropertyField(m_CoordinateCurveU);
                    EditorGUILayout.PropertyField(m_CoordinateCurveV);
                    EditorGUILayout.PropertyField(m_Rotation);
                    break;
                case (int)EZGradient2DTextureGenerator.CoordinateMode.X:
                    EditorGUILayout.PropertyField(m_CoordinateCurveU);
                    break;
                case (int)EZGradient2DTextureGenerator.CoordinateMode.Y:
                    EditorGUILayout.PropertyField(m_CoordinateCurveV);
                    break;
                case (int)EZGradient2DTextureGenerator.CoordinateMode.AdditiveXY:
                    EditorGUILayout.PropertyField(m_CoordinateCurveU);
                    EditorGUILayout.PropertyField(m_CoordinateCurveV);
                    break;
                case (int)EZGradient2DTextureGenerator.CoordinateMode.MultiplyXY:
                    EditorGUILayout.PropertyField(m_CoordinateCurveU);
                    EditorGUILayout.PropertyField(m_CoordinateCurveV);
                    break;
                case (int)EZGradient2DTextureGenerator.CoordinateMode.DifferenceXY:
                    EditorGUILayout.PropertyField(m_CoordinateCurveU);
                    EditorGUILayout.PropertyField(m_CoordinateCurveV);
                    break;
                case (int)EZGradient2DTextureGenerator.CoordinateMode.Radial:
                    EditorGUILayout.PropertyField(m_CoordinateCurveU);
                    EditorGUILayout.PropertyField(m_CoordinateCurveV);
                    break;
                case (int)EZGradient2DTextureGenerator.CoordinateMode.Angle:
                    EditorGUILayout.PropertyField(m_CoordinateCurveU);
                    EditorGUILayout.PropertyField(m_CoordinateCurveV);
                    EditorGUILayout.PropertyField(m_Rotation);
                    break;
            }
        }
    }
}
