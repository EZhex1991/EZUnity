--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2017-05-23 19:00:42
- Orgnization:  #ORGNIZATION#
- Description:  
--]==]
local moduleName = ...
local M = {}
M.__index = M
----- begin module -----

-- float和int参数造成UnityEngine.Random.Range重载调用不明确
print("Random.Range(0,10)", "->", CS.UnityEngine.Random.Range(0, 10)) -- float和int在lua上无法区分，这里会调用Range(float, float)
print("OverloadWrap.RandomInt(0,10)", "->", CS.EZhex1991.EZUnity.XLuaExample.OverloadWrap.RandomInt(0, 10)) -- 自己把这个方法封装一下RandomInt(int, int) -> Range(int, int)
print("OverloadWrap.RandomFoat(0,10)", "->", CS.EZhex1991.EZUnity.XLuaExample.OverloadWrap.RandomFloat(0, 10)) -- RandomFloat(float, float) -> Range(float, float)

-- out和ref参数造成UnityEngine.Physics.Raycast重载调用不明确
local origin = CS.UnityEngine.Vector3.one * -1
local direction = CS.UnityEngine.Vector3.one
local ray = CS.UnityEngine.Ray(origin, direction)
local hit1, info1 = CS.UnityEngine.Physics.Raycast(ray) -- 无法区分Raycast(Ray)和Raycast(Ray, out HitInfo)，这里会调用Raycast(Ray)
print(hit1, info1 and info1.collider.name)
local hit2, info2 = CS.EZhex1991.EZUnity.XLuaExample.OverloadWrap.Raycast(ray) -- 自己封装Raycast(Ray, out HitInfo)
print(hit2, info2 and info2.collider.name)
----- end -----
return M
