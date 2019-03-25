/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-12-11 17:39:15
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZUnity.Animation
{
    [Serializable]
    public class EZAnimationSegment
    {
        [SerializeField]
        private float m_Duration = 1f;
        public float duration { get { return m_Duration; } set { m_Duration = value >= 0 ? value : 0; } }
        [SerializeField]
        private AnimationCurve m_Curve = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve curve { get { return m_Curve; } set { m_Curve = value; } }
    }
}