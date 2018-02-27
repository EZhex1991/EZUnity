--[==[
- Author:       熊哲
- CreateTime:   2018-02-27 17:18:49
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = {}
----- CODE -----
function M.LCBinder(injector)
    local self = M
    self.btns = {}
    injector:Inject(self)
    self.btns[1].onClick:AddListener(self.LuckyBall)
    self.btns[2].onClick:AddListener(self.SpaceShooter)
    return self
end

function M.LuckyBall()
    CS.UnityEngine.SceneManagement.SceneManager.LoadScene("LuckyBall")
end

function M.SpaceShooter()
    require("SpaceShooter.GameController"):Init()
end
----- CODE -----
return M
