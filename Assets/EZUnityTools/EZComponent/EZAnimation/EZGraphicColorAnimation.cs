/*
 * Author:      熊哲
 * CreateTime:  11/2/2017 5:15:40 PM
 * Description:
 * 
*/
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EZComponent.EZAnimation
{
    [RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
    public class EZGraphicColorAnimation : EZAnimation<Color, ColorPhase>
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

        protected override void OnPhaseUpdate()
        {
            graphic.color = Color.Lerp(currentPhase.startValue, currentPhase.endValue, frameValue);
        }
    }
}