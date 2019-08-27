/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-20 13:45:47
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CustomEditor(typeof(EZTexturePipeline))]
    public class EZTexturePipelineEditor : EZTextureGeneratorEditor
    {
        protected SerializedProperty m_TextureProcessors;
        protected ReorderableList textureProcessorList;

        protected override void GetInputProperties()
        {
            m_TextureProcessors = serializedObject.FindProperty("textureProcessors");
            textureProcessorList = new ReorderableList(serializedObject, m_TextureProcessors)
            {
                drawHeaderCallback = DrawProcessorListHeader,
                drawElementCallback = DrawProcessorListElement,
            };
        }
        protected override void DrawInputSettings()
        {
            textureProcessorList.DoLayoutList();
        }

        private void DrawProcessorListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, m_TextureProcessors.displayName);
        }
        private void DrawProcessorListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = m_TextureProcessors.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(new Rect(rect) { height = EditorGUIUtility.singleLineHeight }, element);
        }
    }
}
