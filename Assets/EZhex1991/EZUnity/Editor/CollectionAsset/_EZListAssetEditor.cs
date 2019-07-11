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

        protected SerializedProperty m_Keys;
        protected ReorderableList itemList;

        protected Vector2 scrollPosition;

        protected virtual void OnEnable()
        {
            listAsset = target as EZListAsset;
            m_Keys = serializedObject.FindProperty("m_Keys");
            itemList = new ReorderableList(serializedObject, m_Keys, true, true, true, true)
            {
                drawHeaderCallback = DrawItemListHeader,
                drawElementCallback = DrawItemListElement,
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

        protected void DrawItemListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.DrawReorderableListCount(rect, itemList);
            EditorGUI.LabelField(rect, "Elements");
        }
        protected void DrawItemListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty key = m_Keys.GetArrayElementAtIndex(index);

            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, serializedObject, m_Keys);
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, key, GUIContent.none);
        }
    }
}
