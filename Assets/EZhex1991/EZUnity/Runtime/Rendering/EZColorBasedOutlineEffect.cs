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
        private const string PropertyName_OutlineTolerance = "_OutlineTolerance";
        private const string PropertyName_OutlineColor = "_OutlineColor";
        private const string PropertyName_OutlineThickness = "_OutlineThickness";

        public float outlineTolerance = 50f;
        public Color outlineColor = new Color(0, 0, 0, 1);
        public int outlineThickness = 1;

        protected override void SetMaterial()
        {
            material.SetFloat(PropertyName_OutlineTolerance, outlineTolerance);
            material.SetColor(PropertyName_OutlineColor, outlineColor);
            material.SetInt(PropertyName_OutlineThickness, outlineThickness);
        }

        private void OnValidate()
        {
            outlineTolerance = Mathf.Max(0, outlineTolerance);
            outlineThickness = Mathf.Max(0, outlineThickness);
        }
    }
}
