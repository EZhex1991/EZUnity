/*
 * Author:      熊哲
 * CreateTime:  8/17/2017 11:59:45 AM
 * Description:
 * 
*/
using UnityEngine;

namespace EZUnityTools
{
    public class EZBreathingScale : EZBreathingEffects
    {
        [SerializeField]
        private Vector3 m_Scale1 = Vector3.one * 0.5f;
        public Vector3 scale1 { get { return m_Scale1; } set { m_Scale1 = value; } }

        [SerializeField]
        private Vector3 m_Scale2 = Vector3.one;
        public Vector3 scale2 { get { return m_Scale2; } set { m_Scale2 = value; } }

        public override void DoEffects(float lerp)
        {
            transform.localScale = Vector3.Lerp(scale1, scale2, lerp);
        }
    }
}