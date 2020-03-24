/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-13 20:10:05
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;
using System.Linq;
#if UNITYADDRESSABLES
using UnityEngine.AddressableAssets;
#endif

namespace EZhex1991.EZUnity.Framework
{
    [Serializable]
    public class FileInfo
    {
        public string fileName;
        public string md5;
        public int size;

        [NonSerialized]
        public string filePath;
        [NonSerialized]
        public bool isUpated;
    }

    public class EZResources : EZMonoBehaviourSingleton<EZResources>
    {
        private EZApplication ezApplication { get { return EZApplication.Instance; } }
        private EZApplicationSettings settings { get { return EZApplicationSettings.Instance; } }

        // 文件列表
        private Dictionary<string, FileInfo> fileList = new Dictionary<string, FileInfo>();

        // Develop模式下用到的源文件路径
        private Dictionary<string, string> assetPathDict = new Dictionary<string, string>();
        // 记录已加载过的bundle
        private Dictionary<string, AssetBundle> bundleDict = new Dictionary<string, AssetBundle>();
        // 记录正在加载的bundle
        private Dictionary<string, AssetBundleCreateRequest> abcrDict = new Dictionary<string, AssetBundleCreateRequest>();

        private AssetBundleManifest m_Manifest;
        private AssetBundleManifest manifest
        {
            get
            {
                if (m_Manifest == null)
                {
                    AssetBundle bundle = AssetBundle.LoadFromFile(fileList["StreamingAssets"].filePath);
                    if (bundle != null)
                    {
                        m_Manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    }
                }
                return m_Manifest;
            }
        }

        public delegate void OnAssetLoadedAction(UObject asset);
        public delegate void OnAssetLoadedAction<T>(T asset) where T : UObject;
        public delegate void OnSceneChangedAction();
        public delegate void OnBundleLoadedAction(AssetBundle bundle);

        public bool isUpdated { get; private set; }
        public event Action onUpdateCompleteEvent;

        // 初始化资源管理器，读取StreamingAssets的所有依赖（即所有的资源包名）
        protected override void Init()
        {
            switch (ezApplication.packageMode)
            {
                case PackageMode.Develop:
                    LoadBuiltinFileList();
                    break;
                case PackageMode.Local:
                    LoadBuiltinFileList();
                    break;
                case PackageMode.Remote:
                    LoadBuiltinFileList();
                    StartCoroutine(Cor_Update());
                    break;
            }
        }
        protected override void Dispose()
        {
            foreach (AssetBundle bundle in bundleDict.Values)
            {
                bundle.Unload(true);
            }
        }

        public string CalcFileMD5(string filePath)
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

        private void LoadBuiltinFileList()
        {
            Log("Loading file list...");
            fileList.Clear();

            try
            {
                string[] builtinList = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(settings.fileListName)).text.Split('\n');
                for (int i = 0; i < builtinList.Length; i++)
                {
                    if (string.IsNullOrEmpty(builtinList[i])) continue;
                    FileInfo info = JsonUtility.FromJson<FileInfo>(builtinList[i]);
                    info.filePath = Path.Combine(Application.streamingAssetsPath, info.fileName);
                    info.isUpated = true;
                    fileList[info.fileName] = info;
                }
            }
            catch (Exception ex)
            {
                LogWarningFormat("Failed to Load built-in List: {0}", ex.Message);
            }
        }

        public bool IsUpdated(string fileName)
        {
            if (ezApplication.packageMode != PackageMode.Remote) return true;

            FileInfo info; if (!fileList.TryGetValue(fileName, out info))
            {
                LogWarningFormat("Failed to get file info: {0}", fileName);
                return info.isUpated;
            }
            return false;
        }

