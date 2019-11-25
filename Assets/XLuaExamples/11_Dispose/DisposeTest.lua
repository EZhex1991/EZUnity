--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2019-11-25 14:04:22
- Organization: #ORGANIZATION#
- Description:  
--]==]
local M = {}
----- CODE -----

function TestLuaFunction1()
    print('Lua Function1')
end

function TestLuaFunction2()
    print('Lua Function2')
end

local DisposeTest = CS.EZhex1991.EZUnity.XLuaExample.DisposeTest
DisposeTest.testAction = TestLuaFunction1
DisposeTest.testEvent('+', TestLuaFunction2)

require('xlua.util').print_func_ref_by_csharp()

-- 反注册回调函数
function Unregister()
    DisposeTest.testAction = DisposeTest.testAction - TestLuaFunction1
    -- 当然，你也可以这样：
    -- DisposeTest.testAction = nil

    -- 而event，在lua里你只能这样：
    DisposeTest.testEvent('-', TestLuaFunction2)

    -- 当然，你也可以直接在C#里清空掉这些回调
end

----- CODE -----
return M
