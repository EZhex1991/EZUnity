/*
 * Author:      熊哲
 * CreateTime:  8/1/2017 6:15:32 PM
 * Description:
 * 
*/
using UnityEngine;
using UnityEditor;

namespace EZUnityTools.EZEditor
{
    public static class EZDefaultAssetGenerator
    {
        public static Material GenerateMaterial(bool reset = false)
        {
            string path = "Assets/Resources/default.mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(Shader.Find("Standard"));
                AssetDatabase.CreateAsset(material, path);
                AssetDatabase.Refresh();
            }
            if (reset)
            {
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                Color color = material.color; color.a = 0.3f; material.color = color;
            }
            return material;
        }
    }
}