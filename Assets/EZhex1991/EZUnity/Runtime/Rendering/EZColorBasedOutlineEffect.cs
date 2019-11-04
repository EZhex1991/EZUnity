/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-27 19:09:20
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.Rendering
{
    [DisallowMultipleComponent]
    public class EZColorBasedOutlineEffect : EZImageEffect
    {
        private static class Uniforms
        {
            public static readonly int PropertyID_GrayWeight = Shader.PropertyToID("_GrayWeight");
            public static readonly int PropertyID_Tolerance = Shader.PropertyToID("_Tolerance");
            public static readonly int PropertyID_OutlineColor = Shader.PropertyToID("_OutlineColor");
            public static readonly int PropertyID_OutlineThickness = Shader.PropertyToID("_OutlineThickness");
        }

        public Color grayWeight = new Color(0.299f, 0.587f, 0.114f);
        [Range(0, 255)]
        public float tolerance = 50f;
        public Color outlineColor = new Color(0, 0, 0, 0.5f);
        public int outlineThickness = 1;

        protected override void SetMaterial()
        {
            material.SetVector(Uniforms.PropertyID_GrayWeight, grayWeight);
            material.SetFloat(Uniforms.PropertyID_Tolerance, tolerance);
            material.SetColor(Uniforms.PropertyID_OutlineColor, outlineColor);
            material.SetInt(Uniforms.PropertyID_OutlineThickness, outlineThickness);
        }

        private void OnValidate()
        {
            outlineThickness = Mathf.Max(0, outlineThickness);
        }
    }
}
