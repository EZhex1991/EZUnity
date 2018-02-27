--[==[
- Author:       熊哲
- CreateTime:   11/15/2017 7:11:43 PM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Object = CS.UnityEngine.Object
local TriggerMessage = CS.EZFramework.XLuaExtension.TriggerMessage
local LuaUtility = CS.EZFramework.XLuaExtension.LuaUtility
local bind = require("xlua.util").bind

local M = require("ezlua.module"):module()
----- CODE -----
function M.LCBinder(injector)
    self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.transform = self.gameObject.transform
    TriggerMessage.Require(self.gameObject).onTriggerEnter:AddAction(bind(self.OnTriggerEnter, self))
    self:Start()
    return self
end

function M:Start()
    self.gameController = require("SpaceShooter.GameController")
end

function M:OnTriggerEnter(collider)
    if collider.tag == "Boundary" or collider.tag == "Enemy" then
        return
    end
    if not LuaUtility.IsNull(self.go_Explosion) then
        local go = Object.Instantiate(self.go_Explosion, self.transform.position, self.transform.rotation)
        require("SpaceShooter.DestroyByTime"):new(go, 2)
    end
    if collider.tag == "Player" then
        local go = Object.Instantiate(self.go_PlayerExplosion, collider.transform.position, collider.transform.rotation)
        require("SpaceShooter.DestroyByTime"):new(go, 2)
        self.gameController:GameOver()
    end
    self.gameController:AddScore(self.n_ScoreValue)
    Object.Destroy(collider.gameObject)
    Object.Destroy(self.gameObject)
end
----- CODE -----
return M
