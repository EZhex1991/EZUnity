/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-15 20:10:19
 * Organization:    #ORGANIZATION#
 * Description:     
 * 
 * 1. Index: 数据列表，初始化时遍历主目录；
 * 2. Cache: 修改记录，当对内存进行修改时外存不会同步修改，而是在缓存中记录下修改的字段、方式和值；（实时外存读写）
 * 3. Data: 数据是一个键值对的集合，采用的数据结构是Dictionary<string, string>，第一个string是字段名称，第二个是序列化之后的值，每个数据会保存成一个文件；
 * 运行时会首先遍历主目录并将数据文件记录至Index，然后读取Cache，如果Cache不为空则根据Cache对Data进行读取和修改；
 * 当需要从Data中取某一键值对记录时，会先判断该Data是否已加载到内存，如果没有则判断在Index中是否存在该数据，如果存在则加载对应文件，如果不存在则新建数据；
 * 当缓存达到一定数量或者程序退出时，会查看Cache中有哪些数据的修改记录，然后将这些数据直接从内存写到外存文件中。
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace EZhex1991.EZUnity.Framework
{
    public class EZDatabase : EZMonoBehaviourSingleton<EZDatabase>
    {
        private EZApplication ezApplication { get { return EZApplication.Instance; } }

        public string mainPath { get { return ezApplication.persistentDataPath + "/EZDatabase"; } }

        public static class Operation
        {
            public const string Add = "A";
            public const string Set = "S";
            public const string Del = "D";
        }
        public static class FileName
        {
            public const string Cache = "Cache";
            public const string DataExtension = ".data";
        }
        [Serializable]
        public class Cache
        {
            private const char SEPARATOR = '|';

            public string dataName;
            public string operation;
            public string key;
            public string value;

            public Cache(string dataName, string operation, string key, string value)
            {
                this.dataName = dataName;
                this.operation = operation;
                this.key = key;
                this.value = value;
            }
            public static Cache Load(string str)
            {
                return JsonUtility.FromJson<Cache>(str);
            }
            public override string ToString()
            {
                return JsonUtility.ToJson(this);
            }
        }
        [Serializable]
        public class Data : ISerializationCallbackReceiver
        {
            private const string TIME_FORMAT = "yyyyMMddHHmmss";
            public static string timeNow { get { return DateTime.UtcNow.ToString(TIME_FORMAT); } }

            public string timeModified = timeNow;

            [SerializeField]
            private List<string> keys;
            [SerializeField]
            private List<string> values;

            public Dictionary<string, string> data { get; private set; }

            public void OnBeforeSerialize()
            {
                keys.Clear();
                values.Clear();
                foreach (var kvp in data)
                {
                    keys.Add(kvp.Key);
                    values.Add(kvp.Value);
                }
            }
            public void OnAfterDeserialize()
            {
                data = new Dictionary<string, string>();
                int length = Mathf.Min(keys.Count, values.Count);
                for (int i = 0; i < length; i++)
                {
                    data.Add(keys[i], values[i]);
                }
            }

            public void Add(string key, string value)
            {
                data.Add(key, value);
                timeModified = timeNow;
            }
            public void Set(string key, string value)
            {
                data[key] = value;
                timeModified = timeNow;
            }
            public void Del(string key)
            {
                data.Remove(key);
                timeModified = timeNow;
            }

            public string Get(string key)
            {
                return data[key];
            }
            public string Get(string key, string value)
            {
                data.TryGetValue(key, out value);
                return value;
            }

            public bool IsEmpty()
            {
                return data.Count == 0;
            }

            public Data()
            {
                this.keys = new List<string>();
                this.values = new List<string>();
                this.data = new Dictionary<string, string>();
                this.timeModified = DateTime.UtcNow.ToString(TIME_FORMAT);
            }
            public static Data Load(string str)
            {
                return JsonUtility.FromJson<Data>(str);
            }
            public override string ToString()
            {
                return JsonUtility.ToJson(this);
            }
        }

        public static EZDatabase instance { get; private set; }
        private static Encoding encoding = Encoding.UTF8;

        public int cacheSize = 1000;
        private string cacheFilePath;
        private int cacheCount = 0;

        private List<string> dataList = new List<string>();
        private Dictionary<string, Data> dataDict = new Dictionary<string, Data>();
        private StreamWriter cacheFileStream;

        protected override void Init()
        {
            cacheFilePath = Path.Combine(mainPath, FileName.Cache);
            Directory.CreateDirectory(mainPath);
            LoadFileList();
            LoadCache();
        }
        protected override void Dispose()
        {
            if (cacheFileStream != null)
            {
                cacheFileStream.Flush();
                cacheFileStream.Close();
            }
        }

        private string GetDataFilePath(string dataName)
        {
            return string.Format("{0}/{1}{2}", mainPath, dataName, FileName.DataExtension);
        }
        private void LoadFileList()
        {
            string[] files = Directory.GetFiles(mainPath, "*" + FileName.DataExtension);
            for (int i = 0; i < files.Length; i++)
            {
                string dataName = Path.GetFileNameWithoutExtension(files[i]);
                dataList.Add(dataName);
            }
        }

        private void LoadCache()
        {
            if (!File.Exists(cacheFilePath))
            {
                cacheFileStream = new StreamWriter(File.Create(cacheFilePath));
            }
            else
            {
                string[] cacheStrings = File.ReadAllLines(cacheFilePath, encoding);
                HashSet<string> dataModified = new HashSet<string>();
                for (int i = 0; i < cacheStrings.Length; i++)
                {
                    if (string.IsNullOrEmpty(cacheStrings[i])) return;
                    Cache cache = Cache.Load(cacheStrings[i]);
                    Data data = LoadData(cache.dataName);
                    switch (cache.operation)
                    {
                        case Operation.Add:
                            data.Add(cache.key, cache.value);
                            break;
                        case Operation.Set:
                            data.Set(cache.key, cache.value);
                            break;
                        case Operation.Del:
                            data.Del(cache.key);
                            break;
                    }
                    dataModified.Add(cache.dataName);
                }
                foreach (string dataName in dataModified)
                {
                    SaveData(dataName);
                }
                cacheCount = 0;
                File.WriteAllText(cacheFilePath, string.Empty, encoding);
                cacheFileStream = new StreamWriter(File.Open(cacheFilePath, FileMode.Append));
            }
        }
        private void SaveCache(Cache cache)
        {
            cacheFileStream.WriteLine(cache.ToString());
            cacheFileStream.Flush();
            if (cacheCount++ >= cacheSize)
            {
                SaveData();
                cacheCount = 0;
            }
        }

        public void LoadData()
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                LoadData(dataList[i]);
            }
        }
        public void SaveData()
        {
            if (!File.Exists(cacheFilePath))
            {
                LogFormat("Cache file is empty: {0}", cacheFilePath);
                return;
            }
            string[] cacheStrings = File.ReadAllLines(cacheFilePath, encoding);
            HashSet<string> dataModified = new HashSet<string>();
            for (int i = 0; i < cacheStrings.Length; i++)
            {
                if (string.IsNullOrEmpty(cacheStrings[i])) continue;
                Cache cache = Cache.Load(cacheStrings[i]);
                dataModified.Add(cache.dataName);
            }
            foreach (string dataName in dataModified)
            {
                SaveData(dataName);
            }
            File.WriteAllText(cacheFilePath, string.Empty, encoding);
            cacheCount = 0;
        }

        public Data LoadData(string dataName, string dataString)
        {
            Data data = Data.Load(dataString);
            dataDict[dataName] = data;
            LogFormat("Load data from string: ", dataName);
            return data;
        }

        public void Add<T>(string dataName, string key, T value)
        {
            string serializedValue = JsonUtility.ToJson(value);
            LoadData(dataName).Add(key, serializedValue);
            Cache cache = new Cache(dataName, Operation.Add, key, serializedValue);
            SaveCache(cache);
        }
        public void Set<T>(string dataName, string key, T value)
        {
            string serializedValue = JsonUtility.ToJson(value);
            LoadData(dataName).Set(key, serializedValue);
            Cache cache = new Cache(dataName, Operation.Set, key, serializedValue);
            SaveCache(cache);
        }
        public void Del(string dataName, string key)
        {
            LoadData(dataName).Del(key);
            Cache cache = new Cache(dataName, Operation.Del, key, string.Empty);
            SaveCache(cache);
        }
        public T Get<T>(string dataName, string key, T value)
        {
            string serializedValue = LoadData(dataName).Get(key, string.Empty);
            if (string.IsNullOrEmpty(serializedValue)) return value;
            else return JsonUtility.FromJson<T>(serializedValue);
        }

        public bool CheckExist(string dataName)
        {
            return dataList.Contains(dataName);
        }
        public bool CheckEmpty(string dataName)
        {
            Data data;
            if (dataDict.TryGetValue(dataName, out data))
            {
                return data.IsEmpty();
            }
            else if (dataList.Contains(dataName))
            {
                return LoadDataFromFile(dataName).IsEmpty();
            }
            return true;
        }
        public void SaveData(string dataName)
        {
            string filePath = GetDataFilePath(dataName);
            try
            {
                File.WriteAllText(filePath, LoadData(dataName).ToString(), encoding);
                LogFormat("Save to file: {0}", dataName);
            }
            catch (Exception e)
            {
                LogErrorFormat("Save to file: {0} failed\n{1}", dataName, e.Message);
            }
        }
        public Data LoadData(string dataName)
        {
            Data data;
            if (dataDict.TryGetValue(dataName, out data)) { return data; }
            else if (dataList.Contains(dataName))
            {
                return LoadDataFromFile(dataName);
            }
            else
            {
                data = new Data();
                dataDict.Add(dataName, data);
                dataList.Add(dataName);
                LogFormat("Create new data: {0}", dataName);
                return data;
            }
        }
        public Data LoadDataFromFile(string dataName)
        {
            Data data;
            try
            {
                string filePath = GetDataFilePath(dataName);
                data = Data.Load(File.ReadAllText(filePath, encoding));
                LogFormat("Load data from file: " + dataName);
            }
            catch (Exception e)
            {
                LogWarningFormat("Load data from file: {0} failed\n{1}", dataName, e.Message);
                data = new Data();
                dataList.Add(dataName);
            }
            dataDict[dataName] = data;
            return data;
        }
    }
}
