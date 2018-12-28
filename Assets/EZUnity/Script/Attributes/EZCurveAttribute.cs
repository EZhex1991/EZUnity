/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-12-24 13:22:59
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZUnity
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EZCurveRangeAttribute : PropertyAttribute
    {
        public Color color = Color.green;
        public Rect range;

        public EZCurveRangeAttribute(Rect range)
        {
            this.range = range;
        }
        public EZCurveRangeAttribute(float x, float y, float width, float height)
        {
            this.range = new Rect(x, y, width, height);
        }
        public EZCurveRangeAttribute(Rect range, Color color)
        {
            this.range = range;
            this.color = color;
        }
        public EZCurveRangeAttribute(float x, float y, float width, float height, Color color)
        {
            this.range = new Rect(x, y, width, height);
            this.color = color;
        }
    }
}
