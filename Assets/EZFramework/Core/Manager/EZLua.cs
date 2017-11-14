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

        private List<string> luaDirList = new List<string>();
        private List<AssetBundle> luaBundleList = new List<AssetBundle>();

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
            switch (EZFrameworkSettings.Instance.runMode)
            {
                case EZFrameworkSettings.RunMode.Develop:
                    for (int i = 0; i < EZFrameworkSettings.Instance.luaDirList.Count; i++)
                    {
                        luaDirList.Add(EZFacade.dataDirPath + EZFrameworkSettings.Instance.luaDirList[i] + "/");
                    }
                    luaEnv.AddLoader(LoadFromFile);
                    break;
                case EZFrameworkSettings.RunMode.Local:
                    for (int i = 0; i < EZFrameworkSettings.Instance.luaBundleList.Count; i++)
                    {
                        luaBundleList.Add(AssetBundle.LoadFromFile(EZFacade.streamingDirPath + EZFrameworkSettings.Instance.luaBundleList[i].ToLower() + EZFrameworkSettings.Instance.bundleExtension));
                    }
                    luaEnv.AddLoader(LoadFromBundle);
                    break;
                case EZFrameworkSettings.RunMode.Update:
                    for (int i = 0; i < EZFrameworkSettings.Instance.luaBundleList.Count; i++)
                    {
                        luaBundleList.Add(AssetBundle.LoadFromFile(EZFacade.persistentDirPath + EZFrameworkSettings.Instance.luaBundleList[i].ToLower() + EZFrameworkSettings.Instance.bundleExtension));
                    }
                    luaEnv.AddLoader(LoadFromBundle);
                    break;
            }
        }

        private byte[] LoadFromFile(ref string fileName)
        {
            for (int i = 0; i < luaDirList.Count; i++)
            {
                string filePath = luaDirList[i] + fileName.Replace('.', '/') + ".lua";             // lua文件的实际路径
                try
                {
                    // File.ReadAllBytes返回值可能会带有BOM（0xEF，0xBB，0xBF），这会导致脚本加载出错（<\239>）
                    byte[] script = System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(filePath));
                    return script;
                }
                catch
                {
                    continue;
                }
            }
            return null;
        }
        private byte[] LoadFromBundle(ref string fileName)
        {
            for (int i = 0; i < luaBundleList.Count; i++)
            {
                fileName = fileName.Replace("/", "_").Replace(".", "_") + ".lua.txt";
                TextAsset luaText = luaBundleList[i].LoadAsset<TextAsset>(fileName);
                if (luaText != null) return luaText.bytes;
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