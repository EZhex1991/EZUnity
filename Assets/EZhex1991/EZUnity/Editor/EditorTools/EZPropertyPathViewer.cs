/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-04-08 10:34:13
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZPropertyPathViewer : EditorWindow
    {
        public Object target;
        public SerializedObject serializedObject;
        public bool enterChildren = true;
        public bool showInvisible = false;

        private Vector2 scrollPosition;

        private void OnEnable()
        {
            if (target != null)
            {
                serializedObject = new SerializedObject(target);
            }
        }
        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);
            bool wideMode = EditorGUIUtility.wideMode;
            EditorGUIUtility.wideMode = true;

            EditorGUI.BeginChangeCheck();
            target = EditorGUILayout.ObjectField("Target", target, typeof(Object), true);
            if (EditorGUI.EndChangeCheck())
            {
                if (target != null)
                {
                    serializedObject = new SerializedObject(target);
                }
                else
                {
                    serializedObject = null;
                }
            }
            enterChildren = EditorGUILayout.Toggle("Enter Children", enterChildren);
            showInvisible = EditorGUILayout.Toggle("Show Invisible", showInvisible);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
            // change scene will destroy target
            if (target != null && serializedObject != null)
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                serializedObject.Update();
                SerializedProperty sp = serializedObject.GetIterator();
                sp.Next(true);
                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2;
                if (showInvisible)
                {
                    do { PropertyField(sp); }
                    while (sp.Next(enterChildren));
                }
                else
                {
                    do { PropertyField(sp); }
                    while (sp.NextVisible(enterChildren));
                }
                EditorGUIUtility.labelWidth = 0;
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndScrollView();
            }

            EditorGUIUtility.wideMode = wideMode;
        }
        private void PropertyField(SerializedProperty sp)
        {
            EditorGUILayout.PropertyField(sp, new GUIContent(sp.propertyPath), false);
        }
    }
}
