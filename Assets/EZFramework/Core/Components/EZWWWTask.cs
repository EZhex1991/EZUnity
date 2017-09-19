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
    public class EZWWWTask : MonoBehaviour
    {
        public string url { get; private set; }
        public byte[] postData { get; private set; }

        public float progress { get { return www == null ? 0 : www.progress; } }
        public bool isDone { get { return www == null ? false : www.isDone; } }

        public Action<float> onProgressCallback;
        public Action<string, byte[]> onStopCallback;

        private UCoroutine cor;
        private WWW www;

        public void SetTask(string url, byte[] postData)
        {
            this.url = url;
            this.postData = postData;
        }
        public void StartTask(float timeout = 600)
        {
            if (cor != null) return;
            cor = StartCoroutine(Cor_Task(timeout));
        }
        public void StopTask(bool destroy = false)
        {
            if (onStopCallback != null) onStopCallback(url, null);
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
                if (onProgressCallback != null) onProgressCallback(www.progress);
            }
            if (onStopCallback != null) onStopCallback(url, www.error == null ? www.bytes : null);
        }
    }
}