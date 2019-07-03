--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2017-11-14 13:53:24
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Vector3 = CS.UnityEngine.Vector3
local EZLuaUtility = CS.EZhex1991.EZUnity.XLuaExtension.EZLuaUtility
local ActivityMessage = CS.EZhex1991.EZUnity.XLuaExtension.ActivityMessage
local UpdateMessage = CS.EZhex1991.EZUnity.XLuaExtension.UpdateMessage
local ezutil = require("ezlua.util")

local M = {}
----- CODE -----
M.Hazards = {}
M.b_GameOver = false
M.b_Restart = false
M.n_Score = 0

-- 重新开始时的场景加载，可以忽略
function M:Init()
    CS.EZhex1991.EZUnity.Framework.EZResources.Instance:LoadSceneAsync(
        "spaceshooter",
        "SpaceShooter",
        CS.UnityEngine.SceneManagement.LoadSceneMode.Single
    )
end
-- EZLuaBehaviour的Lua-CSharp逻辑绑定，Awake逻辑也写在这里
function M.LuaAwake(injector)
    local self = M
    injector:Inject(self)
    self.behaviour = injector
    self.gameObject = injector.gameObject
    UpdateMessage.Require(self.gameObject).update:AddAction(ezutil.bind(self.LuaUpdate, self))
    return self
end

function M:LuaStart()
    self.b_GameOver = false
    self.b_Restart = false
    self.text_Restart.text = ""
    self.text_GameOver.text = ""
    self.n_Score = 0
    self:UpdateScore()
    self.behaviour:StartCoroutine(self:Cor_SpawnWaves())
end

function M:LuaUpdate()
    if self.b_Restart then
        if CS.UnityEngine.Input.GetKeyDown(CS.UnityEngine.KeyCode.R) then
            self:Init()
        end
    end
end

function M:Cor_SpawnWaves()
    return require("xlua.util").cs_generator(
        function()
            coroutine.yield(CS.UnityEngine.WaitForSeconds(self.n_StartWait))
            while (true) do
                for i = 1, self.n_HazardCount do
                    local index = EZLuaUtility.RandomInt(0, #self.Hazards) + 1 -- CS.UnityEngine.Random会得到小数
                    local x = EZLuaUtility.RandomFloat(-self.v3_SpawnValues.x, self.v3_SpawnValues.x)
                    local position = Vector3(x, self.v3_SpawnValues.y, self.v3_SpawnValues.z)
                    local rotation = CS.UnityEngine.Quaternion.identity
                    local hazard = CS.UnityEngine.Object.Instantiate(self.Hazards[index], position, rotation)
                    coroutine.yield(CS.UnityEngine.WaitForSeconds(self.n_SpawnWait))
                end
                coroutine.yield(CS.UnityEngine.WaitForSeconds(self.n_WaveWait))
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
----- CODE -----
return M
