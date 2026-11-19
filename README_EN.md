# EZUnity

[中文说明](README.md)

EZUnity is a collection of foundational project code and examples. It provides general-purpose runtime components, editor helper scripts, shaders, and demos that can be used as direct references.

EZUnity is not published as a standalone assembly or Package. Projects can use its scripts directly as needed. Some features have been migrated to independent Packages, and EZUnity does not depend on those Packages.

The code is kept as readable as possible so it can be used for learning, logic reference, or AI-assisted development. It is released under the MIT License and can be simplified or reimplemented for individual projects.

## Independent Packages

The following features have been migrated from EZUnity and are maintained and released in independent repositories:

| Package | Description |
| --- | --- |
| [EZSoftBone](https://github.com/EZhex1991/EZSoftBone) | A soft-body simulation tool supporting objects such as hair, tails, breasts, and skirts, with customizable colliders and wind simulation. |
| [EZEditorTools](https://github.com/EZhex1991/EZEditorTools) | Unity editor windows, asset inspection and processing tools, renaming, image capture, script statistics, build tools, and material mapping tools. |
| [EZAssetGenerator](https://github.com/EZhex1991/EZAssetGenerator) | Mesh and texture generation tools, including Plane, Box, and UV Sphere meshes, gradients, noise, LUTs, texture compositing, channel adjustment, outlines, blur, and distortion. |
| [EZProjectSettings](https://github.com/EZhex1991/EZProjectSettings) | Unity Project Settings editor extensions, including editor settings, Graphics settings, and script template management. |
| [EZAnimation](https://github.com/EZhex1991/EZAnimation) | Stateless interpolation animation components, including Transform and RectTransform animation, Graphic/Renderer property animation, segments, Transform paths, and Timeline tracks and clips. |
| [EZPostProcessing](https://github.com/EZhex1991/EZPostProcessing) | Custom Color, Depth, Distortion, and Glow post-processing effects based on the Unity Post-Processing Stack. |
| [XLuaExtension](https://github.com/EZhex1991/XLuaExtension) | Unofficial examples and extension tools based on Tencent's XLua. |

The samples under `Assets` are retained for example editing and release preparation. Their sources and versions are recorded in [`Assets/Samples/README.md`](Assets/Samples/README.md).

## Migration Status

- `Runtime/Rendering` differs from `EZPostProcessing`: the former contains legacy image-effect components, while the latter depends on the Unity Post-Processing Stack. They are not currently considered duplicate functionality.
- `Runtime/Playables` retains Playables related to material properties and Transform components; it is not the Segment animation system in `EZAnimation`.

## Current EZUnity Contents

### Runtime

- `CollectionAsset`: Serializable collection assets such as List and Map.
- `Common`: General utilities, extension methods, animation-curve processing, singletons, and basic attributes.
- `Framework`: Foundational application, database, event, and input components.
- `Miscellaneous`: Paths, UI, timers, cameras, grids, and other general-purpose components.
- `Mixer`: Basic mixer components.
- `Platform`: Android- and Windows-related helper code.
- `Playables`: Playable components for material properties, Transform constraints, Keyframes, and related features.
- `Rendering`: Image-effect components based on the legacy `OnRenderImage` workflow.
- `UniSDK`: Interfaces and basic adapters for third-party SDKs.

### Shaders

The shader collection includes general-purpose materials, Unlit effects, surface effects, reflections, outlines, shadows, and dynamic material shaders. Usage examples are available in [`Demo/Materials`](Assets/EZhex1991/EZUnity/Demo/Materials/README.md).

Representative shaders and material effects include:

- `DynamicFlame`: Dynamic fire.
- `DynamicFluid`: Dynamic fluid effects, including refraction and dispersion.
- `WobblingLiquid`: A physically inspired liquid effect, such as liquid in a blood bottle.
- `Fur`: Fur effects.
- `Reflection`: Reflection effects.
- `Matcap`: Material Capture effects.
- `ColorFilter`: RGB-to-grayscale conversion and HSV color correction.
- `StripeCutoff`: Striped fade and cutoff effects.
- `MultiTexture3x`: Multi-texture blending.
- `Pattern`: Procedural patterns.

### Demo

- `AStar`: A* pathfinding example.
- `HanoiTower`: Tower of Hanoi example.
- `Materials`: EZUnity shader and material examples.
- `Presets`: Particle, material preview, and physics material presets.
- `Textures`: Textures used by the shader examples.

## Contact

- QQ: 361994819
- WeChat: EZhex1991
- Email: ezhex1991@163.com
