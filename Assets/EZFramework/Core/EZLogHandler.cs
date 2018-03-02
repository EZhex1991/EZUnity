/*
 * Author:      熊哲
 * CreateTime:  4/7/2017 4:05:30 PM
 * Description:
 * Debug.logger是静态的，在编辑器下退出游戏时资源并不会释放，这会导致日志文件持续占用无法读取。
 * Debug.logger的更改建议发生在Start中，并在更改前保存其默认值，在OnApplicationQuit时将其还原。
*/
using System;
using System.IO;
using UnityEngine;

namespace EZFramework
{
    public class EZLogHandler : ILogHandler
    {
#if UNITY_5
        private static ILogHandler m_UnityLogHandler = Debug.logger.logHandler;
#else
        private static ILogHandler m_UnityLogHandler = Debug.unityLogger.logHandler;
#endif
        public ILogHandler unityLogHandler { get { return m_UnityLogHandler; } }

        public string mainDirPath { get; private set; }
        public string currentLogFile { get; private set; }

        public int logMax { get; private set; }
        public int logCount { get; private set; }

        private string timeTag { get { return DateTime.Now.ToString("HH:mm:ss"); } }
        private FileStream fileStream { get; set; }
        private StreamWriter streamWrite { get; set; }

        public EZLogHandler(string logPath, int maxLogCount = 10000)
        {
            mainDirPath = logPath;
            logMax = maxLogCount;
            NewLogFile();
        }
        private void NewLogFile()
        {
            currentLogFile = mainDirPath + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
            logCount = 0;
            Directory.CreateDirectory(mainDirPath);
            fileStream = new FileStream(currentLogFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            streamWrite = new StreamWriter(fileStream);
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            unityLogHandler.LogException(exception, context);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            streamWrite.WriteLine(timeTag.PadRight(10) + "\t" + logType.ToString().PadRight(10) + "\t" + string.Format(format, args));
            streamWrite.Flush();
            if (logCount++ > logMax) NewLogFile();
            unityLogHandler.LogFormat(logType, context, format, args);
        }
    }
}