/*
 * Author:      熊哲
 * CreateTime:  8/22/2017 2:20:56 PM
 * Description:
 * 
*/
using UnityEngine;
using UnityEditor;

namespace EZUnityTools
{
    [CustomEditor(typeof(EZ3DGridLayout))]
    public class EZ3DGridLayoutEditor : Editor
    {
        private EZ3DGridLayout layout;
        private SerializedProperty m_UpdateMode;
        private SerializedProperty m_AxisOrder;
        private SerializedProperty m_Constraint1;
        private SerializedProperty m_Constraint2;
        private SerializedProperty m_Offset;
        private SerializedProperty m_Distance;

        protected virtual void OnEnable()
        {
            layout = target as EZ3DGridLayout;
            m_UpdateMode = serializedObject.FindProperty("m_UpdateMode");
            m_AxisOrder = serializedObject.FindProperty("m_AxisOrder");
            m_Constraint1 = serializedObject.FindProperty("m_Constraint1");
            m_Constraint2 = serializedObject.FindProperty("m_Constraint2");
            m_Offset = serializedObject.FindProperty("m_Offset");
            m_Distance = serializedObject.FindProperty("m_Distance");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Reset Children"))
            {
                layout.ResetChildren();
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                if (layout.updateMode == EZ3DGridLayout.UpdateMode.OnChange)
                {
                    layout.ResetChildren();
                }
            }
        }
    }
}