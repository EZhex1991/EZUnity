--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2018-02-27 16:20:26
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Object = CS.UnityEngine.Object
local ActivityMessage = CS.EZhex1991.EZUnity.XLuaExtension.ActivityMessage
local TriggerMessage = CS.EZhex1991.EZUnity.XLuaExtension.TriggerMessage

local M = {}
----- CODE -----
function M.LuaAwake(injector) -- bind lua(table) with C#(MonoBehaviour)
    injector:Inject(M)
    M.gameObject = injector.gameObject
    M.transform = M.gameObject.transform
    return M
end

function M.LuaStart()
    M.Triggers = {}
    for i = 1, M.transform.childCount do
        local go = M.transform:GetChild(i - 1).gameObject
        TriggerMessage.Require(go).onTriggerEnter:AddAction(
            function(other)
                if (other.name == "Ball(Clone)") then
                    Object.Destroy(other.gameObject, 2)
                end
                go:GetComponent("MeshRenderer").enabled = true
                M.Judge(i)
            end
        )
        M.Triggers[i] = false
    end
end

function M.Judge(i)
    M.Triggers[i] = true
    local p = 0
    local q = 0
    local count = 0
    for i = 1, #M.Triggers do
        if M.Triggers[i] then
            q = i
        else
            count = math.max(q - p, count)
            p = i
            q = i
        end
        count = math.max(q - p, count)
    end
    M.controllerObj:GetComponent("EZLuaBehaviour").luaTable.ShowResult(count)
    -- 以下为封装过的扩展方法，一个GameObject上有多个EZLuaBehaviour时可通过制定moduleName获取特定behaviour或者table
    -- moduleName可指定短名称"GameController"或者完整名称"LuckyBall.GameController"
    M.controllerObj:GetLuaBehaviour("GameController").luaTable.ShowResult(count)
    M.controllerObj:GetLuaBehaviour("LuckyBall.GameController").luaTable.ShowResult(count)
    M.controllerObj:GetLuaTable("GameController").ShowResult(count)
    M.controllerObj:GetLuaTable("LuckyBall.GameController").ShowResult(count)
end
----- CODE -----
return M
