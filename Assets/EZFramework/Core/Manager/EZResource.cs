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
        // 记录已加载过的资源包
        protected Dictionary<string, AssetBundle> bundleDict = new Dictionary<string, AssetBundle>();
        protected AssetBundleManifest manifest;

        public delegate void OnAssetLoadedAction(Object asset);
        public delegate void OnAssetLoadedAction<T>(T asset) where T : Object;
        public delegate void OnSceneLoadedAction();

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
#if UNITY_EDITOR
            LoadBundle(bundleName);
            if (EZFrameworkSettings.Instance.runMode == EZFrameworkSettings.RunMode.Develop)
            {
                string assetKey = GetAssetKey(bundleName, assetName);
                string assetPath;
                if (!assetPathDict.TryGetValue(assetKey, out assetPath))
                    Debug.LogWarning(assetKey + " not exist.");
                else
                    return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
            return LoadBundle(bundleName).LoadAsset<T>(assetName);
#else
            return LoadBundle(bundleName).LoadAsset<T>(assetName);
#endif
        }
        public Object LoadAsset(string bundleName, string assetName)
        {
#if UNITY_EDITOR
            LoadBundle(bundleName);
            if (EZFrameworkSettings.Instance.runMode == EZFrameworkSettings.RunMode.Develop)
            {
                string assetKey = GetAssetKey(bundleName, assetName);
                string assetPath;
                if (!assetPathDict.TryGetValue(assetKey, out assetPath))
                    Debug.LogWarning(assetKey + " not exist.");
                else
                    return UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
            }
            return LoadBundle(bundleName).LoadAsset(assetName);
#else
            return LoadBundle(bundleName).LoadAsset(assetName);
#endif
        }
        public Object LoadAsset(string bundleName, string assetName, Type type)
        {
#if UNITY_EDITOR
            LoadBundle(bundleName);
            if (EZFrameworkSettings.Instance.runMode == EZFrameworkSettings.RunMode.Develop)
            {
                string assetKey = GetAssetKey(bundleName, assetName);
                string assetPath;
                if (assetPathDict.TryGetValue(assetKey, out assetPath))
                    Debug.LogWarning(assetKey + " not exist.");
                else
                    return UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, type);
            }
            return LoadBundle(bundleName).LoadAsset(assetName, type);
#else
            return LoadBundle(bundleName).LoadAsset(assetName, type);
#endif
        }
        // 异步加载资源
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
            yield return null;
            yield return Cor_LoadBundleAsync(bundleName);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
                AssetBundleRequest abR = bundle.LoadAssetAsync<T>(assetName);
                yield return abR;
                if (abR.isDone) callback(abR.asset as T);
            }
        }
        IEnumerator Cor_LoadAssetAsync(string bundleName, string assetName, OnAssetLoadedAction<Object> callback)
        {
            yield return null;
            yield return Cor_LoadBundleAsync(bundleName);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
                AssetBundleRequest abR = bundle.LoadAssetAsync(assetName);
                yield return abR;
                if (abR.isDone) callback(abR.asset);
            }
        }
        IEnumerator Cor_LoadAssetAsync(string bundleName, string assetName, Type type, OnAssetLoadedAction<Object> callback)
        {
            yield return null;
            yield return Cor_LoadBundleAsync(bundleName);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
                AssetBundleRequest abR = bundle.LoadAssetAsync(assetName, type);
                yield return abR;
                if (abR.isDone) callback(abR.asset);
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
        // 异步加载场景
        public void LoadSceneAsync(string bundleName, string sceneName, LoadSceneMode mode, OnSceneLoadedAction action = null)
        {
            StartCoroutine(Cor_LoadSceneAsync(bundleName, sceneName, mode, true, action));
        }
        public void LoadSceneAsync(string bundleName, string sceneName, LoadSceneMode mode, bool setActive, OnSceneLoadedAction action = null)
        {
            StartCoroutine(Cor_LoadSceneAsync(bundleName, sceneName, mode, setActive, action));
        }
        IEnumerator Cor_LoadSceneAsync(string bundleName, string sceneName, LoadSceneMode mode, bool setActive, OnSceneLoadedAction action)
        {
            if (loadingPanel != null) loadingPanel.ShowProgress("Loading", 0);
            yield return null;
            if (!(EZFrameworkSettings.Instance.runMode == EZFrameworkSettings.RunMode.Develop))
                yield return Cor_LoadBundleAsync(bundleName);
            AsyncOperation opr = SceneManager.LoadSceneAsync(sceneName, mode);
            while (!opr.isDone)
            {
                if (loadingPanel != null) loadingPanel.ShowProgress("Loading", opr.progress + 0.1f);
                yield return null;
            }
            if (setActive) SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            yield return null;
            if (loadingPanel != null) loadingPanel.LoadComplete();
            if (action != null) action();
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
                bundleDict.Add(bundleName, bundle);
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
        IEnumerator Cor_LoadBundleAsync(string bundleName)
        {
            bundleName = bundleName.ToLower();
            if (!bundleName.EndsWith(bundleExtension)) bundleName += bundleExtension;
            yield return null;
            yield return Cor_LoadDependenciesAsync(bundleName);
            if (!bundleDict.ContainsKey(bundleName))
            {
                Log("Load bundle from file async: " + bundleName);
                string bundlePath = bundleDirPath + bundleName;
                AssetBundleCreateRequest abCR = AssetBundle.LoadFromFileAsync(bundlePath);
                yield return abCR;
                if (abCR.isDone) bundleDict.Add(bundleName, abCR.assetBundle);
                GetAssetPathFromBundle(abCR.assetBundle);
            }
        }
        IEnumerator Cor_LoadDependenciesAsync(string bundleName)
        {
            if (manifest == null) yield break;
            yield return null;
            string[] dependencies = manifest.GetAllDependencies(bundleName);
            foreach (string dep in dependencies)
            {
                yield return Cor_LoadBundleAsync(dep);
            }
        }

        // 卸载场景
        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadScene(sceneName);
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