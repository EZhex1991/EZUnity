# EZProjectSettings

## EZEditorSettings

- `Hierarchy Toggle Enabled`: 是否在Hierarchy窗口显示ActiveToggle
  - 该选项提供在非Inspector窗口（在不改变`Selection.activeObject`的情况下）开启/关闭物体的功能
- `Importer Preset Enabled`: 开启该选项后，新资源导入时会寻找目录中的ImporterPreset，文件名包含某个Tag的资源会优先寻找以该Tag为后缀的Preset  
（寻找顺序：同级目录TaggedImporter => 同级目录DefaultImporter => {父级目录TaggedImporter => 父级目录DefaultImport} 递归）

![EZEditorSettings](.SamplePicture/EZEditorSettings.png)

## EZGraphicSettings

提供一个编辑Always Included Shaders的工具

![EZGraphicSettings](.SamplePicture/EZGraphicSettings.png)

## EZScriptSettings

**用于在编辑器下管理脚本模板，使用之前请为User添加Unity的Editor目录的write权限**。

Unity新建脚本的模板位于 [Unity安装目录/Editor/Resources/ScriptTemplates] 目录中，其中的".txt"文件如果命名符合一定的格式，那么Unity的Project右键菜单Create选项中会出现对应的选项（由于这个Create菜单是在Unity启动时初始化，所以添加模板后需要重启Unity才能生效，修改模板则会即时生效）。

![CreateMenu](.SamplePicture/CreateMenu.png)

从 [ProjectSettings/EZUnity/EZScriptSettings] 打开界面，并在Project中选中后缀符合规则的TextAsset后，界面上会出现选中的文件，点击添加按钮即可将文件拷贝到Unity模板目录中（可多选）。如果拷贝意味着替换，那么文件会显示在红底框中，如下图。

![EZScriptSettings](.SamplePicture/EZScriptSettings.png)

在新建脚本并重命名后，Unity会使用脚本文件名称来替换模板中的"#SCRIPTNAME#"，以该规则为基础，个人添加了一个"#CREATETIME#"变量，会在新建脚本时替换为当前的时间。另外，为了方便扩展，也可以自定义变量，添加在Custom Pattern中。这个功能使用`AssetModificationProcessor`实现，只对特定的后缀名有效（这里指的是文件后缀，模板后缀均为".txt"，对名以"脚本后缀+.txt"结尾的如".cs.txt"，该工具会将其判定为模板文件，不会替换这些变量）

![ScriptTemplate](.SamplePicture/ScriptTemplate.png)