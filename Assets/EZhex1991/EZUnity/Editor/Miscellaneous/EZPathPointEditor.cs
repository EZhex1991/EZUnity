/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-02 17:30:18
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZPathPoint))]
    public class EZPathPointEditor : Editor
    {
        private EZPathPoint pathPoint;

        private SerializedProperty m_BrokenTangent;
        private SerializedProperty m_StartTangent;
        private SerializedProperty m_EndTangent;

        private void OnEnable()
        {
            pathPoint = target as EZPathPoint;
            m_BrokenTangent = serializedObject.FindProperty("m_BrokenTangent");
            m_StartTangent = serializedObject.FindProperty("m_StartTangent");
            m_EndTangent = serializedObject.FindProperty("m_EndTangent");
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.MonoBehaviourTitle(pathPoint);
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_BrokenTangent);
            EditorGUILayout.PropertyField(m_StartTangent);
            EditorGUILayout.PropertyField(m_EndTangent);
            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            DrawTangentHandles(pathPoint);
        }

        public static void DrawTangentHandles(EZPathPoint pathPoint)
        {
            Handles.matrix = pathPoint.transform.localToWorldMatrix;

            Handles.color = Color.green;
            Vector3 startTangent = Handles.FreeMoveHandle(pathPoint.startTangent, Quaternion.identity, HandleUtility.GetHandleSize(pathPoint.startTangent) * 0.15f, Vector3.zero, Handles.SphereHandleCap);
            if (startTangent != pathPoint.startTangent)
            {
                Undo.RegisterCompleteObjectUndo(pathPoint, "Change StartTangent");
                pathPoint.startTangent = startTangent;
                if (!pathPoint.brokenTangent) pathPoint.endTangent = -startTangent;
                EditorUtility.SetDirty(pathPoint);
            }
            Handles.DrawDottedLine(Vector3.zero, pathPoint.startTangent, 1);

            Handles.color = Color.red;
            Vector3 endTangent = Handles.FreeMoveHandle(pathPoint.endTangent, Quaternion.identity, HandleUtility.GetHandleSize(pathPoint.endTangent) * 0.15f, Vector3.zero, Handles.SphereHandleCap);
            if (endTangent != pathPoint.endTangent)
            {
                Undo.RegisterCompleteObjectUndo(pathPoint, "Change EndTangent");
                pathPoint.endTangent = endTangent;
                if (!pathPoint.brokenTangent) pathPoint.startTangent = -endTangent;
                EditorUtility.SetDirty(pathPoint);
            }
            Handles.DrawDottedLine(Vector3.zero, pathPoint.endTangent, 1);
        }

    }
}
