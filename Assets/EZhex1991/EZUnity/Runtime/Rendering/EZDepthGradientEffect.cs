/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-18 13:34:56
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.Rendering
{
    [DisallowMultipleComponent]
    public class EZDepthGradientEffect : EZImageEffect
    {
        private static class Uniforms
        {
            public static readonly int PropertyID_ColorNear = Shader.PropertyToID("_ColorNear");
            public static readonly int PropertyID_ColorFar = Shader.PropertyToID("_ColorFar");
            public static readonly int PropertyID_GradientPower = Shader.PropertyToID("_GradientPower");
            public static readonly int PropertyID_GradientSoftness = Shader.PropertyToID("_GradientSoftness");
        }

        [ColorUsage(true)]
        public Color colorNear = Color.white;
        [ColorUsage(true)]
        public Color colorFar = new Color(0.9f, 1, 1, 1);
        [Range(0.1f, 128)]
        public float gradientPower = 1f;
        [EZMinMax]
        public Vector4 gradientSoftness = new Vector4(0, 1, 0, 1);

        protected override void SetMaterial()
        {
            material.SetColor(Uniforms.PropertyID_ColorNear, colorNear);
            material.SetColor(Uniforms.PropertyID_ColorFar, colorFar);
            material.SetFloat(Uniforms.PropertyID_GradientPower, gradientPower);
            material.SetVector(Uniforms.PropertyID_GradientSoftness, gradientSoftness);
        }

        private void Reset()
        {
            camera.depthTextureMode |= DepthTextureMode.Depth;
        }
    }
}
