/*
 * Author:      熊哲
 * CreateTime:  11/22/2016 2:38:16 PM
 * Description:
 * 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace EZFramework
{
    public class EZResource : _EZManager<EZResource>
    {
        public EZLoadingPanel loadingPanel;

        public string bundleDirPath { get; private set; }
        public string bundleExtension { get; private set; }
        // Develop模式下用到的源文件路径
        protected Dictionary<string, string> assetPathDict = new Dictionary<string, string>();
        // 记录已加载过的bundle
        protected Dictionary<string, AssetBundle> bundleDict = new Dictionary<string, AssetBundle>();
        // 记录正在加载的bundle
        protected Dictionary<string, AssetBundleCreateRequest> abcrDict = new Dictionary<string, AssetBundleCreateRequest>();
        protected AssetBundleManifest manifest;

        public delegate void OnAssetLoadedAction(Object asset);
        public delegate void OnAssetLoadedAction<T>(T asset) where T : Object;
        public delegate void OnSceneLoadedAction();
        public delegate void OnBundleLoadedAction(AssetBundle bundle);

        // 初始化资源管理器，读取StreamingAssets的所有依赖（即所有的资源包名）
        public override void Init()
        {
            base.Init();
            switch (EZFrameworkSettings.Instance.runMode)
            {
                case EZFrameworkSettings.RunMode.Develop:
                    bundleDirPath = EZFacade.streamingDirPath;
                    break;
                case EZFrameworkSettings.RunMode.Local:
                    bundleDirPath = EZFacade.streamingDirPath;
                    break;
                case EZFrameworkSettings.RunMode.Update:
                    bundleDirPath = EZFacade.persistentDirPath;
                    break;
            }
            bundleExtension = EZFrameworkSettings.Instance.bundleExtension;
            AssetBundle bundle = AssetBundle.LoadFromFile(bundleDirPath + "StreamingAssets");
            if (bundle != null)
            {
                manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
        }
        public override void Exit()
        {
            base.Exit();
        }

        // 记录bundle里的资源路径，在Editor+Develop模式时可以直接从路径加载文件
        private void GetAssetPathFromBundle(AssetBundle bundle)
        {
            if (EZFrameworkSettings.Instance.runMode == EZFrameworkSettings.RunMode.Develop)
            {
                foreach (string filePath in bundle.GetAllAssetNames())
                {
                    string assetName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    assetPathDict[GetAssetKey(bundle.name, assetName)] = filePath;
                }
                foreach (string scenePath in bundle.GetAllScenePaths())
                {
                    string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                    assetPathDict[GetAssetKey(bundle.name, sceneName)] = scenePath;
                }
            }
        }
        private string GetAssetKey(string bundleName, string assetName)
        {
            bundleName = bundleName.ToLower();
            assetName = assetName.ToLower();
            if (!bundleName.EndsWith(bundleExtension)) bundleName += bundleExtension;
            return bundleName + "-" + assetName;
        }

        // 同步加载资源
        public T LoadAsset<T>(string bundleName, string assetName) where T : Object
        {
            AssetBundle bundle = LoadBundle(bundleName);
#if UNITY_EDITOR
            if (EZFrameworkSettings.Instance.runMode == EZFrameworkSettings.RunMode.Develop)
            {
                string assetKey = GetAssetKey(bundleName, assetName);
                string assetPath;
                if (!assetPathDict.TryGetValue(assetKey, out assetPath))
                {
                    LogWarning(assetKey + " not exist.");
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
        public Object LoadAsset(string bundleName, string assetName)
        {
            AssetBundle bundle = LoadBundle(bundleName);
#if UNITY_EDITOR
            if (EZFrameworkSettings.Instance.runMode == EZFrameworkSettings.RunMode.Develop)
            {
                string assetKey = GetAssetKey(bundleName, assetName);
                string assetPath;
                if (!assetPathDict.TryGetValue(assetKey, out assetPath))
                {
                    LogWarning(assetKey + " not exist.");
                    return null;
                }
                else
                {
                    return UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
                }
            }
#endif
            return bundle.LoadAsset(assetName);
        }
        public Object LoadAsset(string bundleName, string assetName, Type type)
        {
            AssetBundle bundle = LoadBundle(bundleName);
#if UNITY_EDITOR
            if (EZFrameworkSettings.Instance.runMode == EZFrameworkSettings.RunMode.Develop)
            {
                string assetKey = GetAssetKey(bundleName, assetName);
                string assetPath;
                if (assetPathDict.TryGetValue(assetKey, out assetPath))
                {
                    LogWarning(assetKey + " not exist.");
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
        // 异步加载资源，把异步封装成了同步+回调，Editor+Develop模式会变成同步加载，可能复现不了一些异步问题
        public void LoadAssetAsync<T>(string bundleName, string assetName, OnAssetLoadedAction<T> callback) where T : Object
        {
            StartCoroutine(Cor_LoadAssetAsync<T>(bundleName, assetName, callback));
        }
        public void LoadAssetAsync(string bundleName, string assetName, OnAssetLoadedAction<Object> callback)
        {
            StartCoroutine(Cor_LoadAssetAsync(bundleName, assetName, callback));
        }
        public void LoadAssetAsync(string bundleName, string assetName, Type type, OnAssetLoadedAction<Object> callback)
        {
            StartCoroutine(Cor_LoadAssetAsync(bundleName, assetName, type, callback));
        }
        IEnumerator Cor_LoadAssetAsync<T>(string bundleName, string assetName, OnAssetLoadedAction<T> callback) where T : Object
        {
            yield return Cor_LoadBundleAsync(bundleName, null);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
#if UNITY_EDITOR
                if (EZFrameworkSettings.Instance.runMode == EZFrameworkSettings.RunMode.Develop)
                {
                    string assetKey = GetAssetKey(bundleName, assetName);
                    string assetPath;
                    if (!assetPathDict.TryGetValue(assetKey, out assetPath))
                    {
                        LogWarning(assetKey + " not exist.");
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
                LogError("Bundle not loaded: " + bundleName);
            }
        }
        IEnumerator Cor_LoadAssetAsync(string bundleName, string assetName, OnAssetLoadedAction<Object> callback)
        {
            yield return Cor_LoadBundleAsync(bundleName, null);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
#if UNITY_EDITOR
                if (EZFrameworkSettings.Instance.runMode == EZFrameworkSettings.RunMode.Develop)
                {
                    string assetKey = GetAssetKey(bundleName, assetName);
                    string assetPath;
                    if (!assetPathDict.TryGetValue(assetKey, out assetPath))
                    {
                        LogWarning(assetKey + " not exist.");
                        callback(null);
                    }
                    else
                    {
                        callback(UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object)));
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
                LogError("Bundle not loaded: " + bundleName);
            }
        }
        IEnumerator Cor_LoadAssetAsync(string bundleName, string assetName, Type type, OnAssetLoadedAction<Object> callback)
        {
            yield return Cor_LoadBundleAsync(bundleName, null);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
#if UNITY_EDITOR
                if (EZFrameworkSettings.Instance.runMode == EZFrameworkSettings.RunMode.Develop)
                {
                    string assetKey = GetAssetKey(bundleName, assetName);
                    string assetPath;
                    if (assetPathDict.TryGetValue(assetKey, out assetPath))
                    {
                        LogWarning(assetKey + " not exist.");
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
                LogError("Bundle not loaded: " + bundleName);
            }
        }

        // 同步加载场景，经测试5.3.5为同步非阻塞，基本没有实际用途
        public void LoadScene(string bundleName, string sceneName, LoadSceneMode mode, bool setActive = true)
        {
            if (loadingPanel != null) loadingPanel.ShowProgress("Loading");
            LoadBundle(sceneName);
            SceneManager.LoadScene(sceneName, mode);
            if (setActive) SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName)); // 这里并非同步阻塞，该语句执行不了
            if (loadingPanel != null) loadingPanel.LoadComplete();
        }
        // 异步加载场景，把异步封装成了同步+回调
        public void LoadSceneAsync(string bundleName, string sceneName, LoadSceneMode mode = LoadSceneMode.Single, OnSceneLoadedAction action = null)
        {
            StartCoroutine(Cor_LoadSceneAsync(bundleName, sceneName, mode, action));
        }
        IEnumerator Cor_LoadSceneAsync(string bundleName, string sceneName, LoadSceneMode mode, OnSceneLoadedAction action)
        {
            if (loadingPanel != null) loadingPanel.ShowProgress("Loading", 0);
            yield return null;
#if UNITY_EDITOR    // Editor+Develop模式下不加载bundle，直接读场景文件（需要将场景加到BuildSettings，打包时取消勾选）
            if (EZFrameworkSettings.Instance.runMode != EZFrameworkSettings.RunMode.Develop)
#endif
                yield return Cor_LoadBundleAsync(bundleName, null);
            AsyncOperation opr = SceneManager.LoadSceneAsync(sceneName, mode);
            while (!opr.isDone)
            {
                if (loadingPanel != null) loadingPanel.ShowProgress("Loading", opr.progress + 0.1f);
                yield return null;
            }
            if (action != null) action();
            yield return null;
            if (loadingPanel != null) loadingPanel.LoadComplete();
        }
        // 卸载场景
        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadScene(sceneName);
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
            if (!bundleName.EndsWith(bundleExtension)) bundleName += bundleExtension;
            LoadDependencies(bundleName);
            AssetBundle bundle;
            if (!bundleDict.TryGetValue(bundleName, out bundle))
            {
                Log("Load bundle from file: " + bundleName);
                string bundlePath = bundleDirPath + bundleName;
                bundle = AssetBundle.LoadFromFile(bundlePath);
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
            if (!bundleName.EndsWith(bundleExtension)) bundleName += bundleExtension;
            yield return Cor_LoadDependenciesAsync(bundleName);
            AssetBundle bundle;
            if (!bundleDict.TryGetValue(bundleName, out bundle)) // 是否已加载
            {
                AssetBundleCreateRequest abcr;
                if (!abcrDict.TryGetValue(bundleName, out abcr)) // 是否正在加载
                {
                    Log("Load bundle from file async: " + bundleName);
                    string bundlePath = bundleDirPath + bundleName;
                    abcr = AssetBundle.LoadFromFileAsync(bundlePath);
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
        IEnumerator Cor_LoadDependenciesAsync(string bundleName)
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
            if (!bundleName.EndsWith(bundleExtension)) bundleName += bundleExtension;
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
                Log("Unload bundle: " + bundleName);
                bundle.Unload(unloadAll);
                bundleDict.Remove(bundleName);
            }
        }
    }
}