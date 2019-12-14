--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   #CREATETIME#
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local moduleName = ...
local M = {}
M.__index = M
----- begin module -----
local gameObject = CS.UnityEngine.GameObject.Find("DelegateTest")
local testObject = gameObject:GetComponent("DelegateTest")

-- 把function()添加到System.Action，使用'+'前注意先判断是否为nil
function Start()
    print("Start in lua")
end
if testObject.testAction == nil then
    testObject.testAction = Start
else
    testObject.testAction = testObject.testAction + Start
end

-- 关于闭包，使用Action<LuaTable>不是一个高效的方法（而且还得外加一个参数把table传过去）
-- 建议的方式：xlua.util.bind可以将table绑定到一个function实现闭包，
function M:BindTest(gameObjectName)
    -- 注意该方法用冒号声明，等同于BindTest(self, gameObjectName)
    print("BindTest", "function=" .. tostring(self.BindTest), "gameObjectName=" .. gameObjectName)
end
testObject:testEvent('+', require("xlua.util").bind(M.BindTest, M))

-- UI组件
function OnButtonClick()
    print("OnButtonClick")
end
function OnToggleValueChanged(value)
    print("OnToggleValueChanged, value:", value)
end
testObject.button.onClick:AddListener(OnButtonClick)
testObject.toggle.onValueChanged:AddListener(OnToggleValueChanged)

-- xlua.util.createdelegate的使用，使用一个C#方法在lua上创建一个C#Delegate
-- 五个参数分别是：delegate的类型，C#方法所作用的实例，实例的类型，方法的名称，参数的类型列表
-- 如果是静态方法，那么“C#方法所作用的实例”使用nil，如果方法无参，那么“参数的类型列表”空着即可
local nonStaticDelegate = require("xlua.util").createdelegate(
    CS.UnityEngine.Events.UnityAction,
    testObject,
    CS.EZhex1991.EZUnity.XLuaExample.DelegateTest,
    "NonStaticFunction",
    {}
)
local staticDelegate = require("xlua.util").createdelegate(
    CS.UnityEngine.Events["UnityAction`1[System.Boolean]"],
    nil,
    CS.EZhex1991.EZUnity.XLuaExample.DelegateTest,
    "StaticFunction",
    {typeof(CS.System.Boolean)}
)
testObject.button.onClick:AddListener(nonStaticDelegate)
testObject.toggle.onValueChanged:AddListener(staticDelegate)
----- end -----
return M
