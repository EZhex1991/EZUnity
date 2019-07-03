/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-07 16:37:12
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZPropertyList))]
    public class EZPropertyListEditor : Editor
    {
        protected SerializedProperty m_IsList;
        protected SerializedProperty m_Elements;
        protected ReorderableList elementList;

        protected virtual void OnEnable()
        {
            m_IsList = serializedObject.FindProperty("m_IsList");
            m_Elements = serializedObject.FindProperty("m_Elements");
            elementList = new ReorderableList(serializedObject, m_Elements, true, true, true, true)
            {
                drawHeaderCallback = DrawElementListHeader,
                drawElementCallback = DrawElementListElement
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.MonoBehaviourTitle(target as MonoBehaviour);

            elementList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawElementListHeader(Rect rect)
        {
            rect.y += 1;
            float offset = 45;
            m_IsList.boolValue = EditorGUI.ToggleLeft(new Rect(rect.x, rect.y, offset, rect.height), "List", m_IsList.boolValue);
            if (m_IsList.boolValue)
            {
                float width = (rect.width - offset) / 2, space = 5;
                rect.x += offset; rect.width = width - space;
                EditorGUI.LabelField(rect, "Type");
                rect.x += width;
                EditorGUI.LabelField(rect, "Value");
            }
            else
            {
                float width = (rect.width - offset) / 3, space = 5;
                rect.x += offset; rect.width = width - space;
                EditorGUI.LabelField(rect, "Type");
                rect.x += width;
                EditorGUI.LabelField(rect, "Key");
                rect.x += width;
                EditorGUI.LabelField(rect, "Value");
            }
        }
        protected void DrawElementListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_Elements, index);
            SerializedProperty element = m_Elements.GetArrayElementAtIndex(index);
            if (m_IsList.boolValue)
            {
                float width = rect.width / 2, space = 5;
                rect.width = width - space;
                rect.height = EditorGUIUtility.singleLineHeight;
                EZPropertyDrawer.DrawTypeOption(rect, element);
                rect.x += width;
                EZPropertyDrawer.DrawValueContent(rect, element);
            }
            else
            {
                EditorGUI.PropertyField(rect, element);
            }
        }
    }
}
