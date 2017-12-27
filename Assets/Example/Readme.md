## 该示例以官方Tutorial-SpaceShooter为模板，使用lua重写（xLua），运行该示例前请注意以下几点。
1. 从Example.unity场景启动lua逻辑。
1. 本人尽可能保证lua代码的结构与C#代码结构相似，方便初学者对比学习，甚至还保留了原示例的部分bug（比如物体没销毁啥的），**该示例并不官方，仅供初学者参考。其中面向对象的模拟方式比较独特，如有自己的面向对象规则可以不用深究。**
1. **注意，Tags和Layers只能通过UnityEditor去添加，所以在热更中使用并不方便(个人在5.x下没找到其他方式)。该示例中分别使用Unity内置Tag:"Finish"和"Respawn"替代了原示例中的"Boundary"和"Enemy"。**
1. 该示例实际用到的C#脚本除了LuaEnv的启动以外还有：  
EZFramework/XLuaExtension/LuaInjector  
EZFramework/XLuaExtension/LuaUtility  
EZFramework/XLuaExtension/LuaMessage/...  
EZFramework/Core/Manager/EZResource（这个可以忽略）
1. 热更新原理请自行Google，xlua只是“Unity下的lua编程解决方案”，“热更新”是下载替换lua代码的文本实现了逻辑变更的效果而已。