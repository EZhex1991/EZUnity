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

function TestLuaFunction()
    print("Lua Function")
end

print("----------Action Test----------")
DisposeTest.testAction = TestLuaFunction
DisposeTest.testAction = DisposeTest.testAction + DisposeTest.TestFunction
DisposeTest.testAction()
DisposeTest.testAction = DisposeTest.testAction - DisposeTest.TestFunction
DisposeTest.testAction()

print("----------Event Test----------")
DisposeTest.testEvent("+", TestLuaFunction)
DisposeTest.testEvent("+", DisposeTest.TestFunction)
DisposeTest["&testEvent"]()
DisposeTest.testEvent("-", DisposeTest.TestFunction)
DisposeTest["&testEvent"]()

print("----------Unregister Test----------")
require("xlua.util").print_func_ref_by_csharp()
-- 反注册回调函数
function Unregister()
    DisposeTest.testAction = DisposeTest.testAction - TestLuaFunction
    -- 当然，你也可以这样：
    -- DisposeTest.testAction = nil

    -- 而event，在lua里你只能这样：
    DisposeTest.testEvent("-", TestLuaFunction)

    -- 当然，你也可以直接在C#里清空掉这些回调
end

----- CODE -----
return M
