/*
 * Author:      熊哲
 * CreateTime:  9/11/2017 11:03:16 AM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZComponent.EZProcess
{
    public enum LerpMode
    {
        Linear = 0,
        Square = 1,
        Sqrt = 2,
    }
    public class Phase<T>
    {
        [SerializeField]
        private T m_EndValue;
        public T endValue { get { return m_EndValue; } set { m_EndValue = value; } }

        [SerializeField]
        private float m_Duration;
        public float duration { get { return m_Duration; } set { m_Duration = value; } }

        [SerializeField]
        private float m_Interval;
        public float interval { get { return m_Interval; } set { m_Interval = value; } }

        [SerializeField]
        private LerpMode m_LerpMode;
        public LerpMode lerpMode { get { return m_LerpMode; } set { m_LerpMode = value; } }

        public Phase(T endValue, float duration, float interval = 0, LerpMode lerpMode = LerpMode.Linear)
        {
            this.m_EndValue = endValue;
            this.m_Duration = duration;
            this.m_Interval = interval;
            this.m_LerpMode = lerpMode;
        }
    }
    [Serializable]
    public class Vector2Phase : Phase<Vector2>
    {
        public Vector2Phase(Vector2 endValue, float duration, float interval = 0, LerpMode lerpMode = LerpMode.Linear) : base(endValue, duration, interval, lerpMode)
        {
        }
    }
    [Serializable]
    public class Vector3Phase : Phase<Vector3>
    {
        public Vector3Phase(Vector3 endValue, float duration, float interval = 0, LerpMode lerpMode = LerpMode.Linear) : base(endValue, duration, interval, lerpMode)
        {
        }
    }
    [Serializable]
    public class ColorPhase : Phase<Color>
    {
        public ColorPhase(Color endValue, float duration, float interval = 0, LerpMode lerpMode = LerpMode.Linear) : base(endValue, duration, interval, lerpMode)
        {
        }
    }

    public abstract class _EZProcess<T, U> : MonoBehaviour
        where U : Phase<T>
    {
        [SerializeField]
        private bool m_Loop;
        public bool loop { get { return m_Loop; } set { m_Loop = value; } }

        [SerializeField]
        private bool m_RestartOnEnable;
        public bool restartOnEnable { get { return m_RestartOnEnable; } set { m_RestartOnEnable = value; } }

        [SerializeField]
        private bool m_StartFromOrigin;
        public bool startFromOrigin { get { return m_StartFromOrigin; } set { m_StartFromOrigin = value; } }

        public abstract T origin { get; set; }

        [SerializeField]
        private List<U> m_PhaseList;
        public List<U> phaseList { get { return m_PhaseList; } set { m_PhaseList = value; } }

        public int currentIndex { get; private set; }
        public U currentPhase { get; private set; }

        public bool started { get; private set; }
        public float lerp { get; private set; }
        public T value { get; protected set; }
        public event Action<T> onPhaseUpdatedEvent;
        public event Action<int> onPhaseEndEvent;
        public event Action onProcessEndEvent;

        private bool updating;
        private float timeInPhase;
        protected T startValue;
        protected T endValue;

        public void AppendPhase(U phase)
        {
            phaseList.Add(phase);
        }

        protected virtual void StartPhase(int index = 0)
        {
            started = true;
            currentIndex = index;
            currentPhase = phaseList[currentIndex];
            updating = true;
            timeInPhase = 0;
            lerp = 0;
            if (currentIndex == 0 && startFromOrigin) startValue = origin;
            endValue = currentPhase.endValue;
            UpdatePhase(lerp);
            if (onPhaseUpdatedEvent != null) onPhaseUpdatedEvent(value);
        }
        protected abstract void UpdatePhase(float lerp);
        protected virtual void EndPhase()
        {
            startValue = currentPhase.endValue;
            currentIndex++;
            if (currentIndex >= phaseList.Count)
            {
                if (onProcessEndEvent != null) onProcessEndEvent();
                currentIndex = 0;
                if (!loop)
                {
                    started = false;
                    return;
                }
            }
            StartPhase(currentIndex);
        }

        protected virtual void OnEnable()
        {
            if (!started)
            {
                startValue = origin;
            }
            if (restartOnEnable || !started)
            {
                StartPhase(0);
            }
        }

        protected void Update()
        {
            if (!started) return;
            if (updating)
            {
                UpdatePhase(lerp);
                if (onPhaseUpdatedEvent != null) onPhaseUpdatedEvent(value);
                if (lerp >= 1) updating = false;
            }
            else if (timeInPhase > currentPhase.duration + currentPhase.interval)
            {
                if (onPhaseEndEvent != null) onPhaseEndEvent(currentIndex);
                EndPhase();
            }

            if (currentPhase.duration == 0 || timeInPhase > currentPhase.duration)
            {
                lerp = 1;
            }
            else
            {
                lerp = Mathf.Clamp01(timeInPhase / currentPhase.duration);
                switch (currentPhase.lerpMode)
                {
                    case LerpMode.Linear:
                        break;
                    case LerpMode.Square:
                        lerp = lerp * lerp;
                        break;
                    case LerpMode.Sqrt:
                        lerp = Mathf.Sqrt(lerp);
                        break;
                }
            }
            timeInPhase += Time.deltaTime;
        }
    }
}