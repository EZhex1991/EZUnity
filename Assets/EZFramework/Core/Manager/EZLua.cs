/*
 * Author:      熊哲
 * CreateTime:  1/16/2017 4:16:01 PM
 * Description:
 * 
*/
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using XLua;

namespace EZFramework
{
    [LuaCallCSharp]
    public class EZLua : _EZManager<EZLua>
    {
        public string luaDirPath { get; private set; }
        public AssetBundle luaBundle { get; private set; }

        public LuaEnv luaEnv { get; private set; }
        private Action luaStart;
        private Action LuaExit;

        public override void Init()
        {
            base.Init();
            luaEnv = new LuaEnv();
            AddBuildin();
            AddLoader();
            luaEnv.DoString("require 'Main'");
            luaStart = luaEnv.Global.Get<Action>("Start");
            LuaExit = luaEnv.Global.Get<Action>("Exit");
            luaStart();
        }
        public override void Exit()
        {
            base.Exit();
            LuaExit();
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
                    luaDirPath = EZFacade.dataDirPath + EZFrameworkSettings.Instance.luaDirName + "/";
                    luaEnv.AddLoader(LoadFromFile);
                    break;
                case EZFrameworkSettings.RunMode.Local:
                    luaBundle = AssetBundle.LoadFromFile(EZFacade.streamingDirPath + EZFrameworkSettings.Instance.luaDirName.ToLower() + EZFrameworkSettings.Instance.bundleExtension);
                    luaEnv.AddLoader(LoadFromBundle);
                    break;
                case EZFrameworkSettings.RunMode.Update:
                    luaBundle = AssetBundle.LoadFromFile(EZFacade.persistentDirPath + EZFrameworkSettings.Instance.luaDirName.ToLower() + EZFrameworkSettings.Instance.bundleExtension);
                    luaEnv.AddLoader(LoadFromBundle);
                    break;
            }
        }

        private byte[] LoadFromFile(ref string fileName)
        {
            string filePath = luaDirPath + fileName.Replace('.', '/') + ".lua";             // lua文件的实际路径
            fileName = luaDirPath + fileName.Replace('.', '/');     // 返给lua调试器的路径
            try
            {
                // File.ReadAllBytes返回值可能会带有BOM（0xEF，0xBB，0xBF），这会导致脚本加载出错（</239>）
                byte[] script = System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(filePath));
                return script;
            }
            catch
            {
                return null;
            }
        }
        private byte[] LoadFromBundle(ref string fileName)
        {
            fileName = fileName.Replace("/", "_").Replace(".", "_") + ".lua.txt";
            TextAsset luaText = luaBundle.LoadAsset<TextAsset>(fileName);
            return luaText ? luaText.bytes : null;
        }

        public void Yield(object cor, Action callback)
        {
            StartCoroutine(Cor(cor, callback));
        }
        public IEnumerator Cor(object cor, Action callback)
        {
            if (cor is IEnumerator) yield return StartCoroutine((IEnumerator)cor);
            else yield return cor;
            callback();
        }

        public static bool IsNull(UnityEngine.Object o)
        {
            return o == null;
        }

        public static void SetItem(IDictionary dict, object key, object value)
        {
            dict[key] = value;
        }
    }
}