/*
 * Author:      熊哲
 * CreateTime:  4/19/2017 2:20:04 PM
 * Description:
 * 
*/
using System;
using System.Collections;
using UnityEngine;
using UCoroutine = UnityEngine.Coroutine;

namespace EZFramework
{
    public class WWWTask : MonoBehaviour
    {
        public string url { get; private set; }
        public byte[] postData { get; private set; }
        public Action<WWWTask, bool> callback { get; private set; }

        public UCoroutine cor { get; private set; }
        public WWW www { get; private set; }
        public float progress { get { return www == null ? 0 : www.progress; } }
        public bool isDone { get { return www == null ? false : www.isDone; } }

        public void SetTask(string url, byte[] postData, Action<WWWTask, bool> callback = null)
        {
            this.url = url;
            this.postData = postData;
            this.callback = callback;
        }
        public void StartTask(float timeout = 600)
        {
            if (cor != null) return;
            cor = StartCoroutine(Cor_Task(timeout));
        }
        public void StopTask(bool destroy = false)
        {
            if (callback != null) callback(this, false);
            if (cor != null)
            {
                StopCoroutine(cor);
            }
            if (www != null)
            {
                www.Dispose();
                www = null;
            }
            if (destroy)
            {
                Destroy(this);
            }
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
            }
            if (www.error == null)
            {
                if (callback != null) callback(this, true);
            }
            else
            {
                if (callback != null) callback(this, false);
            }
        }
    }
}