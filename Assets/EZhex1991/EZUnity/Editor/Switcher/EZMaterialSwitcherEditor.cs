/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-04-17 15:51:49
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZMaterialSwitcher))]
    public class EZMaterialSwitcherEditor : EZSwitcherEditor
    {
        private SerializedProperty m_Renderer;
        private SerializedProperty m_SharedMaterial;

        protected override void OnEnable()
        {
            m_Renderer = serializedObject.FindProperty("m_Renderer");
            m_SharedMaterial = serializedObject.FindProperty("m_SharedMaterial");
            base.OnEnable();
        }

        protected override void DrawOtherProperties()
        {
            EditorGUILayout.PropertyField(m_Renderer);
            EditorGUILayout.PropertyField(m_SharedMaterial);
        }
    }
}
