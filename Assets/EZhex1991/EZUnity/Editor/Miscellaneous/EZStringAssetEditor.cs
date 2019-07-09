/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-07 18:39:48
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZStringAsset))]
    public class EZStringAssetEditor : Editor
    {
        private EZStringAsset stringAsset;
        private SerializedProperty m_Items;
        private ReorderableList itemList;

        private Vector2 scrollPosition;

        private void OnEnable()
        {
            stringAsset = target as EZStringAsset;
            m_Items = serializedObject.FindProperty("m_Items");
            itemList = new ReorderableList(serializedObject, m_Items, true, false, true, true)
            {
                drawElementCallback = DrawItemListElement,
                drawHeaderCallback = DrawItemListHeader,
                onAddCallback = OnItemListAdd,
                elementHeightCallback = GetItemHeight,
            };
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, !serializedObject.isEditingMultipleObjects);

            serializedObject.Update();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            itemList.DoLayoutList();
            GUILayout.EndScrollView();

            serializedObject.ApplyModifiedProperties();
        }

        private float GetItemHeight(int index)
        {
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 5;
        }

        private void DrawItemListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect);
            float gridWidth = rect.width / 5; float margin = 5;
            rect.width = gridWidth - margin;
            EditorGUI.LabelField(rect, "Key");
            rect.x += gridWidth;
            rect.width = gridWidth * 2 - margin;
            EditorGUI.LabelField(rect, "CH");
            rect.x += gridWidth * 2;
            EditorGUI.LabelField(rect, "EN");
        }
        private void DrawItemListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty item = m_Items.GetArrayElementAtIndex(index);
            SerializedProperty key = item.FindPropertyRelative("m_Key");
            SerializedProperty ch = item.FindPropertyRelative("m_CH");
            SerializedProperty en = item.FindPropertyRelative("m_EN");

            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_Items, index);
            float keyWidth = rect.width / 5;
            float valueWidth = keyWidth * 2;
            float margin = 5;
            rect.height -= EditorGUIUtility.standardVerticalSpacing;

            Color originalBackgroundColor = GUI.backgroundColor;
            if (stringAsset.IsKeyDuplicate(key.stringValue))
            {
                GUI.backgroundColor = Color.red;
            }
            rect.width = keyWidth - margin;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), key, GUIContent.none);
            GUI.backgroundColor = originalBackgroundColor;

            rect.x += keyWidth;
            rect.width = valueWidth - margin;
            EditorGUI.PropertyField(rect, ch, GUIContent.none);
            rect.x += valueWidth;
            EditorGUI.PropertyField(rect, en, GUIContent.none);
        }
        private void OnItemListAdd(ReorderableList list)
        {
            int index = m_Items.arraySize;
            m_Items.InsertArrayElementAtIndex(index);
            SerializedProperty item = m_Items.GetArrayElementAtIndex(index);
            SerializedProperty key = item.FindPropertyRelative("m_Key");
            key.stringValue = "";
            list.index = index;
        }
    }
}
