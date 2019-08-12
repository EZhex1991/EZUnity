/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-19 13:48:59
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.IO;
using UnityEngine;

namespace EZhex1991.EZUnity.Framework
{
    public class EZLogHandler : EZMonoBehaviourSingleton<EZLogHandler>, ILogHandler
    {
        private EZApplication ezApplication { get { return EZApplication.Instance; } }

        public string mainPath { get { return ezApplication.persistentDataPath + "/EZLogHandler"; } }

        public int logCountPerFile = 10000;

        private ILogHandler defaultLogHandler = Debug.unityLogger.logHandler;
        private string DateTimeTag { get { return DateTime.Now.ToString("yyyyMMdd_HHmmss"); } }
        private string TimeTag { get { return DateTime.Now.ToString("HH:mm:ss"); } }
        private StreamWriter streamWriter;
        private int logCount;

        protected override void Init()
        {
            Debug.unityLogger.logHandler = this;
            Directory.CreateDirectory(mainPath);
        }
        protected override void Dispose()
        {
            if (streamWriter != null)
            {
                streamWriter.Flush();
                streamWriter.Close();
            }
            Debug.unityLogger.logHandler = defaultLogHandler;
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            defaultLogHandler.LogException(exception, context);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            if (streamWriter == null) streamWriter = NewLogWriter();
            streamWriter.WriteLine(string.Format("{0}\t{1}\t{2}", TimeTag.PadRight(10), logType.ToString().PadRight(10), string.Format(format, args)));
            streamWriter.Flush();
            if (logCount++ >= logCountPerFile) streamWriter.Close();
            defaultLogHandler.LogFormat(logType, context, format, args);
        }

        private StreamWriter NewLogWriter()
        {
            logCount = 0;
            string path = string.Format("{0}/{1}.log", mainPath, DateTimeTag);
            return new StreamWriter(File.Create(path));
        }
    }
}
