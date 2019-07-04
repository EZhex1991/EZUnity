/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-02 17:30:02
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZPath))]
    public class EZPathEditor : Editor
    {
        private EZPath path;

        private SerializedProperty m_PathMode;
        private SerializedProperty m_ClosedPath;

        private void OnEnable()
        {
            path = target as EZPath;
            m_PathMode = serializedObject.FindProperty("m_PathMode");
            m_ClosedPath = serializedObject.FindProperty("m_ClosedPath");
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.MonoBehaviourTitle(path);
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_PathMode);
            EditorGUILayout.PropertyField(m_ClosedPath);
            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            if (path.pathMode == EZPath.PathMode.Bezier)
            {
                for (int i = 0; i < path.pathPoints.Count; i++)
                {
                    EZPathPointEditor.DrawTangentHandles(path.pathPoints[i]);
                }
            }
        }
    }
}
