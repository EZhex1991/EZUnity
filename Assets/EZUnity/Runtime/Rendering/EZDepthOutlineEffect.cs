/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-18 13:34:39
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity.Rendering
{
    [DisallowMultipleComponent]
    public class EZDepthOutlineEffect : EZImageEffect
    {
        public static readonly string ShaderName = "Hidden/EZUnity/Effects/EZDepthOutline";

        private Material m_Material;
        public override Material material
        {
            get
            {
                if (m_Material == null)
                {
                    m_Material = new Material(Shader.Find(ShaderName));
                }
                return m_Material;
            }
        }

        private void Reset()
        {
            camera.depthTextureMode |= DepthTextureMode.DepthNormals;
            material.CopyPropertiesFromMaterial(new Material(Shader.Find(ShaderName)));
        }
    }
}
