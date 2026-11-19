# EZUnity

EZUnity 是项目开发基础代码和示例集合，提供通用 Runtime 组件、编辑器辅助脚本、Shader 以及可直接参考的 Demo。

EZUnity 本身不作为独立程序集或 Package 发布，项目可以按需直接使用其中的脚本。部分功能已经拆分为独立 Package，EZUnity 不依赖这些 Package。

所有代码已尽可能保持高可读性，方便作为逻辑参考学习或AI使用，MIT许可证，可根据个人项目需求进行简化或重新实现。

## 已拆分的独立 Package

这些功能已经从 EZUnity 中迁移，并在独立仓库中维护和发布：

| Package | 说明 |
| --- | --- |
| [EZSoftBone](https://github.com/EZhex1991/EZSoftBone) | 柔性物体模拟工具，支持头发、尾巴、胸部、裙子等对象，并支持自定义碰撞体和风力模拟。 |
| [EZEditorTools](https://github.com/EZhex1991/EZEditorTools) | Unity 编辑器窗口、资源查看和处理工具、重命名、截图、脚本统计、构建工具和材质映射工具。包含 Renamer、Material Optimizer、Guid Generator、Color Blender、PlayerPrefs Editor、Property Path Viewer 和 Type Reflection Helper 等功能。 |
| [EZAssetGenerator](https://github.com/EZhex1991/EZAssetGenerator) | 网格和纹理资源生成工具，包括 Plane、Box、UV Sphere 网格生成，以及渐变、噪声、LUT、纹理拼合、通道调整、描边、模糊和扭曲等纹理功能。 |
| [EZProjectSettings](https://github.com/EZhex1991/EZProjectSettings) | Unity Project Settings 编辑器扩展，包括编辑器设置、Graphics 设置和脚本模板管理。 |
| [EZAnimation](https://github.com/EZhex1991/EZAnimation) | 无状态插值动画组件，包含 Transform 和 RectTransform 动画、Graphic/Renderer 属性动画、Segment、Transform 路径以及 Timeline Track 和 Clip。 |
| [EZPostProcessing](https://github.com/EZhex1991/EZPostProcessing) | 基于 Unity Post-Processing Stack 的 Color、Depth、Distortion 和 Glow 自定义后处理效果。 |
| [XLuaExtension](https://github.com/EZhex1991/XLuaExtension) | 基于腾讯 XLua 的非官方示例和扩展工具 |

## 迁移状态

- `Runtime/Rendering` 与 `EZPostProcessing` 的实现不同：前者是旧式图像效果组件，后者依赖 Unity Post-Processing Stack，暂不视为重复功能。
- `Runtime/Playables` 保留的是材质属性和 Transform 相关 Playable，并非 `EZAnimation` 的 Segment 动画组件。

## EZUnity 当前内容

### Runtime

- `CollectionAsset`：List、Map 等可序列化集合资产。
- `Common`：通用工具、扩展方法、动画曲线处理、单例和基础属性。
- `Framework`：应用、数据库、事件和输入等基础框架组件。
- `Miscellaneous`：路径、UI、计时器、相机、网格和其他通用组件。
- `Mixer`：基础混合器组件。
- `Platform`：Android 和 Windows 平台相关辅助代码。
- `Playables`：材质属性、Transform 约束和 Keyframe 等 Playable 组件。
- `Rendering`：基于旧式 `OnRenderImage` 的图像效果组件。
- `UniSDK`：第三方 SDK 的接口和基础适配层。

### Shaders

包含通用材质、Unlit 效果、表面效果、反射、描边、阴影以及动态材质 Shader，可参考 [`Demo/Materials`](Assets/EZhex1991/EZUnity/Demo/Materials/README.md) 中的使用示例。

当前较有代表性的 Shader 和材质效果包括：

- `DynamicFlame`：动态火焰。
- `DynamicFluid`：动态液体效果，包括折射和色散。
- `WobblingLiquid`：仿物理液体效果，例如血瓶。
- `Fur`：毛发效果。
- `Reflection`：反射效果。
- `Matcap`：Material Capture 材质效果。
- `ColorFilter`：RGB 转灰阶和 HSV 校色。
- `StripeCutoff`：条纹渐隐和渐出。
- `MultiTexture3x`：多贴图叠加。
- `Pattern`：程序化图案。

### Demo

- `AStar`：A* 寻路示例。
- `HanoiTower`：汉诺塔示例。
- `Materials`：EZUnity Shader 和材质示例。
- `Presets`：粒子、材质预览和物理材质预设。
- `Textures`：Shader 示例使用的纹理资源。

## 联系方式

- QQ：361994819
- 微信：EZhex1991
- 邮箱：ezhex1991@163.com
