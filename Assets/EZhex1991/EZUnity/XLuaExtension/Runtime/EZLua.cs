/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-01-16 16:16:01
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
using EZhex1991.EZUnity.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity.XLuaExtension
{
    public delegate LuaTable LuaRequire(string moduleName);
    public delegate void LuaAction();
    public delegate void LuaAction<T>(T arg);

    public class EZLua : EZMonoBehaviourSingleton<EZLua>
    {
        private EZApplication ezApplication { get { return EZApplication.Instance; } }
        private EZResources ezResources { get { return EZResources.Instance; } }
        private EZApplicationSettings settings { get { return EZApplicationSettings.Instance; } }

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

        // require大小写敏感而文件路径大小写不敏感，还有点"."和斜线"/"的混用，会造成重复加载等一系列问题
        // 这里提前把所有文件记录于Dictionary中，直接对CustomLoader传入的参数进行TryGetValue，保证lua侧require参数的统一
        // field和property结合实现lazy load
        private Dictionary<string, string> m_LuaFiles;
        private Dictionary<string, string> luaFiles
        {
            get
            {
                if (m_LuaFiles == null)
                {
                    m_LuaFiles = new Dictionary<string, string>();
                    for (int i = 0; i < settings.luaFolders.Length; i++)
                    {
                        string dir = Path.Combine(ezApplication.dataPath, settings.luaFolders[i]);
                        IEnumerable<string> files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
                            .Where(path => path.EndsWith(".lua") || path.EndsWith(".lua.txt"));
                        foreach (string filePath in files)
                        {
                            string key = filePath.Substring(dir.Length + 1).Replace("\\", "/").Replace("/", ".").Replace(".lua.txt", "").Replace(".lua", "");
                            m_LuaFiles.Add(key, filePath);
                        }
                    }
                }
                return m_LuaFiles;
            }
        }
        private Dictionary<string, TextAsset> m_LuaAssets;
        private Dictionary<string, TextAsset> luaAssets
        {
            get
            {
                if (m_LuaAssets == null)
                {
                    m_LuaAssets = new Dictionary<string, TextAsset>();
                    for (int i = 0; i < settings.luaBundles.Length; i++)
                    {
                        AssetBundle bundle;
                        switch (ezApplication.runMode)
                        {
                            case RunMode.Package:
                                string bundlePath = Path.Combine(ezApplication.streamingAssetsPath, settings.luaBundles[i].ToLower());
                                bundle = AssetBundle.LoadFromFile(bundlePath);
                                break;
                            case RunMode.Update:
                                bundle = ezResources.LoadBundle(settings.luaBundles[i].ToLower());
                                break;
                            default:
                                bundle = null;
                                break;
                        }
                        if (bundle == null) continue;
                        TextAsset[] assets = bundle.LoadAllAssets<TextAsset>();
                        for (int j = 0; j < assets.Length; j++)
                        {
                            string key = assets[j].name.Substring(0, assets[j].name.Length - 4).Replace("__", ".");
                            m_LuaAssets.Add(key, assets[j]);
                        }
                    }
                }
                return m_LuaAssets;
            }
        }

        private LuaAction luaStart;
        private LuaAction luaExit;

        protected override void Init()
        {
            AddBuildin();
            switch (ezApplication.runMode)
            {
                case RunMode.Develop:
                    luaEnv.AddLoader(LoadFromFile);
                    break;
                case RunMode.Package:
                    luaEnv.AddLoader(LoadFromBundle);
                    break;
                case RunMode.Update:
                    luaEnv.AddLoader(LoadFromBundle);
                    break;
            }
            ezApplication.onApplicationQuitEvent += ExitLua;
        }

        public void StartLua()
        {
            if (!string.IsNullOrEmpty(settings.luaBootModule))
            {
                luaRequire(settings.luaBootModule);
                luaStart = luaEnv.Global.Get<LuaAction>(settings.luaEntrance);
                luaExit = luaEnv.Global.Get<LuaAction>(settings.luaExit);
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

        private byte[] LoadFromFile(ref string fileKey)
        {
            string filePath;
            if (luaFiles.TryGetValue(fileKey, out filePath))
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
            else
            {
                return null;
            }
        }
        private byte[] LoadFromBundle(ref string fileKey)
        {
            TextAsset luaText;
            if (luaAssets.TryGetValue(fileKey, out luaText))
            {
                return luaText.bytes;
            }
            else
            {
                return null;
            }
        }

        void Update()
        {
            if (luaEnv != null)
            {
                luaEnv.Tick();
            }
        }
    }
}
#endif
