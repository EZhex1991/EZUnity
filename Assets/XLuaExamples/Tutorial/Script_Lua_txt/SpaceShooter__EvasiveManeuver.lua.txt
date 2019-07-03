--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2017-11-16 14:08:10
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Mathf = CS.UnityEngine.Mathf
local Time = CS.UnityEngine.Time
local Vector3 = CS.UnityEngine.Vector3
local Quaternion = CS.UnityEngine.Quaternion
local EZLuaUtility = CS.EZhex1991.EZUnity.XLuaExtension.EZLuaUtility
local UpdateMessage = CS.EZhex1991.EZUnity.XLuaExtension.UpdateMessage
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

function M.LuaAwake(injector)
    local self = M:new()
    injector:Inject(self)
    self.behaviour = injector
    self.gameObject = injector.gameObject
    self.transform = self.gameObject.transform
    self.rigidbody = self.gameObject:GetComponent("Rigidbody")
    UpdateMessage.Require(self.gameObject).fixedUpdate:AddAction(ezutil.bind(self.LuaFixedUpdate, self))
    return self
end

function M:LuaStart()
    self.currentSpeed = self.rigidbody.velocity.z
    self.behaviour:StartCoroutine(self:Cor_Evade())
end

function M:Cor_Evade()
    return require("xlua.util").cs_generator(
        function()
            coroutine.yield(
                CS.UnityEngine.WaitForSeconds(EZLuaUtility.RandomFloat(self.v2_StartWait[0], self.v2_StartWait[1]))
            )
            while not EZLuaUtility.IsNull(self.gameObject) do
                self.targetManeuver = EZLuaUtility.RandomFloat(1, self.n_Dodge) * -Mathf.Sign(self.transform.position.x)
                coroutine.yield(
                    CS.UnityEngine.WaitForSeconds(
                        EZLuaUtility.RandomFloat(self.v2_ManeuverTime[0], self.v2_ManeuverTime[1])
                    )
                )
                self.targetManeuver = 0
                coroutine.yield(
                    CS.UnityEngine.WaitForSeconds(
                        EZLuaUtility.RandomFloat(self.v2_ManeuverWait[0], self.v2_ManeuverWait[1])
                    )
                )
            end
        end
    )
end

function M:LuaFixedUpdate()
    local newManeuver =
        Mathf.MoveTowards(self.rigidbody.velocity.x, self.targetManeuver, self.n_Smoothing * Time.deltaTime)
    self.rigidbody.velocity = Vector3(newManeuver, 0, self.currentSpeed)
    self.rigidbody.position =
        Vector3(
        EZLuaUtility.ClampFloat(self.rigidbody.position.x, self.Boundary.xMin, self.Boundary.xMax),
        0,
        EZLuaUtility.ClampFloat(self.rigidbody.position.z, self.Boundary.zMin, self.Boundary.zMax)
    )
    self.rigidbody.rotation = Quaternion.Euler(0, 0, self.rigidbody.velocity.x * -self.n_Tilt)
end
----- CODE -----
return M
