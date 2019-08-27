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
        private const string PropertyName_SampleDistance = "_SampleDistance";
        private const string PropertyName_DepthSensitivity = "_DepthSensitivity";
        private const string PropertyName_NormalSensitivity = "_NormalSensitivity";
        private const string PropertyName_CoverColor = "_CoverColor";
        private const string PropertyName_CoverStrength = "_CoverStrength";
        private const string PropertyName_OutlineColor = "_OutlineColor";
        private const string PropertyName_OutlineStrength = "_OutlineStrength";

        public float sampleDistance = 0.5f;
        public float depthSensitivity = 10f;
        public float normalSensitivity = 10f;
        public Color coverColor = new Color(0, 0, 0, 1);
        public float coverStrength = 0;
        public Color outlineColor = new Color(0, 0, 1, 1);
        public float outlineStrength = 0;

        protected override void SetMaterial()
        {
            material.SetFloat(PropertyName_SampleDistance, sampleDistance);
            material.SetFloat(PropertyName_DepthSensitivity, depthSensitivity);
            material.SetFloat(PropertyName_NormalSensitivity, normalSensitivity);
            material.SetColor(PropertyName_CoverColor, coverColor);
            material.SetFloat(PropertyName_CoverStrength, coverStrength);
            material.SetColor(PropertyName_OutlineColor, outlineColor);
            material.SetFloat(PropertyName_OutlineStrength, outlineStrength);
        }

        private void Reset()
        {
            camera.depthTextureMode |= DepthTextureMode.DepthNormals;
        }
    }
}
