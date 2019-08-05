/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-08 14:31:43
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.EZCollectionAsset
{
    [CustomEditor(typeof(EZMapAsset_String_String))]
    public class EZMapAssetEditor_String_String : EZMapAssetEditor
    {
        private EZMapAsset_String_String stringMapAsset;

        private Transform transform;

        protected override void OnEnable()
        {
            base.OnEnable();
            stringMapAsset = target as EZMapAsset_String_String;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Key Value Swap"))
            {
                KeyValueSwap();
            }
            EditorGUILayout.BeginHorizontal();
            transform = (Transform)EditorGUILayout.ObjectField("", transform, typeof(Transform), true);
            if (GUILayout.Button("Get Transform Hierarchy"))
            {
                if (transform != null)
                {
                    stringMapAsset.GetTransformHierarchy(transform);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public void KeyValueSwap()
        {
            for (int i = 0; i < m_Keys.arraySize; i++)
            {
                SerializedProperty key = m_Keys.GetArrayElementAtIndex(i);
                SerializedProperty value = m_Values.GetArrayElementAtIndex(i);
                string temp = key.stringValue;
                key.stringValue = value.stringValue;
                value.stringValue = temp;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
