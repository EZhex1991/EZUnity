/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-11-22 18:08:40
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    public static partial class EZEditorGUIUtility
    {
        public static float space = 5;
        public static float indexWidth = 30;
        public static float reorderableListHeaderIndent = 15;
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

        public static Rect DrawReorderableListHeaderIndex(Rect rect, bool draggable = true)
        {
            return DrawReorderableListHeaderIndex(rect, draggable ? (indexWidth + reorderableListHeaderIndent) : indexWidth);
        }
        public static Rect DrawReorderableListHeaderIndex(Rect rect, float width)
        {
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width, singleLineHeight), "NO.");
            rect.x += width; rect.width -= width;
            return rect;
        }

        public static Rect DrawReorderableListIndex(Rect rect, SerializedProperty property, int index)
        {
            return DrawReorderableListIndex(rect, property, index, indexWidth);
        }
        public static Rect DrawReorderableListIndex(Rect rect, SerializedProperty property, int index, float width)
        {
            if (GUI.Button(new Rect(rect.x, rect.y, width, singleLineHeight), index.ToString("00"), EditorStyles.label))
            {
                DrawReorderMenu(property, index).ShowAsContext();
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
    }
}