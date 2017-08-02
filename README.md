### EZFramework 

自己的Unity框架（其实只是一些API进行了二次封装），集成了XLua，不定期更新。
与XLua耦合度很低，不需要XLua的可以直接将XLua和XLuaExtension拿掉。

### EZUnityTools

自己的常用组件和编辑器扩展，组件基本没啥用，编辑器扩展的主要功能：

- EZAssetProcessor: 用于对导入资源的默认属性进行修改。
- EZBundle: EditorGUI打包工具。
- EZKeystore: 存放签名密码的工具。。。
- EZRename: 批量重命名工具，支持正则式。
- EZScript: 脚本模板添加工具。

部分功能参数是用asset(ScriptableObject)存放的，自定义EditorWindow只是提供更好的编辑界面，asset默认存放位置在EZAsset目录。

-----

因为不是一套完整的解决方案，直接使用可能会有很多坑，所以建议仅做参考。。。
本人QQ：361994819。