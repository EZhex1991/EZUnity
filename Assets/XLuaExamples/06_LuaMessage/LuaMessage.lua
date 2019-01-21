--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   #CREATETIME#
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = {}
M._modulename = ...
M.__index = M
----- begin module -----
local GameObject = CS.UnityEngine.GameObject
local colliderA = GameObject.CreatePrimitive(CS.UnityEngine.PrimitiveType.Cube)
local colliderB = GameObject.CreatePrimitive(CS.UnityEngine.PrimitiveType.Cube)
require("06_LuaMessage.ColliderA"):New(colliderA)
require("06_LuaMessage.ColliderB"):New(colliderB) -- 注意ColliderB中并没有New方法，是从ColliderA中“继承”的
----- end -----
return M
