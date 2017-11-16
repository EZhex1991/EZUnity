--[==[
Author:     熊哲
CreateTime: 11/16/2017 1:10:02 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
M.gameObject = nil
M.n_Speed = -5
M.n_ScoreValue = 20
M.n_FireRate = 1.5
M.n_Delay = 0.5

function M:New(go)
    self = new(self)
    self.gameObject = go
    require("SpaceShooter.Mover"):New(go, self.n_Speed)
    require("SpaceShooter.DestroyByContact"):New(go, self.n_ScoreValue)
    require("SpaceShooter.WeaponController"):New(go, self.n_FireRate, self.n_Delay)
    require("SpaceShooter.EvasiveManeuver"):New(go)
    return self
end
----- end -----
return M
