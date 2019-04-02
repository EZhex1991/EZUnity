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
        float segmentTime { get; }
        float segmentProcess { get; }

        void StartSegment(int index);
        void Process(float time);

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
        protected List<T> m_Segments = new List<T>();
        public List<T> segments { get { return m_Segments; } set { m_Segments = value; } }
        [SerializeField]
        protected float m_Time;
        public float time { get { return m_Time; } set { m_Time = value; } }

        public int segmentIndex { get; private set; }
        public float segmentTime { get; private set; }
        public T activeSegment { get { return segments[segmentIndex]; } }
        public float segmentProcess { get; private set; }

        public event OnAnimationEndAction onAnimationEndEvent;

        public virtual void StartSegment(int index = 0)
        {
            if (index >= segments.Count) return;
            time = 0;
            for (int i = 0; i < index; i++)
            {
                time += segments[i].duration;
            }
            status = Status.Running;
            segmentIndex = index;
            segmentTime = 0;
            OnSegmentStart();
            ProcessSegment(0);
        }
        protected virtual void ProcessSegment(float deltaTime)
        {
            time += deltaTime;
            segmentTime += deltaTime;
            segmentProcess = activeSegment.duration <= 0 ? 1 : activeSegment.curve.Evaluate(segmentTime);
            OnSegmentUpdate();
            if (segmentTime > activeSegment.duration)
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
            time = 0;
            status = Status.Stopped;
            segmentIndex = 0;
            segmentTime = 0;
            segmentProcess = 0;
        }

        public void Process(float _time)
        {
            time = _time;
            if (segments.Count == 0) return;
            int _segmentIndex = 0;
            segmentProcess = Process(ref _segmentIndex, ref _time);
            segmentIndex = _segmentIndex;
            segmentTime = _time;
            ProcessSegment(0);
        }
        private float Process(ref int segmentIndex, ref float segmentTime)
        {
            if (segmentIndex >= segments.Count)
            {
                if (loop)
                {
                    segmentIndex = 0;
                    return Process(ref segmentIndex, ref segmentTime);
                }
                else
                {
                    segmentIndex = segments.Count - 1;
                    segmentTime = segments[segmentIndex].duration;
                    return 1;
                }
            }

            float duration = segments[segmentIndex].duration;
            if (segmentTime > duration)
            {
                segmentIndex++;
                segmentTime -= duration;
                return Process(ref segmentIndex, ref segmentTime);
            }
            else
            {
                return duration <= 0 ? 1 : (segmentTime / duration);
            }
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
            return activeSegment != null && status == Status.Running;
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
                    ProcessSegment(Time.deltaTime);
                    break;
                case AnimatorUpdateMode.UnscaledTime:
                    ProcessSegment(Time.unscaledDeltaTime);
                    break;
            }
        }
        protected void FixedUpdate()
        {
            if (updateMode != AnimatorUpdateMode.AnimatePhysics) return;
            if (!IsRunning()) return;
            ProcessSegment(Time.fixedDeltaTime);
        }
    }
}