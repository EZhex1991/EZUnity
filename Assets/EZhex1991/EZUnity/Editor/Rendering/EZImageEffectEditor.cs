/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-18 11:01:30
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.Rendering
{
    [CustomEditor(typeof(EZImageEffect), true)]
    public class EZImageEffectEditor : Editor
    {
        private EZImageEffect effect;
        private MaterialEditor materialEditor;

        private void OnEnable()
        {
            effect = target as EZImageEffect;
            materialEditor = CreateEditor(effect.material) as MaterialEditor;
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.MonoBehaviourTitle(effect);
            effect.camera.depthTextureMode = (DepthTextureMode)EditorGUILayout.EnumFlagsField("Camera Depth Texture Mode", effect.camera.depthTextureMode);
            materialEditor.PropertiesGUI();
        }
    }
}
