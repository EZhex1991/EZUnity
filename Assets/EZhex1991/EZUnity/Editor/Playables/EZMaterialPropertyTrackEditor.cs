/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-03-25 18:19:30
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.Playables
{
    [CustomEditor(typeof(EZMaterialPropertyTrack))]
    public class EZMaterialPropertyTrackEditor : EZMaterialPropertyClipEditor
    {
        private SerializedProperty m_MaterialIndex;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_MaterialIndex = m_Template.FindPropertyRelative("materialIndex");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, false);

            EditorGUILayout.PropertyField(m_MaterialIndex);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Float Properties", EditorStyles.boldLabel);
            floatPropertyList.DoLayoutList();
            EditorGUILayout.LabelField("Color Properties", EditorStyles.boldLabel);
            colorPropertyList.DoLayoutList();
            EditorGUILayout.LabelField("Vector Properties", EditorStyles.boldLabel);
            vectorPropertyList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
