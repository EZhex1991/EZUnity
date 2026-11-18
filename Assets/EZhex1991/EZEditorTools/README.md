# EZEditorTools

EZEditorTools 是一个独立的 Unity 编辑器工具模块，提供通用的编辑器窗口、资源辅助功能和编辑器基础能力。

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
| `EZPropertyPathViewer` | 查看对象的序列化属性和属性路径。 |
| `EZAssetReferenceViewer` | 查看资源依赖关系和场景引用。 |
| `EZFontReferenceViewer` | 查找引用指定字体的 UI 文本组件。 |
| `EZCorrespondingObjectViewer` | 查看对象及其对应的源对象和资源。 |
| `EZHierachyDiffChecker` | 对比两个 Transform 层级结构的差异。 |
| `EZTypeReflectionHelper` | 通过反射查看类型的字段、属性、方法和枚举值。 |
| `EZAnimatorControllerCombiner` | 通过编辑器窗口合并 Animator Controller 数据。 |

## 目录结构和脚本说明

```text
EZEditorTools/
└── Editor/
    ├── EZGeneralMenuItems.cs
    ├── EZMenuItemOrder.cs
    ├── Common/
    ├── General/
    ├── Renamer/
    ├── ScriptStatistics/
    └── Builder/

```

`EZGeneralMenuItems` 位于 `Editor` 外层，负责注册 `EZEditorTools` 菜单，并整理各个窗口的菜单入口。

`EZMenuItemOrder` 定义 `EZEditorTools` 菜单项的显示顺序。

### Common

存放编辑器内部复用的基础脚本和辅助实现：

- `EZEditorGUIUtility`：通用编辑器 GUI 绘制辅助方法。
- `EZEditorUtility`：编辑器资源、类型和着色器查询辅助方法。
- `EZCopyList`：用于构建流程的文件复制列表及其 Inspector 绘制器。
- `EZGameViewUtility`：访问 Unity Game View，并读取或修改当前目标 Display。
- `TransformExt`：Transform 扩展方法，包括层级 Transform 的 TRS 复制。

### General

存放一般用途的独立编辑器窗口，包括 `EZShaderKeywordManager`。窗口类及其功能见上方“窗口和功能说明”。

### Renamer

存放资源重命名相关功能：

- `EZRenamerWindow`：通用重命名窗口。
- `EZAssetListRenamer`：保存资源列表和重命名规则的编辑器资产。
- `EZAssetListRenamerEditor`：`EZAssetListRenamer` 的 Inspector。

### ScriptStatistics

存放脚本统计和分析相关功能：

- `EZScriptStatistics`：保存脚本统计结果的编辑器资产。
- `EZScriptStatisticsEditor`：`EZScriptStatistics` 的 Inspector 和统计执行界面。
- `EZScriptStatisticResult`：保存单个脚本统计结果的子资产。
- `EZScriptStatisticResultEditor`：`EZScriptStatisticResult` 的 Inspector。

### Builder

存放 AssetBundle 和 Player 构建相关功能：

- `EZBundleBuilder`：AssetBundle 构建配置和执行逻辑。
- `EZBundleBuilderEditor`：`EZBundleBuilder` 的 Inspector。
- `EZBundleManager`：AssetBundle 管理窗口。
- `EZPlayerBuilder`：Player 构建配置和执行逻辑。
- `EZPlayerBuilderEditor`：`EZPlayerBuilder` 的 Inspector。
- `EZBuilderOptions`：构建相关选项定义。

### 程序集

编辑器代码位于 `EZhex1991.EZEditorTools.Editor` 程序集中，仅面向 Unity Editor 平台。
