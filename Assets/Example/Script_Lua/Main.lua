--[==[
- Author:       熊哲
- CreateTime:   11/14/2017 11:46:53 AM
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = {}
----- CODE -----
function Start()
    print("Start")
    CS.UnityEngine.SceneManagement.SceneManager.LoadScene("Menu")
end

function Exit()
    print("Exit")
end
----- CODE -----
return M
