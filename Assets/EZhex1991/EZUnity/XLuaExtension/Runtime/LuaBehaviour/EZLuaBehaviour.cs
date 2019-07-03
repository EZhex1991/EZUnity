/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-02-24 14:47:57
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
using XLua;

namespace EZhex1991.EZUnity.XLuaExtension
{
    public class EZLuaBehaviour : EZLuaInjector
    {
        public EZLua ezLua { get { return EZLua.Instance; } }

        public string moduleName;

        private LuaTable m_LuaTable;
        public LuaTable luaTable
        {
            get
            {
                // Awake时会自动Bind，但如果调用发生在Awake之前，那么就提前进行Bind
                if (m_LuaTable == null) m_LuaTable = GetLuaTable();
                return m_LuaTable;
            }
        }

        public delegate LuaTable LuaAwake(EZLuaInjector injector);
        public LuaAction<LuaTable> luaStart;
        public LuaAction<LuaTable> luaOnEnable;
        public LuaAction<LuaTable> luaOnDisable;
        public LuaAction<LuaTable> luaOnDestroy;

        private LuaTable GetLuaTable()
        {
            LuaTable luaModule = ezLua.luaRequire(moduleName);
            LuaAwake awake = luaModule.Get<LuaAwake>("LuaAwake");
            LuaTable table = awake == null ? luaModule : awake.Invoke(this);
            luaStart = table.Get<LuaAction<LuaTable>>("LuaStart");
            luaOnEnable = table.Get<LuaAction<LuaTable>>("LuaOnEnable");
            luaOnDisable = table.Get<LuaAction<LuaTable>>("LuaOnDisable");
            luaOnDestroy = table.Get<LuaAction<LuaTable>>("LuaOnDestroy");
            return table;
        }

        private void Awake()
        {
            if (m_LuaTable == null) m_LuaTable = GetLuaTable();
        }
        private void Start()
        {
            if (luaStart != null) luaStart(luaTable);
        }
        private void OnEnable()
        {
            if (luaOnEnable != null) luaOnEnable(luaTable);
        }
        private void OnDisable()
        {
            if (luaOnDisable != null) luaOnDisable(luaTable);
        }
        private void OnDestroy()
        {
            if (luaOnDestroy != null) luaOnDestroy(luaTable);
            if (m_LuaTable != null) m_LuaTable.Dispose();
        }
    }
}
#endif
