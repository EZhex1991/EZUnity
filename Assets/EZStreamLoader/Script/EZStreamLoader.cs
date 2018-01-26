/*
 * Author:      #AUTHORNAME#
 * CreateTime:  #CREATETIME#
 * Description:
 * 
*/
using UnityEngine;

public static class EZStreamLoader
{
#if UNITY_EDITOR
    public static byte[] LoadFile(string path)
    {
        path = Application.streamingAssetsPath + "/" + path;
        return System.IO.File.ReadAllBytes(path);
    }
    public static string LoadText(string path)
    {
        path = Application.streamingAssetsPath + "/" + path;
        return System.IO.File.ReadAllText(path);
    }
#elif UNITY_ANDROID
    private static AndroidJavaClass jc = new AndroidJavaClass("com.ezhex1991.ezunityplugins.StreamLoader");

    public static byte[] LoadFile(string path)
    {
        return jc.CallStatic<byte[]>("loadFile", path);
    }

    public static string LoadText(string path)
    {
        byte[] bytes = LoadFile(path);
        return bytes == null ? "" : System.Text.Encoding.UTF8.GetString(bytes);
    }
#endif
}