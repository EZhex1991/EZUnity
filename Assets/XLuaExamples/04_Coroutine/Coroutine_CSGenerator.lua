--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2017-06-09 17:21:56
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local moduleName = ...
local M = {}
M.__index = M
----- begin module -----
-- 需要用一个Monobehaviour的StartCoroutine启动
local util = require("xlua.util")

function Coroutine1(...)
	local params = {...}
	return util.cs_generator(
		function()
			print("Coroutine Test")
			coroutine.yield(CS.UnityEngine.WaitForSeconds(0.5))
			print("tencent majesty")
			coroutine.yield(CS.UnityEngine.WaitForSeconds(0.5))
			print(table.unpack(params))

			while true do
				coroutine.yield(CS.UnityEngine.WaitForSeconds(0.5))
				print("...")
			end
		end
	)
end

function Coroutine2(cor)
	return util.cs_generator(
		function()
			coroutine.yield(CS.UnityEngine.WaitForSeconds(3))
			CS.EZhex1991.EZUnity.XLuaExample.LuaManager.Instance:StopCoroutine(cor)
			print("Coroutine Stopped")
		end
	)
end

-- 启动一个协程并返回
local cor = CS.EZhex1991.EZUnity.XLuaExample.LuaManager.Instance:StartCoroutine(Coroutine1("xlua", "banzai"))
-- 启动第二个协程，由该协程来结束第一个协程
CS.EZhex1991.EZUnity.XLuaExample.LuaManager.Instance:StartCoroutine(Coroutine2(cor))
----- end -----
return M
