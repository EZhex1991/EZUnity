--[==[
Author:     熊哲
CreateTime: 4/18/2017 5:29:24 PM
Description:

--]==]
local moduleName = ...;
local M = { };
M.__index = M;
----- begin module -----
local DictSS = CS.System.Collections.Generic["Dictionary`2[System.String,System.String]"];
print(DictSS);
local dictSS = DictSS();
dictSS:Add("0", "zero");
dictSS:Add("1", "one");
print(dictSS.Count);

local DictIS = CS.System.Collections.Generic["Dictionary`2[System.Int32,System.Object]"];
local dictIS = DictIS();
dictIS:Add(0, "zero");
print(dictIS.Count);

local DictSV = CS.System.Collections.Generic["Dictionary`2[System.String,UnityEngine.Vector3]"];
local dictSV = DictSV();
dictSV:Add("0", Vector3.zero);
dictSV:Add("1", Vector3.one);
print(dictSV.Count);
----- end -----
return M;