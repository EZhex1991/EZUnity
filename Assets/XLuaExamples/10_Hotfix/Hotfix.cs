/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-11 13:14:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace EZhex1991.EZUnity.XLuaExample
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
                xlua.hotfix(CS.EZhex1991.EZUnity.XLuaExample.Hotfix, 'Update', function(self)   -- private方法，直接fix
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
                xlua.private_accessible(CS.EZhex1991.EZUnity.XLuaExample.Hotfix)   -- 获取private字段的访问权限
                xlua.hotfix(CS.EZhex1991.EZUnity.XLuaExample.Hotfix, {  -- 直接Fix整个Class的写法
                    Update = function(self)
                        self.console_CSharp.text = 'Time: ' .. CS.UnityEngine.Time.time -- 访问private字段
                        self.console_Lua.text = 'Time: ' .. CS.UnityEngine.Time.time
                    end,
                    FixClear = function(self)
                        self:StartCoroutine(self:Cor1_FixClear())
                        self:StartCoroutine(NewCor2_FixClear(self)) -- 注意！这里并非是self:Cor2_FixClear！而是调用了在lua端新增的方法
                        print('Fix Clear')
                    end,
                    Cor1_FixClear = function(self)
                        return require('xlua.util').cs_generator(function()
                            coroutine.yield(CS.UnityEngine.WaitForEndOfFrame())
                            self.btn_Hotfix.gameObject:SetActive(false)
                            print('button disabled')
                        end)
                    end
                })

                NewCor2_FixClear = function(self) -- 原Cor2_FixClear返回值错误，这里实际上是lua端的新增函数而不是修复函数
                    return require('xlua.util').cs_generator(function()
                        coroutine.yield(CS.UnityEngine.WaitForSeconds(1))
                        self.luaEnv:DoString('xlua.hotfix(CS.EZhex1991.EZUnity.XLuaExample.Hotfix, { Update = false, FixClear = false })') -- 清空Hotfix
                        print('Cleared')
                    end)
                end
            ");
        }
        public void FixClear()  // 要fix首先得保证有该方法
        {
            Cor1_FixClear();
            Cor2_FixClear();
        }
        public IEnumerator Cor1_FixClear()
        {
            yield return null;
        }
        public void Cor2_FixClear()
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