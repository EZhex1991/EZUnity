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
        public static float width;
        public static float GetDigitWidth(float number)
        {
            return ((int)Mathf.Log10(number) + 1) * 8 + 5;
        }

        public const float marginHorizontal = 2;
        public const float dragHandleWidth = 15;
        public const float countRectWidth = 48;
        public const float verticalOffset = 2f;

        public readonly static GUIStyle rightAlignedLabel = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleRight
        };

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

        public static void DoLayoutReorderableList(ReorderableList list)
        {
            DoLayoutReorderableList(list, list.serializedProperty.displayName);
        }
        public static void DoLayoutReorderableList(ReorderableList list, string label)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            Rect countRect = new Rect(rect.x + rect.width - countRectWidth, rect.y + 2, countRectWidth, rect.height);
            int count = EditorGUI.DelayedIntField(countRect, GUIContent.none, list.count);
            if (count != list.count)
            {
                list.serializedProperty.arraySize = count;
            }
            if (list.serializedProperty.isExpanded = EditorGUI.Foldout(rect, list.serializedProperty.isExpanded, label, true))
            {
                list.DoLayoutList();
            }
        }

        public static Rect CalcReorderableListHeaderRect(Rect rect, ReorderableList list)
        {
            float indentWidth = GetDigitWidth(list.count - 1);
            if (list.draggable) indentWidth += dragHandleWidth;
            rect.x += indentWidth; rect.width -= indentWidth;
            return rect;
        }

        public static Rect DrawReorderableListIndex(Rect rect, int index, ReorderableList list)
        {
            return DrawReorderableListIndex(rect, index, list.serializedProperty);
        }
        public static Rect DrawReorderableListIndex(Rect rect, int index, params SerializedProperty[] listProperties)
        {
            rect.y += verticalOffset;
            rect.height -= verticalOffset;
            float labelWidth = GetDigitWidth(listProperties[0].arraySize - 1);
            if (GUI.Button(new Rect(rect) { width = labelWidth - marginHorizontal, height = EditorGUIUtility.singleLineHeight }, index.ToString(), rightAlignedLabel))
            {
                DrawReorderMenu(index, listProperties).ShowAsContext();
            }
            rect.x += labelWidth; rect.width -= labelWidth;
            return rect;
        }
        private static GenericMenu DrawReorderMenu(int index, params SerializedProperty[] listProperties)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Insert Above"), false, delegate
            {
                for (int i = 0; i < listProperties.Length; i++)
                {
                    listProperties[i].InsertArrayElementAtIndex(index);
                    listProperties[i].serializedObject.ApplyModifiedProperties();
                }
            });
            menu.AddItem(new GUIContent("Insert Below"), false, delegate
            {
                for (int i = 0; i < listProperties.Length; i++)
                {
                    listProperties[i].InsertArrayElementAtIndex(index + 1);
                    listProperties[i].serializedObject.ApplyModifiedProperties();
                }
            });
            menu.AddItem(new GUIContent("Delete"), false, delegate
            {
                for (int i = 0; i < listProperties.Length; i++)
                {
                    listProperties[i].DeleteArrayElementAtIndex(index);
                    listProperties[i].serializedObject.ApplyModifiedProperties();
                }
            });
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Move to Top"), false, delegate
            {
                for (int i = 0; i < listProperties.Length; i++)
                {
                    listProperties[i].MoveArrayElement(index, 0);
                    listProperties[i].serializedObject.ApplyModifiedProperties();
                }
            });
            menu.AddItem(new GUIContent("Move to Bottom"), false, delegate
            {
                for (int i = 0; i < listProperties.Length; i++)
                {
                    listProperties[i].MoveArrayElement(index, listProperties[i].arraySize - 1);
                    listProperties[i].serializedObject.ApplyModifiedProperties();
                }
            });
            return menu;
        }

        public static Vector2 MinMaxSliderV2(Rect position, Vector2 value, float limitMin, float limitMax)
        {
            float valueRectWidth = 50f;
            float margin = 5f;
            float sliderRectWidth = position.width - (valueRectWidth + margin) * 2f;

            position.width = valueRectWidth;
            value.x = EditorGUI.FloatField(position, value.x);

            position.x += valueRectWidth + margin;
            position.width = sliderRectWidth;
            EditorGUI.MinMaxSlider(position, ref value.x, ref value.y, limitMin, limitMax);

            position.x += sliderRectWidth + margin;
            position.width = valueRectWidth;
            value.y = EditorGUI.FloatField(position, value.y);

            value.x = Mathf.Clamp(value.x, limitMin, limitMax);
            value.y = Mathf.Clamp(value.y, value.x, limitMax);
            return value;
        }
        // zw components will leave unchanged
        public static Vector4 MinMaxSliderV4(Rect position, Vector4 value)
        {
            Vector2 valueXY = MinMaxSliderV2(position, value, value.z, value.w);
            value.x = valueXY.x;
            value.y = valueXY.y;
            return value;
        }
        // zw components will leave unchanged
        public static Vector4 MinMaxSliderV4(Rect position, Vector4 value, float limitMin, float limitMax)
        {
            Vector2 valueXY = MinMaxSliderV2(position, value, limitMin, limitMax);
            value.x = valueXY.x;
            value.y = valueXY.y;
            return value;
        }

        public static void GUIDrawSprite(Rect rect, Sprite sprite)
        {
            Rect spriteRect = sprite.rect;
            Texture2D texture = sprite.texture;
            Rect texCoords = new Rect(spriteRect.x / texture.width, spriteRect.y / texture.height, spriteRect.width / texture.width, spriteRect.height / texture.height);
            GUI.DrawTextureWithTexCoords(rect, texture, texCoords);
        }
    }
}