--[==[
Author:     熊哲
CreateTime: 6/9/2017 5:21:56 PM
Description:

--]==]
local moduleName = ...
local M = {}
M.__index = M
----- begin module -----
-- 多用于Hotfix，需要用一个Monobehaviour的StartCoroutine启动
local util = require("xlua.util")
function Coroutine(...)
	local params = {...}
	local cor = function()
		return util.cs_generator(
			function()
				print("Coroutine Test")

				local wwwB = CS.UnityEngine.WWW("www.baidu.com")
				coroutine.yield(wwwB)
				print(wwwB.text)
				print("baidu go die")

				local wwwA = CS.UnityEngine.WWW("www.alibaba.com")
				while not wwwA.isDone do
					coroutine.yield(CS.UnityEngine.WaitForSeconds(2))
					print(wwwA.progress)
				end
				print("ali father")

				local wwwT = CS.UnityEngine.WWW("www.tencent.com")
				while not wwwT.isDone do
					coroutine.yield(CS.UnityEngine.WaitForEndOfFrame())
					print(wwwT.progress)
				end
				print("tencent majesty")

				print(table.unpack(params))
			end
		)
	end
	CS.EZUnity.XLuaExample.LuaManager.Instance:StartCoroutine(cor())
end
Coroutine("xlua", "banzai")
----- end -----
return M
