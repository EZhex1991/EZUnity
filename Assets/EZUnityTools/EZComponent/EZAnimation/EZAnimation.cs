/*
 * Author:      熊哲
 * CreateTime:  10/31/2017 3:25:39 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using UnityEngine;

namespace EZComponent.EZAnimation
{
    public interface IEZAnimation
    {
        int currentIndex { get; }
        float time { get; }
        float frameValue { get; }
    }

    public delegate void OnAnimationEndAction();

    public abstract class EZAnimation<T, U> : MonoBehaviour, IEZAnimation
        where T : struct
        where U : Phase<T>
    {
        [SerializeField]
        private bool m_Loop;
        public bool loop { get { return m_Loop; } set { m_Loop = value; } }

        [SerializeField]
        private bool m_RestartOnEnable;
        public bool restartOnEnable { get { return m_RestartOnEnable; } set { m_RestartOnEnable = value; } }

        [SerializeField]
        private List<U> m_PhaseList = new List<U>();
        public List<U> phaseList { get { return m_PhaseList; } set { m_PhaseList = value; } }

        public int currentIndex { get; protected set; }
        public U currentPhase { get; protected set; }

        public float time { get; protected set; }
        public float frameValue { get; protected set; }

        public event OnAnimationEndAction onAnimationEndEvent;

        public virtual void StartPhase(int index = 0)
        {
            enabled = true;
            if (index >= phaseList.Count) return;
            currentIndex = index;
            currentPhase = phaseList[currentIndex];
            OnPhaseStart();
        }
        protected virtual void EndPhase()
        {
            OnPhaseEnd();
            currentIndex++;
            if (currentIndex >= phaseList.Count)
            {
                if (onAnimationEndEvent != null) onAnimationEndEvent();
                currentIndex = 0;
                if (loop)
                {
                    StartPhase(currentIndex);
                }
                else
                {
                    Stop();
                }
            }
            else
            {
                StartPhase(currentIndex);
            }
        }
        public virtual void Stop()
        {
            enabled = false;
        }

        protected virtual void OnPhaseStart()
        {
            time = 0;
            frameValue = 0;
        }
        protected abstract void UpdatePhase();
        protected virtual void OnPhaseEnd()
        {

        }

        protected virtual void Start()
        {
            StartPhase(0);
        }
        protected virtual void OnEnable()
        {
            if (restartOnEnable)
            {
                StartPhase(0);
            }
        }
        protected void Update()
        {
            if (currentPhase == null) return;
            time += Time.deltaTime;
            frameValue = currentPhase.duration <= 0 ? 1 : currentPhase.curve.Evaluate(time);
            UpdatePhase();
            if (time > currentPhase.duration)
            {
                EndPhase();
            }
        }
    }
}