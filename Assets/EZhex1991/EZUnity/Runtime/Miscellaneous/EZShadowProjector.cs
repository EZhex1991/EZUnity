/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-29 20:00:53
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [RequireComponent(typeof(Camera), typeof(Projector))]
    [ExecuteInEditMode]
    public class EZShadowProjector : MonoBehaviour
    {
        [Header("Shader Settings")]
        public Shader shadowCollectorShader;
        public Shader shadowCasterShader;

        [Header("Texture Settings")]
        public Vector2Int textureResolution = new Vector2Int(512, 512);
        public RenderTextureFormat textureFormat = RenderTextureFormat.R8;

        [Header("Render Settings")]
        public float nearClipPlane = 0.1f;
        public float farClipPlane = 100;
        public float orthographicSize = 10;
        public int depth = -100;
        public LayerMask casterLayerMask = 0;
        public LayerMask receiverLayerMask;
        public Color shadowColor = new Color(0, 0, 0, 0.5f);

        private RenderTexture renderTexture;
        private Material casterMaterial;

        private Camera m_Camera;
        private Camera camera
        {
            get
            {
                if (m_Camera == null)
                    m_Camera = GetComponent<Camera>();
                return m_Camera;
            }
        }
        private Projector m_Projector;
        private Projector projector
        {
            get
            {
                if (m_Projector == null)
                    m_Projector = GetComponent<Projector>();
                return m_Projector;
            }
        }

        private void OnEnable()
        {
            camera.enabled = true;
            projector.enabled = true;
            SetupCamera();
            SetupProjector();
        }
        private void OnDisable()
        {
            camera.enabled = false;
            projector.enabled = false;
        }
        private void OnValidate()
        {
            SetupCamera();
            SetupProjector();
        }

        private void Update()
        {
            if (renderTexture == null ||
                renderTexture.width != textureResolution.x || renderTexture.height != textureResolution.y ||
                renderTexture.format != textureFormat)
            {
                renderTexture = new RenderTexture(textureResolution.x, textureResolution.y, 0, textureFormat);
                renderTexture.useMipMap = false;
            }

            if (casterMaterial == null)
            {
                casterMaterial = new Material(shadowCasterShader);
            }
            casterMaterial.SetTexture("_ShadowTex", renderTexture);
            casterMaterial.SetColor("_ShadowColor", shadowColor);

            SetupCamera();
            SetupProjector();

            projector.material = casterMaterial;
        }

        private void SetupCamera()
        {
            camera.SetReplacementShader(shadowCollectorShader, "RenderType");
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            camera.orthographic = true;
            camera.targetTexture = renderTexture;

            camera.cullingMask = casterLayerMask;
            camera.orthographicSize = orthographicSize;
            camera.nearClipPlane = nearClipPlane;
            camera.farClipPlane = farClipPlane;
            camera.depth = depth;
        }
        private void SetupProjector()
        {
            projector.orthographic = true;

            projector.nearClipPlane = nearClipPlane;
            projector.farClipPlane = farClipPlane;
            projector.orthographicSize = orthographicSize;
            projector.material = casterMaterial;
            projector.ignoreLayers = ~receiverLayerMask;
        }
    }
}
