## 该示例以官方Tutorial-SpaceShooter为模板，使用lua重写，运行该示例前请注意以下几点。
1. 请在运行前请添加“Enemy”到 Edit > Project Settings > Tags and Layers > Tag。
1. 本人尽可能保证lua代码的结构与C#代码结构相似，方便初学者对比学习，甚至还保留了原示例的部分bug（比如物体没销毁），**并不代表xlua“就该这么用”**。
1. 该示例为xlua的 **纯lua开发模式中，lua唯一入口方式** 的使用示例。（个人对xlua在unity中的使用方式分类：Hotfix，唯一入口，MonoBehaviour一对一，）  
实际用到的C#脚本除了LuaEnv的启动还包含：  
EZFramework/XLuaExtension/LuaInjector  
EZFramework/XLuaExtension/LuaUtility  
EZFramework/XLuaExtension/LuaMessage/  
1. 热更新原理请自行Google，xlua只是“Unity下的lua编程解决方案”，“热更新”是下载替换lua代码的文本文件实现的效果而已。