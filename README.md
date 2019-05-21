# EZUnity

## 常用组件

- [EZPhysicsBone](https://github.com/EZhex1991/EZPhysicsBone): 动态骨骼，效果参考来源于AssetStore上的DynamicBone。支持所有碰撞体；独立的材质"EZPhysicsBoneMaterial"存放参数，重用度高；代码可读性强；碰撞体可通过继承EZPhysicsBoneColliderBase进行自定义扩展。
- [EZAnimation](Assets/EZUnity/Demo/EZAnimation/README.md): 插值动画组件，有一个可以可视化编辑移动轨迹的EZTransformAnimation（支持贝塞尔曲线移动）。

## 功能菜单（EZUnity/..）

- Save Assets: Editor下部分资源的修改不会立马写入到磁盘，使用该菜单强制存档资源
- Renamer: 批量重命名工具窗口，支持正则式匹配，整理资源目录很方便
- Guid Generator: 生成Guid的工具窗口
- Asset Bundle Manager: [Obsolete]老的Bundle管理工具
- PlayerPrefs Editor: PlayerPrefs编辑工具，目前只有Win下可以用

## 附加设置 ([Edit/ProjectSettings/EZUnity/..](Assets/EZUnity/Demo/EZProjectSettings/README.md))

- EZEditorSettings: 开启某些选项能提高工作效率
- EZGraphicsSettings: 提供更加方便的界面来管理AlwaysIncludedShaders，其他功能开发中
- EZScriptSettings: 提供脚本模板的管理功能

## 附加资源 ([Asset/Create/EZUnity/...](Assets/EZUnity/Demo/CustomAssets/README.md))

- EZImageCapture: 截图工具
- EZPlayerBuilder: Build Player Pipeline，打包工具。
- EZBundleBuilder: Build Bundle Pipeline，AssetBundle构建工具。两种模式：  
  - EZMode: 偏向目录结构管理，设置bundle名称、路径和文件搜索条件去进行build。
  - Manager Mode: 偏向单个资源设置，会读取当前项目中Inspector中对单个资源的bundle设置。
- EZScriptStatistics: 用来统计代码量的工具，可以通过正则式来对代码文件进行分类统计，需要预先对代码模板进行设置。通过指定IncludePaths、ExcludePaths和正则式匹配来统计代码

## 资源生成器 ([Asset/Create/EZUnity/EZAssetGenerator/...](Assets/EZUnity/Demo/EZAssetGenerator/README.md))

- EZMeshGenerator:
  - EZPlaneGenerator: 用来生成自定义平面网格的工具
- EZTextureGenerator:
  - EZTextureChannelModifier: 用来调整图片通道的工具
  - EZGradientGenerator: 使用渐变和坐标曲线生成图片的工具

## 一些比较有意思的Shader ([Materials](Assets/EZUnity/Demo/Materials/README.md))

- Reflection: 反射
- Fur: 毛发
- Matcap: Material Capture
- ColorFilter: RGB转灰阶，HSV校色
- StripeCutoff: 条纹渐隐/渐出
- MultiTexture3x: 多贴图叠加
- Pattern: 坐标花纹
- DynamicFlame: 动态火焰
- DynamicLiquid: 动态液体效果（折射，色散）

## 基于XLua的逻辑热更方案（需要加宏'XLUA'启用）

- [XLuaExtension](Assets/EZUnity/XLuaExtension/README.md)
- [Example](Assets/Example/README.md)

-----

东西较杂，但所有代码都尽可能保证可读性，方便大家作为逻辑参考根据自己的需求简化代码或者重新实现  
QQ：361994819 欢迎留言
