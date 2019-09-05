/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-27 16:41:00
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CustomEditor(typeof(EZTextureProcessor), true)]
    public class EZTextureProcessorEditor : EZTextureGeneratorEditor
    {
        protected EZTextureProcessor processor;
        protected RenderTexture previewRenderTexture;

        protected SerializedProperty m_Shader;

        protected override void GetInputProperties()
        {
            processor = target as EZTextureProcessor;
            previewRenderTexture = RenderTexture.GetTemporary(processor.previewResolution.x, processor.previewResolution.y);
            m_Shader = serializedObject.FindProperty("m_Shader");
        }
        protected override void DrawInputSettings()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(m_Shader);
            GUI.enabled = true;
            SerializedProperty iterator = m_Shader.Copy();
            while (iterator.Next(false))
            {
                EditorGUILayout.PropertyField(iterator, iterator.isExpanded);
            }
        }

        public override void DrawPreview(Rect previewArea)
        {
            EditorGUI.DrawPreviewTexture(previewArea, previewRenderTexture, null, processor.previewScaleMode);
        }
        protected override void RefreshPreview(bool checkResolution)
        {
            base.RefreshPreview(checkResolution);
            if (checkResolution)
            {
                if (previewRenderTexture.width != generator.previewResolution.x || previewRenderTexture.height != generator.previewResolution.y)
                {
                    RenderTexture.ReleaseTemporary(previewRenderTexture);
                    previewRenderTexture = RenderTexture.GetTemporary(generator.previewResolution.x, generator.previewResolution.y);
                }
            }
        }
        protected override void RefreshPreview()
        {
            base.RefreshPreview();
            processor.ProcessTexture(processor.inputTexture, previewRenderTexture);
        }
    }
}
