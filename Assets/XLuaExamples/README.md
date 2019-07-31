# XLuaExamples

经常说到的“纯lua”逻辑并不是整个项目不用到任何C#，而是在已有的框架下，游戏机制相关逻辑都使用lua编写。该示例用到的C#脚本主要在[EZhex1991/EZUnity/XLuaExtension](../EZhex1991/EZUnity/XLuaExtension)目录下。

该示例分两个部分。

## 常见问题的示例

0. [CustomLoader](00_CustomLoader) - “lua只能放Resources？只能用‘.txt’后缀？”
0. [Dictionary](01_Dictionary) - “词典不可以用？泛型不能用？”
0. [InexpliciteOverload](02_InexpliciteOverload) - “Physics.Raycat返回值错误？out和ref不能用？”
0. [IsNilOrNull](03_IsNilOrNull) - “销毁了不是nil？GetComponent拿不到nil？”
0. [Coroutine](04_Coroutine) - “协程不能用？协程不能停？”
0. [DelegateAndEvent](05_DelegateAndEvent) - “delegate不能？event不能用？ButtonClickedEvent是个class，不能用？”
0. [LuaMessage](06_LuaMessage) - “MonoBehaviour的Start方法不能在lua里写？”
0. [FromLua](07_FromLua) - “不能再C#这边调用lua的东西？”
0. [RawObject](08_RawObject) - “int传过去成了long？byte[]传过去成了string？”
0. [DOTween](09_DOTween) - “DOTween不能用？扩展方法不能用？”
0. [Hotfix](10_Hotfix) - “Hotfix不能用？官方示例报错？HotFix不能调用原来的方法？”

## Tutorial（相对完整的demo）

包括lua间的交互，如何设计Injector来减少Find和GetComponent并方便策划配置，如何使用工具来简化bundle的构建，lua代码的管理等。

### 00_LuaBehaviour

1. LuaBehaviour的基本使用；
1. 知识点：如何在lua和C#之前传递数据，LuaBehaviour之间如何相互调用；
1. 与xLua的官方例子区别很大，先了解如何用，然后再去看如何实现；
1. LuaBehaviour脚本位于[EZhex1991/EZUnity/XLuaEntension/Runtime/LuaBehaviour](../EZhex1991/EZUnity/XLuaExtension/Runtime/LuaBehaviour)目录下，LuaBehaviour继承于LuaInjector，LuaInjector中实现了变量的注入，LuaBehaviour中实现了Lua的脚本的启动；
1. lua脚本位于[Tutorial/Script_Lua/LuaBehaviour](Tutorial/Script_Lua/LuaBehaviour)目录下；

### 01_LuckyBall

1. 鼠标点击屏幕投球；
1. 知识点：如何为LuaBehaviour绑定Update、OnTrigger之类的MonoBehaviour消息；
1. MonoBehaviour消息已经进行了分类封装，详见[EZhex1991/EZUnity/XLuaExtension/Runtime/LuaMessage](../EZhex1991/EZUnity/XLuaExtension/Runtime/LuaMessage)；
1. lua脚本位于[Tutorial/Script_Lua/LuckBall](Tutorial/Script_Lua/LuckyBall)目录下；
1. 脚本外其它资源在[Tutorial/01_LuckyBall](Tutorial/01_LuckyBall)目录下；

### 02_SpaceShooter

该示例以Unity官方Tutorial - Space Shooter为模板，使用lua重写（xLua方案），运行该示例前请注意以下几点。

1. 本人尽可能保证lua代码的结构与C#代码结构相似，方便初学者对比学习，甚至还保留了原示例的部分bug（比如物体没销毁啥的）；
1. **注意，Tags和Layers只能在Editor下添加，所以在热更中使用并不方便(个人在5.x下没找到其他方式)。如果报错显示找不到Tag，请自行添加，Tag0:"Enemy", Tag1:"Bonudary"**；
1. lua脚本位于[Tutorial/Script_Lua/SpaceShooter](Tutorial/Script_Lua/SpaceShooter)目录下；
1. prefab和场景均使用EZLuaBehaviour代替了原本的C#脚本，放在[Tutorial/02_SpaceShooter/_Complete-Lua](Tutorial/02_SpaceShooter/_Complete-Lua)目录下；
