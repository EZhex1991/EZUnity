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
    public enum Status
    {
        Running = 1,
        Paused = 2,
        Stopped = 3
    }

    public interface IEZAnimation
    {
        Status status { get; }
        int currentIndex { get; }
        float time { get; }
        float frameValue { get; }

        void StartPhase(int index);
        void Pause();
        void Resume();
        void Stop();
    }

    public delegate void OnAnimationEndAction();

    public abstract class EZAnimation<T, U> : MonoBehaviour, IEZAnimation
        where T : struct
        where U : Phase<T>
    {
        [SerializeField]
        private bool m_Loop = false;
        public bool loop { get { return m_Loop; } set { m_Loop = value; } }

        [SerializeField]
        private bool m_RestartOnEnable = false;
        public bool restartOnEnable { get { return m_RestartOnEnable; } set { m_RestartOnEnable = value; } }

        [SerializeField]
        private AnimatorUpdateMode m_UpdateMode = AnimatorUpdateMode.Normal;
        public AnimatorUpdateMode updateMode { get { return m_UpdateMode; } set { m_UpdateMode = value; } }

        [SerializeField]
        private List<U> m_PhaseList = new List<U>();
        public List<U> phaseList { get { return m_PhaseList; } set { m_PhaseList = value; } }

        public Status status { get; protected set; }
        public int currentIndex { get; protected set; }
        public U currentPhase { get; protected set; }

        public float time { get; protected set; }
        public float frameValue { get; protected set; }

        public event OnAnimationEndAction onAnimationEndEvent;

        public virtual void StartPhase(int index = 0)
        {
            if (index >= phaseList.Count) return;
            status = Status.Running;
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
                if (loop)
                {
                    StartPhase(0);
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

        public void Pause()
        {
            if (status == Status.Running)
                status = Status.Paused;
        }
        public void Resume()
        {
            if (status == Status.Paused)
                status = Status.Running;
        }
        public void Stop()
        {
            status = Status.Stopped;
            currentIndex = 0;
            currentPhase = phaseList[currentIndex];
            time = 0;
            frameValue = 0;
        }

        protected virtual void OnPhaseStart()
        {
            time = 0;
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
            if (currentPhase == null || status != Status.Running) return;
            switch (updateMode)
            {
                case AnimatorUpdateMode.Normal:
                    time += Time.deltaTime;
                    break;
                case AnimatorUpdateMode.UnscaledTime:
                    time += Time.unscaledDeltaTime;
                    break;
            }
            frameValue = currentPhase.duration <= 0 ? 1 : currentPhase.curve.Evaluate(time);
            UpdatePhase();
            if (time > currentPhase.duration)
            {
                EndPhase();
            }
        }
    }
}