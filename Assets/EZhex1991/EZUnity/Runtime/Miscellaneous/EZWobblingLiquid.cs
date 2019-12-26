/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-12-26 11:59:58
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [ExecuteInEditMode]
    public class EZWobblingLiquid : MonoBehaviour
    {
        private static class Uniforms
        {
            public static int PropertyID_SurfaceHeight = Shader.PropertyToID("_SurfaceHeight");
            public static int PropertyID_FoamThickness = Shader.PropertyToID("_FoamThickness");
            public static int PropertyID_Centroid = Shader.PropertyToID("_Centroid");
        }
        private const float timeStep = 0.1f;

        public Renderer renderer;
        [Range(0, 1)]
        public float fillAmount = 0.5f;
        [Range(0, 1)]
        public float foamAmount = 0.1f;

        public float waveSpeed = 10f;
        [Range(0, 1)]
        public float damping = 0.9f;

        private MaterialPropertyBlock propertyBlock;
        private Vector3 lastPos;
        private Vector3 movement;
        private Vector3 centroid;
        private float amplitude;
        private float deltaTime;

        private void OnEnable()
        {
            propertyBlock = new MaterialPropertyBlock();
            lastPos = transform.position;
        }
        private void Update()
        {
            if (renderer == null) return;

            float totalHeight = renderer.bounds.max.y - renderer.bounds.min.y;
            float surfaceHeight = renderer.bounds.min.y + totalHeight * fillAmount;
            float foamThickness = totalHeight * fillAmount * foamAmount;

            propertyBlock.SetFloat(Uniforms.PropertyID_SurfaceHeight, surfaceHeight);
            propertyBlock.SetFloat(Uniforms.PropertyID_FoamThickness, foamThickness);

            if (Application.isPlaying)
            {
                Vector3 translation = transform.position - lastPos;
                if (translation.magnitude != 0)
                {
                    movement = movement * amplitude + translation;
                    amplitude = 1;
                }

                centroid = Mathf.Sin(Time.time * waveSpeed) * -movement * amplitude;

                deltaTime += Time.deltaTime;
                while (deltaTime >= timeStep)
                {
                    deltaTime -= timeStep;
                    amplitude *= damping;
                }
                lastPos = transform.position;

                propertyBlock.SetVector(Uniforms.PropertyID_Centroid, centroid);
            }

            renderer.SetPropertyBlock(propertyBlock);
        }

        private void Reset()
        {
            renderer = GetComponent<Renderer>();
            propertyBlock.Clear();
        }
    }
}
