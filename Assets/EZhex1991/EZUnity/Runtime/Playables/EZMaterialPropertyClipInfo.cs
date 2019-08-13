/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-08 19:05:34
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZhex1991.EZUnity.Playables
{
    public class EZMaterialPropertyClipInfo
    {
        public string propertyName;
    }

    public abstract class EZMaterialPropertyClipInfo<T> : EZMaterialPropertyClipInfo
    {
        public T startValue;
        public T endValue;

        public abstract T GetValue(float time);
        public abstract void SetBlockValue(MaterialPropertyBlock propertyBlock, float time);
    }

    [Serializable]
    public class EZMaterialFloatClipInfo : EZMaterialPropertyClipInfo<float>
    {
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
    public class EZMaterialVectorClipInfo : EZMaterialPropertyClipInfo<Vector4>
    {
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
    public class EZMaterialColorClipInfo : EZMaterialPropertyClipInfo<Color>
    {
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
