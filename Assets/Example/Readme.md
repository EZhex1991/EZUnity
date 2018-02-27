## 该示例以官方Tutorial-SpaceShooter为模板，使用lua重写（xLua方案），运行该示例前请注意以下几点。
1. SpaceShooter.unity场景启动。
1. 本人尽可能保证lua代码的结构与C#代码结构相似，方便初学者对比学习，甚至还保留了原示例的部分bug（比如物体没销毁啥的），**该示例并不官方，仅供初学者参考。其中lua面向对象的模拟方式比较独特，如有自己的面向对象规则可以不用深究。**
1. **注意，Tags和Layers只能在Editor下添加，所以在热更中使用并不方便(个人在5.x下没找到其他方式)。如果报错显示找不到Tag，请自行添加，Tag0:"Enemy", Tag1:"Bonudary"。**
1. 该示例实际用到的C#脚本除了LuaEnv的启动以外还有：  
EZFramework/XLuaExtension/LuaInjector/...  
EZFramework/XLuaExtension/LuaMessage/...  
EZFramework/XLuaExtension/LuaUtility  
EZFramework/Core/Manager/EZResource（这个可以忽略）

-----
2018/02/07: 
- 官方示例升级到1.4
- 代码结构由之前的唯一入口更改为LuaBehaviour一对一的方式