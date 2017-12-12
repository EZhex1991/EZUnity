--[==[
Author:     熊哲
CreateTime: 11/15/2017 7:11:43 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
local Object = CS.UnityEngine.Object
local bind = require("xlua.util").bind
local GameController = require("SpaceShooter.GameController")

M.gameObject = nil
M.transform = nil
M.go_Explosion = nil
M.go_PlayerExplosion = nil
M.n_ScoreValue = 0

function M:New(go, n_ScoreValue)
    self = new(self)
    self.gameObject = go
    go:GetComponent("LuaInjector"):Inject(self)
    self.transform = go.transform
    self.n_ScoreValue = n_ScoreValue
    CS.EZFramework.XLuaExtension.TriggerMessage.Require(go).onTriggerEnter:AddAction(bind(self.OnTriggerEnter, self))
    return self
end

function M:OnTriggerEnter(collider)
    if collider.tag == "Finish" or collider.tag == "Respawn" then
        return
    end
    if self.go_Explosion ~= nil then
        local go = Object.Instantiate(self.go_Explosion, self.transform.position, self.transform.rotation)
        require("SpaceShooter.DestroyByTime"):New(go, 2)
    end
    if collider.tag == "Player" then
        local go = Object.Instantiate(self.go_PlayerExplosion, collider.transform.position, collider.transform.rotation)
        require("SpaceShooter.DestroyByTime"):New(go, 2)
        GameController:GameOver()
    end
    GameController:AddScore(self.n_ScoreValue)
    Object.Destroy(collider.gameObject)
    Object.Destroy(self.gameObject)
end
----- end -----
return M
