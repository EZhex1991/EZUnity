/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-08-10 10:03:19
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZScrollRect))]
    public class EZScrollRectEditor : UnityEditor.UI.ScrollRectEditor
    {
        SerializedProperty m_HorizontalCount;
        SerializedProperty m_VerticalCount;
        SerializedProperty m_InertialSpeedThreshold;
        SerializedProperty m_RepositionTime;

        protected override void OnEnable()
        {
            m_HorizontalCount = serializedObject.FindProperty("m_HorizontalCount");
            m_VerticalCount = serializedObject.FindProperty("m_VerticalCount");
            m_InertialSpeedThreshold = serializedObject.FindProperty("m_InertialSpeedThreshold");
            m_RepositionTime = serializedObject.FindProperty("m_RepositionTime");
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_HorizontalCount);
            EditorGUILayout.PropertyField(m_VerticalCount);
            EditorGUILayout.PropertyField(m_InertialSpeedThreshold);
            EditorGUILayout.PropertyField(m_RepositionTime);
            serializedObject.ApplyModifiedProperties();
        }
    }
}