/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-11 13:14:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity.XLuaExample
{
    public class FromLua : MonoBehaviour
    {
        LuaEnv luaEnv = new LuaEnv();

        [CSharpCallLua]
        public delegate int MultiValueFromLua(out float floatValue, ref string stringValue);
        [CSharpCallLua]
        public delegate LuaTable TableFromLua();

        void Start()
        {
            luaEnv.DoString(@"
                function ReturnMultiValue()
                    return 1, 1.5, 'MultiValueFromLua'
                end
                function ReturnTable()
                    return { intValue = 2, floatValue = 2.5, stringValue = 'TableFromLua' }
                end
                function ArrayParamTest()
                    local ArrayTest = CS.EZhex1991.EZUnity.XLuaExample.FromLua.ArrayTest
                    local Color = CS.UnityEngine.Color
                    ArrayTest({ Color.red, Color.green, Color.blue }, 0)
                    ArrayTest({ Color.red, Color.green, Color.blue }, 1)
                end
            ");
            int intValue = 0;
            float floatValue = 0;
            string stringValue = "";

            MultiValueFromLua func1 = luaEnv.Global.Get<MultiValueFromLua>("ReturnMultiValue");
            intValue = func1(out floatValue, ref stringValue);
            print(string.Format("intValue={0}, floatValue={1}, stringValue={2}", intValue, floatValue, stringValue));

            TableFromLua func2 = luaEnv.Global.Get<TableFromLua>("ReturnTable");
            LuaTable table = func2();
            intValue = table.Get<int>("intValue");
            floatValue = table.Get<float>("floatValue");
            stringValue = table.Get<string>("stringValue");
            print(string.Format("intValue={0}, floatValue={1}, stringValue={2}", intValue, floatValue, stringValue));

            LuaFunction func3 = luaEnv.Global.Get<LuaFunction>("ArrayParamTest");
            func3.Call();

            table.ForEach<string, object>((key, value) => print(string.Format("{0}={1}, type:{2}", key, value, value.GetType())));
        }

        public static void ArrayTest(Color[] colors, int index)
        {
            Debug.Log(colors[index]);
        }
    }
}