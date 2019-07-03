--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2018-03-08 10:49:04
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = {}
----- CODE -----
function M.LuaAwake(injector)
    injector:Inject(M)
    -- 与其他LuaBehaviour进行交互
    M.go_Cube:GetComponent("EZLuaBehaviour").luaTable.ChangePosition(M.v3)
    -- Behaviour可以用injector注入，节省GetComponent的调用开销
    M.lb_Cube.luaTable.ChangePosition(M.v3)
    return M
end
----- CODE -----
return M