        private IEnumerator Cor_Update()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Log("Network Error");
                yield return null;
            }
            Log("Checking Update...");
            yield return Cor_LoadRemoteFileList();
            List<string> updateList = new List<string>();
            foreach (var fileInfo in fileList.Values)
            {
                if (fileInfo.fileName.StartsWith(settings.ignorePrefix) || fileInfo.fileName.EndsWith(settings.ignoreSuffix))
                {
                    LogFormat("File with ignore pattern: {0}", fileInfo.fileName);
                    continue;
                }
                if (!fileInfo.isUpated)
                {
                    updateList.Add(fileInfo.fileName);
                }
            }
            Log("Updating...");
            foreach (string fileName in updateList)
            {
                yield return Cor_Download(fileName);
            }
            Log("Update Complete");
            isUpdated = true;
            if (onUpdateCompleteEvent != null) onUpdateCompleteEvent();
        }
        private IEnumerator Cor_LoadRemoteFileList()
        {
            string fileName = settings.fileListName + ".txt";
            yield return Cor_Download(fileName);
            try
            {
                string[] infoList = File.ReadAllLines(Path.Combine(ezApplication.persistentDataPath, fileName));
                for (int i = 0; i < infoList.Length; i++)
                {
                    if (string.IsNullOrEmpty(infoList[i])) continue;
                    FileInfo info = JsonUtility.FromJson<FileInfo>(infoList[i]);
                    if (fileList.ContainsKey(info.fileName) && fileList[info.fileName].md5 == info.md5) continue;
                    info.filePath = Path.Combine(ezApplication.persistentDataPath, info.fileName);
                    if (File.Exists(info.filePath) && CalcFileMD5(info.filePath) == info.md5)
                    {
                        info.isUpated = true;
                    }
                    fileList[info.fileName] = info;
                }
            }
            catch (Exception ex)
            {
                LogWarningFormat("Failed to get remote file List: {0}", ex.Message);
            }
        }
        private IEnumerator Cor_Download(string fileName)
        {
            LogFormat("Downloading -> {0}", fileName);
            string sour = Path.Combine(settings.updateServer, fileName);
            string dest = Path.Combine(ezApplication.persistentDataPath, fileName);
            UnityWebRequest request = UnityWebRequest.Get(sour);
            yield return request.SendWebRequest();
            if (string.IsNullOrEmpty(request.error))
            {
                File.WriteAllBytes(dest, request.downloadHandler.data);
                if (fileList.ContainsKey(fileName)) fileList[fileName].isUpated = true;
            }
            else
            {
                LogError(request.error);
            }
        }

        // 记录bundle里的资源路径，在Editor+Develop模式时可以直接从路径加载文件
        private void GetAssetPathFromBundle(AssetBundle bundle)
        {
            if (ezApplication.packageMode == PackageMode.Develop)
            {
                foreach (string filePath in bundle.GetAllAssetNames())
                {
                    string assetName = Path.GetFileNameWithoutExtension(filePath);
                    assetPathDict[GetAssetKey(bundle.name, assetName)] = filePath;
                }
                foreach (string scenePath in bundle.GetAllScenePaths())
                {
                    string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                    assetPathDict[GetAssetKey(bundle.name, sceneName)] = scenePath;
                }
            }
        }
        private string GetAssetKey(string bundleName, string assetName)
        {
            bundleName = bundleName.ToLower();
            assetName = assetName.ToLower();
            return bundleName + "-" + assetName;
        }

        // 同步加载资源
        public T LoadAsset<T>(string bundleName, string assetName) where T : UObject
        {
            AssetBundle bundle = LoadBundle(bundleName);
#if UNITY_EDITOR
            if (ezApplication.packageMode == PackageMode.Develop)
            {
                string assetKey = GetAssetKey(bundleName, assetName);
                string assetPath;
                if (!assetPathDict.TryGetValue(assetKey, out assetPath))
                {
                    LogWarningFormat("{0} not exist", assetKey);
                    return null;
                }
                else
                {
                    return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                }
            }
#endif
            return bundle.LoadAsset<T>(assetName);
        }
        public UObject LoadAsset(string bundleName, string assetName)
        {
            AssetBundle bundle = LoadBundle(bundleName);
#if UNITY_EDITOR
            if (ezApplication.packageMode == PackageMode.Develop)
            {
                string assetKey = GetAssetKey(bundleName, assetName);
                string assetPath;
                if (!assetPathDict.TryGetValue(assetKey, out assetPath))
                {
                    LogWarningFormat("{0} not exist", assetKey);
                    return null;
                }
                else
                {
                    return UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(UObject));
                }
            }
