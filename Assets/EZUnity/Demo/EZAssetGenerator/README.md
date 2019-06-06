# EZAssetGenerator

根据一些简单的参数来生成资源的工具

## EZGradientGenerator

利用渐变来生成图片的工具，配合XY轴的曲线可以生成很多复杂图案

![EZGradientGenerator](.SamplePicture/EZGradientGenerator.png)

## EZNoiseGenerator

噪点图片生成工具

![EZNoiseGenerator](.SamplePicture/EZNoiseGenerator.png)

## EZPerlinNoiseGenerator

柏林噪声图片生成工具

![EZPerlinNoiseGenerator](.SamplePicture/EZPerlinNoiseGenerator.png)

## EZTextureChannelModifier

图片通道调整工具（交换通道、提取单通道、调整特定通道曲线）

- 普通模式下会根据ReferenceTexture和参数设置来生成并覆盖TargetTexture（如果不指定TargetTexture则会生成新的图片资源）
- 批处理模式下(Batch Mode Window)，会根据参数设置直接对当前选中的图片进行处理（使用前请自行备份相关资源）

![EZTextureChannelModifier](.SamplePicture/EZTextureChannelModifier.png)

## EZTextureCombiner

图片合并工具

![EZTextureCombiner](.SamplePicture/EZTextureCombiner.png)

## EZPlaneGenerator

用来生成自定义细分精度的Plane(Quad)  
*预览界面用到的材质是"VR/SpatialMapping/Wireframe"，目前发现部分版本该材质不可用，不影响网格Mesh的生成*

![EZPlaneGenerator](.SamplePicture/EZPlaneGenerator.png)
