/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-08 14:31:43
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZStringDictionaryAsset))]
    public class EZStringDictionaryAssetEditor : Editor
    {
        private EZStringDictionaryAsset stringDictionary;
        private SerializedProperty m_Pairs;
        private ReorderableList pairList;

        private Vector2 scrollPosition;

        private void OnEnable()
        {
            stringDictionary = target as EZStringDictionaryAsset;
            m_Pairs = serializedObject.FindProperty("m_Pairs");
            pairList = new ReorderableList(serializedObject, m_Pairs, true, true, true, true)
            {
                drawElementCallback = DrawPairListElement,
                drawHeaderCallback = DrawPairListHeader,
                onAddCallback = OnPairListAdd,
            };
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptableObjectTitle(stringDictionary, !serializedObject.isEditingMultipleObjects);

            serializedObject.Update();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            pairList.DoLayoutList();
            GUILayout.EndScrollView();

            if (GUILayout.Button("Key Value Swap"))
            {
                KeyValueSwap();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPairListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect);
            rect.width /= 2;
            EditorGUI.LabelField(rect, "Key");
            rect.x += rect.width;
            EditorGUI.LabelField(rect, "Value");
        }
        private void DrawPairListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty pair = m_Pairs.GetArrayElementAtIndex(index);
            SerializedProperty key = pair.FindPropertyRelative("m_Key");
            SerializedProperty value = pair.FindPropertyRelative("m_Value");
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_Pairs, index);
            float width = rect.width / 2; float margin = 5;
            rect.width = width - margin;
            rect.height = EditorGUIUtility.singleLineHeight;
            Color originalBackgroundColor = GUI.backgroundColor;
            if (stringDictionary.IsKeyDuplicate(key.stringValue))
            {
                GUI.backgroundColor = Color.red;
            }
            EditorGUI.PropertyField(rect, key, GUIContent.none);
            rect.x += width;
            EditorGUI.PropertyField(rect, value, GUIContent.none);
            GUI.backgroundColor = originalBackgroundColor;

        }
        private void OnPairListAdd(ReorderableList list)
        {
            int index = m_Pairs.arraySize;
            m_Pairs.InsertArrayElementAtIndex(index);
            SerializedProperty pair = m_Pairs.GetArrayElementAtIndex(index);
            SerializedProperty key = pair.FindPropertyRelative("m_Key");
            SerializedProperty value = pair.FindPropertyRelative("m_Value");
            key.stringValue = "";
            value.stringValue = "";
        }

        public void KeyValueSwap()
        {
            for (int i = 0; i < m_Pairs.arraySize; i++)
            {
                SerializedProperty pair = m_Pairs.GetArrayElementAtIndex(i);
                SerializedProperty key = pair.FindPropertyRelative("m_Key");
                SerializedProperty value = pair.FindPropertyRelative("m_Value");
                string temp = key.stringValue;
                key.stringValue = value.stringValue;
                value.stringValue = temp;
            }
        }
    }
}
