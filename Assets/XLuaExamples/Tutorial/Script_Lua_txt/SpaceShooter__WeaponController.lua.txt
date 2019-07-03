--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2017-11-16 13:25:17
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Object = CS.UnityEngine.Object
local Time = CS.UnityEngine.Time
local bind = require("xlua.util").bind

local M = require("ezlua.module"):module()
----- CODE -----
function M.LuaAwake(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.timer = require("ezlua.timer"):new()
    CS.EZhex1991.EZUnity.XLuaExtension.UpdateMessage.Require(self.gameObject).update:AddAction(bind(self.LuaUpdate, self))
    return self
end

function M:LuaStart()
    self.timer:invokerepeat(self.n_Delay, self.n_FireRate, bind(self.Fire, self))
end

function M:LuaUpdate()
    self.timer:tick(Time.deltaTime)
end

function M:Fire()
    local shot = Object.Instantiate(self.go_Shot, self.tf_ShotSpawn.position, self.tf_ShotSpawn.rotation)
    require("SpaceShooter.Mover"):new(shot, 20)
    require("SpaceShooter.DestroyByContact"):new(shot, 0)
    self.gameObject:GetComponent("AudioSource"):Play()
end
----- CODE -----
return M
