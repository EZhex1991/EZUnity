--[==[
Author:     熊哲
CreateTime: 11/16/2017 12:29:50 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
M.gameObject = nil
M.n_Speed = -5
M.n_Tumble = 5
M.n_ScoreValue = 10

function M:New(go)
    self = new(self)
    self.gameObject = go
    require("SpaceShooter.Mover"):New(go, self.n_Speed)
    require("SpaceShooter.RandomRotator"):New(go, self.n_Tumble)
    require("SpaceShooter.DestroyByContact"):New(go, self.n_ScoreValue)
    return self
end
----- end -----
return M
