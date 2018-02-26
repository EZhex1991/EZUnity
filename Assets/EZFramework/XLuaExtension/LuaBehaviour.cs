/* Author:          熊哲
 * CreateTime:      2018-02-24 14:47:57
 * Orgnization:     #ORGNIZATION#
 * Description:     
 */
using UnityEngine;
using XLua;

namespace EZFramework.XLuaExtension
{
    [RequireComponent(typeof(LuaInjector))]
    public class LuaBehaviour : MonoBehaviour
    {
        public string moduleName;

        private LuaTable m_LuaModule;
        private LuaTable luaModule
        {
            get
            {
                if (m_LuaModule == null)
                {
                    m_LuaModule = EZLua.Instance.luaRequire(moduleName);
                }
                return m_LuaModule;
            }
        }

        private LuaTable m_LuaTable;
        public LuaTable luaTable { get { return m_LuaTable; } private set { m_LuaTable = value; } }

        private LuaInjector m_LuaInjector;
        public LuaInjector luaInjector
        {
            get
            {
                if (m_LuaInjector == null)
                {
                    m_LuaInjector = GetComponent<LuaInjector>();
                }
                return m_LuaInjector;
            }
        }

        public delegate LuaTable Binder(LuaInjector injector);

        void Awake()
        {
            luaTable = luaModule.Get<Binder>("LuaBinder").Invoke(luaInjector);
        }
    }
}