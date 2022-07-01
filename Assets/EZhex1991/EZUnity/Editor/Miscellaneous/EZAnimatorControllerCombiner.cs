/* Author:          ezhex1991@outlook.com
 * CreateTime:      2022-07-01 19:50:32
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZAnimatorControllerCombiner : EditorWindow
    {
        public class Controllers : ScriptableObject
        {
            public AnimatorController output;
            public AnimatorController[] elements;
        }

        public static Controllers m_Controllers;
        public static Controllers controllers
        {
            get
            {
                if (m_Controllers == null)
                {
                    m_Controllers = CreateInstance<Controllers>();
                }
                return m_Controllers;
            }
        }

        public SerializedObject serializedObject;
        private SerializedProperty m_Output;
        private SerializedProperty m_Elements;

        protected void Awake()
        {
            serializedObject = new SerializedObject(controllers);
            m_Output = serializedObject.FindProperty(nameof(Controllers.output));
            m_Elements = serializedObject.FindProperty(nameof(Controllers.elements));
        }

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            EditorGUILayout.PropertyField(m_Output);
            EditorGUILayout.PropertyField(m_Elements);

            if (GUILayout.Button("Combine"))
            {
                if (controllers.output != null)
                {
                    foreach (AnimatorController controller in controllers.elements)
                    {
                        foreach (AnimatorControllerLayer layer in controller.layers)
                        {
                            controllers.output.AddLayer(layer);
                        }
                    }
                    EditorUtility.SetDirty(controllers.output);
                    Selection.activeObject = controllers.output;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
