/*
 * Author:      #AUTHORNAME#
 * CreateTime:  #CREATETIME#
 * Description:
 * 
*/
using UnityEngine;
using XLua;

namespace EZUnity.XLuaExample
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

            table.ForEach<string, object>((key, value) => print(string.Format("{0}={1}, type:{2}", key, value, value.GetType())));
        }
    }
}