# EZUnity

## 常用组件和编辑器扩展  

- [EZPhysicsBone](Assets/EZUnity/EZPhysicsBone): 动态骨骼，效果参考来源于AssetStore上的DynamicBone。优势: 支持所有碰撞体（包括MeshCollider，但效果一般）；独立的材质"EZPhysicsBoneMaterial"存放参数，通用性强；代码可读性高，碰撞体可通过继承EZPhysicsBoneColliderBase进行自定义扩展。

- EZRenamer: 批量重命名工具，支持正则式匹配，整理资源目录很方便
- EZPlayerPrefsEditor: 用于在编辑器下对PlayerPrefs进行编辑，目前只有Win下5.x以上版本可以用
- [EZScripting](Assets/EZUnity/Editor/EditorTools/Scripting):
  - EZScriptTemplate: 脚本模板管理工具（之前是“添加”工具，现在可以直接在Unity里“删除”代码模板了，添加和删除模板后重启Unity才有效）
  - EZScriptStatistics: 用来统计代码量的工具，可以通过正则式来对代码文件进行分类统计，需要预先对代码模板进行设置。通过指定IncludePaths、ExcludePaths和正则式匹配来统计代码
- [EZAssetProcessor](Assets/EZUnity/Editor/EditorTools/AssetProcessor): 用于对命名满足一定规范的资源进行默认的导入参数修改，命名规则比较有主观性，通用性不高，仅可作为代码参考（为了防止对别人的项目造成破坏，这个需要加宏`EZASSETPOSTPROCESSOR`启用）
- [EZBundle](Assets/EZUnity/Editor/EditorTools/Bundle): AssetBundle build工具，可以保存build选项。两种模式：  
  - EZBundle Mode: 偏向目录结构管理，设置bundle名称、路径和文件搜索条件去进行build。
  - Manager Mode: 偏向单个资源设置，会读取当前项目中Inspector中对单个资源的bundle设置。

## 基于XLua的逻辑热更方案（需要加宏'XLUA'启用）

- [XLuaExtension](Assets/EZUnity/XLuaExtension)
- [Example](Assets/Example)

-----

本人QQ：361994819，可能不会及时回复，欢迎留言。

2019-01-26:

- 优化了一些命名空间
- XLua的配置文件放到了XLuaExample下面
- **`EZLuaInjector`(`EZPropertyList`)做了List和Map两种表现形式的整合，之前使用'#'来指定List编号的方式废弃**
- 编辑器扩展放到了统一的Editor目录下
- 动态骨骼`EZPhysicsBone`的实现完成

2018-10-16:

- 本次提交有**相当大的改动**
- 测试使用的版本是**2018.2.7f1，旧版本可能无法打开**
- 目录结构优化，移除了部分第三方插件的依赖；XLua的扩展部分放到了一个目录，不需要的可以删除；如果使用`EZUnity.Framework`，文件存放位置放到了"工程目录/EZPersistent"下（以前在"工程目录/Assets/EZUnity"下）
- 取消了一些在固定目录下生成自定义Asset并打开自定义Window的选项，所有自定义Asset都需要通过`Assets/Create/EZUnity`菜单(同Project视图下的右键)进行创建，并在Inspector视图下编辑
- 命名空间统一以`EZUnity`开头
  - `EZUnity.Famework`（以前的`EZFramework`，优化了很多东西）
  - `EZUnity.XLuaExtension`（这部分需要加宏`XLUA`启用）

2018-04-04:

- 补了一些之前可能误删的文件
- Unity升级到2017.4.0f1，代码没有任何变动
- EZScript添加模板报错的问题，请在使用前手动为Unity的安装目录添加访问权限：Users-Write

2018-03-28:

- Unity升级到2017.3.0f3
- 目录重新整理了一下
- 把一些由于历史原因不能更改的东西改掉了

2018-02-11:

- 加了一些说明文档，直接导入项目中使用的同学可以参考

2017-09-19（重构）:

- EZFramework升级了xLua，比较重要的是dictionary的索引方式把setter也去掉了，需要自己实现方法代替
- EZUnityTools分成几个部分，命名空间和目录结构都有变化，自定义编辑器扩展的命名空间和菜单栏改成了"EZUnityEditor"
- meta文件我是尽量没有做过删除动作，但有可能有过误操作造成meta文件重新生成的情况，不建议直接更新

2017-08-17:

- 放于github的目的只是方便自己工作，个人能力不足精力有限，暂时不会有demo。。。需要完整解决方案的，github可以搜到很多优秀的开源项目