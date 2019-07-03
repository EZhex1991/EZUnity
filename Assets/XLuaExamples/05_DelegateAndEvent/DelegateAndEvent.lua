--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   #CREATETIME#
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local moduleName = ...
local M = {}
M.__index = M
----- begin module -----
local gameObject = CS.UnityEngine.GameObject.Find("Cube")
local behaviour = gameObject:GetComponent("TestBehaviour")
M.time = 0

-- 把function()赋值给System.Action
function Update()
	M.time = M.time + CS.UnityEngine.Time.deltaTime
end
behaviour.updateAction = Update

-- 把function(collider)添加到delegate void OnTriggerEnterAction(Collider collider)
-- 上面的System.Action也可以用加减(记得先判断是否为nil)
function OnTriggerEnter(collider)
	print("OnTriggerEnter in lua, objName:" .. collider.name)
end
behaviour.onTriggerEnterAction = behaviour.onTriggerEnterAction + OnTriggerEnter

-- 注意：这是个OnCollisionEnterEvent类型实例，通过调用封装过的方法把function(collision)添加到Action<Collision>，
-- OnCollisionEnterEvent需要加到LuaCallCSharp，Action<Collision>需要加到CSharpCallLua
-- Button.onClick:AddListener同理
function OnCollisionEnter(collision)
	print("OnCollisionEnter in lua, objName:" .. collision.collider.name)
	CS.UnityEngine.Object.Destroy(gameObject)
end
behaviour.onCollisionEnterEvent:Add(OnCollisionEnter)

-- C#知识：event用来修饰一个delegate，使其只能由声明类调用，非声明类中其只能作为+=/-=表达式的左值
-- 在xLua中，使用':'，并且第一个参数使用'+'或'-'，为一个event添加和移除function监听(添加C#delegate监听参考下面createdelegate的使用)
function OnDestroy()
	print("OnDestroy in lua, time:" .. M.time)
end
behaviour:onDestroyEvent("+", OnDestroy)

-- 关于闭包，使用Action<LuaTable>不是一个高效的方法（而且还得外加一个参数把table传过去）
-- 建议的方式：xlua.util.bind可以将table绑定到一个function实现闭包
function M:OnDestroyInBind()
	print("bind test, self.time=" .. self.time)
end
local closure = require("xlua.util").bind(M.OnDestroyInBind, M)
behaviour:onDestroyEvent("+", closure)

-- xlua.util.createdelegate的使用，使用一个C#方法在lua上创建一个C#delegate为event添加监听（多用于hotfix）
-- 五个参数分别是：delegate的类型，C#方法所作用的实例，实例的类型，方法的名称，参数的类型列表
-- 如果是静态方法，那么“C#方法所作用的实例”使用nil，如果方法无参，那么“参数的类型列表”空着即可
local nonStaticDelegate =
	require("xlua.util").createdelegate(
	CS.System.Action,
	behaviour,
	CS.EZhex1991.EZUnity.XLuaExample.TestBehaviour,
	"NonStaticFunction",
	{}
)
local staticDelegate =
	require("xlua.util").createdelegate(
	CS.System["Action`1[System.Int32]"],
	nil,
	CS.EZhex1991.EZUnity.XLuaExample.TestBehaviour,
	"StaticFunction",
	{typeof(CS.System.Int32)}
)
behaviour:onDestroyEvent("+", nonStaticDelegate)
behaviour:testEvent("+", staticDelegate)
----- end -----
return M
