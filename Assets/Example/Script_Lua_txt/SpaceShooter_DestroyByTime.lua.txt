--[==[
Author:     熊哲
CreateTime: 11/15/2017 7:06:50 PM
Description:

--]==]
local M = {}
M._moduleName = ...
M.__index = M
----- begin module -----
function M:New(go, n_Lifetime)
    self = new(self)
    CS.UnityEngine.Object.Destroy(go, n_Lifetime)
    return self
end
----- end -----
return M
