/*
 * Author:      熊哲
 * CreateTime:  11/17/2017 2:23:29 PM
 * Description:
 * 
*/
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    [CustomEditor(typeof(TrailRenderer)), CanEditMultipleObjects]
    public class TrailRendererEditor : Editor
    {
        SerializedProperty m_SortingLayerID;
        SerializedProperty m_SortingOrder;

        MethodInfo sortingLayerField;

        void OnEnable()
        {
            m_SortingLayerID = serializedObject.FindProperty("m_SortingLayerID");
            m_SortingOrder = serializedObject.FindProperty("m_SortingOrder");
            sortingLayerField = typeof(EditorGUILayout).GetMethod("SortingLayerField", BindingFlags.NonPublic | BindingFlags.Static
                , null, new Type[] { typeof(GUIContent), typeof(SerializedProperty), typeof(GUIStyle) }, null);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            sortingLayerField.Invoke(null, new object[] { new GUIContent("SortingLayer"), m_SortingLayerID, EditorStyles.popup });
            EditorGUILayout.PropertyField(m_SortingOrder);
            serializedObject.ApplyModifiedProperties();
        }
    }
}