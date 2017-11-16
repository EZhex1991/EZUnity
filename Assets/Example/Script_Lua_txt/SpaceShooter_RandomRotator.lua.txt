--[==[
Author:     熊哲
CreateTime: 11/15/2017 4:14:15 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
M.gameObject = nil
M.rigidbody = nil
M.n_Tumble = 0

function M:New(go, n_Tumble)
    self = new(self)
    self.gameObject = go
    self.rigidbody = go:GetComponent("Rigidbody")
    self.n_Tumble = n_Tumble
    self:Start()
    return self
end

function M:Start()
    self.rigidbody.angularVelocity = CS.UnityEngine.Random.insideUnitSphere * self.n_Tumble
end
----- end -----
return M
