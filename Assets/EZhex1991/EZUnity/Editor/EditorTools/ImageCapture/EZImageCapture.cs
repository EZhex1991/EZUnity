/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-08-03 19:54:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CreateAssetMenu(fileName = "EZImageCapture", menuName = "EZUnity/EZImageCapture", order = (int)EZAssetMenuOrder.EZImageCapture)]
    public class EZImageCapture : ScriptableObject
    {
        public Vector2Int resolution = new Vector2Int(1920, 1080);
        public TextureFormat textureFormat = TextureFormat.ARGB32;
        public string filePath = "EZScreenshots";

        public void CameraCapture(Camera camera, string path)
        {
            Texture2D texture;
            if (camera.targetTexture == null)
            {
                RenderTexture tempRT = new RenderTexture(resolution.x, resolution.y, 32);
                tempRT.Create();
                camera.targetTexture = tempRT;
                RenderTexture.active = tempRT;
                camera.Render();
                texture = GetTexture(resolution.x, resolution.y);
                camera.targetTexture = null;
                RenderTexture.active = null;
                tempRT.Release();
            }
            else
            {
                RenderTexture.active = camera.targetTexture;
                camera.Render();
                texture = GetTexture(camera.targetTexture.width, camera.targetTexture.height);
            }
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
        }
        public Texture2D GetTexture(int width, int height)
        {
            Texture2D tex = new Texture2D(width, height, textureFormat, false);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            return tex;
        }

        // Path is related to project path and must have extension name
        public void ScreenCapture(string path, int superSize = 2)
        {
            if (EditorApplication.isPlaying)
            {
                if (!Path.HasExtension(path)) path = path + ".jpg";
                UnityEngine.ScreenCapture.CaptureScreenshot(path, superSize);
            }
            else
            {
                Debug.LogError("ScreenCapture can only be used in play mode!");
            }
        }
        public Texture2D ScreenCapture(int superSize = 2)
        {
            return UnityEngine.ScreenCapture.CaptureScreenshotAsTexture(superSize);
        }
    }
}
