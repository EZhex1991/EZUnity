/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 15:22:32
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = nameof(EZTextureChannelModifier),
        menuName = MenuName_TextureProcessor + nameof(EZTextureChannelModifier))]
    public class EZTextureChannelModifier : EZTextureProcessor
    {
        private static class Uniforms
        {
            public static readonly string ShaderName = "Hidden/EZTextureProcessor/ChannelModifier";
            public static readonly int PropertyID_TexR = Shader.PropertyToID("_TexR");
            public static readonly int PropertyID_ChannelR = Shader.PropertyToID("_ChannelR");
            public static readonly int PropertyID_LutR = Shader.PropertyToID("_LutR");

            public static readonly int PropertyID_TexG = Shader.PropertyToID("_TexG");
            public static readonly int PropertyID_ChannelG = Shader.PropertyToID("_ChannelG");
            public static readonly int PropertyID_LutG = Shader.PropertyToID("_LutG");

            public static readonly int PropertyID_TexB = Shader.PropertyToID("_TexB");
            public static readonly int PropertyID_ChannelB = Shader.PropertyToID("_ChannelB");
            public static readonly int PropertyID_LutB = Shader.PropertyToID("_LutB");

            public static readonly int PropertyID_TexA = Shader.PropertyToID("_TexA");
            public static readonly int PropertyID_ChannelA = Shader.PropertyToID("_ChannelA");
            public static readonly int PropertyID_LutA = Shader.PropertyToID("_LutA");
        }

        public override string defaultShaderName { get { return Uniforms.ShaderName; } }

        protected Material m_Material;
        public override Material material
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

        [SerializeField]
        private Texture2D m_InputTexture;
        public override Texture inputTexture { get { return m_InputTexture; } }
        [EZCurveRect]
        public AnimationCurve outputCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [System.NonSerialized]
        private Texture2D m_OutputLut;
        public Texture2D outputLut
        {
            get
            {
                if (m_OutputLut == null)
                {
                    m_OutputLut = Texture2DExt.GetLut(outputCurve);
                }
                return m_OutputLut;
            }
        }

        public Texture2D overrideTextureR;
        public ColorChannel overrideChannelR = ColorChannel.R;
        [EZCurveRect]
        public AnimationCurve overrideCurveR = AnimationCurve.Linear(0, 0, 1, 1);
        [System.NonSerialized]
        private Texture2D m_OverrideLutR;
        public Texture2D overrideLutR
        {
            get
            {
                if (m_OverrideLutR == null)
                {
                    m_OverrideLutR = Texture2DExt.GetLut(overrideCurveR);
                }
                return m_OverrideLutR;
            }
        }

        public Texture2D overrideTextureG;
        public ColorChannel overrideChannelG = ColorChannel.G;
        [EZCurveRect]
        public AnimationCurve overrideCurveG = AnimationCurve.Linear(0, 0, 1, 1);
        [System.NonSerialized]
        private Texture2D m_OverrideLutG;
        public Texture2D overrideLutG
        {
            get
            {
                if (m_OverrideLutG == null)
                {
                    m_OverrideLutG = Texture2DExt.GetLut(overrideCurveG);
                }
                return m_OverrideLutG;
            }
        }

        public Texture2D overrideTextureB;
        public ColorChannel overrideChannelB = ColorChannel.B;
        [EZCurveRect]
        public AnimationCurve overrideCurveB = AnimationCurve.Linear(0, 0, 1, 1);
        [System.NonSerialized]
        private Texture2D m_OverrideLutB;
        public Texture2D overrideLutB
        {
            get
            {
                if (m_OverrideLutB == null)
                {
                    m_OverrideLutB = Texture2DExt.GetLut(overrideCurveB);
                }
                return m_OverrideLutB;
            }
        }

        public Texture2D overrideTextureA;
        public ColorChannel overrideChannelA = ColorChannel.A;
        [EZCurveRect]
        public AnimationCurve overrideCurveA = AnimationCurve.Linear(0, 0, 1, 1);
        [System.NonSerialized]
        private Texture2D m_OverrideLutA;
        public Texture2D overrideLutA
        {
            get
            {
                if (m_OverrideLutA == null)
                {
                    m_OverrideLutA = Texture2DExt.GetLut(overrideCurveA);
                }
                return m_OverrideLutA;
            }
        }

        public Texture2D ResampleTexture(Texture2D texture)
        {
            Texture2D newTexture = new Texture2D(texture.width, texture.height, outputFormat, false);
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Color color = texture.GetPixel(x, y);
                    color = new Color(
                        outputCurve.Evaluate(color.r),
                        outputCurve.Evaluate(color.g),
                        outputCurve.Evaluate(color.b),
                        outputCurve.Evaluate(color.a)
                    );
                    newTexture.SetPixel(x, y, color);
                }
            }
            newTexture.Apply();
            return newTexture;
        }

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                if (overrideTextureR != null)
                {
                    material.SetTexture(Uniforms.PropertyID_TexR, overrideTextureR);
                    material.SetInt(Uniforms.PropertyID_ChannelR, (int)overrideChannelR);
                    material.SetTexture(Uniforms.PropertyID_LutR, overrideLutR);
                }
                else
                {
                    material.SetTexture(Uniforms.PropertyID_TexR, inputTexture);
                    material.SetInt(Uniforms.PropertyID_ChannelR, 0);
                    material.SetTexture(Uniforms.PropertyID_LutR, outputLut);
                }

                if (overrideTextureG != null)
                {
                    material.SetTexture(Uniforms.PropertyID_TexG, overrideTextureG);
                    material.SetInt(Uniforms.PropertyID_ChannelG, (int)overrideChannelG);
                    material.SetTexture(Uniforms.PropertyID_LutG, overrideLutG);
                }
                else
                {
                    material.SetTexture(Uniforms.PropertyID_TexG, inputTexture);
                    material.SetInt(Uniforms.PropertyID_ChannelG, 1);
                    material.SetTexture(Uniforms.PropertyID_LutG, outputLut);
                }

                if (overrideTextureB != null)
                {
                    material.SetTexture(Uniforms.PropertyID_TexB, overrideTextureB);
                    material.SetInt(Uniforms.PropertyID_ChannelB, (int)overrideChannelB);
                    material.SetTexture(Uniforms.PropertyID_LutB, overrideLutB);
                }
                else
                {
                    material.SetTexture(Uniforms.PropertyID_TexB, inputTexture);
                    material.SetInt(Uniforms.PropertyID_ChannelB, 2);
                    material.SetTexture(Uniforms.PropertyID_LutB, outputLut);
                }

                if (overrideTextureA != null)
                {
                    material.SetTexture(Uniforms.PropertyID_TexA, overrideTextureA);
                    material.SetInt(Uniforms.PropertyID_ChannelA, (int)overrideChannelA);
                    material.SetTexture(Uniforms.PropertyID_LutA, overrideLutA);
                }
                else
                {
                    material.SetTexture(Uniforms.PropertyID_TexA, inputTexture);
                    material.SetInt(Uniforms.PropertyID_ChannelA, 3);
                    material.SetTexture(Uniforms.PropertyID_LutA, outputLut);
                }

                Graphics.Blit(null, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }

        private void OnValidate()
        {
            outputLut.SetLut(outputCurve);
            overrideLutR.SetLut(overrideCurveR);
            overrideLutG.SetLut(overrideCurveG);
            overrideLutB.SetLut(overrideCurveB);
        }
    }
}
