/*
 * Author:      熊哲
 * CreateTime:  9/11/2017 6:45:02 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent.EZProcess
{
    public class EZColorProcess : _EZProcess<Color, ColorPhase>
    {
        [SerializeField]
        private Color m_Origin;
        public override Color origin { get { return m_Origin; } set { m_Origin = value; } }

        protected override void UpdatePhase(float lerp)
        {
            value = Color.Lerp(startValue, endValue, lerp);
        }
    }
}