### EZFramework 

Unity部分API的二次封装。

### EZUnityTools

常用组件(namespace EZComponent)  
编辑器(namespace EZUnityEditor)  

组件项目针对性很强基本没啥用，编辑器扩展的主要功能：
- EZAssetProcessor: 用于对命名满足一定规范的资源进行默认的导入参数修改，命名规则比较有主观性，所以只是一个参考。
- EZBundle: EditorGUI打包工具，设置bundle名称、路径和文件搜索条件去进行批量打包，并且会保存这些设置（还可以用SaveAs备份）。
- EZKeystore: 存放签名密码的工具。。。不想介绍，偷懒不是件好事情。。。有严重Bug。。。
- EZRename: 批量重命名工具，支持正则式匹配，整理资源目录很方便。
- EZScriptTemplate: 脚本模板管理工具（之前是“添加”工具，现在可以直接在Unity里“删除”代码模板了，添加和删除模板后重启Unity才有效）。

部分功能参数是用asset(ScriptableObject)存放的，可以直接选定后在Inspector界面修改，asset默认存放位置在Assets/EZAssets目录。

-----

因为不是一套完整的解决方案，直接使用可能会有很多坑，所以建议仅做代码参考。
本人QQ：361994819，可能不会及时回复，但是欢迎留言提意见。

2017/09/19（重构）:
- EZFramework升级了xLua，比较重要的是dictionary的索引方式把setter也去掉了，需要自己实现方法代替。
- EZUnityTools分成几个部分，命名空间和目录结构都有变化，自定义编辑器扩展的命名空间和菜单栏改成了"EZUnityEditor"。
- meta文件我是尽量没有做过删除动作，但有可能有过误操作造成meta文件重新生成的情况，不建议直接更新。

2017/08/17:
- 放于github的目的只是方便自己工作，个人能力不足精力有限，暂时不会有demo。。。需要完整解决方案的，github可以搜到很多优秀的开源项目。