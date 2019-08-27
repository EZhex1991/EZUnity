# EZTextureProcessor

参数化图片处理工具

输出设置：（注：输出格式的修改不会影响导入格式）

- Output Resolution: 输出图片的分辨率
- Output Format: 输出图片的通道格式
- Output Encoding: 输出图片的编码格式
- Output Texture: 绑定的输出图片（输出时覆盖图片内容，不会更改导入选项，如果不指定则新建图片文件并绑定）
- Cooresponding Generator: 需要响应的其他图片生成器（执行当前文件后会自动执行响应文件，如果响应文件也有响应文件，则顺序执行）

## EZGaussianLutGenerator

高斯查找表

- Texture Type
  - Wave: 波浪图
  - Lut1D: U方向的1D查找表
  - Lut2D: 2D查找表
- Range: 取值范围
- Sigma: 标准差

![EZGaussianLutGenerator](.SamplePicture/EZGaussianLutGenerator.png)

## EZGradient1DTextureGenerator

U方向，做Gradient的映射

- Gradient: 渐变
- Coordinate Curve: 取值与渐变的映射关系

![EZGradient1DTextureGenerator](.SamplePicture/EZGradient1DTextureGenerator.png)

## EZGradient2DTextureGenerator

渐变生成图片，配合XY轴的曲线可以生成很多复杂图案

UV方向的取值到Gradient的映射

- Gradient: 渐变
- Gradient Curve: 取值与渐变的映射关系
- Coordinate Mode: UV坐标的取值方式
  - X: U
  - Y: V
  - AdditiveXY: (U + V) / 2
  - MultiplyXY: U * V
  - DifferenceXY: Abs(U - V)
  - Radial: Sqrt((U - 0.5) ^ 2 + (V - 0.5) ^ 2)) * 2
  - Angle: Mathf.Atan2(V - 0.5, U - 0.5) / Mathf.PI * 0.5f;
- Coordinate Curve U: U坐标变化曲线
- Coordinate Curve V: V坐标变化曲线

![EZGradient2DTextureGenerator](.SamplePicture/EZGradient2DTextureGenerator.png)

## EZPerlinNoiseTextureGenerator

柏林噪声图片生成

- Output Curve: 柏林函数的输出与颜色输出的对应曲线
- Density: 柏林噪声的密度

![EZPerlinNoiseTextureGenerator](.SamplePicture/EZPerlinNoiseTextureGenerator.png)

## EZRandomNoiseTextureGenerator

随机噪点生成图片

- Colored: 带色噪点
- Output Curve: 随机数输出与颜色输出的对应曲线

![EZRandomNoiseTextureGenerator](.SamplePicture/EZRandomNoiseTextureGenerator.png)

## EZWaveTextureGenerator

利用曲线来生成波浪形状

- Wave Shape: 波浪形状曲线
- Antialiasing: 抗锯齿

![EZWaveTextureGenerator](.SamplePicture/EZWaveTextureGenerator.png)

## EZTextureBlurProcessor

对图片进行模糊处理

- Shader: 使用的模糊Shader
- Input Texture: 需要模糊的图片
- Blur Weight Texture: 模糊使用的权重查找表
- Blur Radius: 模糊范围

![EZTextureBlurProcessor](.SamplePicture/EZTextureBlurProcessor.png)

## EZTextureChannelModifier

图片通道调整（交换通道、提取单通道、调整特定通道曲线）

- Input Texture: 基础图片，默认输出白色
- Value Curve: 基础图片的调整曲线
- Overrides: 各通道单独指定输入输出
  - Texture: 该通道的输入图片
  - Channel: 输入图片的输入通道
  - Curve: 输入通道的输出曲线

*批处理模式下(Batch Mode Window)，会根据参数设置直接对当前选中的图片进行处理（使用前请自行备份相关资源）*

![EZTextureChannelModifier](.SamplePicture/EZTextureChannelModifier.png)

## EZTextureCombiner

整合图片，小图的尺寸需要相同

![EZTextureCombiner](.SamplePicture/EZTextureCombiner.png)
