--[==[
Author:     熊哲
CreateTime: 4/18/2017 3:39:37 PM
Description:

--]==]
local moduleName = ...;
local M = { };
M.__index = M;
----- begin module -----
function Start()
	print("Lua Start");
end

function Exit()
	print("Lua Exit");
end
----- end -----
return M;