/*
 * Author:      熊哲
 * CreateTime:  9/12/2017 4:45:05 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent.EZProcess
{
    public class EZVector3Process : _EZProcess<Vector3, Vector3Phase>
    {
        [SerializeField]
        private Vector3 m_Origin;
        public override Vector3 origin { get { return m_Origin; } set { m_Origin = value; } }

        protected override void UpdatePhase(float lerp)
        {
            value = Vector3.Lerp(startValue, endValue, lerp);
        }
    }
}