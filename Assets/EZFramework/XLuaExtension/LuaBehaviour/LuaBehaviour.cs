/* Author:          熊哲
 * CreateTime:      2018-02-24 14:47:57
 * Orgnization:     #ORGNIZATION#
 * Description:     
 */
using XLua;

namespace EZFramework.XLuaExtension
{
    public class LuaBehaviour : LuaInjector
    {
        public string moduleName;

        private LuaTable m_LuaTable;
        public LuaTable luaTable
        {
            get
            {
                // Awake时会自动Bind，但如果调用发生在Awake之前，那么就提前进行Bind
                if (m_LuaTable == null) Bind();
                return m_LuaTable;
            }
        }

        public delegate LuaTable LCBinder(LuaInjector injector);
        private void Bind()
        {
            LuaTable luaModule = EZLua.Instance.luaRequire(moduleName);
            LCBinder binder = luaModule.Get<LCBinder>("LCBinder");
            m_LuaTable = binder == null ? luaModule : binder.Invoke(this);
        }

        protected virtual void Awake()
        {
            if (m_LuaTable == null) Bind();
        }
        protected virtual void OnDestory()
        {
            if (m_LuaTable != null) m_LuaTable.Dispose();
        }
    }
}