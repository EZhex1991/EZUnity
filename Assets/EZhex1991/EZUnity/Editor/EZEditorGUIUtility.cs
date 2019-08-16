/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-11-22 18:08:40
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static partial class EZEditorGUIUtility
    {
        public const float digitWidth_2 = 30;
        public const float digitWidth_3 = 40;
        public const float dragHandleWidth = 15;

        public static float reorderableListPrefixWidth = digitWidth_2;

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

        public static Rect CalcReorderableListHeaderRect(Rect rect, ReorderableList list)
        {
            float indentWidth = list.draggable ? (dragHandleWidth + reorderableListPrefixWidth) : reorderableListPrefixWidth;
            rect.x += indentWidth; rect.width -= indentWidth;
            return rect;
        }
        public static Rect DrawReorderableListCount(Rect rect, ReorderableList list)
        {
            float indentWidth = list.draggable ? (dragHandleWidth + reorderableListPrefixWidth) : reorderableListPrefixWidth;
            Rect countRect = new Rect(rect);
            countRect.width = indentWidth - 5;
            countRect.y += 2;
            int length = EditorGUI.DelayedIntField(countRect, list.count, EditorStyles.miniTextField);
            if (length != list.count)
            {
                list.serializedProperty.arraySize = length;
            }
            rect.x += indentWidth; rect.width -= indentWidth;
            return rect;
        }

        public static Rect DrawReorderableListIndex(Rect rect, SerializedProperty listProperty, int index)
        {
            if (GUI.Button(new Rect(rect.x, rect.y, reorderableListPrefixWidth, EditorGUIUtility.singleLineHeight), index.ToString("00"), EditorStyles.label))
            {
                DrawReorderMenu(listProperty, index).ShowAsContext();
            }
            rect.x += reorderableListPrefixWidth; rect.width -= reorderableListPrefixWidth;
            return rect;
        }
        public static GenericMenu DrawReorderMenu(SerializedProperty listProperty, int index)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Insert"), false, delegate
            {
                listProperty.InsertArrayElementAtIndex(index);
                listProperty.serializedObject.ApplyModifiedProperties();
            });
            menu.AddItem(new GUIContent("Delete"), false, delegate
            {
                listProperty.DeleteArrayElementAtIndex(index);
                listProperty.serializedObject.ApplyModifiedProperties();
            });
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Move to Top"), false, delegate
            {
                listProperty.MoveArrayElement(index, 0);
                listProperty.serializedObject.ApplyModifiedProperties();
            });
            menu.AddItem(new GUIContent("Move to Bottom"), false, delegate
            {
                listProperty.MoveArrayElement(index, listProperty.arraySize - 1);
                listProperty.serializedObject.ApplyModifiedProperties();
            });
            return menu;
        }

        public static Rect DrawReorderableListIndex(Rect rect, int index, SerializedObject serializedObject, params SerializedProperty[] listProperties)
        {
            if (GUI.Button(new Rect(rect.x, rect.y, reorderableListPrefixWidth, EditorGUIUtility.singleLineHeight), index.ToString("00"), EditorStyles.label))
            {
                DrawReorderMenu(index, serializedObject, listProperties).ShowAsContext();
            }
            rect.x += reorderableListPrefixWidth; rect.width -= reorderableListPrefixWidth;
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