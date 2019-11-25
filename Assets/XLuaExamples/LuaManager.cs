/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-25 11:13:00
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity.XLuaExample
{
    [LuaCallCSharp]
    public class LuaManager : MonoBehaviour
    {
        private static LuaManager m_Instance;
        public static LuaManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = FindObjectOfType<LuaManager>();
                    if (m_Instance == null)
                    {
                        m_Instance = new GameObject("LuaManager").AddComponent<LuaManager>();
                    }
                }
                return m_Instance;
            }
        }

        public static string luaDirPath { get { return Application.dataPath + "/XLuaExamples/"; } }

        private static LuaEnv m_LuaEnv;
        public LuaEnv luaEnv
        {
            get
            {
                if (m_LuaEnv == null)
                {
                    m_LuaEnv = new LuaEnv();
                    m_LuaEnv.AddLoader(LoadFromFile); // AddLoader(CustomLoader)文档上有说明，自己读取lua源码以byte[]形式返回即可。
                    print("LuaEnv Initialized");
                }
                return m_LuaEnv;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            m_Instance = this;
        }
        private void Update()
        {
            luaEnv.Tick();
        }

        private byte[] LoadFromFile(ref string fileName)    // 这里的fileName就是lua中require的参数
        {
            string filePath = luaDirPath + fileName.Replace('.', '/') + ".lua";             // lua文件的实际路径
            fileName = fileName.Replace('.', '/');     // 返给lua调试器的路径，不用调试lua的就不用管这个了
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

        #region 这里的两个方法是处理协程的，不看协程的可以忽略
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
        #endregion
    }
}
