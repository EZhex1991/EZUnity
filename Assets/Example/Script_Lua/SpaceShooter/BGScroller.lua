--[==[
- Author:       熊哲
- CreateTime:   11/14/2017 1:46:08 PM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Mathf = CS.UnityEngine.Mathf
local Time = CS.UnityEngine.Time
local Vector3 = CS.UnityEngine.Vector3
local ActivityMessage = CS.EZUnity.XLuaExtension.ActivityMessage
local UpdateMessage = CS.EZUnity.XLuaExtension.UpdateMessage
local bind = require("xlua.util").bind

local M = require("ezlua.module"):module()
----- CODE -----
function M.LuaAwake(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.transform = self.gameObject.transform
    UpdateMessage.Require(self.gameObject).update:AddAction(bind(self.LuaUpdate, self))
    return self
end

function M:LuaStart()
    self.v3_StartPosition = self.transform.position
end

function M:LuaUpdate()
    local newPosition = Mathf.Repeat(Time.time * self.n_ScrollSpeed, self.n_TileSizeZ)
    self.transform.position = self.v3_StartPosition + Vector3.forward * newPosition
end
----- CODE -----
return M
