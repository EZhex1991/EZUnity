/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-05-23 18:20:26
 * Organization:    #ORGANIZATION#
 * Description:     
 
 * `require`会通过xlua的`Loader`来获取lua代码，且xlua已经自带了“从Resources和StreamingAssets目录加载lua文件”的Loader。
 * 你自己可以通过实现`CustomLoader`来增加自定义的lua代码读取方式。lua端require的字符串参数会作为C#端CustomLoader的参数给
 * 你，你可以对这个字符串进行任何更改，然后读取文件/加载Bundle/网络请求，只要最终以byte[]形式返回lua代码即可。
 * 
 * require的参数是你写的，CustomLoader的逻辑也是你写的，xlua并不关心你在其中做了什么。
 */
using System.IO;
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity.XLuaExample
{
    public class CustomLoader : MonoBehaviour
    {
        public string fileName;

        private void Start()
        {
            LuaEnv luaEnv = new LuaEnv();
            luaEnv.AddLoader(LoadFromFile); // AddLoader(CustomLoader)文档上有说明，自己读取lua源码以byte[]形式返回即可。
            luaEnv.DoString("require('" + fileName + "')");
        }

        private byte[] LoadFromFile(ref string fileName)    // 这里的fileName就是lua中require的参数
        {
            string filePath = Application.dataPath + "/XLuaExamples/" + fileName.Replace('.', '/') + ".lua";             // lua文件的实际路径
            fileName = fileName.Replace('.', '/');     // 返给lua调试器的路径，不用调试lua的就不用管这个了
            try
            {
                // File.ReadAllBytes返回值可能会带有BOM（0xEF，0xBB，0xBF），这会导致脚本加载出错（<\239>）
                byte[] script = System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(filePath));
                return script;
            }
            catch
            {
                return null;
            }
        }
    }
}