--[==[
Author:     熊哲
CreateTime: 11/16/2017 2:08:10 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
local Mathf = CS.UnityEngine.Mathf
local Time = CS.UnityEngine.Time
local Vector3 = CS.UnityEngine.Vector3
local Quaternion = CS.UnityEngine.Quaternion
local LuaUtility = CS.EZFramework.XLuaExtension.LuaUtility
local bind = require("xlua.util").bind

local Boundary = {
    xMin = -6,
    xMax = 6,
    zMin = -20,
    zMax = 20
}
M.gameObject = nil
M.transform = nil
M.rigidbody = nil
M.n_Tilt = 10
M.n_Dodge = 5
M.n_Smoothing = 7.5
M.StartWait = {0.5, 1}
M.ManeuverTime = {1, 2}
M.ManeuverWait = {1, 2}

M.currentSpeed = 0
M.targetManeuver = 0

function M:New(go)
    self = new(self)
    self.gameObject = go
    CS.EZFramework.XLuaExtension.UpdateMessage.Require(go).fixedUpdate:AddAction(bind(self.FixedUpdate, self))
    self.transform = go.transform
    self.rigidbody = go:GetComponent("Rigidbody")
    self.currentSpeed = self.rigidbody.velocity.z
    coroutine.resume(self:Evade())
    return self
end

function M:Evade()
    return coroutine.create(
        function()
            WaitForSeconds(LuaUtility.RandomFloat(self.StartWait[1], self.StartWait[2]))
            while not LuaUtility.IsNull(self.gameObject) do
                self.targetManeuver = LuaUtility.RandomFloat(1, self.n_Dodge) * -Mathf.Sign(self.transform.position.x)
                WaitForSeconds(LuaUtility.RandomFloat(self.ManeuverTime[1], self.ManeuverTime[2]))
                self.targetManeuver = 0
                WaitForSeconds(LuaUtility.RandomFloat(self.ManeuverWait[1], self.ManeuverWait[2]))
            end
        end
    )
end

function M:FixedUpdate()
    local newManeuver =
        Mathf.MoveTowards(self.rigidbody.velocity.x, self.targetManeuver, self.n_Smoothing * Time.deltaTime)
    self.rigidbody.velocity = Vector3(newManeuver, 0, self.currentSpeed)
    self.rigidbody.position =
        Vector3(
        LuaUtility.ClampFloat(self.rigidbody.position.x, Boundary.xMin, Boundary.xMax),
        0,
        LuaUtility.ClampFloat(self.rigidbody.position.z, Boundary.zMin, Boundary.zMax)
    )
    self.rigidbody.rotation = Quaternion.Euler(0, 0, self.rigidbody.velocity.x * -self.n_Tilt)
end
----- end -----
return M