#endif
            return bundle.LoadAsset(assetName);
        }
        public UObject LoadAsset(string bundleName, string assetName, Type type)
        {
            AssetBundle bundle = LoadBundle(bundleName);
#if UNITY_EDITOR
            if (ezApplication.packageMode == PackageMode.Develop)
            {
                string assetKey = GetAssetKey(bundleName, assetName);
                string assetPath;
                if (assetPathDict.TryGetValue(assetKey, out assetPath))
                {
                    LogWarningFormat("{0} not exist.", assetKey);
                    return null;
                }
                else
                {
                    return UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, type);
                }
            }
#endif
            return bundle.LoadAsset(assetName, type);
        }
        public T[] LoadAllAssets<T>(string bundleName) where T : UObject
        {
            AssetBundle bundle = LoadBundle(bundleName);
#if UNITY_EDITOR
            if (ezApplication.packageMode == PackageMode.Develop)
            {
                string[] assetPaths = bundle.GetAllAssetNames();
                T[] assets = new T[assetPaths.Length];
                for (int i = 0; i < assetPaths.Length; i++)
                {
                    assets[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPaths[i]);
                }
                return assets;
            }
#endif
            return bundle.LoadAllAssets<T>();
        }
        public UObject[] LoadAllAssets(string bundleName)
        {
            AssetBundle bundle = LoadBundle(bundleName);
#if UNITY_EDITOR
            if (ezApplication.packageMode == PackageMode.Develop)
            {
                string[] assetPaths = bundle.GetAllAssetNames();
                UObject[] assets = new UObject[assetPaths.Length];
                for (int i = 0; i < assetPaths.Length; i++)
                {
                    assets[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<UObject>(assetPaths[i]);
                }
                return assets;
            }
#endif
            return bundle.LoadAllAssets();
        }
        public UObject[] LoadAllAssets(string bundleName, Type type)
        {
            AssetBundle bundle = LoadBundle(bundleName);
#if UNITY_EDITOR
            if (ezApplication.packageMode == PackageMode.Develop)
            {
                string[] assetPaths = bundle.GetAllAssetNames();
                List<UObject> assets = new List<UObject>();
                for (int i = 0; i < assetPaths.Length; i++)
                {
                    UObject asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPaths[i], type);
                    if (asset != null) assets.Add(asset);
                }
                return assets.ToArray();
            }
#endif
            return bundle.LoadAllAssets(type);
        }
        // 异步加载资源，把异步封装成了同步+回调，Editor+Develop模式会变成同步加载，可能复现不了一些异步问题
        public void LoadAssetAsync<T>(string bundleName, string assetName, OnAssetLoadedAction<T> callback) where T : UObject
        {
            StartCoroutine(Cor_LoadAssetAsync<T>(bundleName, assetName, callback));
        }
        public void LoadAssetAsync(string bundleName, string assetName, OnAssetLoadedAction<UObject> callback)
        {
            StartCoroutine(Cor_LoadAssetAsync(bundleName, assetName, callback));
        }
        public void LoadAssetAsync(string bundleName, string assetName, Type type, OnAssetLoadedAction<UObject> callback)
        {
            StartCoroutine(Cor_LoadAssetAsync(bundleName, assetName, type, callback));
        }
        IEnumerator Cor_LoadAssetAsync<T>(string bundleName, string assetName, OnAssetLoadedAction<T> callback) where T : UObject
        {
            yield return Cor_LoadBundleAsync(bundleName, null);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
#if UNITY_EDITOR
                if (ezApplication.packageMode == PackageMode.Develop)
                {
                    string assetKey = GetAssetKey(bundleName, assetName);
                    string assetPath;
                    if (!assetPathDict.TryGetValue(assetKey, out assetPath))
                    {
                        LogWarningFormat("{0} not exist.", assetKey);
                        callback(null);
                    }
                    else
                    {
                        callback(UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath));
                    }
                    yield break;
                }
