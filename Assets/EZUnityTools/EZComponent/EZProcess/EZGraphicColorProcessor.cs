/*
 * Author:      熊哲
 * CreateTime:  9/12/2017 4:50:56 PM
 * Description:
 * 
*/
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EZComponent.EZProcess
{
    [RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
    public class EZGraphicColorProcessor : EZColorProcess
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

        protected override void StartPhase(int index)
        {
            startValue = graphic.color;
            base.StartPhase(index);
        }
        protected override void UpdatePhase(float lerp)
        {
            base.UpdatePhase(lerp);
            graphic.color = value;
        }
    }
}