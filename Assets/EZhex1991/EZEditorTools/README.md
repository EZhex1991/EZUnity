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
| `EZImageCapture` | 配置并执行 Game View 或目标 Camera 的截图。 |
| `EZPropertyPathViewer` | 查看对象的序列化属性和属性路径。 |
| `EZAssetReferenceViewer` | 查看资源依赖关系和场景引用。 |
| `EZFontReferenceViewer` | 查找引用指定字体的 UI 文本组件。 |
| `EZCorrespondingObjectViewer` | 查看对象及其对应的源对象和资源。 |
| `EZHierachyDiffChecker` | 对比两个 Transform 层级结构的差异。 |
| `EZTypeReflectionHelper` | 通过反射查看类型的字段、属性、方法和枚举值。 |
| `EZAnimatorControllerCombiner` | 通过编辑器窗口合并 Animator Controller 数据。 |
| `EZBundleManager` | 查看和管理 AssetBundle 构建结果及相关资源。 |

## 自定义资产和功能说明

| 自定义资产 | 功能 |
| --- | --- |
| `EZAssetListRenamer` | 保存资源列表和重命名规则，并执行批量重命名。 |
| `EZScriptStatistics` | 保存脚本统计配置，并生成脚本行数和作者贡献统计结果。 |
| `EZImageCapture` | 保存截图分辨率、纹理格式和输出路径配置，执行 Game View 或 Camera 截图。 |
| `EZBundleBuilder` | 保存 AssetBundle 构建配置，并执行 AssetBundle 构建。 |
| `EZPlayerBuilder` | 保存 Player 构建配置，并执行 Player 构建流程。 |

## 目录结构和脚本说明

以下目录树列出当前模块中的目录、脚本和程序集定义。为便于阅读，省略 Unity 自动生成的 `.meta` 文件。

```text
EZEditorTools/                                    — EZEditorTools 模块根目录。
├── README.md                                     — 模块说明文档。
└── Editor/                                       — 编辑器脚本和程序集定义目录。
    ├── EZGeneralMenuItems.cs                     — 注册 EZEditorTools 菜单，并整理窗口菜单入口。
    ├── EZMenuItemOrder.cs                        — 定义 EZEditorTools 菜单项的显示顺序。
    ├── EZhex1991.EZEditorTools.Editor.asmdef     — 定义仅面向 Unity Editor 的程序集。
    ├── Common/                                   — 编辑器内部复用的基础脚本和辅助实现。
    │   ├── EZCopyList.cs                         — 定义文件复制列表及其 Inspector 绘制器。
    │   ├── EZEditorGUIUtility.cs                 — 提供通用编辑器 GUI 绘制辅助方法。
    │   ├── EZEditorUtility.cs                    — 提供编辑器资源、类型和着色器查询辅助方法。
    │   ├── EZGameViewUtility.cs                  — 访问 Unity Game View，并读取或修改当前目标 Display。
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
    │   ├── EZTimePanel.cs                        — 查看和解析时间信息的窗口。
    │   └── EZTypeReflectionHelper.cs             — 查看类型反射信息的窗口。
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
