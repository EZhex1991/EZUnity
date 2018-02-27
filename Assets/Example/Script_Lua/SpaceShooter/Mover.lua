--[==[
- Author:       熊哲
- CreateTime:   11/15/2017 4:07:58 PM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = require("ezlua.module"):module()
----- CODE -----
function M.LCBinder(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.transform = self.gameObject.transform
    self.rigidbody = self.gameObject:GetComponent("Rigidbody")
    self:Start()
    return self
end

function M:Start()
    self.rigidbody.velocity = self.transform.forward * self.n_Speed
end
----- CODE -----
return M