#endif
                AssetBundleRequest abR = bundle.LoadAssetAsync<T>(assetName);
                yield return abR;
                if (abR.isDone) callback(abR.asset as T);
            }
            else
            {
                LogErrorFormat("Bundle not loaded: {0}", bundleName);
            }
        }
        IEnumerator Cor_LoadAssetAsync(string bundleName, string assetName, OnAssetLoadedAction<UObject> callback)
        {
            yield return Cor_LoadBundleAsync(bundleName, null);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
#if UNITY_EDITOR
                if (ezApplication.packageMode == PackageMode.Develop)
                {
                    string assetKey = GetAssetKey(bundleName, assetName);
                    string assetPath;
                    if (!assetPathDict.TryGetValue(assetKey, out assetPath))
                    {
                        LogWarningFormat("{0} not exist.", assetKey);
                        callback(null);
                    }
                    else
                    {
                        callback(UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(UObject)));
                    }
                    yield break;
                }
#endif
                AssetBundleRequest abR = bundle.LoadAssetAsync(assetName);
                yield return abR;
                if (abR.isDone) callback(abR.asset);
            }
            else
            {
                LogErrorFormat("Bundle not loaded: {0}", bundleName);
            }
        }
        IEnumerator Cor_LoadAssetAsync(string bundleName, string assetName, Type type, OnAssetLoadedAction<UObject> callback)
        {
            yield return Cor_LoadBundleAsync(bundleName, null);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
#if UNITY_EDITOR
                if (ezApplication.packageMode == PackageMode.Develop)
                {
                    string assetKey = GetAssetKey(bundleName, assetName);
                    string assetPath;
                    if (assetPathDict.TryGetValue(assetKey, out assetPath))
                    {
                        LogWarningFormat("{0} not exist", assetKey);
                        callback(null);
                    }
                    else
                    {
                        callback(UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, type));
                    }
                    yield break;
                }
#endif
                AssetBundleRequest abR = bundle.LoadAssetAsync(assetName, type);
                yield return abR;
                if (abR.isDone) callback(abR.asset);
            }
            else
            {
                LogErrorFormat("Bundle not loaded: {0}", bundleName);
            }
        }

        // 同步加载场景，经测试5.3.5为同步非阻塞
        public void LoadScene(string bundleName, string sceneName, LoadSceneMode mode)
        {
#if UNITY_EDITOR    // Editor+Develop模式下不加载bundle，直接读场景文件（需要将场景加到BuildSettings，打包时取消勾选）
            if (ezApplication.packageMode != PackageMode.Develop)
#endif
                LoadBundle(bundleName);
            SceneManager.LoadScene(sceneName, mode);
        }
        // 异步加载场景，把异步封装成了同步+回调
        public void LoadSceneAsync(string bundleName, string sceneName, LoadSceneMode mode = LoadSceneMode.Single, OnSceneChangedAction action = null)
        {
            StartCoroutine(Cor_LoadSceneAsync(bundleName, sceneName, mode, action));
        }
        IEnumerator Cor_LoadSceneAsync(string bundleName, string sceneName, LoadSceneMode loadSceneMode, OnSceneChangedAction action)
        {
            yield return null;
#if UNITY_EDITOR    // Editor+Develop模式下不加载bundle，直接读场景文件（需要将场景加到BuildSettings，打包时取消勾选）
            if (ezApplication.packageMode != PackageMode.Develop)
#endif
                yield return Cor_LoadBundleAsync(bundleName, null);
            AsyncOperation opr = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            yield return opr;
            if (action != null) action();
        }
        // 卸载场景
        public void UnloadSceneAsync(string sceneName, OnSceneChangedAction action = null)
        {
            StartCoroutine(Cor_UnloadSceneAsync(sceneName, action));
        }
        IEnumerator Cor_UnloadSceneAsync(string sceneName, OnSceneChangedAction action)
        {
            AsyncOperation opr = SceneManager.UnloadSceneAsync(sceneName);
            yield return opr;
            if (action != null) action();
        }
        // 场景切换，多场景的项目在加载和卸载场景后指定当前活动场景
        public void SetActiveScene(string sceneName)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        // 同步加载AssetBundle
        public AssetBundle LoadBundle(string bundleName)
        {
            bundleName = bundleName.ToLower();
            LoadDependencies(bundleName);
            AssetBundle bundle;
            if (!bundleDict.TryGetValue(bundleName, out bundle))
            {
                LogFormat("Load bundle from file: {0}", bundleName);
                bundle = AssetBundle.LoadFromFile(fileList[bundleName].filePath);
                bundleDict[bundleName] = bundle;
                GetAssetPathFromBundle(bundle);
            }
            return bundle;
        }
        protected void LoadDependencies(string bundleName)
        {
            if (manifest == null) return;
            string[] dependencies = manifest.GetAllDependencies(bundleName);
            foreach (string dep in dependencies)
            {
                LoadBundle(dep);
            }
        }
        // 异步加载AssetBundle
        public void LoadBundleAsync(string bundleName, OnBundleLoadedAction action = null)
        {
            StartCoroutine(Cor_LoadBundleAsync(bundleName, action));
        }
        IEnumerator Cor_LoadBundleAsync(string bundleName, OnBundleLoadedAction action)
        {
            bundleName = bundleName.ToLower();
            yield return Cor_LoadDependenciesAsync(bundleName);
            AssetBundle bundle;
            if (!bundleDict.TryGetValue(bundleName, out bundle)) // 是否已加载
            {
                AssetBundleCreateRequest abcr;
                if (!abcrDict.TryGetValue(bundleName, out abcr)) // 是否正在加载
                {
                    LogFormat("Load bundle from file async: {0}", bundleName);
                    abcr = AssetBundle.LoadFromFileAsync(fileList[bundleName].filePath);
                    abcrDict[bundleName] = abcr;
                }
                while (!abcr.isDone)
                {
                    yield return null;
                }
                abcrDict.Remove(bundleName); // 不管加载是否成功都需要移除
                if (abcr.isDone && abcr.assetBundle != null)
                {
                    bundle = abcr.assetBundle;
                    bundleDict[bundleName] = bundle;
                    GetAssetPathFromBundle(bundle);
                }
            }
            if (action != null) action(bundle);
        }
        private IEnumerator Cor_LoadDependenciesAsync(string bundleName)
        {
            if (manifest == null) yield break;
            yield return null;
            string[] dependencies = manifest.GetAllDependencies(bundleName);
            foreach (string dep in dependencies)
            {
                yield return Cor_LoadBundleAsync(dep, null);
            }
        }

        // 卸载AssetBundle
        public void UnloadBundle(string bundleName, bool unloadAll = false)
        {
            bundleName = bundleName.ToLower();
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
                LogFormat("Unload bundle: {0}", bundleName);
                bundle.Unload(unloadAll);
                bundleDict.Remove(bundleName);
            }
        }

