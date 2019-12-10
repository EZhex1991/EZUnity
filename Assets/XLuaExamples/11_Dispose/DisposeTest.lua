--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2019-11-25 14:04:22
- Organization: #ORGANIZATION#
- Description:  
--]==]
local M = {}
----- CODE -----
local DisposeTest = CS.EZhex1991.EZUnity.XLuaExample.DisposeTest
local testObject = CS.UnityEngine.GameObject.Find("DisposeTest")
                        :GetComponent("EZhex1991.EZUnity.XLuaExample.DisposeTest")

local LuaFunction = function()
    print("Lua Function")
end

-- xlua.util.createdelegate的使用，使用一个C#方法在lua上创建一个C#Delegate
-- 五个参数分别是：delegate的类型，C#方法所作用的实例，实例的类型，方法的名称，参数的类型列表
-- 如果是静态方法，那么“C#方法所作用的实例”使用nil，如果方法无参，那么“参数的类型列表”空着即可
local CSharpDelegate = require("xlua.util").createdelegate(
    CS.System.Action,
    testObject,
    DisposeTest,
    "CSharpFunction",
    {}
)

-- 注册
function Register()
    print("----------Register----------");
    -- 先判断是否为nil才能用'+'
    -- if testObject.testAction == nil then
    --     testObject.testAction = LuaFunction
    --     testObject.testAction = testObject.testAction + CSharpDelegate
    -- else
    --     testObject.testAction = testObject.testAction + LuaFunction
    --     testObject.testAction = testObject.testAction + CSharpDelegate
    -- end

    -- testObject:testEvent("+", LuaFunction)
    -- testObject:testEvent("+", CSharpDelegate)
end

-- 注销
function Unregister()
    print("----------Unregister----------");
    -- if testObject.testAction ~= nil then 
    --     testObject.testAction = testObject.testAction - LuaFunction
    -- end
    -- if testObject.testAction ~= nil then 
    --     testObject.testAction = testObject.testAction - CSharpDelegate
    -- end

    -- testObject:testEvent("-", LuaFunction)
    -- testObject:testEvent("-", CSharpDelegate)

    testObject.button_Register.onClick:RemoveListener(Register)
    testObject.button_Unregister.onClick:RemoveListener(Unregister)
end

testObject.button_Register.onClick:AddListener(Register)
testObject.button_Unregister.onClick:AddListener(Unregister)
----- CODE -----
return M
