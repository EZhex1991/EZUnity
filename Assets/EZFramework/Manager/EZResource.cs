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
    public class EZResource : TEZManager<EZResource>
    {
        public LoadingPanel loadingPanel;

        public string bundleDirPath { get; private set; }
        public string bundleExtension { get; private set; }
        // 记录已加载过的资源包
        protected Dictionary<string, AssetBundle> bundleDict = new Dictionary<string, AssetBundle>();
        protected AssetBundleManifest manifest;
        
        // 初始化资源管理器，读取StreamingAssets的所有依赖（即所有的资源包名）
        public override void Init()
        {
            base.Init();
            bundleDirPath = EZSettings.Instance.runMode == EZSettings.RunMode.Develop
                            ? EZUtility.streamingDirPath
                            : EZUtility.persistentDirPath;
            bundleExtension = EZSettings.Instance.bundleExtension;
            AssetBundle bundle = AssetBundle.LoadFromFile(bundleDirPath + "StreamingAssets");
            manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        public override void Exit()
        {
            base.Exit();
        }

        // 同步加载资源
        public T LoadAsset<T>(string bundleName, string assetName) where T : Object
        {
            return LoadBundle(bundleName).LoadAsset<T>(assetName);
        }
        public Object LoadAsset(string bundleName, string assetName)
        {
            return LoadBundle(bundleName).LoadAsset(assetName);
        }
        public Object LoadAsset(string bundleName, string assetName, Type type)
        {
            return LoadBundle(bundleName).LoadAsset(assetName, type);
        }
        // 异步加载资源
        public void LoadAssetAsync<T>(string bundleName, string assetName, Action<T> callback) where T : Object
        {
            StartCoroutine(Cor_LoadAssetAsync<T>(bundleName, assetName, callback));
        }
        public void LoadAssetAsync(string bundleName, string assetName, Action<Object> callback)
        {
            StartCoroutine(Cor_LoadAssetAsync(bundleName, assetName, callback));
        }
        public void LoadAssetAsync(string bundleName, string assetName, Type type, Action<Object> callback)
        {
            StartCoroutine(Cor_LoadAssetAsync(bundleName, assetName, type, callback));
        }
        IEnumerator Cor_LoadAssetAsync<T>(string bundleName, string assetName, Action<T> callback) where T : Object
        {
            yield return Cor_LoadBundleAsync(bundleName);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
                AssetBundleRequest abR = bundle.LoadAssetAsync<T>(assetName);
                yield return abR;
                if (abR.isDone) callback(abR.asset as T);
            }
        }
        IEnumerator Cor_LoadAssetAsync(string bundleName, string assetName, Action<Object> callback)
        {
            yield return Cor_LoadBundleAsync(bundleName);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
                AssetBundleRequest abR = bundle.LoadAssetAsync(assetName);
                yield return abR;
                if (abR.isDone) callback(abR.asset);
            }
        }
        IEnumerator Cor_LoadAssetAsync(string bundleName, string assetName, Type type, Action<Object> callback)
        {
            yield return Cor_LoadBundleAsync(bundleName);
            AssetBundle bundle;
            if (bundleDict.TryGetValue(bundleName, out bundle))
            {
                AssetBundleRequest abR = bundle.LoadAssetAsync(assetName, type);
                yield return abR;
                if (abR.isDone) callback(abR.asset);
            }
        }
        // 同步加载场景，经测试5.3.5为同步非阻塞，实际意义不大
        public void LoadScene(string sceneName, LoadSceneMode mode, bool setActive = true)
        {
            if (loadingPanel != null) loadingPanel.ShowProgress("Loading");
            LoadBundle(sceneName);
            SceneManager.LoadScene(sceneName, mode);
            if (setActive) SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName)); // 这里并非同步阻塞，该语句执行不了
            if (loadingPanel != null) loadingPanel.LoadComplete();
        }
        // 异步加载场景
        public void LoadSceneAsync(string sceneName, LoadSceneMode mode, Action action)
        {
            StartCoroutine(Cor_LoadSceneAsync(sceneName, mode, true, action));
        }
        public void LoadSceneAsync(string sceneName, LoadSceneMode mode, bool setActive = true, Action action = null)
        {
            StartCoroutine(Cor_LoadSceneAsync(sceneName, mode, setActive, action));
        }
        IEnumerator Cor_LoadSceneAsync(string sceneName, LoadSceneMode mode, bool setActive, Action action)
        {
            if (loadingPanel != null) loadingPanel.ShowProgress("Loading");
            yield return Cor_LoadBundleAsync(sceneName);
            AsyncOperation opr = SceneManager.LoadSceneAsync(sceneName, mode);
            while (!opr.isDone)
            {
                if (loadingPanel != null) loadingPanel.ShowProgress("Loading", opr.progress);
                yield return null;
            }
            if (setActive) SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            if (action != null) action();
            if (loadingPanel != null) loadingPanel.LoadComplete();
        }

        // 同步加载AssetBundle
        public AssetBundle LoadBundle(string bundleName)
        {
            bundleName = bundleName.ToLower();
            if (!bundleName.EndsWith(bundleExtension))
            {
                bundleName += bundleExtension;
            }
            LoadDependencies(bundleName);
            AssetBundle bundle;
            if (!bundleDict.TryGetValue(bundleName, out bundle))
            {
                Log("Load bundle from file: " + bundleName);
                string bundlePath = bundleDirPath + bundleName;
                bundle = AssetBundle.LoadFromFile(bundlePath);
                bundleDict.Add(bundleName, bundle);
            }
            return bundle;
        }
        protected void LoadDependencies(string bundleName)
        {
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
            if (!bundleName.EndsWith(bundleExtension))
            {
                bundleName += bundleExtension;
            }
            yield return Cor_LoadDependenciesAsync(bundleName);
            if (!bundleDict.ContainsKey(bundleName))
            {
                Log("Load bundle from file async: " + bundleName);
                string bundlePath = bundleDirPath + bundleName;
                AssetBundleCreateRequest abCR = AssetBundle.LoadFromFileAsync(bundlePath);
                yield return abCR;
                if (abCR.isDone) bundleDict.Add(bundleName, abCR.assetBundle);
            }
        }
        IEnumerator Cor_LoadDependenciesAsync(string bundleName)
        {
            string[] dependencies = manifest.GetAllDependencies(bundleName);
            foreach (string dep in dependencies)
            {
                yield return Cor_LoadBundleAsync(dep);
            }
        }

        // 卸载场景
        public void UnloadScene(string sceneName, bool unloadBundle = false, bool unloadAll = false)
        {
            SceneManager.UnloadScene(sceneName);
            if (unloadBundle)
            {
                UnloadBundle(sceneName, unloadAll);
            }
        }
        // 卸载AssetBundle
        public void UnloadBundle(string bundleName, bool unloadAll = false)
        {
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