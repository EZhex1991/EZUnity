/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-19 16:33:00
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;

namespace EZUnity
{
    [CustomEditor(typeof(EZGradientGenerator))]
    public class EZGradientGeneratorEditor : EZTextureGeneratorEditorLayout
    {
        private SerializedProperty gradient;
        private SerializedProperty coordinateMode;
        private SerializedProperty coordinateX;
        private SerializedProperty coordinateY;
        private SerializedProperty rotation;

        protected override void GetTextureSettings()
        {
            gradient = serializedObject.FindProperty("gradient");
            coordinateMode = serializedObject.FindProperty("coordinateMode");
            coordinateX = serializedObject.FindProperty("coordinateX");
            coordinateY = serializedObject.FindProperty("coordinateY");
            rotation = serializedObject.FindProperty("rotation");
        }
        protected override void DrawTextureSettings()
        {
            EditorGUILayout.PropertyField(gradient);
            EditorGUILayout.PropertyField(coordinateMode);
            EditorGUILayout.PropertyField(coordinateX);
            EditorGUILayout.PropertyField(coordinateY);
            EditorGUILayout.PropertyField(rotation);
        }
    }
}
