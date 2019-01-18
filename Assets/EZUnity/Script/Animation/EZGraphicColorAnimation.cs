/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-11-02 17:15:40
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EZUnity.Animation
{
    [RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
    public class EZGraphicColorAnimation : EZAnimation<Color, EZColorAnimationSegment>
    {
        [NonSerialized]
        private Graphic m_Graphic;
        private Graphic graphic
        {
            get
            {
                if (m_Graphic == null)
                {
                    m_Graphic = GetComponent<Graphic>();
                }
                return m_Graphic;
            }
        }

        protected override void OnSegmentUpdate()
        {
            graphic.color = Color.Lerp(segment.startValue, segment.endValue, value);
        }
    }
}