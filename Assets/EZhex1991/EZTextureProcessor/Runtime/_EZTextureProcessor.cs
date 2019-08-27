/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-27 16:34:04
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    public abstract class EZTextureProcessor : EZTextureGenerator
    {
        [SerializeField]
        private Shader m_Shader;
        public Shader shader { get { return m_Shader; } }

        [SerializeField]
        private Texture m_InputTexture;
        public Texture inputTexture { get { return m_InputTexture; } set { m_InputTexture = value; } }

        protected Material m_Material;
        protected Material material
        {
            get
            {
                if (m_Material == null && shader != null)
                {
                    m_Material = new Material(shader);
                }
                return m_Material;
            }
        }
    }
}
