/* Author:          #AUTHORNAME#
 * CreateTime:      2023-07-01 16:30:39
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity.EZCollectionAsset;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZMaterialMap))]
    public class EZMaterialMapEditor : EZMapAssetEditor
    {
        protected EZMaterialMap materialMap;
        protected SerializedProperty m_TargetModelAsset;

        protected override void OnEnable()
        {
            base.OnEnable();
            materialMap = target as EZMaterialMap;
            m_TargetModelAsset = serializedObject.FindProperty(nameof(m_TargetModelAsset));
        }
        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject);

            serializedObject.Update();

            EZEditorGUIUtility.DoLayoutReorderableList(itemList, "Material Map");
            EditorGUILayout.PropertyField(m_TargetModelAsset);

            serializedObject.ApplyModifiedProperties();

            if (materialMap.externalObjectMap != null && materialMap.externalObjectMap.Count > 0)
            {
                EditorGUILayout.LabelField("External Object Map");
                EditorGUI.indentLevel++;
                foreach (var pair in materialMap.externalObjectMap)
                {
                    EditorGUILayout.ObjectField(pair.Key.name, pair.Value, pair.Key.type, false);
                }
                EditorGUI.indentLevel--;
            }

            if (materialMap.materialMap != null && materialMap.materialMap.Count > 0)
            {
                EditorGUILayout.LabelField("Material Map");
                EditorGUI.indentLevel++;
                foreach (var pair in materialMap.materialMap)
                {
                    EditorGUILayout.ObjectField(pair.Key, pair.Value, pair.Value.GetType(), false);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}
