/*
 * Author:      熊哲
 * CreateTime:  4/7/2017 11:57:23 AM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace EZUnityTools
{
    public static class EZUtility
    {
        public static string projectDirPath
        {
            get { return PathNormalize(Application.dataPath).Substring(0, Application.dataPath.LastIndexOf("/") + 1); }
        }
        public static string dataDirPath
        {
            get { return PathNormalize(Application.dataPath + "/"); }
        }
        public static string streamingDirPath
        {
            get { return PathNormalize(Application.streamingAssetsPath + "/"); }
        }
        public static string persistentDirPath
        {
            get
            {
#if UNITY_EDITOR
                return "C:/UnityPersistent/" + Application.productName + "/";
#else
            return PathNormalize(Application.persistentDataPath + "/Data/");   // persistent没有删除权限，建立子文件夹读写更方便
#endif
            }
        }
        // 标准化路径
        public static string PathNormalize(string path)
        {
            return path.Replace("\\", "/");
        }

        // 网络可用返回true，否则返回false
        public static bool IsNetAvailable
        {
            get { return Application.internetReachability != NetworkReachability.NotReachable; }
        }
        // 本地连接（有线或无线，非手机网络）返回true，否则返回false
        public static bool IsNetLocal
        {
            get { return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork; }
        }

        // 以1970年为基准，获取毫秒时间差
        public static long TimeSpan
        {
            get
            {
                TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
                return (long)ts.TotalMilliseconds;
            }
        }

        // 获取所有类型
        public static IEnumerable<Type> GetAllTypes()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies() // 获取当前所有程序集
                   where !(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder) // 排除动态模块
                   from type in assembly.GetExportedTypes() // 获取所有公共类
                   where true
                   select type;
        }

        // 计算字符串的MD5值
        public static string MD5String(string content)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] md5Data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(content));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < md5Data.Length; i++)
            {
                sBuilder.Append(md5Data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        // 计算文件的MD5值
        public static string MD5File(string filePath)
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
                throw new Exception("MD5File() fail, error: " + ex.Message);
            }
        }
    }
}