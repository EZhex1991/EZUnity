--[==[
Author:     熊哲
CreateTime: 11/14/2017 11:46:53 AM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
-- lua entrance
local SpaceShooter = require("SpaceShooter.GameController")

function Start()
    SpaceShooter:Init()
end

function Exit()
    print("exit")
end

-- global function
function new(class)
    local t = {}
    setmetatable(t, class)
    return t
end

local util = require "xlua.util"
Yield =
    util.async_to_sync(
    function(yield, callback)
        CS.EZFramework.EZLua.Instance:Yield(yield, callback)
    end
)
function WaitForEndOfFrame()
    Yield(CS.UnityEngine.WaitForEndOfFrame())
end
function WaitForFixedUpdate()
    Yield(CS.UnityEngine.WaitForFixedUpdate())
end
function WaitForSeconds(sec)
    Yield(CS.UnityEngine.WaitForSeconds(sec))
end
----- end -----
return M
