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
        public const float digitWidth_3 = 35;
        public const float dragHandleWidth = 15;

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
            float indentWidth = list.count > 100 ? digitWidth_3 : digitWidth_2;
            if (list.draggable) indentWidth += dragHandleWidth;
            rect.x += indentWidth; rect.width -= indentWidth;
            return rect;
        }
        public static Rect DrawReorderableListCount(Rect rect, ReorderableList list)
        {
            float indentWidth = list.count > 100 ? digitWidth_3 : digitWidth_2;
            if (list.draggable) indentWidth += dragHandleWidth;
            Rect countRect = new Rect(rect);
            countRect.width = indentWidth - 5;
            if (list.serializedProperty != null)
            {
                int length = EditorGUI.DelayedIntField(countRect, list.count, EditorStyles.miniTextField);
                if (length != list.count)
                {
                    list.serializedProperty.arraySize = length;
                }
            }
            else
            {
                EditorGUI.LabelField(countRect, list.count.ToString(), EditorStyles.miniLabel);
            }
            rect.x += indentWidth; rect.width -= indentWidth;
            return rect;
        }

        public static Rect DrawReorderableListIndex(Rect rect, int index, ReorderableList list)
        {
            float labelWidth = list.count > 100 ? digitWidth_3 : digitWidth_2;
            if (list.serializedProperty != null)
            {
                if (GUI.Button(new Rect(rect.x, rect.y, labelWidth, EditorGUIUtility.singleLineHeight), index.ToString(), EditorStyles.label))
                {
                    DrawReorderMenu(index, list.serializedProperty).ShowAsContext();
                }
            }
            else
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, EditorGUIUtility.singleLineHeight), index.ToString());
            }
            rect.x += labelWidth; rect.width -= labelWidth;
            return rect;
        }
        private static GenericMenu DrawReorderMenu(int index, SerializedProperty listProperty)
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

        public static Rect DrawReorderableListIndex(Rect rect, int index, params SerializedProperty[] listProperties)
        {
            float labelWidth = listProperties[0].arraySize > 100 ? digitWidth_3 : digitWidth_2;
            if (GUI.Button(new Rect(rect.x, rect.y, labelWidth, EditorGUIUtility.singleLineHeight), index.ToString(), EditorStyles.label))
            {
                DrawReorderMenu(index, listProperties).ShowAsContext();
            }
            rect.x += labelWidth; rect.width -= labelWidth;
            return rect;
        }
        private static GenericMenu DrawReorderMenu(int index, params SerializedProperty[] listProperties)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Insert"), false, delegate
            {
                for (int i = 0; i < listProperties.Length; i++)
                {
                    listProperties[i].InsertArrayElementAtIndex(index);
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