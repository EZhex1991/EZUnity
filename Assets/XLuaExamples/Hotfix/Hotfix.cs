/*
 * Author:      #AUTHORNAME#
 * CreateTime:  #CREATETIME#
 * Description:
 * 
*/
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace EZhex1991.XLuaExample
{
    [Hotfix]
    public class Hotfix : MonoBehaviour
    {
        LuaEnv luaEnv = new LuaEnv();

        public Button btn_Hotfix;
        [SerializeField]
        private Text console_CSharp;    // private字段
        public Text console_Lua;        // public字段

        public void Fix()
        {
            Debug.Log("Fix");
            btn_Hotfix.onClick.RemoveAllListeners();
            btn_Hotfix.onClick.AddListener(FixClass);
            luaEnv.DoString(@"
                xlua.hotfix(CS.EZhex1991.XLuaExample.Hotfix, 'Update', function(self)   -- private方法，直接fix
                    self.console_Lua.text = 'Time: ' .. CS.UnityEngine.Time.time    -- public字段，直接访问
                end)
            ");
        }
        public void FixClass()
        {
            Debug.Log("Fix Class");
            btn_Hotfix.onClick.RemoveAllListeners();
            btn_Hotfix.onClick.AddListener(FixClear);
            luaEnv.DoString(@"
                xlua.private_accessible(CS.EZhex1991.XLuaExample.Hotfix);   -- 获取private字段的访问权限
                xlua.hotfix(CS.EZhex1991.XLuaExample.Hotfix, {  -- 直接Fix整个Class的写法
                    Update = function(self)
                        self.console_CSharp.text = 'Time: ' .. CS.UnityEngine.Time.time -- 访问private字段
                        self.console_Lua.text = 'Time: ' .. CS.UnityEngine.Time.time
                    end,
                    FixClear = function(self)
                        print('Fix Clear')
                        self.btn_Hotfix.gameObject:SetActive(false)
                        self.luaEnv:DoString('xlua.hotfix(CS.EZhex1991.XLuaExample.Hotfix, { Update = false, FixClear = false })') -- 清空Hotfix
                    end,
                })
            ");
        }
        public void FixClear()  // 要fix首先得保证有该方法
        {

        }

        void Start()
        {
#if HOTFIX_ENABLE
            btn_Hotfix.gameObject.SetActive(true);
            btn_Hotfix.onClick.AddListener(Fix);
#else
            Debug.Log("Read document first!");
#endif
        }
        void Update()
        {

        }
    }
}