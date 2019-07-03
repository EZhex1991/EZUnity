--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2018-02-26 11:42:24
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Input = CS.UnityEngine.Input
local GameObject = CS.UnityEngine.GameObject
local Mathf = CS.UnityEngine.Mathf
local Quaternion = CS.UnityEngine.Quaternion
local Vector3 = CS.UnityEngine.Vector3
local UpdateMessage = CS.EZhex1991.EZUnity.XLuaExtension.UpdateMessage
local EZLuaUtility = CS.EZhex1991.EZUnity.XLuaExtension.EZLuaUtility
local ezutil = require("ezlua.util")

local M = {}
----- CODE -----
function M.LuaAwake(injector) -- bind lua(table) with C#(MonoBehaviour)
    CS.UnityEngine.Physics.gravity = Vector3(0, -98, 0)
    injector:Inject(M)
    M.behaviour = injector
    UpdateMessage.Require(injector.gameObject).update:AddAction(M.LuaUpdate)
    return M
end

function M.LuaUpdate()
    if Input.GetMouseButtonDown(0) then
        local ray = M.camera:ScreenPointToRay(Input.mousePosition)
        local hit, hitInfo = EZLuaUtility.Raycast(ray)
        if hit then
            local position = hitInfo.point
            position.x =
                Mathf.Clamp(
                position.x,
                M.behaviour:Get("v2_XRange").vector2Value.x,
                M.behaviour:Get("v2_XRange").vector2Value.y
            )
            position.y = M.behaviour:Get("n_Height").floatValue
            local ball = GameObject.Instantiate(M.go_Ball, position, Quaternion.identity)
            ball:GetComponent("Rigidbody").angularVelocity =
                Vector3.forward * ezutil.randomfloat(M.v2_AngularVelocity[0], M.v2_AngularVelocity[1])
        end
    end
end

local current = 0
function M.ShowResult(result)
    if result ~= current then
        print("Lucky Ball!")
        current = result
        M.text_Result.text = result
    end
end
----- CODE -----
return M
