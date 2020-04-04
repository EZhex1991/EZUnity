/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-11 14:47:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity.EZCollectionAsset
{
    [CustomEditor(typeof(EZListAsset), true)]
    public class EZListAssetEditor : Editor
    {
        protected EZListAsset listAsset;

        protected SerializedProperty m_Elements;
        protected ReorderableList elementList;

        protected Vector2 scrollPosition;

        protected virtual void OnEnable()
        {
            listAsset = target as EZListAsset;
            m_Elements = serializedObject.FindProperty("m_Elements");
            elementList = new ReorderableList(serializedObject, m_Elements, true, true, true, true)
            {
                drawHeaderCallback = DrawElementListHeader,
                drawElementCallback = DrawElementListElement,
            };
        }
        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject);

            serializedObject.Update();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            elementList.DoLayoutList();
            GUILayout.EndScrollView();

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawElementListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.DrawReorderableListCount(rect, elementList);
            EditorGUI.LabelField(rect, "Elements");
        }
        protected void DrawElementListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty key = m_Elements.GetArrayElementAtIndex(index);

            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, elementList);
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, key, GUIContent.none);
        }
    }
}
