--[==[
Author:     熊哲
CreateTime: 11/15/2017 4:07:58 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
M.gameObject = nil
M.transform = nil
M.rigidbody = nil
M.n_Speed = 0

function M:New(go, n_Speed)
    self = new(self)
    self.gameObject = go
    self.transform = go.transform
    self.rigidbody = go:GetComponent("Rigidbody")
    self.n_Speed = n_Speed
    self:Start()
    return self
end

function M:Start()
    self.rigidbody.velocity = self.transform.forward * self.n_Speed
end
----- end -----
return M
