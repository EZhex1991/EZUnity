/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-05-23 18:20:26
 * Organization:    #ORGANIZATION#
 * Description:     
 * 
 * xlua中require实际就是调用Loader来读取lua代码
 * xlua已经实现了从Resources和StreamingAssets目录加载lua文件的Loader。你可以通过CustomLoader来自定义lua代码的读取方式
 * lua端require时传入字符串，C#端CustomLoader里根据这个字符串返回lua代码byte[]，这个过程是读取文件，还是加载bundle，都由你自己决定。
 */
using System.IO;
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity.XLuaExample
{
    public class CustomLoader : MonoBehaviour
    {
        public string fileName;

        private string luaDirPath;
        private LuaEnv luaEnv;

        void Start()
        {
            luaDirPath = Application.dataPath + "/XLuaExamples/";
            luaEnv = new LuaEnv();
            luaEnv.AddLoader(LoadFromFile); // AddLoader(CustomLoader)文档上有说明，自己读取lua源码以byte[]形式返回即可。
            luaEnv.DoString("require('" + fileName + "')");
        }

        private byte[] LoadFromFile(ref string fileName)    // 这里的fileName就是lua中require的参数
        {
            string filePath = luaDirPath + fileName.Replace('.', '/') + ".lua";             // lua文件的实际路径
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