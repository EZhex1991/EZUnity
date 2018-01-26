/*
 * Author:      #AUTHORNAME#
 * CreateTime:  #CREATETIME#
 * Description:
 * 
*/
using UnityEngine;
using System.IO;

public class Test : MonoBehaviour
{
    string path = "files.txt";
    string text = "";

    void StreamLoad(string path)
    {
        text = EZStreamLoader.LoadText(path);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 300, 80), "Load " + path))
        {
            StreamLoad(path);
        }
        GUI.TextField(new Rect(10, 200, 500, 290), text);
    }
}