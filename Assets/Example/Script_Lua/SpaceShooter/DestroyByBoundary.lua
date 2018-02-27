--[==[
- Author:       熊哲
- CreateTime:   11/14/2017 2:25:47 PM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Object = CS.UnityEngine.Object
local TriggerMessage = CS.EZFramework.XLuaExtension.TriggerMessage
local bind = require("xlua.util").bind

local M = require("ezlua.module"):module()
----- CODE -----
function M.LCBinder(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    TriggerMessage.Require(self.gameObject).onTriggerExit:AddAction(bind(self.OnTriggerExit, self))
    return self
end

function M:OnTriggerExit(collider)
    Object.Destroy(collider.gameObject)
end
----- CODE -----
return M
