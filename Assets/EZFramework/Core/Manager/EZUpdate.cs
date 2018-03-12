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
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace EZFramework
{
    public class EZUpdate : _EZManager<EZUpdate>
    {
        // 超时会停止更新
        [Range(5, 30)]
        public int timeout = 10;
        // 解压提示
        public string extractHint = "Extracting...";
        // 更新提示
        public string updateHint = "Updating...";
        // 文件清单名称
        public string fileListName = "files.txt";
        // 在进游戏时会忽略该前缀的更新
        public string ignorePrefix = "";
        // 在进游戏时会忽略该后缀的更新
        public string ignoreSuffix = "";
        // 用于显示更新进度的UI，可以不指定
        public EZLoadingPanel loadingPanel;

        public string sourceDirPath { get; private set; }
        public string runtimeDirPath { get; private set; }
        public string serverAddress { get; private set; }
        public string timeTag { get; private set; }

        public delegate void OnUpdateCompleteAction();
        public event OnUpdateCompleteAction onUpdateCompleteEvent;

        protected const string EXTRACTED_FLAG = "EZUpdate_Extracted";
        protected const char DELIMITER = '|';
        protected class FileInfo
        {
            public string md5 { get; private set; }
            public int size { get; private set; }
            public FileInfo(string md5, int size)
            {
                this.md5 = md5;
                this.size = size;
            }
            public FileInfo(string md5, string size)
            {
                this.md5 = md5;
                this.size = Convert.ToInt32(size);
            }
        }
        protected Dictionary<string, FileInfo> fileList;
        protected List<string> downloadList = new List<string>();

        private bool updateMode;

        protected override void Awake()
        {
            base.Awake();
            fileList = new Dictionary<string, FileInfo>();
            sourceDirPath = EZFacade.Instance.streamingDirPath;
            switch (EZFrameworkSettings.Instance.runMode)
            {
                case RunMode.Develop:
                    runtimeDirPath = EZFacade.Instance.streamingDirPath;
                    serverAddress = "";
                    updateMode = false;
                    break;
                case RunMode.Local:
                    runtimeDirPath = EZFacade.Instance.streamingDirPath;
                    serverAddress = "";
                    updateMode = false;
                    break;
                case RunMode.Update:
                    runtimeDirPath = EZFacade.Instance.persistentDirPath;
                    serverAddress = EZFrameworkSettings.Instance.updateServer;
                    updateMode = true;
                    break;
            }
            timeTag = "?v=" + DateTime.Now.ToString("yyyymmddhhmmss");
        }
        void Start()
        {
            StartUpdate();
        }

        private void ShowLoadProgress(string str)
        {
            if (loadingPanel == null) return;
            loadingPanel.ShowProgress(str);
        }
        private void ShowLoadProgress(float progress)
        {
            if (loadingPanel == null) return;
            loadingPanel.ShowProgress(progress);
        }
        private void ShowLoadProgress(string str, float progress)
        {
            if (loadingPanel == null) return;
            loadingPanel.ShowProgress(str, progress);
        }
        private void ShowLoadComplete()
        {
            if (loadingPanel == null) return;
            loadingPanel.LoadComplete();
        }
        
        public void StartUpdate()
        {
            StartCoroutine(Cor_StartUpdate());
        }
        private IEnumerator Cor_StartUpdate()
        {
            ShowLoadProgress("", 0);
            yield return null;
            if (updateMode)
            {
                yield return Cor_Extract();
                yield return Cor_Update();
            }
            ShowLoadComplete();
            if (onUpdateCompleteEvent != null) onUpdateCompleteEvent();
        }
        private IEnumerator Cor_LoadFileList()
        {
            fileList.Clear();
            Log("Loading file list...");
            try
            {
                string[] fileInfoList = File.ReadAllLines(runtimeDirPath + fileListName);
                for (int i = 0; i < fileInfoList.Length; i++)
                {
                    if (string.IsNullOrEmpty(fileInfoList[i])) continue;
                    string[] fileInfo = fileInfoList[i].Split(DELIMITER);
                    fileList.Add(fileInfo[0], new FileInfo(fileInfo[1], fileInfo[2]));
                }
                Log("File list loaded.");
            }
            catch (Exception ex) { LogError("Load file list error: " + ex.Message); }
            yield return null;
        }
        private IEnumerator Cor_Extract()
        {
            if (PlayerPrefs.GetString(EXTRACTED_FLAG, "") == Application.version)
            {
                Log("Already extracted: " + Application.version);
                yield break;
            }
            Log("Extracting...");
            ShowLoadProgress(extractHint, 0);
            yield return null;
            yield return Cor_ExtractFile(fileListName);
            yield return Cor_LoadFileList();
            int total = fileList.Count, process = 0;
            foreach (string relativePath in fileList.Keys)
            {
                process++;
                ShowLoadProgress(extractHint + process + "/" + total, (float)process / total);
                yield return null;
                yield return Cor_ExtractFile(relativePath);
            }
            Log("Extract Complete.");
            PlayerPrefs.SetString(EXTRACTED_FLAG, Application.version);
        }
        private IEnumerator Cor_ExtractFile(string relativePath)
        {
            string source = sourceDirPath + relativePath;
            string destination = runtimeDirPath + relativePath;
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
#if UNITY_EDITOR || UNITY_STANDALONE
            if (!File.Exists(source)) // 不要在安卓平台下检查dataPath和streamingAssetsPath下的文件是否存在
            {
                Log("File missing: " + source);
                yield break;
            }
            Log("Extracting-> " + source);
            Log("To        -> " + destination);
            File.Copy(source, destination, true);
            yield return null;
#else
            WWW www = new WWW(source); // WWW的流式读取在发生错误时（文件不存在时）会有www.error；
            yield return www;
            if (www.error == null)
            {
                Log("Extracting-> " + source);
                Log("To        -> " + destination);
                File.WriteAllBytes(destination, www.bytes);
            }
            else
            {
                LogWarning("Extract error: " + source + " " + www.error);
            }
#endif
        }
        private IEnumerator Cor_Update()
        {
            if (!EZUtility.IsNetAvailable)
            {
                Log("Network Error!");
                yield return Cor_LoadFileList();
                yield break;
            }
            Log("Checking update...");
            ShowLoadProgress(updateHint, 0);
            yield return null;
            yield return Cor_UpdateFile(fileListName, timeout);
            yield return Cor_LoadFileList();
            List<string> updateList = new List<string>();
            foreach (var fileInfo in fileList)
            {
                if (fileInfo.Key.StartsWith(ignorePrefix)) continue;
                if (fileInfo.Key.EndsWith(ignoreSuffix)) continue;
                if (CheckUpdate(fileInfo.Key, fileInfo.Value.md5) > 0)
                {
                    updateList.Add(fileInfo.Key);
                }
            }
            Log("Updating...");
            int total = updateList.Count, progress = 0;
            foreach (var relativePath in updateList)
            {
                progress++;
                string hint = updateHint + progress + "/" + total;
                //ShowLoadProgress(hint, (float)progress / total); // 显示总体进度
                yield return null;
                yield return Cor_UpdateFile(relativePath, float.PositiveInfinity, hint);
            }
            Log("Update Complete.");
        }
        private IEnumerator Cor_UpdateFile(string relativePath, float timeout = float.PositiveInfinity, string str = "")
        {
            string source = serverAddress + relativePath;
            string destination = runtimeDirPath + relativePath;
            FileInfo info; int size = 0;
            if (fileList.TryGetValue(relativePath, out info))
                size = info.size;
            Log("Updating-> " + source);
            Log("To      -> " + destination);
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            WWW www = new WWW(source);
            while (!www.isDone)
            {
                if (str != "") ShowLoadProgress(str + " (" + (int)(size * www.progress) + "/" + size + "KB)", www.progress); // 显示当前进度
                yield return null;
                timeout -= Time.deltaTime;
                if (timeout < 0) break;
            }
            if (www.isDone && www.error == null)
            {
                File.WriteAllBytes(destination, www.bytes);
                downloadList.Add(source);
            }
        }

        // -1：服务器无此文件；0：不需要更新；1：本地文件不存在；2：存在并需要更新；
        public int CheckUpdate(string relativePath)
        {
            relativePath = relativePath.ToLower();
            if (!updateMode)
            {
                Log("Checked in non-update mode: " + relativePath);
                return 0;
            }
            if (!File.Exists(runtimeDirPath + relativePath))
            {
                Log("Need to update, " + runtimeDirPath + relativePath + " not exist.");
                return 1;
            }

            FileInfo info; if (!fileList.TryGetValue(relativePath, out info))
            {
                LogWarning("File not available, No such file on fileList: " + relativePath);
                return -1;
            }
            return CheckUpdate(relativePath, info.md5);
        }
        public int CheckUpdate(string relativePath, string md5)
        {
            string localPath = runtimeDirPath + relativePath;
            if (!File.Exists(localPath))
            {
                Log("Need to update, " + localPath + " not exist.");
                return 1;
            }
            if (CalcFileMD5(localPath) != md5)
            {
                Log("Need to update, " + localPath + " out of date.");
                return 2;
            }
            return 0;
        }
        public static string CalcFileMD5(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] md5Data = md5Hasher.ComputeHash(fs);
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < md5Data.Length; i++)
                {
                    sBuilder.Append(md5Data[i].ToString("x2"));
                }
                fs.Close();
                return sBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Calculate MD5 failed, error: " + ex.Message);
            }
        }

        public EZWWWTask Download(string relativePath)
        {
            if (!updateMode) return null;
            relativePath = relativePath.ToLower();
            string source = serverAddress + relativePath;
            string localPath = runtimeDirPath + relativePath;
            EZWWWTask task = EZNetwork.Instance.NewTask(source, null);
            task.onStopEvent += delegate (string url, byte[] bytes)
            {
                if (bytes != null)
                {
                    downloadList.Add(url);
                    File.WriteAllBytes(localPath, bytes);
                    Log("Task complete -> " + url);
                }
                else
                {
                    Log("Task failed -> " + url);
                }
            };
            return task;
        }
    }
}