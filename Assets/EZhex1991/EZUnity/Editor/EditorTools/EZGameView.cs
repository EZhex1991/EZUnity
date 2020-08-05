/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-03-31 10:48:39
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEditor;

namespace EZhex1991.EZUnity
{
    public static class EZGameView
    {
        private static Type m_Type;
        public static Type type
        {
            get
            {
                if (m_Type == null)
                {
                    m_Type = Type.GetType("UnityEditor.GameView,UnityEditor");
                }
                return m_Type;
            }
        }

        private static EditorWindow m_EditorWindow;
        public static EditorWindow editorWindow
        {
            get
            {
                if (m_EditorWindow == null)
                {
                    m_EditorWindow = EditorWindow.GetWindow(type);
                }
                return m_EditorWindow;
            }
        }

        private static SerializedObject m_SerializedObject;
        public static SerializedObject serialziedObject
        {
            get
            {
                if (m_SerializedObject == null && editorWindow != null)
                {
                    m_SerializedObject = new SerializedObject(editorWindow);
                }
                return m_SerializedObject;
            }
        }

        private static SerializedProperty m_TargetDisplay;
        public static int targetDisplay
        {
            get
            {
                if (m_TargetDisplay == null && serialziedObject != null)
                {
                    m_TargetDisplay = serialziedObject.FindProperty("m_TargetDisplay");
                }
                if (m_TargetDisplay != null)
                {
                    return m_TargetDisplay.intValue;
                }
                // No game view was found
                return -1;
            }
            set
            {
                if (m_TargetDisplay != null)
                {
                    m_TargetDisplay.intValue = value;
                    serialziedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
