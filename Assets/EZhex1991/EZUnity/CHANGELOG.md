# Changelog

## [Unreleased]

### Added

- `EZTextureCombiner`: use to combine textures.
- `EZCorrespondingObjectViewer`: View corresponding objects of Nested Prefab, available on Unity2018.3 or newer.
- Shader "EZUnity/Unlit/LaserProcessed": Different color tint with different view direction.

### Changed

- namespace change to EZhex1991.EZUnity
- folder structure changed

## [0.0.8] - 2019-04-18

### Changed

- File structure changed, in order to match the [Unity Package Layout](https://docs.unity3d.com/2019.1/Documentation/Manual/cus-layout.html).
- If you're using Unity2018.2 or higher, and don't need the XLuaExtension module, it is suggested to place the EZUnity folder to "ProjectRoot/Packages/EZUnity".

## [0.0.7] - 2019-02-22

### Changed

- Use Nested Prefab to create examples, that means Unity version 2018.3 or newer is required for some example scenes.
- It it suggested to use ".Net4xEquivalent" on "RuntimeVersion" setting.
- Shader name changed, now it's all under "EZUnity/...".

## [0.0.6] - 2019-01-26

### Added

- `EZPhysicsBone`: An implementation of dynamic linked bones.

### Changed

- `EZLuaInjector`(`EZPropertyList`) now has two representations - List or Map. you should use nested list instead of '#' in names to define lists.
- All the Editor scripts has been moved to one folder.

## [0.0.5] - 2018-10-16

### Changed

- Namespace `EZFramework` change to `EZUnity.Framework`.
- Namespace `EZXLuaExtension` change to `EZUnity.XLuaExtension`. (Add define symbol `XLUA` to enable it)
- Custom menu items path has been changed, now it's all under "EZUnity/..."

### Removed

- Namespace `EZUnityEditor` has been removed, most related scripts changed namespace to `EZUnity`.
- Embedded XLua has been removed. (You can download it from https://github.com/Tencent/xLua)
- Embedded DOTween has been removed. (You can download it from Unity Asset Store)

## [0.0.4] - 2018-04-04

### Added

- Uploaded ome meta files that been deleted accidentally.

### Changed

- Some changes for 2017.4.0f1 compatibility.

## [0.0.3] - 2018-03-28

### Changed

- Some changes for 2017.3.0f3 compatibility.
- File structure changed.

## [0.0.2] - 2017-09-19

### Changed

- Namespace `EZUnityTools` has been changed to `EZUnityEditor`.
- File structure has been changed.

## [0.0.1] - 2017-08-17

### Added

- First Upload.
