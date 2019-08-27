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
        private const string PropertyName_ColorNear = "_ColorNear";
        private const string PropertyName_ColorFar = "_ColorFar";
        private const string PropertyName_GradientPower = "_GradientPower";
        private const string PropertyName_GradientSoftness = "_GradientSoftness";

        [ColorUsage(true)]
        public Color colorNear = Color.white;
        [ColorUsage(true)]
        public Color colorFar = new Color(0.9f, 1, 1, 1);
        [Range(0.1f, 128)]
        public float gradientPower = 1f;
        [EZMinMaxSlider]
        public Vector4 gradientSoftness = new Vector4(0, 1, 0, 1);

        protected override void SetMaterial()
        {
            material.SetColor(PropertyName_ColorNear, colorNear);
            material.SetColor(PropertyName_ColorFar, colorFar);
            material.SetFloat(PropertyName_GradientPower, gradientPower);
            material.SetVector(PropertyName_GradientSoftness, gradientSoftness);
        }

        private void Reset()
        {
            camera.depthTextureMode |= DepthTextureMode.Depth;
        }
    }
}
