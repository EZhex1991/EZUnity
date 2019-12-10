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
require("06_LuaMessage.ColliderA"):New()
require("06_LuaMessage.ColliderB"):New() -- 注意ColliderB中并没有New方法，是从ColliderA中“继承”的
require("06_LuaMessage.ColliderC"):New() -- 面向对象相关的东西可以暂时忽略
----- end -----
return M
