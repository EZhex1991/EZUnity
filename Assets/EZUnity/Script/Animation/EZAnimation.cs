/*
 * Author:      熊哲
 * CreateTime:  10/31/2017 3:25:39 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity.Animation
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
        int segmentIndex { get; }
        float time { get; }
        float value { get; }

        void StartSegment(int index);
        void Pause();
        void Resume();
        void Stop();
    }

    public delegate void OnAnimationEndAction();

    public abstract class EZAnimation<T, U> : MonoBehaviour, IEZAnimation
        where T : struct
        where U : EZAnimationSegment<T>
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

        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("m_PhaseList")]
        private List<U> m_Segments = new List<U>();
        public List<U> segments { get { return m_Segments; } set { m_Segments = value; } }

        public Status status { get; protected set; }
        public int segmentIndex { get; protected set; }
        public U segment { get; protected set; }

        public float time { get; protected set; }
        public float value { get; protected set; }

        public event OnAnimationEndAction onAnimationEndEvent;

        public virtual void StartSegment(int index = 0)
        {
            if (index >= segments.Count) return;
            status = Status.Running;
            segmentIndex = index;
            segment = segments[segmentIndex];
            OnSegmentStart();
        }
        protected virtual void ProcessSegment()
        {
            value = segment.duration <= 0 ? 1 : segment.curve.Evaluate(time);
            OnSegmentUpdate();
            if (time > segment.duration)
            {
                StopSegment();
            }
        }
        protected virtual void StopSegment()
        {
            OnSegmentStop();
            segmentIndex++;
            if (segmentIndex >= segments.Count)
            {
                if (onAnimationEndEvent != null) onAnimationEndEvent();
                if (loop)
                {
                    StartSegment(0);
                }
                else
                {
                    Stop();
                }
            }
            else
            {
                StartSegment(segmentIndex);
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
            segmentIndex = 0;
            segment = segments[segmentIndex];
            time = 0;
            value = 0;
        }

        protected virtual void OnSegmentStart()
        {
            time = 0;
        }
        protected abstract void OnSegmentUpdate();
        protected virtual void OnSegmentStop()
        {

        }

        public bool IsRunning()
        {
            return segment != null && status == Status.Running;
        }

        protected virtual void Start()
        {
            StartSegment(0);
        }
        protected virtual void OnEnable()
        {
            if (restartOnEnable)
            {
                StartSegment(0);
            }
        }
        protected void Update()
        {
            if (updateMode == AnimatorUpdateMode.AnimatePhysics) return;
            if (!IsRunning()) return;
            switch (updateMode)
            {
                case AnimatorUpdateMode.Normal:
                    time += Time.deltaTime;
                    break;
                case AnimatorUpdateMode.UnscaledTime:
                    time += Time.unscaledDeltaTime;
                    break;
            }
            ProcessSegment();
        }
        protected void FixedUpdate()
        {
            if (updateMode != AnimatorUpdateMode.AnimatePhysics) return;
            if (!IsRunning()) return;
            time += Time.fixedDeltaTime;
            ProcessSegment();
        }
    }
}