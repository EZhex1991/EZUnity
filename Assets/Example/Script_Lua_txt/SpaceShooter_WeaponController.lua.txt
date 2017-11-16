--[==[
Author:     熊哲
CreateTime: 11/16/2017 1:25:17 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
local Object = CS.UnityEngine.Object
local Time = CS.UnityEngine.Time
local bind = require("xlua.util").bind

M.gameObject = nil
M.n_FireRate = 0
M.n_Delay = 0

local nextShot = 0

function M:New(go, n_FireRate, n_Delay)
    self = new(self)
    self.gameObject = go
    go:GetComponent("LuaInjector"):Inject(self)
    CS.EZFramework.XLuaExtension.UpdateMessage.Require(go).update:AddAction(bind(self.Update, self))
    self.n_FireRate = n_FireRate
    self.n_Delay = n_Delay
    nextShot = self.n_Delay
    return self
end

function M:Update()
    nextShot = nextShot - Time.deltaTime
    if nextShot <= 0 then
        self:Fire()
        nextShot = self.n_FireRate
    end
end

function M:Fire()
    local shot = Object.Instantiate(self.go_Shot, self.tf_ShotSpawn.position, self.tf_ShotSpawn.rotation)
    require("SpaceShooter.Mover"):New(shot, 20)
    require("SpaceShooter.DestroyByContact"):New(shot, 0)
    self.audioSource:Play()
end
----- end -----
return M
