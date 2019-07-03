# Examples

经常说到的“纯lua”逻辑并不是整个项目不用到任何C#，而是在已有的框架下，游戏机制相关逻辑都使用lua编写。

该示例用到的C#脚本主要在[EZUnity/XLuaExtension](../EZhex1991/EZUnity/XLuaExtension)目录下

---

## Example 00: LuaBehaviour

1. LuaBehaviour的基本使用；
1. 知识点：如何在lua和C#之前传递数据，LuaBehaviour之间如何相互调用；
1. 与xLua的官方例子区别很大，先了解如何用，然后再去看如何实现；
1. LuaBehaviour脚本位于Assets/EZhex1991/EZUnity/XLuaExtension/LuaBehaviour目录下，LuaBehaviour继承于LuaInjector，LuaInjector实现变量的注入，LuaBehaviour实现Lua的脚本的启动；
1. lua脚本位于[Assets/Example/Script_Lua/LuaBehaviour](Script_Lua/LuaBehaviour)目录下；

## Example 01: Lucky Ball

1. 很普通的一个Demo，点击屏幕投球；
1. 知识点：如何为LuaBehaviour绑定Update、OnTrigger之类的MonoBehaviour消息；
1. MonoBehaviour消息已经进行了分类封装，详见Assets/EZhex1991/EZUnity/XLuaExtension/LuaMessage；
1. lua脚本位于[Assets/Example/Script_Lua/LuckBall](Script_Lua/LuckyBall)目录下；
1. 脚本外其它资源在[Assets/Example/01_LuckyBall](01_LuckyBall)目录下；

## Example 02: Space Shooter

该示例以Unity官方Tutorial - Space Shooter为模板，使用lua重写（xLua方案），运行该示例前请注意以下几点。

1. 本人尽可能保证lua代码的结构与C#代码结构相似，方便初学者对比学习，甚至还保留了原示例的部分bug（比如物体没销毁啥的），**该示例并不官方，仅供初学者参考**；
1. **注意，Tags和Layers只能在Editor下添加，所以在热更中使用并不方便(个人在5.x下没找到其他方式)。如果报错显示找不到Tag，请自行添加，Tag0:"Enemy", Tag1:"Bonudary"**；
1. lua脚本位于[Assets/Example/Script_Lua/SpaceShooter](Script_Lua/SpaceShooter)目录下；
1. prefab和场景在[Assets/Example/02_SpaceShooter/_Complete-Lua](02_SpaceShooter/_Complete-Lua)目录下；
