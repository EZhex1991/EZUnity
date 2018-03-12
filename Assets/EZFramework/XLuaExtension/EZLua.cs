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

namespace EZFramework.XLuaExtension
{
    public delegate LuaTable LuaRequire(string moduleName);
    public delegate void LuaAction();
    public delegate void LuaCoroutineCallback();

    public class EZLua : _EZManager<EZLua>
    {
        public string bootModule;
        public string entrance;
        public string exit;

        private LuaEnv m_LuaEnv = new LuaEnv();
        public LuaEnv luaEnv { get { return m_LuaEnv; } }
        private LuaRequire m_LuaRequire;
        public LuaRequire luaRequire
        {
            get
            {
                if (m_LuaRequire == null)
                {
                    m_LuaRequire = luaEnv.Global.Get<LuaRequire>("require");
                }
                return m_LuaRequire;
            }
        }

        private Dictionary<string, string> luaFiles = new Dictionary<string, string>();

        // require大小写敏感而文件路径大小写不敏感，还有点"."和斜线"/"的混用，会造成重复加载等一系列问题
        // 这里提前把所有文件记录于Dictionary中，直接对CustomLoader传入的参数进行TryGetValue，保证lua侧require参数的统一
        private Dictionary<string, TextAsset> luaAssets = new Dictionary<string, TextAsset>();

        private LuaAction luaStart;
        private LuaAction luaExit;

        protected override void Awake()
        {
            base.Awake();
            AddBuildin();
            switch (EZFrameworkSettings.Instance.runMode)
            {
                case RunMode.Develop:
                    AddDevelopLoader();
                    EZFacade.Instance.onApplicationStartEvent += StartLua;
                    break;
                case RunMode.Local:
                    AddLocalLoader();
                    EZFacade.Instance.onApplicationStartEvent += StartLua;
                    break;
                case RunMode.Update:
                    EZUpdate.Instance.onUpdateCompleteEvent += delegate ()
                    {
                        AddUpdateLoader();  // Update模式下需要先更新再添加Loader
                        StartLua();
                    };
                    break;
            }
            EZFacade.Instance.onApplicationQuitEvent += ExitLua;
        }

        public void StartLua()
        {
            if (!string.IsNullOrEmpty(bootModule))
            {
                luaRequire(bootModule);
                luaStart = luaEnv.Global.Get<LuaAction>(entrance);
                luaExit = luaEnv.Global.Get<LuaAction>(exit);
            }
            if (luaStart != null) luaStart();
        }
        public void ExitLua()
        {
            if (luaExit != null) luaExit();
        }

        private void AddBuildin()
        {
            //luaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
        }
        private void AddDevelopLoader()
        {
            for (int i = 0; i < EZFrameworkSettings.Instance.luaDirList.Count; i++)
            {
                string dir = EZFacade.Instance.dataDirPath + EZFrameworkSettings.Instance.luaDirList[i] + "/";
                string[] files = Directory.GetFiles(dir, "*.lua", SearchOption.AllDirectories);
                foreach (string filePath in files)
                {
                    string key = filePath.Replace("\\", "/").Replace(dir, "").Replace("/", ".").Replace(".lua", "");
                    luaFiles.Add(key, filePath);
                }
            }
            luaEnv.AddLoader(LoadFromFile);
        }
        private void AddLocalLoader()
        {
            for (int i = 0; i < EZFrameworkSettings.Instance.luaBundleList.Count; i++)
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(EZFacade.Instance.streamingDirPath + EZFrameworkSettings.Instance.luaBundleList[i].ToLower());
                if (bundle == null) continue;
                TextAsset[] assets = bundle.LoadAllAssets<TextAsset>();
                for (int j = 0; j < assets.Length; j++)
                {
                    string key = assets[j].name.Replace("__", ".").Replace(".lua", "");
                    luaAssets.Add(key, assets[j]);
                }
            }
            luaEnv.AddLoader(LoadFromBundle);
        }
        private void AddUpdateLoader()
        {
            for (int i = 0; i < EZFrameworkSettings.Instance.luaBundleList.Count; i++)
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(EZFacade.Instance.persistentDirPath + EZFrameworkSettings.Instance.luaBundleList[i].ToLower());
                if (bundle == null) continue;
                TextAsset[] assets = bundle.LoadAllAssets<TextAsset>();
                for (int j = 0; j < assets.Length; j++)
                {
                    string key = assets[j].name.Replace("__", ".").Replace(".lua", "");
                    luaAssets.Add(key, assets[j]);
                }
            }
            luaEnv.AddLoader(LoadFromBundle);
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

        void Update()
        {
            if (luaEnv != null)
            {
                luaEnv.Tick();
            }
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