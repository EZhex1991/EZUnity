/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-22 20:17:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZAssetListRenamer))]
    public class EZAssetListRenamerEditor : Editor
    {
        private EZAssetListRenamer renamer;

        private SerializedProperty m_IndexStep;
        private SerializedProperty m_IndexOffset;
        private SerializedProperty m_IndexFormat;
        private SerializedProperty m_CaptureRegex;
        private SerializedProperty m_ObjectList;
        private ReorderableList objectList;

        private void OnEnable()
        {
            renamer = target as EZAssetListRenamer;
            m_IndexStep = serializedObject.FindProperty("indexStep");
            m_IndexOffset = serializedObject.FindProperty("indexOffset");
            m_IndexFormat = serializedObject.FindProperty("indexFormat");
            m_CaptureRegex = serializedObject.FindProperty("captureRegex");
            m_ObjectList = serializedObject.FindProperty("objectList");
            objectList = new ReorderableList(serializedObject, m_ObjectList, true, true, true, true)
            {
                drawHeaderCallback = DrawObjectListHeader,
                drawElementCallback = DrawObjectListElement,
            };
        }

        private void DrawObjectListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "ObjectList");
        }
        private void DrawObjectListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, objectList);
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, m_ObjectList.GetArrayElementAtIndex(index), GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject);
            serializedObject.Update();

            if (GUILayout.Button("Execute"))
            {
                renamer.Execute();
            }

            EditorGUILayout.PropertyField(m_IndexStep);
            EditorGUILayout.PropertyField(m_IndexOffset);
            EditorGUILayout.PropertyField(m_IndexFormat);
            EditorGUILayout.PropertyField(m_CaptureRegex);
            objectList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
