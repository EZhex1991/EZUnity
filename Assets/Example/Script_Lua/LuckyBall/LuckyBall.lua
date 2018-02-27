--[==[
- Author:       熊哲
- CreateTime:   2018-02-26 11:42:24
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Input = CS.UnityEngine.Input
local GameObject = CS.UnityEngine.GameObject
local Mathf = CS.UnityEngine.Mathf
local Quaternion = CS.UnityEngine.Quaternion
local Vector3 = CS.UnityEngine.Vector3
local UpdateMessage = CS.EZFramework.XLuaExtension.UpdateMessage
local LuaUtility = CS.EZFramework.XLuaExtension.LuaUtility
local ezutil = require("ezlua.util")

local M = require("ezlua.module"):module()
----- CODE -----
function M.LCBinder(injector)
    CS.UnityEngine.Physics.gravity = Vector3(0, -98, 0)
    local self = M:new()
    injector:Inject(self)
    UpdateMessage.Require(injector.gameObject).update:AddAction(ezutil.bind(self.Update, self))
    return self
end

function M:Update()
    if Input.GetMouseButtonDown(0) then
        local ray = self.camera:ScreenPointToRay(Input.mousePosition)
        local hit, hitInfo = LuaUtility.Raycast(ray)
        if hit then
            local position = hitInfo.point
            position.x = Mathf.Clamp(position.x, self.v2_XRange[0], self.v2_XRange[1])
            position.y = self.n_Height
            local ball = GameObject.Instantiate(self.go_Ball, position, Quaternion.identity)
            ball:GetComponent("Rigidbody").angularVelocity =
                Vector3.forward * ezutil.randomfloat(self.v2_AngularVelocity[0], self.v2_AngularVelocity[1])
        end
    end
end
----- CODE -----
return M
