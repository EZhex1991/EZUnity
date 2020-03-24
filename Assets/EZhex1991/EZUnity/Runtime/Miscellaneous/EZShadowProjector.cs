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
        private static class Uniforms
        {
            public static readonly int GlobalPropertyID_ShadowBias = Shader.PropertyToID("_EZShadowCollector_ShadowBias");

            public static readonly int PropertyID_ShadowTex = Shader.PropertyToID("_ShadowTex");
            public static readonly int PropertyID_ShadowColor = Shader.PropertyToID("_ShadowColor");
        }

        [Header("Shader Settings")]
        public Shader shadowCollectorShader;
        public Shader shadowProjectorShader;

        [Header("Texture Settings")]
        public Vector2Int textureResolution = new Vector2Int(512, 512);

        [Header("Render Settings")]
        public float nearClipPlane = 0.1f;
        public float farClipPlane = 100;
        public float orthographicSize = 5;
        public int cameraDepth = -100;
        public LayerMask casterLayerMask = -1;
        public LayerMask receiverLayerMask = -1;
        public float shadowBias = 0.03f;
        public Color shadowColor = new Color(0, 0, 0, 0.5f);

        private GameObject m_CollectorObject;
        private Camera m_Camera;
        private Projector m_Projector;

        private RenderTexture shadowTexture;
        private Vector2Int oldResolution;
        private Material projectorMaterial;

        private void Awake()
        {
            m_CollectorObject = new GameObject(string.Format("{0}-{1}", typeof(EZShadowProjector).Name, GetInstanceID()));
            m_CollectorObject.transform.SetParent(transform, false);
            m_CollectorObject.hideFlags = HideFlags.HideAndDontSave;

            m_Camera = m_CollectorObject.AddComponent<Camera>();
            m_Camera.clearFlags = CameraClearFlags.SolidColor;
            m_Camera.backgroundColor = Color.black;
            m_Camera.orthographic = true;
            m_Camera.depthTextureMode = DepthTextureMode.Depth;
            m_Camera.allowHDR = false;
            m_Camera.allowMSAA = false;

            m_Projector = m_CollectorObject.AddComponent<Projector>();
            m_Projector.orthographic = true;
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
            if (projectorMaterial != null) DestroyImmediate(projectorMaterial);
#else
            Destroy(m_CollectorObject);
            if (projectorMaterial != null) Destroy(projectorMaterial);
#endif
            if (shadowTexture != null) shadowTexture.Release();
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
            Vector3 size = new Vector3(orthographicSize * shadowTexture.width / shadowTexture.height, orthographicSize, length);
            Gizmos.DrawWireCube(center * Vector3.forward, size * 2);
        }

        private void SetupTexture()
        {
            if (oldResolution != textureResolution || shadowTexture == null)
            {
                shadowTexture = new RenderTexture(textureResolution.x, textureResolution.y, 16, RenderTextureFormat.Depth);
                shadowTexture.useMipMap = false;
                oldResolution = textureResolution;
            }
        }
        private void SetupMaterial()
        {
            if (projectorMaterial == null)
            {
                projectorMaterial = new Material(shadowProjectorShader);
            }
            projectorMaterial.SetTexture(Uniforms.PropertyID_ShadowTex, shadowTexture);
            projectorMaterial.SetColor(Uniforms.PropertyID_ShadowColor, shadowColor);
        }
        private void SetupCamera()
        {
            if (m_Camera == null) return;
            m_Camera.targetTexture = shadowTexture;
            m_Camera.SetReplacementShader(shadowCollectorShader, "");
            Shader.SetGlobalFloat(Uniforms.GlobalPropertyID_ShadowBias, shadowBias);

            m_Camera.cullingMask = casterLayerMask;
            m_Camera.orthographicSize = orthographicSize;
            m_Camera.aspect = (float)shadowTexture.width / shadowTexture.height;
            m_Camera.nearClipPlane = nearClipPlane;
            m_Camera.farClipPlane = farClipPlane;
            m_Camera.depth = cameraDepth;
        }
        private void SetupProjector()
        {
            if (m_Projector == null) return;
            m_Projector.aspectRatio = (float)shadowTexture.width / shadowTexture.height;
            m_Projector.nearClipPlane = nearClipPlane;
            m_Projector.farClipPlane = farClipPlane;
            m_Projector.orthographicSize = orthographicSize;
            m_Projector.material = projectorMaterial;
            m_Projector.ignoreLayers = ~receiverLayerMask;
            m_Projector.material = projectorMaterial;
        }
    }
}
