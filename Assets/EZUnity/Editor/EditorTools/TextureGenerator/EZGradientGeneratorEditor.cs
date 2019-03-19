/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-19 16:33:00
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;

namespace EZUnity
{
    [CustomEditor(typeof(EZGradientGenerator))]
    public class EZGradientGeneratorEditor : EZTextureGeneratorEditor
    {
        private SerializedProperty gradient;
        private SerializedProperty coordinateMode;
        private SerializedProperty coordinateX;
        private SerializedProperty coordinateY;

        protected override void OnEnable()
        {
            base.OnEnable();
            gradient = serializedObject.FindProperty("gradient");
            coordinateMode = serializedObject.FindProperty("coordinateMode");
            coordinateX = serializedObject.FindProperty("coordinateX");
            coordinateY = serializedObject.FindProperty("coordinateY");
        }

        public override void DrawTextureSettings()
        {
            EditorGUILayout.PropertyField(gradient);
            EditorGUILayout.PropertyField(coordinateMode);
            EditorGUILayout.PropertyField(coordinateX);
            EditorGUILayout.PropertyField(coordinateY);
        }
    }
}
