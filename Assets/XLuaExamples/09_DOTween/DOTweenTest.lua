--[==[
- Author:       ezhex1991@outlook.com
- CreateTime:   2018-11-28 18:53:31
- Organization: #ORGANIZATION#
- Description:  
--]==]
local GameObject = CS.UnityEngine.GameObject
local PrimitiveType = CS.UnityEngine.PrimitiveType

local M = {}
----- CODE -----
local cube = GameObject.CreatePrimitive(PrimitiveType.Cube)
cube.transform:DOScale(0.1, 1):OnComplete(function()
    print(cube.name, "DOScale Complete")
end)

local sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere)
local sequence = CS.DG.Tweening.DOTween.Sequence()
sequence:Append(sphere.transform:DOMoveX(1, 1))
sequence:Append(sphere.transform:DOScale(1.5, 1))
sequence:OnComplete(function()
    print(sphere.name, "Sequence Complete")
end)
----- CODE -----
return M
