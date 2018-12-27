/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-12-27 15:33:33
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity.PhysicsCompnent
{
    public class EZPhysicsBoneForce : MonoBehaviour
    {
        [SerializeField]
        private bool m_UseLocalDirection;
        public bool useLocalDirection { get { return m_UseLocalDirection; } }

        [SerializeField]
        private Vector3 m_Direction;
        public Vector3 direction { get { return m_Direction; } set { m_Direction = value; } }

        [SerializeField]
        private Vector3 m_Turbulence = new Vector3(0.1f, 0.02f, 0.1f);
        public Vector3 turbulence { get { return m_Turbulence; } set { m_Turbulence = value; } }

        [SerializeField]
        private float m_TurbulenceTimeCycle = 2f;
        public float turbulenceTimeCycle { get { return m_TurbulenceTimeCycle; } set { m_TurbulenceTimeCycle = Mathf.Max(0, value); } }

        [SerializeField, EZCurve(0, -1, 1, 2)]
        private AnimationCurve m_TurbulenceXCurve = AnimationCurve.Linear(0, -1, 1, 1);
        [SerializeField, EZCurve(0, -1, 1, 2)]
        private AnimationCurve m_TurbulenceYCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField, EZCurve(0, -1, 1, 2)]
        private AnimationCurve m_TurbulenceZCurve = AnimationCurve.EaseInOut(0, 1, 1, -1);

        public Vector3 outputForce { get; set; }

        private void Update()
        {
            float t = (Time.time % m_TurbulenceTimeCycle) / m_TurbulenceTimeCycle;
            Vector3 tbl = turbulence;
            tbl.x *= m_TurbulenceXCurve.Evaluate(t);
            tbl.y *= m_TurbulenceYCurve.Evaluate(t);
            tbl.z *= m_TurbulenceZCurve.Evaluate(t);
            if (useLocalDirection)
            {
                outputForce = transform.TransformDirection(direction + tbl);
            }
            else
            {
                outputForce = direction + tbl;
            }
        }
    }
}
