/* Author:          熊哲
 * CreateTime:      2018-08-06 10:27:30
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    [CustomEditor(typeof(EZScreenshot))]
    public class EZScreenshotEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Capture"))
            {
                Capature(target as EZScreenshot);
            }
        }

        private void Capature(EZScreenshot capturer)
        {
            string dir = "Assets/EZScreenshots";
            Directory.CreateDirectory(dir);
            string fileName = string.Format("screenshot-{0}", DateTime.Now.ToString("yyyyMMdd-HHmmss"));
            string path = string.Format("{0}/{1}.{2}", dir, fileName, "png");
            capturer.Capture(path);
            AssetDatabase.Refresh();
        }
    }
}
