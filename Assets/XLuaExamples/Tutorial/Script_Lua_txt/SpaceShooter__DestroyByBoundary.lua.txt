--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2017-11-14 14:25:47
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Object = CS.UnityEngine.Object
local TriggerMessage = CS.EZhex1991.EZUnity.XLuaExtension.TriggerMessage
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
