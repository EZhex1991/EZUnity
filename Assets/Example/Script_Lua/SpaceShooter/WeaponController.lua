--[==[
- Author:       熊哲
- CreateTime:   11/16/2017 1:25:17 PM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Object = CS.UnityEngine.Object
local Time = CS.UnityEngine.Time
local bind = require("xlua.util").bind

local M = require("ezlua.module"):module()
----- CODE -----
function M.LCBinder(injector)
    local self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.timer = require("ezlua.timer"):new()
    CS.EZFramework.XLuaExtension.UpdateMessage.Require(self.gameObject).update:AddAction(bind(self.Update, self))
    self:Start()
    return self
end

function M:Start()
    self.timer:invokerepeat(self.n_Delay, self.n_FireRate, bind(self.Fire, self))
end

function M:Update()
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
