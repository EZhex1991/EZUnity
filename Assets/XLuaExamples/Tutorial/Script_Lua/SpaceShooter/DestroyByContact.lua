--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2017-11-15 19:11:43
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local Object = CS.UnityEngine.Object
local TriggerMessage = CS.EZhex1991.EZUnity.XLuaExtension.TriggerMessage
local EZLuaUtility = CS.EZhex1991.EZUnity.XLuaExtension.EZLuaUtility
local bind = require("xlua.util").bind

local M = require("ezlua.module"):module()
----- CODE -----
function M.LuaAwake(injector)
    self = M:new()
    injector:Inject(self)
    self.gameObject = injector.gameObject
    self.transform = self.gameObject.transform
    TriggerMessage.Require(self.gameObject).onTriggerEnter:AddAction(bind(self.LuaOnTriggerEnter, self))
    return self
end

function M:LuaStart()
    self.gameController = require("SpaceShooter.GameController")
end

function M:LuaOnTriggerEnter(collider)
    if collider.tag == "Boundary" or collider.tag == "Enemy" then
        return
    end
    if not EZLuaUtility.IsNull(self.go_Explosion) then
        local go = Object.Instantiate(self.go_Explosion, self.transform.position, self.transform.rotation)
    end
    if collider.tag == "Player" then
        local go = Object.Instantiate(self.go_PlayerExplosion, collider.transform.position, collider.transform.rotation)
        self.gameController:GameOver()
    end
    self.gameController:AddScore(self.n_ScoreValue)
    Object.Destroy(collider.gameObject)
    Object.Destroy(self.gameObject)
end
----- CODE -----
return M
