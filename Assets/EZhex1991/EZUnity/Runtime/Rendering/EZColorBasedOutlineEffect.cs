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
        private const string PropertyName_GrayWeight = "_GrayWeight";
        private const string PropertyName_Tolerance = "_Tolerance";
        private const string PropertyName_OutlineColor = "_OutlineColor";
        private const string PropertyName_OutlineThickness = "_OutlineThickness";

        public Color grayWeight = new Color(0.299f, 0.587f, 0.114f);
        [Range(0, 255)]
        public float tolerance = 50f;
        public Color outlineColor = new Color(0, 0, 0, 0.5f);
        public int outlineThickness = 1;

        protected override void SetMaterial()
        {
            material.SetVector(PropertyName_GrayWeight, grayWeight);
            material.SetFloat(PropertyName_Tolerance, tolerance);
            material.SetColor(PropertyName_OutlineColor, outlineColor);
            material.SetInt(PropertyName_OutlineThickness, outlineThickness);
        }

        private void OnValidate()
        {
            outlineThickness = Mathf.Max(0, outlineThickness);
        }
    }
}
