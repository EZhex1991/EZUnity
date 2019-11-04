/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-18 13:34:39
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.Rendering
{
    [DisallowMultipleComponent]
    public class EZDepthBasedOutlineEffect : EZImageEffect
    {
        private static class Uniforms
        {
            public static readonly int PropertyID_SampleDistance = Shader.PropertyToID("_SampleDistance");
            public static readonly int PropertyID_DepthSensitivity = Shader.PropertyToID("_DepthSensitivity");
            public static readonly int PropertyID_NormalSensitivity = Shader.PropertyToID("_NormalSensitivity");
            public static readonly int PropertyID_CoverColor = Shader.PropertyToID("_CoverColor");
            public static readonly int PropertyID_CoverStrength = Shader.PropertyToID("_CoverStrength");
            public static readonly int PropertyID_OutlineColor = Shader.PropertyToID("_OutlineColor");
            public static readonly int PropertyID_OutlineStrength = Shader.PropertyToID("_OutlineStrength");
        }

        public float sampleDistance = 0.5f;
        public float depthSensitivity = 10f;
        public float normalSensitivity = 10f;
        public Color coverColor = new Color(0, 0, 0, 1);
        public float coverStrength = 0;
        public Color outlineColor = new Color(0, 0, 1, 1);
        public float outlineStrength = 0;

        protected override void SetMaterial()
        {
            material.SetFloat(Uniforms.PropertyID_SampleDistance, sampleDistance);
            material.SetFloat(Uniforms.PropertyID_DepthSensitivity, depthSensitivity);
            material.SetFloat(Uniforms.PropertyID_NormalSensitivity, normalSensitivity);
            material.SetColor(Uniforms.PropertyID_CoverColor, coverColor);
            material.SetFloat(Uniforms.PropertyID_CoverStrength, coverStrength);
            material.SetColor(Uniforms.PropertyID_OutlineColor, outlineColor);
            material.SetFloat(Uniforms.PropertyID_OutlineStrength, outlineStrength);
        }

        private void Reset()
        {
            camera.depthTextureMode |= DepthTextureMode.DepthNormals;
        }
    }
}
