/*
 * Author:      熊哲
 * CreateTime:  4/7/2017 4:05:30 PM
 * Description:
 * Debug.logger是静态的，在编辑器下退出游戏时资源并不会释放，这会导致日志文件持续占用无法读取。
 * Debug.logger的更改建议发生在Start中，并在更改前保存其默认值，在OnApplicationQuit时将其还原。
*/
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace EZFramework
{
    public class EZLogHandler : ILogHandler
    {
        public void LogException(Exception exception, UnityEngine.Object context)
        {
            m_DefaultLogHandler.LogException(exception, context);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            streamWrite.WriteLine(timeTag.PadRight(10) + "\t" + logType.ToString().PadRight(10) + "\t" + string.Format(format, args));
            streamWrite.Flush();
            if (logCount++ > 10000) NewLogFile();
            m_DefaultLogHandler.LogFormat(logType, context, format, args);
        }

        private ILogHandler m_DefaultLogHandler = Debug.logger.logHandler;
        private string mainDirPath { get; set; }
        private string currentLogFile { get; set; }
        private int logCount { get; set; }
        private string timeTag { get { return DateTime.Now.ToString("HH:mm:ss"); } }
        private FileStream fileStream { get; set; }
        private StreamWriter streamWrite { get; set; }
        public EZLogHandler()
        {
            mainDirPath = EZSettings.Instance.runMode == EZSettings.RunMode.Develop
                ? EZUtility.dataDirPath + "EZLog/"
                : EZUtility.persistentDirPath + "EZLog/";
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
    }
}