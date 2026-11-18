# EZEditorTools

EZEditorTools 是一个独立的 Unity 编辑器工具模块，提供通用的编辑器窗口、资源辅助功能和编辑器基础能力。
代码位于 `EZhex1991.EZEditorTools.Editor` 程序集中，仅面向 Unity Editor 平台。

## 窗口和功能说明

所有窗口均可从 Unity 编辑器的 `EZEditorTools` 根菜单访问。

| 窗口 | 功能 |
| --- | --- |
| `EZRenamerWindow` | 批量重命名选中的资源或对象。 |
| `EZGuidGenerator` | 生成系统 GUID 和 Unity GUID。 |
| `EZRegexTester` | 测试正则表达式，并显示匹配项、分组和捕获结果。 |
| `EZColorBlender` | 预览常见的颜色混合运算结果。 |
| `EZPlayerPrefsEditor` | 查看、修改和删除 PlayerPrefs 数据。 |
| `EZTimePanel` | 查看和解析当前时间及其各类格式化结果。 |
| `EZShaderKeywordManager` | 查看和管理材质及 Shader 的关键字状态。 |
| `EZMaterialOptimizer` | 批量优化选中材质的属性，并复制材质属性。 |
| `EZImageCapture` | 配置并执行 Game View 或目标 Camera 的截图。 |
| `EZPropertyPathViewer` | 查看对象的序列化属性和属性路径。 |
| `EZAssetReferenceViewer` | 查看资源依赖关系和场景引用。 |
| `EZFontReferenceViewer` | 查找引用指定字体的 UI 文本组件。 |
| `EZCorrespondingObjectViewer` | 查看对象及其对应的源对象和资源。 |
| `EZHierachyDiffChecker` | 对比两个 Transform 层级结构的差异。 |
| `EZTypeReflectionHelper` | 通过反射查看类型的字段、属性、方法和枚举值。 |
| `EZAnimatorControllerCombiner` | 通过编辑器窗口合并 Animator Controller 数据。 |
| `EZBundleManager` | 查看和管理 AssetBundle 构建结果及相关资源。 |
| `Project Settings` | 管理 EZEditorTools 提供的编辑器、脚本、资源导入器和 Graphics 项目设置。 |
| `EZTransformContextMenu` | 为 Transform Inspector 提供重置和对齐相关的右键菜单。 |
| `Save Assets` | 保存当前项目资源。 |
| `Refresh AssetDatabase` | 刷新 Unity AssetDatabase。 |
| `Open Persistent Folder` | 打开 Unity 的持久化数据目录。 |
| `Clear Persistent Folder` | 清空 Unity 的持久化数据目录。 |

## 自定义资产和功能说明

| 自定义资产 | 功能 |
| --- | --- |
| `EZAssetListRenamer` | 保存资源列表和重命名规则，并执行批量重命名。 |
| `EZScriptStatistics` | 保存脚本统计配置，并生成脚本行数和作者贡献统计结果。 |
| `EZImageCapture` | 保存截图分辨率、纹理格式和输出路径配置，执行 Game View 或 Camera 截图。 |
| `EZBundleBuilder` | 保存 AssetBundle 构建配置，并执行 AssetBundle 构建。 |
| `EZPlayerBuilder` | 保存 Player 构建配置，并执行 Player 构建流程。 |
| `EZMaterialMap` | 保存模型材质名称与材质资源的映射，并将映射应用到模型导入器。 |

## 目录结构和脚本说明

以下目录树列出当前模块中的目录、脚本和程序集定义。为便于阅读，省略 Unity 自动生成的 `.meta` 文件。

