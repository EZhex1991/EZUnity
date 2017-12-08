/*
 * Author:      熊哲
 * CreateTime:  1/16/2017 4:16:01 PM
 * Description:
 * 
*/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

namespace EZFramework
{
    [LuaCallCSharp]
    public class EZLua : _EZManager<EZLua>
    {
        public LuaEnv luaEnv { get; private set; }

        private Dictionary<string, string> luaFiles = new Dictionary<string, string>();
        private Dictionary<string, TextAsset> luaAssets = new Dictionary<string, TextAsset>();

        public delegate void LuaAction();
        public delegate void LuaCoroutineCallback();
        private LuaAction luaStart;
        private LuaAction luaExit;

        public override void Init()
        {
            base.Init();
            luaEnv = new LuaEnv();
            AddBuildin();
            AddLoader();
            luaEnv.DoString("require 'Main'");
            luaStart = luaEnv.Global.Get<LuaAction>("Start");
            luaExit = luaEnv.Global.Get<LuaAction>("Exit");
            if (luaStart != null) luaStart();
        }
        public override void Exit()
        {
            base.Exit();
            if (luaExit != null) luaExit();
        }

        private void AddBuildin()
        {
            //luaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
        }
        private void AddLoader()
        {
            // require大小写敏感而文件路径大小写不敏感，还有点"."和斜线"/"的混用，会造成重复加载等一系列问题
            // 这里提前把所有文件记录于Dictionary中，直接对CustomLoader传入的参数进行TryGetValue，保证lua侧require参数的统一
            switch (EZFrameworkSettings.Instance.runMode)
            {
                case EZFrameworkSettings.RunMode.Develop:
                    for (int i = 0; i < EZFrameworkSettings.Instance.luaDirList.Count; i++)
                    {
                        string dir = EZFacade.dataDirPath + EZFrameworkSettings.Instance.luaDirList[i] + "/";
                        string[] files = Directory.GetFiles(dir, "*.lua", SearchOption.AllDirectories);
                        foreach (string filePath in files)
                        {
                            string key = filePath.Replace("\\", "/").Replace(dir, "").Replace("/", ".").Replace(".lua", "");
                            luaFiles.Add(key, filePath);
                        }
                    }
                    luaEnv.AddLoader(LoadFromFile);
                    break;
                case EZFrameworkSettings.RunMode.Local:
                    for (int i = 0; i < EZFrameworkSettings.Instance.luaBundleList.Count; i++)
                    {
                        AssetBundle bundle = AssetBundle.LoadFromFile(EZFacade.streamingDirPath + EZFrameworkSettings.Instance.luaBundleList[i].ToLower() + EZFrameworkSettings.Instance.bundleExtension);
                        TextAsset[] assets = bundle.LoadAllAssets<TextAsset>();
                        for (int j = 0; j < assets.Length; j++)
                        {
                            string key = assets[j].name.Replace("__", ".").Replace(".lua", "");
                            luaAssets.Add(key, assets[j]);
                        }
                    }
                    luaEnv.AddLoader(LoadFromBundle);
                    break;
                case EZFrameworkSettings.RunMode.Update:
                    for (int i = 0; i < EZFrameworkSettings.Instance.luaBundleList.Count; i++)
                    {
                        AssetBundle bundle = AssetBundle.LoadFromFile(EZFacade.persistentDirPath + EZFrameworkSettings.Instance.luaBundleList[i].ToLower() + EZFrameworkSettings.Instance.bundleExtension);
                        TextAsset[] assets = bundle.LoadAllAssets<TextAsset>();
                        for (int j = 0; j < assets.Length; j++)
                        {
                            string key = assets[j].name.Replace("__", ".").Replace(".lua", "");
                            luaAssets.Add(key, assets[j]);
                        }
                    }
                    luaEnv.AddLoader(LoadFromBundle);
                    break;
            }
        }

        private byte[] LoadFromFile(ref string filePath)
        {
            if (luaFiles.TryGetValue(filePath, out filePath))
            {
                try
                {
                    // File.ReadAllBytes返回值可能会带有BOM（0xEF，0xBB，0xBF），这会导致脚本加载出错（<\239>）
                    byte[] script = System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(filePath));
                    return script;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
        private byte[] LoadFromBundle(ref string filePath)
        {
            TextAsset luaText;
            if (luaAssets.TryGetValue(filePath, out luaText))
            {
                return luaText.bytes;
            }
            return null;
        }

        public void Yield(object cor, LuaCoroutineCallback callback)
        {
            StartCoroutine(Cor(cor, callback));
        }
        public IEnumerator Cor(object cor, LuaCoroutineCallback callback)
        {
            if (cor is IEnumerator) yield return StartCoroutine((IEnumerator)cor);
            else yield return cor;
            callback();
        }
    }
}