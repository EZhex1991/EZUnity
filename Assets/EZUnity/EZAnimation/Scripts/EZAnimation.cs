/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-10-31 15:25:39
 * Organization:    #ORGANIZATION#
 * Description:     
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
        float process { get; }

        void StartSegment(int index);

        void Play();
        void Pause();
        void Resume();
        void Stop();
    }

    public delegate void OnAnimationEndAction();

    public abstract class EZAnimation<T> : MonoBehaviour, IEZAnimation
        where T : EZAnimationSegment
    {
        [SerializeField]
        protected bool m_Loop = true;
        public bool loop { get { return m_Loop; } set { m_Loop = value; } }

        [SerializeField]
        protected bool m_PlayOnAwake = true;
        public bool playOnAwake { get { return m_PlayOnAwake; } set { m_PlayOnAwake = value; } }

        [SerializeField]
        protected bool m_RestartOnEnable = false;
        public bool restartOnEnable { get { return m_RestartOnEnable; } set { m_RestartOnEnable = value; } }

        [SerializeField]
        protected AnimatorUpdateMode m_UpdateMode = AnimatorUpdateMode.Normal;
        public AnimatorUpdateMode updateMode { get { return m_UpdateMode; } set { m_UpdateMode = value; } }

        [SerializeField]
        protected Status m_Status = Status.Stopped;
        public Status status { get { return m_Status; } protected set { m_Status = value; } }
        [SerializeField]
        protected int m_SegmentIndex;
        public int segmentIndex { get { return m_SegmentIndex; } protected set { m_SegmentIndex = value; } }
        [SerializeField]
        protected float m_Time;
        public float time { get { return m_Time; } protected set { m_Time = value; } }

        [SerializeField]
        protected List<T> m_Segments = new List<T>();
        public List<T> segments { get { return m_Segments; } set { m_Segments = value; } }

        public T segment { get { return segments[segmentIndex]; } }
        public float process { get; protected set; }

        public event OnAnimationEndAction onAnimationEndEvent;

        public virtual void StartSegment(int index = 0)
        {
            if (index >= segments.Count) return;
            status = Status.Running;
            segmentIndex = index;
            time = 0;
            OnSegmentStart();
            ProcessSegment();
        }
        protected virtual void ProcessSegment()
        {
            process = segment.duration <= 0 ? 1 : segment.curve.Evaluate(time);
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

        public void Play()
        {
            StartSegment(0);
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
            time = 0;
            process = 0;
        }

        protected virtual void OnSegmentStart()
        {
        }
        protected abstract void OnSegmentUpdate();
        protected virtual void OnSegmentStop()
        {

        }

        public bool IsRunning()
        {
            return segment != null && status == Status.Running;
        }

        protected virtual void Awake()
        {
            if (playOnAwake) Play();
        }
        protected virtual void OnEnable()
        {
            if (restartOnEnable) Play();
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