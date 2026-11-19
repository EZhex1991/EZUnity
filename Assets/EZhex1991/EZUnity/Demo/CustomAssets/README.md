# Custom Assets

EZUnity 集合资产示例。创建菜单均位于 Unity 的 `Assets/Create/EZUnity` 下。

## EZListAsset

| 创建菜单 | 说明 |
| --- | --- |
| `EZUnity/EZListAsset/Object` | 可序列化的 Unity Object 列表资产。 |
| `EZUnity/EZListAsset/String` | 可序列化的字符串列表资产。 |

## EZMapAsset

| 创建菜单 | 说明 |
| --- | --- |
| `EZUnity/EZMapAsset/String-Int` | 字符串到整数的映射资产。 |
| `EZUnity/EZMapAsset/String-Object` | 字符串到 Unity Object 的映射资产。 |
| `EZUnity/EZMapAsset/String-String` | 字符串到字符串的字典资产，可用于名称适配。Key 重复时会在 Inspector 中显示为红色。 |
| `EZUnity/EZMapAsset/String-TextCollection` | 多语言长文本映射资产。Key 重复时会在 Inspector 中显示为红色。 |

![String Text Collection](.SamplePicture/EZStringCollection.png)

![String Dictionary](.SamplePicture/EZStringDictionary.png)
