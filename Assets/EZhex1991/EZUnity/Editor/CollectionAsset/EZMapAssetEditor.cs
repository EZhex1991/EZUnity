/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-05 11:41:57
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity.EZCollectionAsset
{
    [CustomEditor(typeof(EZMapAsset), true)]
    public class EZMapAssetEditor : Editor
    {
        protected EZMapAsset mapAsset;

        protected SerializedProperty m_Keys;
        protected SerializedProperty m_Values;
        protected ReorderableList itemList;

        protected Vector2 scrollPosition;
        protected int lastSelection;

        protected virtual void OnEnable()
        {
            mapAsset = target as EZMapAsset;
            m_Keys = serializedObject.FindProperty("m_Keys");
            m_Values = serializedObject.FindProperty("m_Values");
            itemList = new ReorderableList(serializedObject, m_Keys, true, true, true, true)
            {
                drawHeaderCallback = DrawItemListHeader,
                drawElementCallback = DrawItemListElement,
                onAddCallback = OnItemListAdd,
                onRemoveCallback = OnItemListRemove,
                onReorderCallback = OnItemListReorder,
                onSelectCallback = OnItemListSelect,
            };
        }
        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject);

            serializedObject.Update();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            itemList.DoLayoutList();
            GUILayout.EndScrollView();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void SplitRect(Rect rect, out Rect keyRect, out Rect valueRect)
        {
            float margin = 5;
            rect.width -= margin;
            keyRect = valueRect = new Rect(rect);
            keyRect.width *= mapAsset.keyRectWidth;
            valueRect.width *= 1 - mapAsset.keyRectWidth;

            keyRect.height = EditorGUIUtility.singleLineHeight;
            valueRect.x += keyRect.width + margin;
            valueRect.height = EditorGUIUtility.singleLineHeight;
        }

        protected void DrawItemListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.DrawReorderableListCount(rect, itemList);
            Rect keyRect, valueRect;
            SplitRect(rect, out keyRect, out valueRect);

            DrawKeyLabel(keyRect);
            DrawValueLabel(valueRect);
        }
        protected virtual void DrawKeyLabel(Rect rect)
        {
            EditorGUI.LabelField(rect, "Key");
        }
        protected virtual void DrawValueLabel(Rect rect)
        {
            EditorGUI.LabelField(rect, "Value");
        }

        protected void DrawItemListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty key = m_Keys.GetArrayElementAtIndex(index);
            while (m_Values.arraySize < m_Keys.arraySize)
            {
                m_Values.InsertArrayElementAtIndex(m_Values.arraySize);
            }
            SerializedProperty value = m_Values.GetArrayElementAtIndex(index);

            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, m_Keys, m_Values);
            Rect keyRect, valueRect;
            SplitRect(rect, out keyRect, out valueRect);

            Color originalBackgroundColor = GUI.backgroundColor;
            if (mapAsset.IsKeyDuplicate(index))
            {
                GUI.backgroundColor = Color.red;
            }
            DrawKeyProperty(keyRect, key);
            GUI.backgroundColor = originalBackgroundColor;

            DrawValueProperty(valueRect, value);
        }
        protected virtual void DrawKeyProperty(Rect rect, SerializedProperty keyProperty)
        {
            EditorGUI.PropertyField(rect, keyProperty, GUIContent.none);
        }
        protected virtual void DrawValueProperty(Rect rect, SerializedProperty valueProperty)
        {
            EditorGUI.PropertyField(rect, valueProperty, GUIContent.none);
        }

        protected virtual void OnItemListAdd(ReorderableList list)
        {
            int index = m_Keys.arraySize;
            m_Keys.InsertArrayElementAtIndex(index);
            while (m_Values.arraySize < m_Keys.arraySize)
            {
                m_Values.InsertArrayElementAtIndex(m_Values.arraySize);
            }
            list.index = index;
        }
        private void OnItemListRemove(ReorderableList list)
        {
            int index = list.index;
            if (index < 0) return;
            m_Keys.DeleteArrayElementAtIndex(index);
            m_Values.DeleteArrayElementAtIndex(index);
            if (list.index >= list.count) list.index--;
        }
        private void OnItemListSelect(ReorderableList list)
        {
            lastSelection = list.index;
        }
        private void OnItemListReorder(ReorderableList list)
        {
            m_Values.MoveArrayElement(lastSelection, list.index);
        }
    }
}
