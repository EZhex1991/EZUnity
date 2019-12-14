/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-29 20:00:53
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
using UnityEngine;

namespace EZhex1991.EZUnity
{
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
        public float orthographicSize = 5;
        public int cameraDepth = -100;
        public LayerMask casterLayerMask = 0;
        public LayerMask receiverLayerMask;
        public Color shadowColor = new Color(0, 0, 0, 0.5f);

        private GameObject m_CollectorObject;
        private Camera m_Camera;
        private Projector m_Projector;

        private RenderTexture renderTexture;
        private Material casterMaterial;

        private void Awake()
        {
            m_CollectorObject = new GameObject(string.Format("{0}-{1}", nameof(EZShadowProjector), GetInstanceID()));
            m_CollectorObject.transform.SetParent(transform, false);
            m_CollectorObject.hideFlags = HideFlags.HideAndDontSave;
            m_Camera = m_CollectorObject.AddComponent<Camera>();
            m_Projector = m_CollectorObject.AddComponent<Projector>();
        }
        private void Start()
        {
            SetupTexture();
            SetupMaterial();
            SetupCamera();
            SetupProjector();
        }
        private void OnEnable()
        {
            m_CollectorObject.SetActive(true);
        }
        private void OnDisable()
        {
            m_CollectorObject.SetActive(false);
        }
        private void OnDestroy()
        {
#if UNITY_EDITOR
            DestroyImmediate(m_CollectorObject);
            if (casterMaterial != null) DestroyImmediate(casterMaterial);
#else
            Destroy(m_CollectorObject);
            if (casterMaterial != null) Destroy(casterMaterial);
#endif
            if (renderTexture != null) renderTexture.Release();
        }

        private void OnValidate()
        {
            SetupTexture();
            SetupMaterial();
            SetupCamera();
            SetupProjector();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            float center = (farClipPlane + nearClipPlane) * 0.5f;
            float length = (farClipPlane - nearClipPlane) * 0.5f;
            Vector3 size = new Vector3(orthographicSize * renderTexture.width / renderTexture.height, orthographicSize, length);
            Gizmos.DrawWireCube(center * Vector3.forward, size * 2);
        }

        private void SetupTexture()
        {
            if (renderTexture == null ||
                renderTexture.width != textureResolution.x || renderTexture.height != textureResolution.y ||
                renderTexture.format != textureFormat)
            {
                renderTexture = new RenderTexture(textureResolution.x, textureResolution.y, 0, textureFormat);
                renderTexture.useMipMap = false;
            }
        }
        private void SetupMaterial()
        {
            if (casterMaterial == null)
            {
                casterMaterial = new Material(shadowCasterShader);
            }
            casterMaterial.SetTexture("_ShadowTex", renderTexture);
            casterMaterial.SetColor("_ShadowColor", shadowColor);
        }
        private void SetupCamera()
        {
            if (m_Camera == null) return;
            m_Camera.SetReplacementShader(shadowCollectorShader, "RenderType");
            m_Camera.clearFlags = CameraClearFlags.SolidColor;
            m_Camera.backgroundColor = Color.black;
            m_Camera.orthographic = true;
            m_Camera.targetTexture = renderTexture;

            m_Camera.cullingMask = casterLayerMask;
            m_Camera.orthographicSize = orthographicSize;
            m_Camera.aspect = (float)renderTexture.width / renderTexture.height;
            m_Camera.nearClipPlane = nearClipPlane;
            m_Camera.farClipPlane = farClipPlane;
            m_Camera.depth = cameraDepth;
        }
        private void SetupProjector()
        {
            if (m_Projector == null) return;
            m_Projector.aspectRatio = (float)renderTexture.width / renderTexture.height;
            m_Projector.orthographic = true;
            m_Projector.nearClipPlane = nearClipPlane;
            m_Projector.farClipPlane = farClipPlane;
            m_Projector.orthographicSize = orthographicSize;
            m_Projector.material = casterMaterial;
            m_Projector.ignoreLayers = ~receiverLayerMask;
            m_Projector.material = casterMaterial;
        }
    }
}
