--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2019-11-25 14:04:22
- Organization: #ORGANIZATION#
- Description:  
--]==]
local M = {}
----- CODE -----
local DisposeTest = CS.EZhex1991.EZUnity.XLuaExample.DisposeTest
xlua.private_accessible(DisposeTest)

function LuaFunction()
    print("Lua Function")
end

print("----------Action----------")
DisposeTest.testAction = LuaFunction
DisposeTest.testAction = DisposeTest.testAction + DisposeTest.CSharpFunction
DisposeTest.testAction()

print("----------CSharpFunction Unregistered----------")
DisposeTest.testAction = DisposeTest.testAction - DisposeTest.CSharpFunction
DisposeTest.testAction()


print("----------Event Test----------")
DisposeTest.testEvent("+", LuaFunction)
DisposeTest.testEvent("+", DisposeTest.CSharpFunction)
DisposeTest["&testEvent"]()

print("----------CSharpFunction Unregistered----------")
DisposeTest.testEvent("-", DisposeTest.CSharpFunction) -- Lua端注册的事件，直接注销
local TestDelegate = require("xlua.util").createdelegate(
    CS.System.Action,
    nil,
    DisposeTest,
    "CSharpFunction",
    {}
)
DisposeTest.testEvent("-", TestDelegate) -- 如果是在C#端注册的事件，那必须先createdelegate然后注销
DisposeTest["&testEvent"]()

function UnregisterLuaFunction()
    print("----------Unregister LuaFunction----------")
    require("xlua.util").print_func_ref_by_csharp()
    DisposeTest.testAction = DisposeTest.testAction - LuaFunction
    -- 当然，你也可以这样：
    -- DisposeTest.testAction = nil

    -- 而event，在lua里你只能这样：
    DisposeTest.testEvent("-", LuaFunction)

    -- 当然，你也可以直接在C#里清空掉这些回调
end

----- CODE -----
return M
