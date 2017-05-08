### 2017.05.05
- EZSettings.asset的存放位置移动至EZFramework目录外（Assets/Resources/，因为不需要同步且必须打包）；
- EZUnityTools的所有asset存放位置移动至EZUnityTools目录外（因为不需要同步）；
- EZScript修复建立其asset所在目录时出现死循环的bug；
- EZDatabase在读取Cache时会偶尔出错，目前对出错的数据直接忽略；

### 2017.05.03
- 对EZLogHandler的单文件日志条数进行限制；

### 2017.04.26
- 部分lua相关的扩展方法修改；更新xLua版本；
- WWWTask从EZNetwork中剥离；

### 2017.04.25
- 控件的点击缩放效果由仅Button组件可用变为Selectable组件可用；

### 2017.04.24
- EZDatabase添加方法IsEmpty（IsExist只能判断是否有该存档，IsEmpty对于有存档无数据会返回true）;

### 2017.04.21
- EZSettings的Local模式可使用测试服务器地址更新，地址为空则以streamingAssetsPath为目录更新；

### 2017.04.20
 * Debug.logger是静态的，在编辑器下退出游戏时资源并不会释放，这会导致日志文件持续占用无法读取。
 * Debug.logger的更改建议发生在Start中，并在更改前保存其默认值，在OnApplicationQuit时将其还原。

### 2017.04.19
- <font color=red>EZKeystore功能修改，解决签名冲突的问题。</font>
- EZScript功能修改，代码模板可从工程目录中添加后拷贝到Unity安装目录；
- 添加UnityEngine.Space的导出；

### 2017.04.18
- EZUnityTools.EZEditor下部分命名空间删除，部分变量名和逻辑问题修改。

### 2017.04.17
- EZDatabase在读取空的index文件时会出现null，导致报错，改为如果读取为空则生成新对象；
- EZDatabase在游戏暂停前进行存档。
- EZLog在制表和换行上存在平台差异，制表改为Padding之后再Tab。部分日志格式修改。建议win上使用notepad++打开；
- EZFramework.Physics的Raycast方法加了几个重载备用;
- EZUpdate.CheckUpdate的返回值改为数字，用于区分“文件不存在”和“文件存在并更新”，并方便日后扩展；
- 解决部分UnityEditor下的报错；
