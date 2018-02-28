/* Author:          熊哲
 * CreateTime:      2018-02-26 18:39:45
 * Orgnization:     #ORGNIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZFramework.XLuaExtension
{
    [System.Serializable]
    public class Injection
    {
        public bool isObjectType = true;
        public string typeName = typeof(UnityEngine.GameObject).FullName;
        public string key;
        public Object value;
        public NonObject nonObjectValue;
    }

    [System.Serializable]
    public struct NonObject
    {
        public int intValue;
        public float floatValue;
        public bool boolValue;
        public string stringValue;
        public Vector2 v2Value;
        public Vector3 v3Value;
        public Vector4 v4Value;
        public AnimationCurve animationCurveValue;
    }
}