#if UNITYADDRESSABLES
        public void LoadAddressableAsset(string address, OnAssetLoadedAction<UObject> callback)
        {
            Addressables.LoadAsset<UObject>(address).Completed += (opr) =>
            {
                if (callback != null) callback(opr.Result);
            };
        }
        public void InstantiateGameObject(string address, Transform parent = null, bool instantiateInWorldSpace = false)
        {
            Addressables.Instantiate<GameObject>(address, parent, instantiateInWorldSpace);
        }
        public void LoadAddressableScene(string address, LoadSceneMode mode, OnSceneChangedAction callback)
        {
            Addressables.LoadScene(address, mode).Completed += (opr) =>
            {
                if (callback != null) callback();
            };
        }
        public List<string> GetAddresses(string rootPath)
        {
            List<string> addresses = new List<string>();
            foreach (var locator in Addressables.ResourceLocators.Where(l => l is ResourceLocationMap))
            {
                foreach (object address in (locator as ResourceLocationMap).Locations.Keys)
                {
                    Debug.LogFormat("Map - {0}", address);
                }
                addresses.AddRange(
                    from key in (locator as ResourceLocationMap).Locations.Keys.Select(k => k as string)
                    where key != null && key.StartsWith(rootPath)
                    select key);
            }
            foreach (var locator in Addressables.ResourceLocators.Where(l => l is ResourceLocationData))
            {
                foreach (string address in (locator as ResourceLocationData).Keys)
                {
                    Debug.LogFormat("Data - {0}", address);
                }
                addresses.AddRange(
                    from key in (locator as ResourceLocationData).Keys.Select(k => k as string)
                    where key != null && key.StartsWith(rootPath)
                    select key);
            }
            return addresses;
        }
#endif
    }
}
