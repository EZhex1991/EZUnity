--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2018-03-08 10:48:57
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = {}
----- CODE -----
-- LuaAwake(injector)用于lua和C#的绑定
-- EZLuaBehaviour（继承EZLuaInjector, EZPropertyList）作为参数injector传入，使用injector:Inject(table)将变量直接注入table中
-- 返回值会保存在EZLuaBehaviour.luaTable中用于Behaviour的相互调用
function M.LuaAwake(injector)
    injector:Inject(M)
    print(injector.name) -- injector.gameObject就是当前绑定的GameObject
    print(M.go_Sphere.name)
    return M
end

function M.ChangePosition(position)
    print("ChangePosition", position)
    M.go_Sphere.transform.position = position
end
----- CODE -----
return M
