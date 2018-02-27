--[==[
- Author:       熊哲
- CreateTime:   11/14/2017 1:46:08 PM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Mathf = CS.UnityEngine.Mathf
local Time = CS.UnityEngine.Time
local Vector3 = CS.UnityEngine.Vector3
local ActivityMessage = CS.EZFramework.XLuaExtension.ActivityMessage
local UpdateMessage = CS.EZFramework.XLuaExtension.UpdateMessage
local bind = require("xlua.util").bind

local M = require("ezlua.module"):module()
----- CODE -----
function M.LCBinder(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.transform = self.gameObject.transform
    ActivityMessage.Require(self.gameObject).start:AddAction(bind(self.Start, self))
    UpdateMessage.Require(self.gameObject).update:AddAction(bind(self.Update, self))
    return self
end

function M:Start()
    self.v3_StartPosition = self.transform.position
end

function M:Update()
    local newPosition = Mathf.Repeat(Time.time * self.n_ScrollSpeed, self.n_TileSizeZ)
    self.transform.position = self.v3_StartPosition + Vector3.forward * newPosition
end
----- CODE -----
return M
