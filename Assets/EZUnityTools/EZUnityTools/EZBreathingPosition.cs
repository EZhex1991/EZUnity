/*
 * Author:      熊哲
 * CreateTime:  8/28/2017 4:17:27 PM
 * Description:
 * 
*/
using System;
using UnityEngine;

namespace EZUnityTools
{
    public class EZBreathingPosition : EZBreathingEffects
    {
        [SerializeField]
        private Vector3 m_Position1 = Vector3.zero;
        public Vector3 position1 { get { return m_Position1; } set { m_Position1 = value; } }

        [SerializeField]
        private Vector3 m_Position2 = Vector3.up;
        public Vector3 position2 { get { return m_Position2; } set { m_Position2 = value; } }

        public override void DoEffects(float lerp)
        {
            transform.localPosition = Vector3.Lerp(position1, position2, lerp);
        }
    }
}