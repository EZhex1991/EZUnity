/*
 * Author:      熊哲
 * CreateTime:  1/19/2017 2:46:49 PM
 * Description:
 * 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity.Framework
{
    public class EZWWWQueue : _EZMonoBehaviourSingleton<EZWWWQueue>
    {
        public enum Status { Queued, Running, Stopped }

        public class Task
        {
            public MonoBehaviour runner { get; private set; }
            public string url { get; private set; }
            public byte[] postData { get; private set; }

            public Status status = Status.Queued;

            public float progress { get { return www == null ? 0 : www.progress; } }
            public bool isDone { get { return www == null ? false : www.isDone; } }

            public delegate void OnProgressAction(float progress);
            public OnProgressAction onProgressEvent;
            public delegate void OnStopAction();
            public OnStopAction onStopEvent;

            private Coroutine coroutine;
            private WWW www;

            public Task(MonoBehaviour runner, string url, byte[] postData)
            {
                this.runner = runner;
                this.url = url;
                this.postData = postData;
            }

            public void StartTask(float timeout = 600)
            {
                if (coroutine != null) return;
                coroutine = runner.StartCoroutine(Cor_Task(timeout));
                status = Status.Running;
            }
            public void StopTask()
            {
                if (onStopEvent != null) onStopEvent();
                if (coroutine != null)
                {
                    runner.StopCoroutine(coroutine);
                }
                if (www != null)
                {
                    www.Dispose();
                }
                status = Status.Stopped;
            }

            private IEnumerator Cor_Task(float timeout)
            {
                www = new WWW(url, postData);
                while (!www.isDone)
                {
                    timeout -= Time.unscaledDeltaTime;
                    if (timeout <= 0)
                    {
                        StopTask();
                    }
                    yield return null;
                    if (onProgressEvent != null) onProgressEvent(www.progress);
                }
                if (onStopEvent != null) onStopEvent();
            }
        }

        // 允许同时运行的任务数量
        [Range(1, 10)]
        public int maxTask = 3;

        private List<Task> taskList = new List<Task>();
        private Queue<Task> taskQueue = new Queue<Task>();    //记录等待的任务

        protected override void Init()
        {

        }
        protected override void Dispose()
        {
            taskQueue.Clear();
            foreach (Task task in taskList)
            {
                task.StopTask();
            }
        }

        public Task NewTask(string url, byte[] postData)
        {
            Task task = new Task(this, url, postData);
            task.onStopEvent += () => OnComplete(task);
            if (taskList.Count >= maxTask)
            {
                taskQueue.Enqueue(task);
            }
            else
            {
                taskList.Add(task);
                task.StartTask();
            }
            return task;
        }
        private void NextTask()
        {
            if (taskQueue.Count <= 0) return;
            Task task = taskQueue.Dequeue();
            taskList.Add(task);
            task.StartTask();
        }
        private void OnComplete(Task task)
        {
            Log("Task over-> " + task.url);
            taskList.Remove(task);
            NextTask();
        }
    }
}