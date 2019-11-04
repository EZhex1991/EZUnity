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
            public const string PropertyID_SampleDistance = "_SampleDistance";
            public const string PropertyID_DepthSensitivity = "_DepthSensitivity";
            public const string PropertyID_NormalSensitivity = "_NormalSensitivity";
            public const string PropertyID_CoverColor = "_CoverColor";
            public const string PropertyID_CoverStrength = "_CoverStrength";
            public const string PropertyID_OutlineColor = "_OutlineColor";
            public const string PropertyID_OutlineStrength = "_OutlineStrength";
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
