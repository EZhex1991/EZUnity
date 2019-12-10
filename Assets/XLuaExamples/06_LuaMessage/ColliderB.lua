--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   #CREATETIME#
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = {}
M._modulename = ...
M.__index = M
setmetatable(M, require("06_LuaMessage.ColliderA")) -- 模拟面向对象继承，ColliderA中做过处理（M.__index = M），所以结合起来就是很多人常用的：setmetatable(ColliderB, {__index = ColliderA})
----- begin module -----
local Vector3 = CS.UnityEngine.Vector3

M.name = "BBB" -- 重写变量
M.position = Vector3(0, 0, 0) -- 重写变量
-- 注意这里并没有New方法，是从ColliderA中“继承”的

-- 以下为ColliderB的行为
function M:Start() -- 不加Rigidbody，Collider设置为Trigger
    self.gameObject:GetComponent("Collider").isTrigger = true
end
function M:OnTriggerEnter(collider)
    getmetatable(M).OnTriggerEnter(self, collider) -- 调用基类方法
    print("OnTriggerEnter:  ", self.name, collider.name)
end
----- end -----
return M
