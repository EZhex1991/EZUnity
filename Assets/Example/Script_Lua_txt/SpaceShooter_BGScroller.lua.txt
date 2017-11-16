--[==[
Author:     熊哲
CreateTime: 11/14/2017 1:46:08 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
local Mathf = CS.UnityEngine.Mathf
local Time = CS.UnityEngine.Time
local Vector3 = CS.UnityEngine.Vector3
local UpdateMessage = CS.EZFramework.XLuaExtension.UpdateMessage
local bind = require("xlua.util").bind

M.gameObject = nil
M.transform = nil
M.v3_StartPosition = nil

M.n_ScrollSpeed = -0.25
M.n_TileSizeZ = 30

function M:New(go)
    self = new(self)
    self.gameObject = go
    self.transform = go.transform
    self.v3_StartPosition = go.transform.position
    UpdateMessage.Require(go).update:AddAction(bind(self.Update, self))
    return self
end

function M:Update()
    local offset = Mathf.Repeat(Time.time * self.n_ScrollSpeed, self.n_TileSizeZ)
    self.transform.position = self.v3_StartPosition + Vector3.forward * offset
end
----- end -----
return M
