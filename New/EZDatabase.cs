/*
 * Author:      熊哲
 * CreateTime:  1/19/2017 4:55:47 PM
 * Description:
 * 一个简易的数据库，使用缓存防止数据在异常情况下的丢失，同时可以减少外存的读写次数；
 * 
 * 1. Index: 索引是这个数据库的基本状态，记录当前有多少缓存和数据库中有哪些数据；
 * 2. Cache: 缓存是内存的修改记录，当对内存进行修改时外存不会同步修改，而是在缓存中记录下修改的字段、方式和值；（只有Cache是直接进行实时外存读写的）
 * 3. Data: 数据是一个键值对的集合，采用的数据结构是Hashtable，每个数据会保存成一个文件；
 * 运行时会首先读取Index，如果发现Cache不为0则会读取Cache，根据Cache对Data进行读取和修改；
 * 当需要从Data中取某一键值对记录时，会先判断该Data是否已加载到内存，如果没有则判断在Index中是否存在该数据，如果存在则加载对应文件，如果不存在则新建数据；
 * 当缓存达到一定数量或者程序退出时，会查看Cache中有哪些数据的修改记录，然后将这些数据直接从内存写到外存文件中。
*/
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace EZFramework
{
    public class EZDatabase : TEZManager<EZDatabase>
    {
        // 设置缓存的大小，根据实际的需要调节，如果需要读写长字符串建议调小一点
        [Range(10, 1000)]
        public int cacheSize = 500;
        protected int cacheCount = 0;

        public string MainDirPath { get; private set; }
        public string IndexFilePath { get; private set; }
        public string CacheFilePath { get; private set; }

        protected const char DELIM_OPR = '\n';

        protected const string EXTENSION_INDEX = ".index";
        protected const string EXTENSION_CACHE = ".cache";
        protected const string EXTENSION_DATA = ".ezdata";

        protected readonly Encoding ENCODING = Encoding.UTF8;

        protected DBIndex DBIndex;
        protected Dictionary<string, DBData> DBDict;

        public override void Init()
        {
            base.Init();
            MainDirPath = EZUtility.persistentDirPath + "EZDatabase/";
            IndexFilePath = MainDirPath + "_DBIndex" + EXTENSION_INDEX;
            CacheFilePath = MainDirPath + "_Cache" + EXTENSION_CACHE;
            Directory.CreateDirectory(MainDirPath);
            DBDict = new Dictionary<string, DBData>();
            LoadIndex();
            LoadCache();
        }
        public override void Exit()
        {
            base.Exit();
            SaveData();
        }
        // 暂停时存档（iOS一般不会退出）
        void OnApplicationFocus(bool focusStatus)
        {
            Log("Application Focus: " + focusStatus);
            if (!focusStatus)
            {
                SaveData();
            }
        }

        public bool Add(string dataName, string key, object value)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            DBData data = LoadData(dataName);
            if (!data.Add(key, serializedValue))
            {
                LogWarning("Add failed: " + dataName + "[" + key + "]");
                return false;
            }

            DBCache cache = new DBCache(dataName, DBCache.Operation.Add, key, serializedValue);
            SaveCache(cache.ToString());
            return true;
        }
        public bool Set(string dataName, string key, object value)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            DBData data = LoadData(dataName);
            if (!data.Set(key, serializedValue))
            {
                LogWarning("Set failed: " + dataName + "[" + key + "]");
                return false;
            }

            DBCache cache = new DBCache(dataName, DBCache.Operation.Set, key, serializedValue);
            SaveCache(cache.ToString());
            return true;
        }
        public bool Del(string dataName, string key)
        {
            DBData data = LoadData(dataName);
            if (!data.Del(key))
            {
                LogWarning("Del failed: " + dataName + "[" + key + "]");
                return false;
            }

            DBCache cache = new DBCache(dataName, DBCache.Operation.Del, key, string.Empty);
            SaveCache(cache.ToString());
            return true;
        }
        public object Get(string dataName, string key, object value)
        {
            DBData data = LoadData(dataName);
            string serializedValue = data.Get(key);
            if (string.IsNullOrEmpty(serializedValue)) return value;
            return JsonConvert.DeserializeObject(serializedValue);
        }
        public T Get<T>(string dataName, string key, T value)  // 此处取值在类型转换上容易出错，建议返回Object之后自行转换
        {
            DBData data = LoadData(dataName);
            string serializedValue = data.Get(key);
            if (string.IsNullOrEmpty(serializedValue)) return value;
            return JsonConvert.DeserializeObject<T>(serializedValue);
        }

        public bool IsExist(string dataName)
        {
            return DBIndex.Contains(dataName);
        }
        public bool IsEmpty(string dataName)
        {
            DBData data;
            if (DBDict.TryGetValue(dataName, out data)) { return data.IsEmpty(); }
            else if (DBIndex.Contains(dataName))
            {
                return LoadDataFromFile(dataName).IsEmpty();
            }
            return true;
        }
        public bool SaveData(string dataName)
        {
            string filePath = MainDirPath + dataName + EXTENSION_DATA;
            try
            {
                File.WriteAllText(filePath, LoadData(dataName).ToString(), ENCODING);
                Log("Save to file: " + dataName);
                return true;
            }
            catch (Exception ex)
            {
                LogError("Save to file: " + dataName + " failed.\n" + ex.Message);
                return false;
            }
        }
        public DBData LoadData(string dataName)
        {
            DBData data;
            if (DBDict.TryGetValue(dataName, out data)) { return data; }
            else if (DBIndex.Contains(dataName))
            {
                return LoadDataFromFile(dataName);
            }
            else
            {
                data = new DBData();
                DBDict.Add(dataName, data);
                DBIndex.Add(dataName); SaveIndex();
                Log("Create new data: " + dataName);
                return data;
            }
        }
        public DBData LoadDataFromFile(string dataName)
        {
            DBData data;
            try
            {
                string filePath = MainDirPath + dataName + EXTENSION_DATA;
                data = DBData.LoadFromString(File.ReadAllText(filePath, ENCODING));
                Log("Load data from file: " + dataName);
            }
            catch (Exception ex)
            {
                LogWarning("Load data from file: " + dataName + " failed");
                LogWarning(ex.Message);
                data = new DBData();
                DBIndex.Add(dataName); SaveIndex();
            }
            DBDict[dataName] = data;
            return data;
        }
        public DBData LoadDataFromString(string dataName, string dataString)
        {
            DBData data = DBData.LoadFromString(dataString);
            DBDict[dataName] = data;
            Log("Load data from string: " + dataString);
            return data;
        }
        public DBData LoadDataFromBase64String(string dataName, string base64String)
        {
            string dataString = Encoding.ASCII.GetString(Convert.FromBase64String(base64String));
            return LoadDataFromString(dataName, dataString);
        }
        public string GetDataString(string dataName)
        {
            return LoadData(dataName).ToString();
        }
        public string GetDataBase64String(string dataName)
        {
            byte[] data = Encoding.ASCII.GetBytes(GetDataString(dataName));
            return Convert.ToBase64String(data);
        }

        protected void LoadCache()
        {
            if (!File.Exists(CacheFilePath)) return;
            string[] cacheStrings = File.ReadAllText(CacheFilePath, ENCODING).Split(DELIM_OPR);
            foreach (string cacheString in cacheStrings)
            {
                if (string.IsNullOrEmpty(cacheString)) continue;
                DBCache cache = DBCache.LoadFromString(cacheString);
                DBData data = LoadData(cache.dataName);
                switch (cache.operation)
                {
                    case DBCache.Operation.Add:
                        data.Add(cache.key, cache.serializedValue);
                        break;
                    case DBCache.Operation.Set:
                        data.Set(cache.key, cache.serializedValue);
                        break;
                    case DBCache.Operation.Del:
                        data.Del(cache.key);
                        break;
                }
                SaveData(cache.dataName);
            }
            File.WriteAllText(CacheFilePath, string.Empty, ENCODING);
            cacheCount = 0;
            SaveIndex();
        }
        protected void SaveCache(string cacheString)
        {
            FileStream fileStream = File.Open(CacheFilePath, FileMode.Append);
            byte[] bytes = ENCODING.GetBytes(cacheString + DELIM_OPR);
            fileStream.Write(bytes, 0, bytes.Length); fileStream.Flush(); fileStream.Close();
            cacheCount++;

            if (cacheCount >= cacheSize)
            {
                SaveData();
            }
        }
        protected void LoadData()   // 这个方法是手动加载所有数据，实际上完全没用
        {
            foreach (string dataName in DBIndex.index)
            {
                LoadData(dataName);
            }
        }
        protected void SaveData()
        {
            if (!File.Exists(CacheFilePath))
            {
                Log("No Cache file: " + CacheFilePath);
                return;
            }
            string[] cacheStrings = File.ReadAllText(CacheFilePath, ENCODING).Split(DELIM_OPR);
            HashSet<string> dataNames = new HashSet<string>();
            foreach (string cacheString in cacheStrings)
            {
                if (string.IsNullOrEmpty(cacheString)) continue;
                DBCache cache = DBCache.LoadFromString(cacheString);
                dataNames.Add(cache.dataName);
            }
            foreach (string dataName in dataNames)
            {
                SaveData(dataName);
            }
            File.WriteAllText(CacheFilePath, string.Empty, ENCODING);
            cacheCount = 0;
            SaveIndex();
        }
        protected void LoadIndex()
        {
            try
            {
                Log("Load index.");
                string data = File.ReadAllText(IndexFilePath, ENCODING);
                DBIndex = DBIndex.LoadFromString(data);
            }
            catch (Exception ex)
            {
                LogWarning("Load index failed, Creating a new one.\n" + ex.Message);
                DBIndex = new DBIndex();
            }
        }
        protected void SaveIndex()
        {
            try
            {
                Log("Save index.");
                File.WriteAllText(IndexFilePath, DBIndex.ToString(), ENCODING);
            }
            catch (Exception ex)
            {
                LogError("Save index failed.\n" + ex.Message);
            }
        }
    }

    // 可以自行添加其他数据
    public class DBIndex
    {
        public List<string> index { get; private set; }

        public bool Add(string dataName)
        {
            if (index.Contains(dataName)) return false;
            index.Add(dataName);
            return true;
        }
        public bool Remove(string dataName)
        {
            return index.Remove(dataName);
        }
        public bool Contains(string dataName)
        {
            return index.Contains(dataName);
        }

        public DBIndex()
        {
            this.index = new List<string>();
        }
        public static DBIndex LoadFromString(string indexString)
        {
            DBIndex dbIndex = JsonConvert.DeserializeObject<DBIndex>(indexString);
            if (dbIndex == null) dbIndex = new DBIndex();
            return dbIndex;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class DBCache
    {
        private const char SEPARATOR = '|';
        public class Operation
        {
            public const string Add = "A";
            public const string Set = "S";
            public const string Del = "D";
        }

        public string dataName { get; private set; }
        public string operation { get; private set; }
        public string key { get; private set; }
        public string serializedValue { get; private set; }

        public DBCache(string dataName, string operation, string key, string serializedValue)
        {
            this.dataName = dataName;
            this.operation = operation;
            this.key = key;
            this.serializedValue = serializedValue;
        }
        public static DBCache LoadFromString(string cacheString)
        {
            string[] args = cacheString.Split(SEPARATOR);
            if (args.Length == 4)
                return new DBCache(args[0], args[1], args[2], args[3]);
            else return new DBCache("CACHEERROR", "A", DateTime.Now.ToString("yyyyMMddHHmmss"), cacheString);
        }
        public override string ToString()
        {
            return string.Join(SEPARATOR.ToString(), new string[] { dataName, operation, key, serializedValue });
        }
    }

    // 这个封装是为了方便替换数据的实现方式
    public class DBData
    {
        public Dictionary<string, string> data { get; private set; }

        public bool Add(string key, string serializedValue)
        {
            if (data.ContainsKey(key)) return false;
            else data.Add(key, serializedValue);
            return true;
        }
        public bool Set(string key, string serializedValue)
        {
            data[key] = serializedValue;
            return true;
        }
        public bool Del(string key)
        {
            if (!data.ContainsKey(key)) return false;
            data.Remove(key);
            return true;
        }
        public string Get(string key)
        {
            string serializedValue = string.Empty;
            data.TryGetValue(key, out serializedValue);
            return serializedValue;
        }

        public bool IsEmpty()
        {
            return data.Count == 0;
        }

        public DBData()
        {
            this.data = new Dictionary<string, string>();
        }
        public static DBData LoadFromString(string dataString)
        {
            DBData dbData = JsonConvert.DeserializeObject<DBData>(dataString);
            if (dbData == null) dbData = new DBData();
            return dbData;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}