--[==[
- Author:       熊哲
- CreateTime:   11/14/2017 1:53:24 PM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Vector3 = CS.UnityEngine.Vector3
local LuaUtility = CS.EZFramework.XLuaExtension.LuaUtility
local ActivityMessage = CS.EZFramework.XLuaExtension.ActivityMessage
local UpdateMessage = CS.EZFramework.XLuaExtension.UpdateMessage
local ezutil = require("ezlua.util")

local M = {}
----- CODE -----
M.Hazards = {}
M.b_GameOver = false
M.b_Restart = false
M.n_Score = 0

-- 重新开始时的场景加载，可以忽略
function M:Init()
    CS.EZFramework.EZResource.Instance:LoadSceneAsync(
        "spaceshooter",
        "SpaceShooter",
        CS.UnityEngine.SceneManagement.LoadSceneMode.Single
    )
end
-- LuaBehaviour的Lua-CSharp逻辑绑定，Awake逻辑也写在这里
function M.LCBinder(injector)
    local self = M
    injector:Inject(self)
    self.gameObject = injector.gameObject
    -- 一般情况下可以直接调用self:Start()，但对于active=false的gameObject来说这和事件绑定是有区别的
    ActivityMessage.Require(self.gameObject).start:AddAction(ezutil.bind(self.Start, self))
    UpdateMessage.Require(self.gameObject).update:AddAction(ezutil.bind(self.Update, self))
    return self
end

function M:Start()
    self.b_GameOver = false
    self.b_Restart = false
    self.text_Restart.text = ""
    self.text_GameOver.text = ""
    self.n_Score = 0
    self:UpdateScore()
    ezutil.startcoroutine(self:Cor_SpawnWaves())
end

function M:Update()
    if self.b_Restart then
        if CS.UnityEngine.Input.GetKeyDown(CS.UnityEngine.KeyCode.R) then
            self:Init()
        end
    end
end

function M:Cor_SpawnWaves()
    return function()
        ezutil.waitforseconds(self.n_StartWait)
        while (true) do
            for i = 1, self.n_HazardCount do
                local index = LuaUtility.RandomInt(0, #self.Hazards) + 1 -- CS.UnityEngine.Random会得到小数
                local x = LuaUtility.RandomFloat(-self.v3_SpawnValues.x, self.v3_SpawnValues.x)
                local position = Vector3(x, self.v3_SpawnValues.y, self.v3_SpawnValues.z)
                local rotation = CS.UnityEngine.Quaternion.identity
                local hazard = CS.UnityEngine.Object.Instantiate(self.Hazards[index], position, rotation)
                ezutil.waitforseconds(self.n_SpawnWait)
            end
            ezutil.waitforseconds(self.n_WaveWait)
            if self.b_GameOver then
                self.text_Restart.text = "Press 'R' for Restart"
                self.b_Restart = true
                break
            end
        end
    end
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
----- CODE -----
return M
