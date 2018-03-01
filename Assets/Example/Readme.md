### Example 01: Lucky Ball
1. 很普通的一个Demo，点击屏幕投球即可；
1. 脚本外其它资源在[Assets/Example/01_LuckyBall](01_LuckyBall)目录下

### Example 02: Space Shooter
该示例以Unity官方Tutorial - Space Shooter为模板，使用lua重写（xLua方案），运行该示例前请注意以下几点。
1. 本人尽可能保证lua代码的结构与C#代码结构相似，方便初学者对比学习，甚至还保留了原示例的部分bug（比如物体没销毁啥的），**该示例并不官方，仅供初学者参考**；
1. **注意，Tags和Layers只能在Editor下添加，所以在热更中使用并不方便(个人在5.x下没找到其他方式)。如果报错显示找不到Tag，请自行添加，Tag0:"Enemy", Tag1:"Bonudary"**；
1. prefab和场景在[Assets/Example/02_SpaceShooter/_Complete-Lua](02_SpaceShooter/_Complete-Lua)目录下；

-----
经常说到的“纯lua”逻辑并不是整个项目不用到任何C#，而是在已有的框架下，游戏机制相关逻辑都使用lua编写。

该示例用到的C#脚本除了LuaEnv的启动以外还有：  
EZFramework/XLuaExtension/LuaBehaviour/...  
EZFramework/XLuaExtension/LuaMessage/...  
EZFramework/XLuaExtension/LuaUtility  
EZFramework/Core/Manager/EZResource（这个可以忽略）

-----
2018/02/07: 
- Space Shooter tutorial 升级到1.4
- 代码结构由之前的唯一入口更改为LuaBehaviour一对一的方式
- 添加Demo: Lucky Ball