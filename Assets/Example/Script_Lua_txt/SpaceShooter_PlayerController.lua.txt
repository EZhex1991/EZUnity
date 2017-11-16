--[==[
Author:     熊哲
CreateTime: 11/15/2017 6:21:40 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
local Object = CS.UnityEngine.Object
local Input = CS.UnityEngine.Input
local Time = CS.UnityEngine.Time
local Vector3 = CS.UnityEngine.Vector3
local LuaUtility = CS.EZFramework.XLuaExtension.LuaUtility
local bind = require("xlua.util").bind

M.Boundary = {
    xMin = -6,
    xMax = 6,
    zMin = -4,
    zMax = 8
}

M.gameObject = nil
M.transform = nil
M.rigidbody = nil
M.go_Shot = nil
M.tf_ShotSpawn = nil

M.n_Speed = 10
M.n_Tilt = 5
M.n_FireRate = 0.25

local n_NextFire = 0
local audioSource = nil

function M:New(go)
    self = new(self)
    self.gameObject = go
    go:GetComponent("LuaInjector"):Inject(self)
    self.transform = go.transform
    self.rigidbody = go:GetComponent("Rigidbody")
    audioSource = go:GetComponent("AudioSource")
    local message = CS.EZFramework.XLuaExtension.UpdateMessage.Require(go)
    message.update:AddAction(bind(self.Update, self))
    message.fixedUpdate:AddAction(bind(self.FixedUpdate, self))
    return self
end

function M:Update()
    if Input.GetButton("Fire1") and Time.time > n_NextFire then
        n_NextFire = Time.time + self.n_FireRate
        local shot = Object.Instantiate(self.go_Shot, self.tf_ShotSpawn.position, self.tf_ShotSpawn.rotation)
        require("SpaceShooter.Mover"):New(shot, 20)
        audioSource:Play()
    end
end

function M:FixedUpdate()
    local horizontal = Input.GetAxis("Horizontal")
    local vertical = Input.GetAxis("Vertical")
    local movement = Vector3(horizontal, 0, vertical)
    self.rigidbody.velocity = movement * self.n_Speed
    self.rigidbody.position =
        Vector3(
        LuaUtility.ClampFloat(self.rigidbody.position.x, self.Boundary.xMin, self.Boundary.xMax),
        0,
        LuaUtility.ClampFloat(self.rigidbody.position.z, self.Boundary.zMin, self.Boundary.zMax)
    )
    self.rigidbody.rotation = CS.UnityEngine.Quaternion.Euler(0, 0, self.rigidbody.velocity.x * -self.n_Tilt)
end
----- end -----
return M
