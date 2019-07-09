/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-11-22 18:08:40
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static partial class EZEditorGUIUtility
    {
        public static float space = 5;
        public static float indexWidth = 30;
        public static float dragHandleWidth = 15;
        public static float singleLineHeight = EditorGUIUtility.singleLineHeight;

        public static void WindowTitle(EditorWindow target)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(target), typeof(MonoScript), false);
            EditorGUILayout.Space();
            GUI.enabled = true;
        }
        public static void ScriptableObjectTitle(ScriptableObject target, bool showTarget = true)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(target), typeof(MonoScript), false);
            if (showTarget)
                EditorGUILayout.ObjectField("Target", target, typeof(Object), false);
            GUI.enabled = true;
        }
        public static void MonoBehaviourTitle(MonoBehaviour target)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target), typeof(MonoScript), false);
            GUI.enabled = true;
        }

        public static Rect CalcReorderableListHeaderRect(Rect rect, bool draggable = true)
        {
            float indent = draggable ? (dragHandleWidth + indexWidth) : indexWidth;
            rect.x += indent; rect.width -= indent;
            return rect;
        }

        public static Rect DrawReorderableListIndex(Rect rect, SerializedProperty listProperty, int index)
        {
            return DrawReorderableListIndex(rect, listProperty, index, indexWidth);
        }
        public static Rect DrawReorderableListIndex(Rect rect, SerializedProperty listProperty, int index, float width)
        {
            if (GUI.Button(new Rect(rect.x, rect.y, width, singleLineHeight), index.ToString("00"), EditorStyles.label))
            {
                DrawReorderMenu(listProperty, index).ShowAsContext();
            }
            rect.x += width; rect.width -= width;
            return rect;
        }

        public static GenericMenu DrawReorderMenu(SerializedProperty property, int index)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Insert"), false, delegate
            {
                property.InsertArrayElementAtIndex(index);
                property.serializedObject.ApplyModifiedProperties();
            });
            menu.AddItem(new GUIContent("Delete"), false, delegate
            {
                property.DeleteArrayElementAtIndex(index);
                property.serializedObject.ApplyModifiedProperties();
            });
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Move to Top"), false, delegate
            {
                property.MoveArrayElement(index, 0);
                property.serializedObject.ApplyModifiedProperties();
            });
            menu.AddItem(new GUIContent("Move to Bottom"), false, delegate
            {
                property.MoveArrayElement(index, property.arraySize - 1);
                property.serializedObject.ApplyModifiedProperties();
            });
            return menu;
        }

        public static Rect DrawReorderableListIndex(Rect rect, int index, SerializedObject serializedObject, params SerializedProperty[] listProperties)
        {
            return DrawReorderableListIndex(rect, index, indexWidth, serializedObject, listProperties);
        }
        public static Rect DrawReorderableListIndex(Rect rect, int index, float width, SerializedObject serializedObject, params SerializedProperty[] listProperties)
        {
            if (GUI.Button(new Rect(rect.x, rect.y, width, singleLineHeight), index.ToString("00"), EditorStyles.label))
            {
                DrawReorderMenu(index, serializedObject, listProperties).ShowAsContext();
            }
            rect.x += width; rect.width -= width;
            return rect;
        }
        public static GenericMenu DrawReorderMenu(int index, SerializedObject serializedObject, params SerializedProperty[] listProperties)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Insert"), false, delegate
            {
                for (int i = 0; i < listProperties.Length; i++)
                {
                    listProperties[i].InsertArrayElementAtIndex(index);
                }
                serializedObject.ApplyModifiedProperties();
            });
            menu.AddItem(new GUIContent("Delete"), false, delegate
            {
                for (int i = 0; i < listProperties.Length; i++)
                {
                    listProperties[i].DeleteArrayElementAtIndex(index);
                }
                serializedObject.ApplyModifiedProperties();
            });
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Move to Top"), false, delegate
            {
                for (int i = 0; i < listProperties.Length; i++)
                {
                    listProperties[i].MoveArrayElement(index, 0);
                }
                serializedObject.ApplyModifiedProperties();
            });
            menu.AddItem(new GUIContent("Move to Bottom"), false, delegate
            {
                for (int i = 0; i < listProperties.Length; i++)
                {
                    listProperties[i].MoveArrayElement(index, listProperties[i].arraySize - 1);
                }
                serializedObject.ApplyModifiedProperties();
            });
            return menu;
        }
    }
}