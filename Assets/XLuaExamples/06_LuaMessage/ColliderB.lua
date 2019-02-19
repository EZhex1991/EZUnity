--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   #CREATETIME#
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = {}
M._modulename = ...
M.__index = M
-- 以上三句是个人的自定义模板，仅供参考（请先看ColliderA.lua）
setmetatable(M, require("06_LuaMessage.ColliderA")) -- 模拟面向对象继承
-- ColliderA中做过处理（M.__index = M），所以结合这句就是很多人常用的：setmetatable(ColliderB, {__index = ColliderA})
----- begin module -----
local Vector3 = CS.UnityEngine.Vector3
local util = require("xlua.util")

M.name = "BBB" -- 重写变量
M.position = Vector3(0, -3, 0) -- 重写变量
-- 重写在ColliderA中用M.初始化的变量

-- 注意这里并没有New方法，是从ColliderA中“继承”的

-- 以下为ColliderB的行为
function M:Start() -- 不加Rigidbody
end
function M:OnCollisionEnter(collision)
    getmetatable(M).OnCollisionEnter(self, collision) -- 调用基类方法，即碰撞时回到原位置，随机换颜色
    local scale = self.gameObject.transform.localScale
    scale.x = scale.x * 2
    scale.z = scale.z * 2
    self.gameObject.transform.localScale = scale -- 碰撞时面积增倍
end
-- OnMouseOver继承于ColliderA
----- end -----
return M
