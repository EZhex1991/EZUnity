/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-01-14 18:18:57
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [ExecuteInEditMode]
    public class EZShadowMap : MonoBehaviour
    {
        private static class Uniforms
        {
            public static readonly int[] GlobalPropertyIDs_ShadowTex = new int[] {
                Shader.PropertyToID("_EZShadowMap_ShadowTex0"),
                Shader.PropertyToID("_EZShadowMap_ShadowTex1"),
                Shader.PropertyToID("_EZShadowMap_ShadowTex2"),
                Shader.PropertyToID("_EZShadowMap_ShadowTex3"),
            };
            public static readonly int GlobalPropertyID_WorldToShadow = Shader.PropertyToID("_EZShadowMap_WorldToShadow");
            public static readonly int GlobalPropertyID_WorldToCamera = Shader.PropertyToID("_EZShadowMap_WorldToCamera");
            public static readonly int GlobalPropertyID_LightDirection = Shader.PropertyToID("_EZShadowMap_LightDirection");
            public static readonly int GlobalPropertyID_LightSplitsNear = Shader.PropertyToID("_EZShadowMap_LightSplitsNear");
            public static readonly int GlobalPropertyID_LightSplitsFar = Shader.PropertyToID("_EZShadowMap_LightSplitsFar");
            public static readonly int GlobalPropertyID_ShadowBias = Shader.PropertyToID("_EZShadowCollector_ShadowBias");
        }
        private static float[] cascades = new float[] { 0.067f, 0.2f, 0.467f, 1 };

        public enum BoundMode { FitToView, FitToCascades }

        public Shader shadowCollectorShader;

        [SerializeField]
        private Camera m_TargetCamera;
        public Camera targetCamera
        {
            get
            {
                if (m_TargetCamera == null) return Camera.current;
                return m_TargetCamera;
            }
        }

        public Vector2Int textureResolution = new Vector2Int(512, 512);
        public LayerMask layerMask = -1;

        public BoundMode boundMode = BoundMode.FitToView;
        public float shadowBias = 0.03f;

        private GameObject m_ShadowRendererObject;
        private Camera m_ShadowCamera;

        private Vector3[] targetCameraCornersNear = new Vector3[4];
        private Vector3[] targetCameraCornersFar = new Vector3[4];
        private Vector3[,] shadowCameraCornersNear = new Vector3[4, 4];
        private Vector3[,] shadowCameraCornersFar = new Vector3[4, 4];

        private RenderTexture[] shadowTextures = new RenderTexture[4];
        private Matrix4x4[] worldToShadow = new Matrix4x4[4];
        private Vector2Int oldResolution;

        private void Awake()
        {
            m_ShadowRendererObject = new GameObject(string.Format("{0}-{1}", typeof(EZShadowMap).Name, GetInstanceID()));
            m_ShadowRendererObject.transform.SetParent(transform, false);
            m_ShadowRendererObject.hideFlags = HideFlags.HideAndDontSave;
            m_ShadowCamera = m_ShadowRendererObject.AddComponent<Camera>();
            m_ShadowCamera.allowHDR = false;
            m_ShadowCamera.allowMSAA = false;
            m_ShadowCamera.allowDynamicResolution = false;
            m_ShadowCamera.depthTextureMode = DepthTextureMode.Depth;
            m_ShadowCamera.clearFlags = CameraClearFlags.SolidColor;
            m_ShadowCamera.backgroundColor = Color.black;
            m_ShadowCamera.orthographic = true;
        }
        private void Start()
        {
            SetupTexture();
            SetupShadowCamera();
        }
        private void OnEnable()
        {
            m_ShadowRendererObject.SetActive(true);
        }
        private void OnDisable()
        {
            m_ShadowRendererObject.SetActive(false);
        }
        private void OnDestroy()
        {
#if UNITY_EDITOR
            DestroyImmediate(m_ShadowRendererObject);
#else
            Destroy(m_ShadowRendererObject);
#endif
            for (int i = 0; i < shadowTextures.Length; i++)
            {
                if (shadowTextures[i] != null) shadowTextures[i].Release();
            }
        }

        private void Update()
        {
            if (targetCamera != null)
            {
                GetShadowTextures();
                Shader.SetGlobalVector(Uniforms.GlobalPropertyID_LightDirection, -transform.forward);
            }
        }

        private void OnValidate()
        {
            SetupTexture();
            SetupShadowCamera();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            for (int cascade = 0; cascade < cascades.Length; cascade++)
            {
                for (int i = 0; i < 4; i++)
                {
                    Gizmos.DrawLine(shadowCameraCornersNear[cascade, i], shadowCameraCornersNear[cascade, (i + 1) % 4]);
                    Gizmos.DrawLine(shadowCameraCornersFar[cascade, i], shadowCameraCornersFar[cascade, (i + 1) % 4]);
                    Gizmos.DrawLine(shadowCameraCornersNear[cascade, i], shadowCameraCornersFar[cascade, i]);
                }
            }
        }

        private void SetupTexture()
        {
            if (oldResolution != textureResolution)
            {
                for (int i = 0; i < 4; i++)
                {
                    shadowTextures[i] = new RenderTexture(textureResolution.x, textureResolution.y, 16, RenderTextureFormat.Depth, RenderTextureReadWrite.Default);
                    shadowTextures[i].useMipMap = false;
                    Shader.SetGlobalTexture(Uniforms.GlobalPropertyIDs_ShadowTex[i], shadowTextures[i]);
                }
                oldResolution = textureResolution;
            }
        }
        private void SetupShadowCamera()
        {
            m_ShadowCamera.cullingMask = layerMask;
            m_ShadowCamera.SetReplacementShader(shadowCollectorShader, "");
        }
        private void GetShadowTextures()
        {
            if (m_ShadowCamera == null) return;

            Vector4 splitsFar = new Vector4(
                Mathf.Lerp(targetCamera.nearClipPlane, targetCamera.farClipPlane, cascades[0]),
                Mathf.Lerp(targetCamera.nearClipPlane, targetCamera.farClipPlane, cascades[1]),
                Mathf.Lerp(targetCamera.nearClipPlane, targetCamera.farClipPlane, cascades[2]),
                targetCamera.farClipPlane
            );
            Vector4 splitsNear = new Vector4(
                targetCamera.nearClipPlane,
                splitsFar.x,
                splitsFar.y,
                splitsFar.z
            );
            Shader.SetGlobalVector(Uniforms.GlobalPropertyID_LightSplitsNear, splitsNear);
            Shader.SetGlobalVector(Uniforms.GlobalPropertyID_LightSplitsFar, splitsFar);

            Shader.SetGlobalFloat(Uniforms.GlobalPropertyID_ShadowBias, shadowBias);
            for (int cascade = 0; cascade < cascades.Length; cascade++)
            {
                targetCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), splitsFar[cascade], Camera.MonoOrStereoscopicEye.Mono, targetCameraCornersFar);
                if (boundMode == BoundMode.FitToCascades)
                {
                    targetCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), splitsNear[cascade], Camera.MonoOrStereoscopicEye.Mono, targetCameraCornersNear);
                }
                else if (boundMode == BoundMode.FitToView)
                {
                    targetCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), splitsNear[0], Camera.MonoOrStereoscopicEye.Mono, targetCameraCornersNear);
                }

                for (int i = 0; i < 4; i++)
                {
                    targetCameraCornersNear[i] = targetCamera.transform.TransformPoint(targetCameraCornersNear[i]);
                    targetCameraCornersFar[i] = targetCamera.transform.TransformPoint(targetCameraCornersFar[i]);
                }

                for (int i = 0; i < 4; i++)
                {
                    shadowCameraCornersNear[cascade, i] = transform.InverseTransformPoint(targetCameraCornersNear[i]);
                    shadowCameraCornersFar[cascade, i] = transform.InverseTransformPoint(targetCameraCornersFar[i]);
                }
                float minX, minY, minZ, maxX, maxY, maxZ;
                minX = minY = minZ = float.MaxValue;
                maxX = maxY = maxZ = float.MinValue;
                for (int i = 0; i < 4; i++)
                {
                    minX = Mathf.Min(minX, Mathf.Min(shadowCameraCornersNear[cascade, i].x, shadowCameraCornersFar[cascade, i].x));
                    minY = Mathf.Min(minY, Mathf.Min(shadowCameraCornersNear[cascade, i].y, shadowCameraCornersFar[cascade, i].y));
                    minZ = Mathf.Min(minZ, Mathf.Min(shadowCameraCornersNear[cascade, i].z, shadowCameraCornersFar[cascade, i].z));
                    maxX = Mathf.Max(maxX, Mathf.Max(shadowCameraCornersNear[cascade, i].x, shadowCameraCornersFar[cascade, i].x));
                    maxY = Mathf.Max(maxY, Mathf.Max(shadowCameraCornersNear[cascade, i].y, shadowCameraCornersFar[cascade, i].y));
                    maxZ = Mathf.Max(maxZ, Mathf.Max(shadowCameraCornersNear[cascade, i].z, shadowCameraCornersFar[cascade, i].z));
                }
                shadowCameraCornersNear[cascade, 0] = new Vector3(minX, minY, minZ);
                shadowCameraCornersNear[cascade, 1] = new Vector3(maxX, minY, minZ);
                shadowCameraCornersNear[cascade, 2] = new Vector3(maxX, maxY, minZ);
                shadowCameraCornersNear[cascade, 3] = new Vector3(minX, maxY, minZ);
                shadowCameraCornersFar[cascade, 0] = new Vector3(minX, minY, maxZ);
                shadowCameraCornersFar[cascade, 1] = new Vector3(maxX, minY, maxZ);
                shadowCameraCornersFar[cascade, 2] = new Vector3(maxX, maxY, maxZ);
                shadowCameraCornersFar[cascade, 3] = new Vector3(minX, maxY, maxZ);

                m_ShadowCamera.transform.position = transform.TransformPoint((shadowCameraCornersNear[cascade, 0] + shadowCameraCornersNear[cascade, 2]) * 0.5f);
                m_ShadowCamera.nearClipPlane = 0;
                m_ShadowCamera.farClipPlane = maxZ - minZ;
                m_ShadowCamera.orthographicSize = (maxY - minY) / 2;
                m_ShadowCamera.aspect = (maxX - minX) / (maxY - minY);

                m_ShadowCamera.targetTexture = shadowTextures[cascade];
                m_ShadowCamera.Render();

                worldToShadow[cascade] = GL.GetGPUProjectionMatrix(m_ShadowCamera.projectionMatrix, false) * m_ShadowCamera.worldToCameraMatrix;
            }
            Shader.SetGlobalMatrixArray(Uniforms.GlobalPropertyID_WorldToShadow, worldToShadow);
            Shader.SetGlobalMatrix(Uniforms.GlobalPropertyID_WorldToCamera, targetCamera.worldToCameraMatrix);
        }
    }
}
