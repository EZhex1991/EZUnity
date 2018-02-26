/*
 * Author:      熊哲
 * CreateTime:  1/19/2017 2:46:49 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZFramework
{
    public class EZNetwork : _EZManager<EZNetwork>
    {
        // 允许同时运行的任务数量
        [Range(1, 10)]
        public int maxTask = 3;

        private List<string> taskList;  //记录所有的任务
        private Queue<string> taskQueue;    //记录等待的任务
        private Dictionary<string, EZWWWTask> taskDict;  //任务名和任务对象的词典

        protected override void Awake()
        {
            base.Awake();
            taskList = new List<string>();
            taskQueue = new Queue<string>();
            taskDict = new Dictionary<string, EZWWWTask>();
        }
        protected override void OnDestroy()
        {
            foreach (string taskname in taskList)
            {
                if (taskDict[taskname] != null) taskDict[taskname].StopTask();
            }
            base.OnDestroy();
        }

        public EZWWWTask NewTask(string url, byte[] postData)
        {
            EZWWWTask task = gameObject.AddComponent<EZWWWTask>();
            task.SetTask(url, postData);
            task.onStopEvent += OnComplete;
            if (taskList.Count >= maxTask)
            {
                taskQueue.Enqueue(url);
                taskDict.Add(url, task);
            }
            else
            {
                taskList.Add(url);
                taskDict.Add(url, task);
                task.StartTask();
            }
            return task;
        }
        private void NextTask()
        {
            if (taskQueue.Count <= 0) return;
            EZWWWTask task = taskDict[taskQueue.Dequeue()];
            taskList.Add(task.url);
            task.StartTask();
        }
        private void OnComplete(string url, byte[] bytes)
        {
            Log("Task over-> " + url);
            taskList.Remove(url);
            taskDict.Remove(url);
            NextTask();
        }

        public bool HasTask(string url)
        {
            return taskDict.ContainsKey(url);
        }
        public bool IsRunning(string url)
        {
            return taskList.Contains(url);
        }
        public bool IsQueuing(string url)
        {
            return taskQueue.Contains(url);
        }
    }
}