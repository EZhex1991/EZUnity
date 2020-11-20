/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-10-28 15:08:25
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZAnimationCurveHandler
    {
        private AnimationCurve curve;
        private AnimationCurve tempCurve;

        public bool hasRecords { get { return tempCurve != null && tempCurve.length != 0; } }

        public EZAnimationCurveHandler(AnimationCurve curve)
        {
            this.curve = curve;
            tempCurve = new AnimationCurve();
        }

        public void Record(float time, float value)
        {
            tempCurve.AppendKey(time, value);
        }
        public void Apply()
        {
            if (tempCurve.length != 0)
            {
                curve.Replace(tempCurve);
            }
            tempCurve.Clear();
        }
        public void Discard()
        {
            tempCurve.Clear();
        }

        public float Evaluate(float time)
        {
            return curve.Evaluate(time);
        }
    }
}
