--[==[
- Author:       熊哲
- CreateTime:   2018-02-27 16:20:26
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Object = CS.UnityEngine.Object
local ActivityMessage = CS.EZFramework.XLuaExtension.ActivityMessage
local TriggerMessage = CS.EZFramework.XLuaExtension.TriggerMessage
local ezutil = require("ezlua.util")

local M = require("ezlua.module"):module()
----- CODE -----
function M.LCBinder(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.transform = self.gameObject.transform
    ActivityMessage.Require(self.gameObject).start:AddAction(ezutil.bind(self.Start, self))
    return self
end

function M:Start()
    self.Triggers = {}
    for i = 1, self.transform.childCount do
        local go = self.transform:GetChild(i - 1).gameObject
        TriggerMessage.Require(go).onTriggerEnter:AddAction(
            function(other)
                if (other.name == "Ball(Clone)") then
                    Object.Destroy(other.gameObject, 2)
                end
                go:GetComponent("MeshRenderer").enabled = true
                self:Judge(i)
            end
        )
        self.Triggers[i] = false
    end
end

function M:Judge(i)
    self.Triggers[i] = true
    local p = 1
    local q = 1
    local count = 0
    for i = 1, #self.Triggers do
        if self.Triggers[i] then
            q = i
        else
            count = math.max(q - p, count)
            p = i
            q = i
        end
        count = math.max(q - p, count)
    end
    self.text_Result.text = count
end
----- CODE -----
return M
