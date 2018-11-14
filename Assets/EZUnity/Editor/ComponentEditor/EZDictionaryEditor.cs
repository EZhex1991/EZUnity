/* Author:          熊哲
 * CreateTime:      2018-05-07 16:37:12
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZUnity
{
    [CustomEditor(typeof(EZDictionary))]
    public class EZDictionaryEditor : Editor
    {
        protected SerializedProperty m_Elements;
        protected ReorderableList elementList;

        protected virtual void OnEnable()
        {
            m_Elements = serializedObject.FindProperty("m_Elements");
            elementList = new ReorderableList(serializedObject, m_Elements, true, true, true, true)
            {
                drawHeaderCallback = DrawElementListHeader,
                drawElementCallback = DrawElementListElement
            };
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptTitle(target);
            serializedObject.Update();
            elementList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawElementListHeader(Rect rect)
        {

        }
        protected void DrawElementListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_Elements, index);
            SerializedProperty element = elementList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element);
        }
    }
}
