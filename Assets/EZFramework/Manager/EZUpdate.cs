/*
 * Author:      熊哲
 * CreateTime:  1/19/2017 2:45:07 PM
 * Description:
 * 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EZFramework
{
    public class EZUpdate : TEZManager<EZUpdate>
    {
        // 超时会停止更新
        [Range(5, 30)]
        public int timeout = 10;
        // 在进游戏时会忽略该前缀的更新
        public string ignorePrefix = "";
        // 在进游戏时会忽略该后缀的更新
        public string ignoreSuffix = "";
        // 用于显示更新进度的UI，可以不指定
        public LoadingPanel loadingPanel;

        public string sourceDirPath { get; private set; }
        public string runtimeDirPath { get; private set; }
        public string serverAddress { get; private set; }
        public string bundleExtension { get; private set; }
        public string timeTag { get; private set; }

        protected const char DELIMITER = '|';
        protected Dictionary<string, string> md5Dict;
        protected List<string> downloadList = new List<string>();

        public override void Init()
        {
            base.Init();
            md5Dict = new Dictionary<string, string>();
            sourceDirPath = EZUtility.streamingDirPath;
            switch (EZSettings.Instance.runMode)
            {
                case EZSettings.RunMode.Develop:
                    runtimeDirPath = EZUtility.streamingDirPath;
#if UNITY_EDITOR_WIN
                    serverAddress = "file:///" + EZUtility.streamingDirPath;
#else
                    serverAddress = "file://" + EZUtility.streamingDirPath;
#endif
                    break;
                case EZSettings.RunMode.Local:
                    runtimeDirPath = EZUtility.persistentDirPath;
                    if (EZSettings.Instance.localServerAddress == "")
                    {
#if UNITY_EDITOR_WIN
                        serverAddress = "file:///" + EZUtility.streamingDirPath;
#else
                        serverAddress = "file://" + EZUtility.streamingDirPath;
#endif
                    }
                    else serverAddress = EZSettings.Instance.localServerAddress;
                    break;
                case EZSettings.RunMode.Update:
                    runtimeDirPath = EZUtility.persistentDirPath;
                    serverAddress = EZSettings.Instance.updateServerAddress;
                    break;
            }
            bundleExtension = EZSettings.Instance.bundleExtension;
            timeTag = "?v=" + DateTime.Now.ToString("yyyymmddhhmmss");
            Directory.CreateDirectory(runtimeDirPath);
        }
        public override void Exit()
        {
            base.Exit();
        }

        /// <summary>
        /// 开始进行更新，并在完成后执行回调
        /// </summary>
        /// <param name="callback">更新完成后的回调</param>
        public void StartUpdate(Action callback)
        {
            StartCoroutine(Cor_StartUpdate(callback));
        }
        private IEnumerator Cor_StartUpdate(Action callback)
        {
            if (loadingPanel != null) loadingPanel.ShowProgress();
            yield return Cor_Extract();
            yield return Cor_Update();
            callback();
            if (loadingPanel != null) loadingPanel.LoadComplete();
        }

        private void LoadFileList()
        {
            md5Dict.Clear();
            try
            {
                string[] fileList = File.ReadAllLines(runtimeDirPath + "files.txt");
                for (int i = 0; i < fileList.Length; i++)
                {
                    if (string.IsNullOrEmpty(fileList[i])) continue;
                    string[] fileInfo = fileList[i].Split(DELIMITER);
                    md5Dict.Add(fileInfo[0], fileInfo[1]);
                }
            }
            catch (Exception ex) { LogError(ex.Message); }
        }
        private IEnumerator Cor_UpdateFileList()
        {
            string remotePath = serverAddress + "files.txt";
            string localPath = runtimeDirPath + "files.txt";
            yield return Cor_UpdateFile(remotePath, localPath, timeout);
            LoadFileList();
        }
        private IEnumerator Cor_Extract()
        {
            if (EZSettings.Instance.runMode == EZSettings.RunMode.Develop) yield break;
            string inputFilePath, outputFilePath;
            inputFilePath = sourceDirPath + "files.txt"; outputFilePath = runtimeDirPath + "files.txt";
            if (File.Exists(outputFilePath)) yield break;

            if (loadingPanel != null) loadingPanel.ShowProgress("Extracting...");
            yield return Cor_ExtractFile(inputFilePath, outputFilePath);
            LoadFileList();
            int total = md5Dict.Count, process = 0;
            foreach (string filePath in md5Dict.Keys)
            {
                process++;
                if (loadingPanel != null) loadingPanel.ShowProgress("Extracting..." + process + "/" + total, (float)process / total);
                inputFilePath = sourceDirPath + filePath; outputFilePath = runtimeDirPath + filePath;
                yield return Cor_ExtractFile(inputFilePath, outputFilePath);
            }
        }
        private IEnumerator Cor_ExtractFile(string source, string destination)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(source);
                yield return www;
                if (www.error == null) File.WriteAllBytes(destination, www.bytes);
            }
            else
            {
                File.Copy(source, destination, true);
            }
        }
        private IEnumerator Cor_Update()
        {
            if (!EZUtility.IsNetAvailable || EZSettings.Instance.runMode == EZSettings.RunMode.Develop)
            {
                LoadFileList();
                yield break;
            }
            if (loadingPanel != null) loadingPanel.ShowProgress("Updating...");
            yield return Cor_UpdateFileList();
            List<string> updateList = new List<string>();
            foreach (var fileInfo in md5Dict)
            {
                if (fileInfo.Key.StartsWith(ignorePrefix)) continue;
                if (fileInfo.Key.EndsWith(ignoreSuffix)) continue;
                if (CheckUpdate(fileInfo.Key, fileInfo.Value) > 0)
                {
                    updateList.Add(fileInfo.Key);
                }
            }
            if (loadingPanel != null) loadingPanel.ShowProgress("Updating...");
            int total = updateList.Count, process = 0;
            foreach (var rPath in updateList)
            {
                process++;
                if (loadingPanel != null) loadingPanel.ShowProgress("Updating..." + process + "/" + total, (float)process / total);
                string remotePath = serverAddress + rPath;
                string localPath = runtimeDirPath + rPath;
                yield return Cor_UpdateFile(remotePath, localPath);
            }
        }
        private IEnumerator Cor_UpdateFile(string source, string destination, float timeout = float.PositiveInfinity)
        {
            Log("Updating-> " + source);
            Log("To      -> " + destination);
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            WWW www = new WWW(source); float time = 0;
            while (!www.isDone)
            {
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
                if (time > timeout) break;
            }
            if (www.error == null)
            {
                File.WriteAllBytes(destination, www.bytes);
                downloadList.Add(source);
            }
        }

        // -1：服务器无此文件；0：不需要更新；1：本地文件不存在；2：存在并需要更新；
        public int CheckUpdate(string resourceName)
        {
            resourceName = resourceName.EndsWith(bundleExtension)
                        ? resourceName.ToLower()
                        : resourceName.ToLower() + bundleExtension;
            if (!File.Exists(runtimeDirPath + resourceName))
            {
                Log("Need to Update, " + runtimeDirPath + resourceName + " not exist.");
                return 1;
            }
            else if (EZSettings.Instance.runMode == EZSettings.RunMode.Develop)
            {
                Log("Checked in DevelopMode: " + resourceName);
                return 0;
            }

            string md5; if (!md5Dict.TryGetValue(resourceName, out md5))
            {
                LogWarning("File not available, No such file on fileList: " + resourceName);
                return -1;
            }
            return CheckUpdate(resourceName, md5);
        }
        public int CheckUpdate(string relativePath, string md5)
        {
            string localPath = runtimeDirPath + relativePath;
            if (!File.Exists(localPath))
            {
                Log("Need to Update, " + localPath + " not exist.");
                return 1;
            }
            if (EZUtility.MD5File(localPath) != md5)
            {
                Log("Need to Update, " + localPath + " out of date.");
                return 2;
            }
            return 0;
        }

        public WWWTask Download(string resourceName, Action<WWWTask, bool> callback = null)
        {
            resourceName = resourceName.EndsWith(bundleExtension)
                        ? resourceName.ToLower()
                        : resourceName.ToLower() + bundleExtension;
            string url = serverAddress + resourceName;
            string localPath = runtimeDirPath + resourceName;
            callback += delegate (WWWTask task, bool succeed)
            {
                Log(task.url + "->" + succeed);
                if (succeed)
                {
                    downloadList.Add(url);
                    File.WriteAllBytes(localPath, task.www.bytes);
                }
            };
            return EZNetwork.Instance.NewTask(url, null, callback);
        }
    }
}