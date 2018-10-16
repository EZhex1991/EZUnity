--[==[
- Author:       熊哲
- CreateTime:   11/14/2017 2:25:47 PM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Object = CS.UnityEngine.Object
local TriggerMessage = CS.EZUnity.XLuaExtension.TriggerMessage
local bind = require("xlua.util").bind

local M = require("ezlua.module"):module()
----- CODE -----
function M.LuaAwake(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    TriggerMessage.Require(self.gameObject).onTriggerExit:AddAction(bind(self.LuaOnTriggerExit, self))
    return self
end

function M:LuaOnTriggerExit(collider)
    Object.Destroy(collider.gameObject)
end
----- CODE -----
return M
