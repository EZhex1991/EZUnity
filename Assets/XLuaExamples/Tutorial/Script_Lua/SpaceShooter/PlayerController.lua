--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2017-11-15 18:21:40
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Object = CS.UnityEngine.Object
local Input = CS.UnityEngine.Input
local Time = CS.UnityEngine.Time
local Vector3 = CS.UnityEngine.Vector3
local EZLuaUtility = CS.EZhex1991.EZUnity.XLuaExtension.EZLuaUtility
local bind = require("xlua.util").bind

local M = require("ezlua.module"):module()
----- CODE -----
M.n_NextFire = 0

function M.LuaAwake(injector)
    self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.transform = self.gameObject.transform
    self.rigidbody = self.gameObject:GetComponent("Rigidbody")
    local updateMessage = CS.EZhex1991.EZUnity.XLuaExtension.UpdateMessage.Require(self.gameObject)
    updateMessage.update:AddAction(bind(self.LuaUpdate, self))
    updateMessage.fixedUpdate:AddAction(bind(self.LuaFixedUpdate, self))
    return self
end

function M:LuaUpdate()
    if Input.GetButton("Fire1") and Time.time > self.n_NextFire then
        self.n_NextFire = Time.time + self.n_FireRate
        local shot = Object.Instantiate(self.go_Shot, self.tf_ShotSpawn.position, self.tf_ShotSpawn.rotation)
        require("SpaceShooter.Mover"):new(shot, 20)
        self.gameObject:GetComponent("AudioSource"):Play()
    end
end

function M:LuaFixedUpdate()
    local moveHorizontal = Input.GetAxis("Horizontal")
    local moveVertical = Input.GetAxis("Vertical")
    local movement = Vector3(moveHorizontal, 0, moveVertical)
    self.rigidbody.velocity = movement * self.n_Speed
    self.rigidbody.position =
        Vector3(
        EZLuaUtility.ClampFloat(self.rigidbody.position.x, self.Boundary.xMin, self.Boundary.xMax),
        0,
        EZLuaUtility.ClampFloat(self.rigidbody.position.z, self.Boundary.zMin, self.Boundary.zMax)
    )
    self.rigidbody.rotation = CS.UnityEngine.Quaternion.Euler(0, 0, self.rigidbody.velocity.x * -self.n_Tilt)
end
----- CODE -----
return M
