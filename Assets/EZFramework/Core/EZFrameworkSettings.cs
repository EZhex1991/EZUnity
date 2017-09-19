/*
 * Author:      熊哲
 * CreateTime:  1/16/2017 2:13:01 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZFramework
{
    public class EZFrameworkSettings : ScriptableObject
    {
        // 必须放置在Resources目录下！
        public const string AssetDirPath = "Assets/Resources/";
        public const string AssetName = "EZFrameworkSettings";
        private static EZFrameworkSettings instance;
        public static EZFrameworkSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<EZFrameworkSettings>(AssetName);
                    if (instance == null) instance = CreateInstance<EZFrameworkSettings>();
                }
                return instance;
            }
        }

        public enum RunMode
        {
            Develop,
            Local,
            Update,
        }
        public enum SleepTimeout
        {
            NeverSleep = UnityEngine.SleepTimeout.NeverSleep,
            SystemSetting = UnityEngine.SleepTimeout.SystemSetting,
        }

        public RunMode runMode = RunMode.Develop;

        public SleepTimeout sleepTimeout = SleepTimeout.NeverSleep;
        public bool runInBackground = true;
        public int targetFrameRate = 45;

        public string updateServer = "";
        public string bundleExtension = ".unity3d";
        public string luaDirName = "Script_Lua";
    }
}