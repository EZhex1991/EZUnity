--[==[
- Author:       熊哲
- CreateTime:   11/15/2017 4:14:15 PM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = require("ezlua.module"):module()
----- CODE -----
function M.LuaAwake(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.rigidbody = self.gameObject:GetComponent("Rigidbody")
    return self
end

function M:LuaStart()
    self.rigidbody.angularVelocity = CS.UnityEngine.Random.insideUnitSphere * self.n_Tumble
end
----- CODE -----
return M
