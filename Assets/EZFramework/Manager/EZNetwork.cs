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
    public class EZNetwork : TEZManager<EZNetwork>
    {
        // 允许同时运行的任务数量
        [Range(1, 10)]
        public int maxTask = 3;

        private List<string> taskList;  //记录所有的任务
        private Queue<string> taskQueue;    //记录等待的任务
        private Dictionary<string, WWWTask> taskDict;  //任务名和任务对象的词典

        public override void Init()
        {
            base.Init();
            taskList = new List<string>();
            taskQueue = new Queue<string>();
            taskDict = new Dictionary<string, WWWTask>();
        }
        public override void Exit()
        {
            base.Exit();
            foreach (string taskname in taskList)
            {
                taskDict[taskname].StopTask();
            }
        }

        public WWWTask NewTask(string url, byte[] postData, Action<WWWTask, bool> callback = null)
        {
            callback += OnComplete;
            WWWTask task = gameObject.AddComponent<WWWTask>();
            task.SetTask(url, postData, callback);
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
            WWWTask task = taskDict[taskQueue.Dequeue()];
            taskList.Add(task.url);
            task.StartTask();
        }
        private void OnComplete(WWWTask task, bool succeed)
        {
            Log("Task over-> " + task.url);
            taskList.Remove(task.url);
            taskDict.Remove(task.url);
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