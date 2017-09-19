/*
 * Author:      熊哲
 * CreateTime:  9/12/2017 3:45:25 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent.EZProcess
{
    public class EZVector2Process : _EZProcess<Vector2, Vector2Phase>
    {
        [SerializeField]
        private Vector2 m_Origin;
        public override Vector2 origin { get { return m_Origin; } set { m_Origin = value; } }

        protected override void UpdatePhase(float lerp)
        {
            value = Vector2.Lerp(startValue, endValue, lerp);
        }
    }
}