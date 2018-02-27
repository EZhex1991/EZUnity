--[==[
- Author:       熊哲
- CreateTime:   11/16/2017 2:08:10 PM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Mathf = CS.UnityEngine.Mathf
local Time = CS.UnityEngine.Time
local Vector3 = CS.UnityEngine.Vector3
local Quaternion = CS.UnityEngine.Quaternion
local LuaUtility = CS.EZFramework.XLuaExtension.LuaUtility
local UpdateMessage = CS.EZFramework.XLuaExtension.UpdateMessage
local ezutil = require("ezlua.util")

local M = require("ezlua.module"):module()
----- CODE -----
M.Boundary = {
    xMin,
    xMax,
    zMin,
    zMax
}
M.currentSpeed = 0
M.targetManeuver = 0

function M.LCBinder(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.transform = self.gameObject.transform
    self.rigidbody = self.gameObject:GetComponent("Rigidbody")
    UpdateMessage.Require(self.gameObject).fixedUpdate:AddAction(ezutil.bind(self.FixedUpdate, self))
    self:Start()
    return self
end

function M:Start()
    self.currentSpeed = self.rigidbody.velocity.z
    ezutil.startcoroutine(self:Cor_Evade())
end

function M:Cor_Evade()
    return function()
        ezutil.waitforseconds(LuaUtility.RandomFloat(self.v2_StartWait[0], self.v2_StartWait[1]))
        while not LuaUtility.IsNull(self.gameObject) do
            self.targetManeuver = LuaUtility.RandomFloat(1, self.n_Dodge) * -Mathf.Sign(self.transform.position.x)
            ezutil.waitforseconds(LuaUtility.RandomFloat(self.v2_ManeuverTime[0], self.v2_ManeuverTime[1]))
            self.targetManeuver = 0
            ezutil.waitforseconds(LuaUtility.RandomFloat(self.v2_ManeuverWait[0], self.v2_ManeuverWait[1]))
        end
    end
end

function M:FixedUpdate()
    local newManeuver =
        Mathf.MoveTowards(self.rigidbody.velocity.x, self.targetManeuver, self.n_Smoothing * Time.deltaTime)
    self.rigidbody.velocity = Vector3(newManeuver, 0, self.currentSpeed)
    self.rigidbody.position =
        Vector3(
        LuaUtility.ClampFloat(self.rigidbody.position.x, self.Boundary.xMin, self.Boundary.xMax),
        0,
        LuaUtility.ClampFloat(self.rigidbody.position.z, self.Boundary.zMin, self.Boundary.zMax)
    )
    self.rigidbody.rotation = Quaternion.Euler(0, 0, self.rigidbody.velocity.x * -self.n_Tilt)
end
----- CODE -----
return M
