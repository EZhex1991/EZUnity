--[==[
Author:     熊哲
CreateTime: 11/14/2017 2:25:47 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
local Object = CS.UnityEngine.Object
local TriggerMessage = CS.EZFramework.XLuaExtension.TriggerMessage
local bind = require("xlua.util").bind

M.gameObject = nil

function M:New(go)
    self = new(self)
    self.gameObject = go
    TriggerMessage.Require(go).onTriggerExit:AddAction(bind(self.OnTriggerExit, self))
    return self
end

function M:OnTriggerExit(collider)
    Object.Destroy(collider.gameObject)
end
----- end -----
return M
