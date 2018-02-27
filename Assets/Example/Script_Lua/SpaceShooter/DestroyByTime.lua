--[==[
- Author:       熊哲
- CreateTime:   11/15/2017 7:06:50 PM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = require("ezlua.module"):module()
----- CODE -----
function M.LCBinder(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    CS.UnityEngine.Object.Destroy(self.gameObject, self.n_Lifetime)
    return self
end
----- CODE -----
return M
