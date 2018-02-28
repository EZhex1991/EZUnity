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

        public delegate LuaTable LCBinder(LuaInjector injector);

        protected override void Awake()
        {
            base.Awake();
            LCBinder binder = luaModule.Get<LCBinder>("LCBinder");
            if (binder == null)
                this.luaTable = luaModule;
            else
                this.luaTable = binder.Invoke(this);
        }
    }
}