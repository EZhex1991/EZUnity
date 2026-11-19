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
        [ColorUsage(true, true)]
        public Color value;
    }
}
