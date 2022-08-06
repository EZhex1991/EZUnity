/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-10-28 14:45:23
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZEditorCurveHandler
    {
        public AnimationClip animationClip;

        private EditorCurveBinding curveBinding;
        private AnimationCurve curve;
        private AnimationCurve tempCurve;

        public bool hasRecords { get { return tempCurve != null && tempCurve.length != 0; } }

        public EZEditorCurveHandler(AnimationClip animationClip, Type type, string propertyName)
        {
            this.animationClip = animationClip;
            curveBinding = new EditorCurveBinding() { type = type, propertyName = propertyName };
            curve = AnimationUtility.GetEditorCurve(animationClip, curveBinding);
            if (curve == null)
            {
                curve = new AnimationCurve();
                AnimationUtility.SetEditorCurve(animationClip, curveBinding, curve);
            }
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
                AnimationUtility.SetEditorCurve(animationClip, curveBinding, curve);
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
