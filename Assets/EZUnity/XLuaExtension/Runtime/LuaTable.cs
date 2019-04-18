/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-01-07 14:04:59
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
namespace XLua
{
    public partial class LuaTable
    {
        public LuaTable NewTable<T>(T key)
        {
            LuaTable table = luaEnv.NewTable();
            Set(key, table);
            return table;
        }
    }
}
#endif
