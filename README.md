# EZUnity

**部分模块已从该目录中移除**

- [EZSoftBone](https://github.com/EZhex1991/EZSoftBone): 柔性物体（头发/尾巴/胸部/裙子）模拟插件。支持所有碰撞体；独立的材质"EZSoftBoneMaterial"存放参数，重用度高；代码可读性强；碰撞体可通过继承EZSoftBoneColliderBase进行自定义扩展。
- [EZTextureProcessor](https://github.com/EZhex1991/EZTextureProcessor): 图片处理工具，可在Unity中进行参数化图片生成/图片拼合/图片通道调整/描边/模糊/扭曲等效果。
- [EZAnimation](https://github.com/EZhex1991/EZAnimation): 插值动画组件，有一个可以可视化编辑移动轨迹的EZTransformAnimation（支持贝塞尔曲线移动）。
- [EZPostProcessing](https://github.com/EZhex1991/EZPostProcessing): 基于PostProcessing Stack V2开发的一系列后处理效果。
- [XLuaExtension](https://github.com/EZhex1991/XLuaExtension): 在腾讯XLua的基础上做的一些非官方示例和扩展工具.

## 功能菜单（EZUnity/..）

- Save Assets: Editor下部分资源的修改不会立马写入到磁盘，使用该菜单强制存档资源
- Renamer: 批量重命名工具窗口，支持正则式匹配，整理资源目录很方便
- Material Optimizer: 用来查看和删除材质中的keywords和多余的properties
- Guid Generator: 生成Guid的工具窗口
- Color Blender: 颜色混合计算器
- Asset Bundle Manager: [Obsolete]老的Bundle管理工具
- PlayerPrefs Editor: PlayerPrefs编辑工具，目前只有Win下可以用
- Property Path Viewer: 可以查看和修改SerializedProperty，所有property的label都会显示路径
- Type Reflection Helper: 通过反射查看无法访问的类，根据成员的名称可以猜到很多有用的东西

## 附加设置 ([Edit/ProjectSettings/EZUnity/..](Assets/EZhex1991/EZUnity/Demo/EZProjectSettings/README.md))

Unity2018.3以上版本在ProjectSettings窗口中，**低版本在Preferences窗口中**

- EZEditorSettings: 开启某些选项能提高工作效率
- EZGraphicsSettings: 提供更加方便的界面来管理AlwaysIncludedShaders，其他功能开发中
- EZScriptSettings: 提供脚本模板的管理功能

## 附加资源 ([Asset/Create/EZUnity/...](Assets/EZhex1991/EZUnity/Demo/CustomAssets/README.md))

- EZImageCapture: 截图工具
- EZPlayerBuilder: Build Player Pipeline，打包工具。
- EZBundleBuilder: Build Bundle Pipeline，AssetBundle构建工具。两种模式：  
  - EZMode: 偏向目录结构管理，设置bundle名称、路径和文件搜索条件去进行build。
  - Manager Mode: 偏向单个资源设置，会读取当前项目中Inspector中对单个资源的bundle设置。
- EZScriptStatistics: 用来统计代码量的工具，可以通过正则式来对代码文件进行分类统计，需要预先对代码模板进行设置。通过指定IncludePaths、ExcludePaths和正则式匹配来统计代码

## 一些比较有意思的Shader ([Materials](Assets/EZhex1991/EZUnity/Demo/Materials/README.md))

- DynamicFlame: 动态火焰
- DynamicFluid: 动态液体效果（折射，色散）
- WobblingLiquid: 仿物理液体（血瓶）
- Fur: 毛发
- Reflection: 反射
- Matcap: Material Capture
- ColorFilter: RGB转灰阶，HSV校色
- StripeCutoff: 条纹渐隐/渐出
- MultiTexture3x: 多贴图叠加
- Pattern: 程序化纹理

-----

东西较杂，但所有代码都尽可能保证可读性，方便大家作为逻辑参考根据自己的需求简化代码或者重新实现  
QQ：361994819 欢迎留言
