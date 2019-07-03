--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   #CREATETIME#
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local M = {} -- 脚本内使用local的M作为表名，不用和文件名一致，方便维护
M._modulename = ... -- 这里是require的参数，通常就是文件路径，很多时候可以作为日志的tag
M.__index = M -- 这个是为了方便模拟继承，详见ColliderB
-- 以上三句是个人的自定义模板，仅供参考
----- begin module -----
local Color = CS.UnityEngine.Color
local Random = CS.UnityEngine.Random
local Vector3 = CS.UnityEngine.Vector3
local Quaternion = CS.UnityEngine.Quaternion
local LuaMessage = CS.EZhex1991.EZUnity.XLuaExample.LuaMessage
local util = require("xlua.util")
-- 以上类似于java的import或者C#的using，使用local避免污染全局环境（虽然有点麻烦，但5.2取消module也是这个原因，尽量遵守）

M.name = "AAA"
M.position = Vector3(0, 3, 0)
-- 使用M.初始化的变量，方便在继承时重写

function M:_New() -- 拷贝一个table
    local t = {}
    setmetatable(t, self)
    return t
end

function M:New(gameObject) -- 初始化
    self = self:_New() -- 获取拷贝的table，右值的self是为了实现多态（这样描述其实不太准确。。。）
    self.gameObject = gameObject
    self.gameObject.name = self.name
    self.gameObject.transform.position = self.position
    self.message = LuaMessage.Require(self.gameObject) -- 获取事件分发组件
    self.message.start:AddAction(util.bind(self.Start, self)) -- 为事件添加方法
    self.message.onCollisionEnter:AddAction(util.bind(self.OnCollisionEnter, self))
    self.message.onMouseOver:AddAction(util.bind(self.OnMouseOver, self))
    return self
end

-- 以下为ColliderA的行为
function M:Start() -- 加了Rigidbody，开始时会下落
    self.gameObject:AddComponent(typeof(CS.UnityEngine.Rigidbody))
end
function M:OnCollisionEnter(collision) -- 碰撞时回到原位置，随机换颜色
    self.gameObject.transform.position = self.position
    self.gameObject:GetComponent("Renderer").material.color =
        Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1)
end
function M:OnMouseOver() -- 鼠标悬停print自己的名字，这个方法会被B继承
    print(self.name)
end
----- end -----
return M
