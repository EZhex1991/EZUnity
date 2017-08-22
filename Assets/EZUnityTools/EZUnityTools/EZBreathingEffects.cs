/*
 * Author:      熊哲
 * CreateTime:  8/17/2017 11:59:45 AM
 * Description:
 * 
*/
using UnityEngine;

namespace EZUnityTools
{
    public abstract class EZBreathingEffects : MonoBehaviour
    {
        [SerializeField]
        private float m_TransitionDuration1 = 1;
        public float transitionDuration1 { get { return m_TransitionDuration1; } set { m_TransitionDuration1 = value; } }

        [SerializeField]
        private float m_TransitionDuration2 = 1;
        public float transitionDuration2 { get { return m_TransitionDuration2; } set { m_TransitionDuration2 = value; } }

        [SerializeField]
        private float m_Delay = 0;
        public float delay { get { return m_Delay; } set { m_Delay = value; } }

        [SerializeField]
        private float m_Interval = 0.2f;
        public float interval { get { return m_Interval; } set { m_Interval = value; } }

        private float time;

        protected void OnEnable()
        {
            time = -delay;
        }

        protected void Update()
        {
            time = (time + Time.deltaTime) % (transitionDuration1 + transitionDuration2 + interval);
            float lerp = time <= transitionDuration1 ? time / transitionDuration1 : 1 - (time - transitionDuration1) / transitionDuration2;
            DoEffects(lerp);
        }

        public abstract void DoEffects(float lerp);
    }
}