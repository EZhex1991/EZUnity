/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-08 14:31:43
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZStringDictionaryAsset))]
    public class EZStringDictionaryAssetEditor : EZDictionaryAssetEditor
    {
        private EZStringDictionaryAsset stringDictionary;

        protected override void OnEnable()
        {
            base.OnEnable();
            stringDictionary = target as EZStringDictionaryAsset;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Key Value Swap"))
            {
                KeyValueSwap();
            }
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

        protected override bool IsKeyDuplicate(SerializedProperty keyProperty)
        {
            return stringDictionary.IsKeyDuplicate(keyProperty.stringValue);
        }
    }
}
