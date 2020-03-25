/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-08 19:05:34
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZhex1991.EZUnity.Playables
{
    [Serializable]
    public struct EZMaterialFloatProperty
    {
        public string propertyName;
        public float value;
    }

    [Serializable]
    public struct EZMaterialVectorProperty
    {
        public string propertyName;
        [EZSingleLineVector4]
        public Vector4 value;
    }

    [Serializable]
    public struct EZMaterialColorProperty
    {
        public string propertyName;
#if UNITY_2018_1_OR_NEWER
        [ColorUsage(true, true)]
#else
        [ColorUsage(true, true, 0, 8, 0.125f, 3)]
#endif
        public Color value;
    }
}
