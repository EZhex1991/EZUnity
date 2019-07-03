--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2017-11-15 19:06:50
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = require("ezlua.module"):module()
----- CODE -----
function M.LuaAwake(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    CS.UnityEngine.Object.Destroy(self.gameObject, self.n_Lifetime)
    return self
end
----- CODE -----
return M
