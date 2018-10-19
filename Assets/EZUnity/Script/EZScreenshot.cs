/* Author:          熊哲
 * CreateTime:      2018-08-03 19:54:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.IO;
using UnityEngine;

namespace EZUnity
{
    [RequireComponent(typeof(Camera))]
    public class EZScreenshot : MonoBehaviour
    {
        private Camera m_Camera;
        public Camera camera
        {
            get
            {
                if (m_Camera == null)
                    m_Camera = GetComponent<Camera>();
                return m_Camera;
            }
        }

        public Vector2Int resolution = new Vector2Int(1920, 1080);
        public TextureFormat format = TextureFormat.ARGB32;
        public string capturePath = "EZScreenshots";

        public void CameraCapture(string path)
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
            Texture2D tex = new Texture2D(width, height, format, false);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            return tex;
        }

        public bool videoRecord;
        [Range(0, 10)]
        public int frameSkip = 0;
        public string videoPath = "EZScreenshots/Video";
        public int frameCount { get; private set; }

        public static string GetFileName()
        {
            return string.Format("screenshot-{0}", DateTime.Now.ToString("yyyyMMdd-HHmmss"));
        }

        // ScreenCapture can be used only in play mode, and the path is related to project path and must have extension name
        public void ScreenCapture(string path, int superSize = 2)
        {
            if (!Path.HasExtension(path)) path = path + ".png";
            UnityEngine.ScreenCapture.CaptureScreenshot(path, superSize);
        }
        public Texture2D ScreenCapture(int superSize = 2)
        {
            return UnityEngine.ScreenCapture.CaptureScreenshotAsTexture(superSize);
        }

        private void Start()
        {
            Directory.CreateDirectory(capturePath);
            Directory.CreateDirectory(videoPath);
        }

        private void Update()
        {
            if (videoRecord && Time.frameCount % (frameSkip + 1) == 0)
            {
                ScreenCapture(Path.Combine(videoPath, GetFileName()));
                frameCount++;
            }
        }
    }
}
