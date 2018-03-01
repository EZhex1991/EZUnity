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

local M = {}
----- CODE -----
function M.LCBinder(injector) -- bind lua(table) with C#(MonoBehaviour)
    CS.UnityEngine.Physics.gravity = Vector3(0, -98, 0)
    injector:Inject(M)
    UpdateMessage.Require(injector.gameObject).update:AddAction(M.Update)
    return M
end

function M.Update()
    if Input.GetMouseButtonDown(0) then
        local ray = M.camera:ScreenPointToRay(Input.mousePosition)
        local hit, hitInfo = LuaUtility.Raycast(ray)
        if hit then
            local position = hitInfo.point
            position.x = Mathf.Clamp(position.x, M.v2_XRange[0], M.v2_XRange[1])
            position.y = M.n_Height
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
