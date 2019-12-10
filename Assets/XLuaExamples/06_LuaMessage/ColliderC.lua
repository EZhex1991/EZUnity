--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   #CREATETIME#
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = {}
M._modulename = ...
M.__index = M
setmetatable(M, require("06_LuaMessage.ColliderA"))
----- begin module -----
local Vector3 = CS.UnityEngine.Vector3

M.name = "CCC"
M.position = Vector3(0, -3, 0)

-- 以下为ColliderC的行为
function M:Start() -- 不加Rigidbody
end
function M:OnCollisionEnter(collision)
    getmetatable(M).OnCollisionEnter(self, collision) -- 调用基类方法
    print("OnCollisionEnter:", self.name, collision.collider.name)
    local scale = self.gameObject.transform.localScale
    scale.x = scale.x * 2
    scale.z = scale.z * 2
    self.gameObject.transform.localScale = scale -- 碰撞时面积增倍
end
----- end -----
return M
