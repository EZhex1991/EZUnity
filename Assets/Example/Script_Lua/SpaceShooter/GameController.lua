--[==[
Author:     熊哲
CreateTime: 11/14/2017 1:53:24 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
local Vector3 = CS.UnityEngine.Vector3
local LuaUtility = CS.EZFramework.XLuaExtension.LuaUtility
local bind = require("xlua.util").bind

M.gameObject = nil

M.v3_SpawnPosition = Vector3(6, 0, 16)
M.n_HazardCount = 10
M.n_SpawnWait = 0.75
M.n_StartWait = 1
M.n_WaveWait = 4

M.b_GameOver = false
M.b_Restart = false
M.n_Score = 0

function M:Init()
    CS.EZFramework.EZResource.Instance:LoadSceneAsync(
        "spaceshooter",
        "SpaceShooter",
        CS.UnityEngine.SceneManagement.LoadSceneMode.Single,
        bind(self.InitScene, self)
    )
end
function M:InitScene()
    self.gameObject = CS.UnityEngine.GameObject.Find("GameController_SpaceShooter")
    self.Hazards = {}
    self.gameObject:GetComponent("LuaInjector"):Inject(self)
    CS.EZFramework.XLuaExtension.UpdateMessage.Require(self.gameObject).update:AddAction(bind(self.Update, self))
    require("SpaceShooter.BGScroller"):New(self.go_Background)
    require("SpaceShooter.DestroyByBoundary"):New(self.go_Boundary)
    require("SpaceShooter.PlayerController"):New(self.go_Player)
    self:InitGame()
end
function M:InitGame()
    self.b_GameOver = false
    self.b_Restart = false
    self.text_Restart.text = ""
    self.text_GameOver.text = ""
    self.n_Score = 0
    self:UpdateScore()
    coroutine.resume(self:Cor_SpawnWaves())
end

function M:Update()
    if self.b_Restart then
        if CS.UnityEngine.Input.GetKeyDown(CS.UnityEngine.KeyCode.R) then
            self:Init()
        end
    end
end

function M:Cor_SpawnWaves()
    return coroutine.create(
        function()
            WaitForSeconds(self.n_StartWait)
            while (true) do
                for i = 1, self.n_HazardCount do
                    local index = LuaUtility.RandomInt(0, #self.Hazards) + 1 -- CS.UnityEngine.Random会得到小数
                    local x = LuaUtility.RandomFloat(-self.v3_SpawnPosition.x, self.v3_SpawnPosition.x)
                    local position = Vector3(x, self.v3_SpawnPosition.y, self.v3_SpawnPosition.z)
                    local rotation = CS.UnityEngine.Quaternion.identity
                    local hazard = CS.UnityEngine.Object.Instantiate(self.Hazards[index], position, rotation)
                    if self.Hazards[index].name == "Enemy Ship" then
                        require("SpaceShooter._EnemyShip"):New(hazard)
                    else
                        require("SpaceShooter._Asteroid"):New(hazard)
                    end
                    WaitForSeconds(self.n_SpawnWait)
                end
                WaitForSeconds(self.n_WaveWait)
                if self.b_GameOver then
                    self.text_Restart.text = "Press 'R' for Restart"
                    self.b_Restart = true
                    break
                end
            end
        end
    )
end

function M:AddScore(n_NewScoreValue)
    self.n_Score = self.n_Score + n_NewScoreValue
    self:UpdateScore()
end

function M:UpdateScore()
    self.text_Score.text = "Score: " .. self.n_Score
end

function M:GameOver()
    self.text_GameOver.text = "Game Over!"
    self.b_GameOver = true
end
----- end -----
return M
