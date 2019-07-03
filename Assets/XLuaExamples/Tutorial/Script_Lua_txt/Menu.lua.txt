--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2018-02-27 17:18:49
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = {}
----- CODE -----
local LoadSceneMode = CS.UnityEngine.SceneManagement.LoadSceneMode

function M.LuaAwake(injector)
    M.btns = {}
    injector:Inject(M)
    M.btns[1].onClick:AddListener(M.Menu)
    M.btns[2].onClick:AddListener(M.LuaBehaviour)
    M.btns[3].onClick:AddListener(M.LuckyBall)
    M.btns[4].onClick:AddListener(M.SpaceShooter)
    return M
end

function M.Menu()
    CS.UnityEngine.SceneManagement.SceneManager.LoadScene("Menu")
end

function M.LuaBehaviour()
    CS.UnityEngine.SceneManagement.SceneManager.LoadScene("LuaBehaviour", LoadSceneMode.Additive)
    M.panel_Menu:SetActive(false)
end

function M.LuckyBall()
    CS.UnityEngine.SceneManagement.SceneManager.LoadScene("LuckyBall", LoadSceneMode.Additive)
    M.panel_Menu:SetActive(false)
end

function M.SpaceShooter()
    require("SpaceShooter.GameController"):Init()
    M.panel_Menu:SetActive(false)
end
----- CODE -----
return M
