/* Author:          熊哲
 * CreateTime:      2018-06-07 18:39:48
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZUnity
{
    [CustomEditor(typeof(EZStringAsset))]
    public class EZStringAssetEditor : Editor
    {
        private SerializedProperty m_Items;
        private ReorderableList itemList;
        private int itemIndex { get { return itemList.index == -1 ? 0 : itemList.index; } }

        private int selection;
        private Vector2 scrollPosition;
        private Vector2 textPosition;
        private float lineHeight = EditorGUIUtility.singleLineHeight;

        private void OnEnable()
        {
            m_Items = serializedObject.FindProperty("m_Items");
            itemList = new ReorderableList(serializedObject, m_Items, true, false, true, true)
            {
                drawElementCallback = DrawItemListCallback,
                elementHeightCallback = GetItemHeight,
                elementHeight = lineHeight
            };
        }

        private float GetItemHeight(int index)
        {
            if (index == itemIndex) return lineHeight * 5;
            else return lineHeight;
        }

        private void DrawItemListCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty item = m_Items.GetArrayElementAtIndex(index);
            SerializedProperty key = item.FindPropertyRelative("key");
            SerializedProperty ch = item.FindPropertyRelative("ch");
            SerializedProperty en = item.FindPropertyRelative("en");
            float width = rect.width / 5; float margin = 3;
            if (index == itemIndex)
            {
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - margin, rect.height), key, GUIContent.none);
                rect.x += width;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, width * 2 - margin, rect.height), ch);
                rect.x += width * 2;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, width * 2 - margin, rect.height), en);
            }
            else
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, width - margin, rect.height), key.stringValue);
                rect.x += width;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, width * 2 - margin, rect.height), ch.stringValue);
                rect.x += width * 2;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, width * 2 - margin, rect.height), en.stringValue);
            }
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptTitle(target);
            serializedObject.Update();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            itemList.DoLayoutList();
            GUILayout.EndScrollView();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