```text
EZEditorTools/                                    — EZEditorTools 模块根目录。
├── README.md                                     — 模块说明文档。
└── Editor/                                       — 编辑器脚本和程序集定义目录。
    ├── EZEditorToolsMenuItems.cs                 — EZEditorTools 总菜单入口，注册窗口和通用编辑器操作。
    ├── EZMenuItemOrder.cs                        — 定义 EZEditorTools 菜单项的显示顺序。
    ├── EZProjectSettingsMenuItems.cs              — 注册 EZEditorTools 项目设置入口和 Settings Provider。
    ├── EZhex1991.EZEditorTools.Editor.asmdef     — 定义仅面向 Unity Editor 的程序集。
    ├── Common/                                   — 编辑器内部复用的基础脚本和辅助实现。
    │   ├── EZCopyList.cs                         — 定义文件复制列表及其 Inspector 绘制器。
    │   ├── EZEditorGUIUtility.cs                 — 提供通用编辑器 GUI 绘制辅助方法。
    │   ├── EZEditorUtility.cs                    — 提供编辑器资源、类型和着色器查询辅助方法。
    │   ├── EZGameViewUtility.cs                  — 访问 Unity Game View，并读取或修改当前目标 Display。
    │   ├── MaterialExt.cs                         — 提供材质关键字设置、禁用和查询的扩展方法。
    │   ├── EZShaderGUIUtility.cs                 — 提供材质 Shader 属性、关键字和额外属性的绘制及优化方法。
    │   └── TransformExt.cs                       — 提供 Transform 扩展方法，包括层级 Transform 的 TRS 复制。
    ├── General/                                  — 存放一般用途的独立编辑器窗口。
    │   ├── EZAnimatorControllerCombiner.cs       — 合并 Animator Controller 数据的窗口。
    │   ├── EZAssetReferenceViewer.cs             — 查看资源依赖关系和场景引用的窗口。
    │   ├── EZColorBlender.cs                     — 预览颜色混合运算结果的窗口。
    │   ├── EZCorrespondingObjectViewer.cs        — 查看对象及其对应源对象和资源的窗口。
    │   ├── EZFontReferenceViewer.cs              — 查找 UI 文本组件字体引用的窗口。
    │   ├── EZGuidGenerator.cs                    — 生成系统 GUID 和 Unity GUID 的窗口。
    │   ├── EZHierachyDiffChecker.cs              — 对比两个 Transform 层级结构的窗口。
    │   ├── EZPlayerPrefsEditor.cs                — 查看、修改和删除 PlayerPrefs 的窗口。
    │   ├── EZPropertyPathViewer.cs               — 查看序列化属性和属性路径的窗口。
    │   ├── EZRegexTester.cs                      — 测试正则表达式的窗口。
    │   ├── EZShaderKeywordManager.cs             — 查看和管理材质及 Shader 关键字的窗口。
    │   ├── EZMaterialOptimizer.cs                — 批量优化选中材质属性的窗口。
    │   ├── EZTimePanel.cs                        — 查看和解析时间信息的窗口。
    │   ├── EZTransformContextMenu.cs              — 为 Transform Inspector 提供重置和对齐右键菜单。
    │   └── EZTypeReflectionHelper.cs             — 查看类型反射信息的窗口。
    ├── MaterialMap/                              — 共同实现材质映射资产及其 Inspector 的脚本集合。
    │   ├── EZButtonPropertyAttribute.cs          — 定义可在 Inspector 中调用对象方法的按钮特性。
    │   ├── EZButtonPropertyDrawer.cs             — 绘制 `EZButtonPropertyAttribute` 对应的 Inspector 按钮。
    │   ├── EZMapAsset.cs                         — 提供可序列化键值映射资产的基础实现。
    │   ├── EZMapAssetEditor.cs                   — 提供键值映射资产的通用 Inspector 和列表绘制。
    │   ├── EZMaterialMap.cs                      — 保存模型材质名称与材质资源的映射资产。
    │   └── EZMaterialMapEditor.cs                — 绘制 `EZMaterialMap` 的映射、外部对象和材质信息。
    ├── ProjectSettings/                           — 提供项目级编辑器设置、脚本处理、资源导入和 Graphics 设置。
    │   ├── _EZProjectSettings.cs                  — 提供项目设置单例的加载和保存基础实现。
    │   ├── EZEditorSettings.cs                    — 保存编辑器设置，并扩展 Hierarchy 窗口显示。
    │   ├── EZScriptSettings.cs                    — 保存脚本扩展名、模板和文件头替换规则。
    │   ├── EZScriptSettingsEditor.cs              — 绘制脚本设置 Inspector，并管理脚本模板。
    │   ├── EZScriptProcessor.cs                   — 在创建脚本或文本资产时执行文件头替换。
    │   ├── EZAssetImporterManager.cs              — 保存资源导入器 Preset 与目录关联配置。
    │   ├── EZAssetImporterManagerInspector.cs     — 绘制资源导入器配置 Inspector。
    │   ├── EZAssetPostprocessor.cs                — 根据导入路径和配置自动应用资源导入 Preset。
    │   └── EZGraphicsSettings.cs                  — 在 Project Settings 中管理项目 Graphics 设置。
    ├── Renamer/                                  — 共同实现资源重命名工具的脚本集合。
    │   ├── EZAssetListRenamer.cs                 — 保存资源列表和重命名规则的编辑器资产。
    │   ├── EZAssetListRenamerEditor.cs           — `EZAssetListRenamer` 的 Inspector。
    │   └── EZRenamerWindow.cs                    — 通用重命名窗口。
    ├── ScriptStatistics/                         — 共同实现脚本统计工具的脚本集合。
    │   ├── EZScriptStatisticResult.cs            — 保存单次脚本统计结果的子资产。
    │   ├── EZScriptStatisticResultEditor.cs      — `EZScriptStatisticResult` 的 Inspector。
    │   ├── EZScriptStatistics.cs                 — 保存脚本统计配置的编辑器资产。
    │   └── EZScriptStatisticsEditor.cs           — `EZScriptStatistics` 的 Inspector 和统计执行界面。
    ├── Builder/                                  — 共同实现 AssetBundle 和 Player 构建工具的脚本集合。
    │   ├── EZBuilderOptions.cs                   — 定义构建相关选项。
    │   ├── EZBundleBuilder.cs                    — 保存并执行 AssetBundle 构建配置。
    │   ├── EZBundleBuilderEditor.cs              — `EZBundleBuilder` 的 Inspector。
    │   ├── EZBundleManager.cs                    — AssetBundle 管理窗口。
    │   ├── EZPlayerBuilder.cs                    — 保存并执行 Player 构建配置。
    │   └── EZPlayerBuilderEditor.cs              — `EZPlayerBuilder` 的 Inspector。
    └── ImageCapture/                             — 共同实现截图工具的脚本集合。
        ├── EZImageCapture.cs                     — 保存截图配置并执行截图。
        └── EZImageCaptureEditor.cs               — `EZImageCapture` 的 Inspector 和操作界面。
```
