--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2017-11-15 16:07:58
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = require("ezlua.module"):module()
----- CODE -----
function M.LuaAwake(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.transform = self.gameObject.transform
    self.rigidbody = self.gameObject:GetComponent("Rigidbody")
    return self
end

function M:LuaStart()
    self.rigidbody.velocity = self.transform.forward * self.n_Speed
end
----- CODE -----
return M
