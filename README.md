### EZFramework 

Unity部分API的二次封装。

### EZUnityTools

常用组件(namespace EZComponent) 及其对应的编辑器(namespace EZComponentEditor)  

编辑器扩展(namespace EZUnityEditor)  
- [EZRename](Assets/EZUnityTools/Editor/EZUnityEditor/EZRename): 批量重命名工具，支持正则式匹配，整理资源目录很方便。
- [EZPlayerPrefsEditor](Assets/EZUnityTools/Editor/EZUnityEditor/EZPlayerPrefs): 用于在编辑器下对PlayerPrefs进行编辑，目前只有Win下5.x以上版本可以用。
- [EZScript](Assets/EZUnityTools/Editor/EZUnityEditor/EZScript):  
  * EZScriptTemplate: 脚本模板管理工具（之前是“添加”工具，现在可以直接在Unity里“删除”代码模板了，添加和删除模板后重启Unity才有效）。
  * EZScriptStatistics: 用来统计代码量的工具，可以通过正则式来对代码文件进行分类统计，需要预先对代码模板进行设置。通过指定IncludePaths和ExcludePaths来统计部分目录下的代码。
- [EZProjectSettings](Assets/EZUnityTools/Editor/EZUnityEditor/EZProjectSettings): 目前只有两个功能，保存并自动设置keystore的密码（有点Bug，不打算修复）和一键添加built-in shader到GraphicsSettings。
- [EZAssetProcessor](Assets/EZUnityTools/Editor/EZUnityEditor/EZAssetProcessor): 用于对命名满足一定规范的资源进行默认的导入参数修改，命名规则比较有主观性，通用性不高，仅可作为代码参考（为了防止对别人的项目造成破坏，这个需要加宏`EZASSETPOSTPROCESSOR`启用）。菜单项用于新建特定的资源。
- [EZBundle](Assets/EZUnityTools/Editor/EZUnityEditor/EZBundle): AssetBundle build工具，可以保存build选项（还可以用SaveAs备份）。两种模式：  
  * EZBundle Mode: 偏向目录结构管理，设置bundle名称、路径和文件搜索条件去进行build。
  * Manager Mode: 偏向单个资源设置，会读取当前项目中Inspector中对单个资源的bundle设置。

-----

本人QQ：361994819，可能不会及时回复，欢迎留言。

2018/02/11: 
- 加了一些说明文档，直接导入项目中使用的同学可以参考。

2017/09/19（重构）:
- EZFramework升级了xLua，比较重要的是dictionary的索引方式把setter也去掉了，需要自己实现方法代替。
- EZUnityTools分成几个部分，命名空间和目录结构都有变化，自定义编辑器扩展的命名空间和菜单栏改成了"EZUnityEditor"。
- meta文件我是尽量没有做过删除动作，但有可能有过误操作造成meta文件重新生成的情况，不建议直接更新。

2017/08/17:
- 放于github的目的只是方便自己工作，个人能力不足精力有限，暂时不会有demo。。。需要完整解决方案的，github可以搜到很多优秀的开源项目。