/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-08 19:05:34
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZhex1991.EZUnity.Playables
{
    public class EZMaterialPropertyRange
    {
        public string propertyName;
    }

    public abstract class EZMaterialPropertyRange<T> : EZMaterialPropertyRange
    {
        public abstract T GetValue(float time);
        public abstract void SetBlockValue(MaterialPropertyBlock propertyBlock, float time);
    }

    [Serializable]
    public class EZMaterialFloatRange : EZMaterialPropertyRange<float>
    {
        public float startValue;
        public float endValue;

        public override float GetValue(float time)
        {
            return Mathf.Lerp(startValue, endValue, time);
        }
        public override void SetBlockValue(MaterialPropertyBlock propertyBlock, float time)
        {
            propertyBlock.SetFloat(propertyName, GetValue(time));
        }
    }
    [Serializable]
    public class EZMaterialVectorRange : EZMaterialPropertyRange<Vector4>
    {
        [EZSingleLineVector4]
        public Vector4 startValue;
        [EZSingleLineVector4]
        public Vector4 endValue;

        public override Vector4 GetValue(float time)
        {
            return Vector4.Lerp(startValue, endValue, time);
        }
        public override void SetBlockValue(MaterialPropertyBlock propertyBlock, float time)
        {
            propertyBlock.SetVector(propertyName, GetValue(time));
        }
    }
    [Serializable]
    public class EZMaterialColorRange : EZMaterialPropertyRange<Color>
    {
#if UNITY_2018_1_OR_NEWER
        [ColorUsage(true, true)]
#else
        [ColorUsage(true, true, 0, 8, 0.125f, 3)]
#endif
        public Color startValue = Color.white;
#if UNITY_2018_1_OR_NEWER
        [ColorUsage(true, true)]
#else
        [ColorUsage(true, true, 0, 8, 0.125f, 3)]
#endif
        public Color endValue = Color.white;

        public override Color GetValue(float time)
        {
            return Color.Lerp(startValue, endValue, time);
        }
        public override void SetBlockValue(MaterialPropertyBlock propertyBlock, float time)
        {
            propertyBlock.SetColor(propertyName, GetValue(time));
        }
    }
}
