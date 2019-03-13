/* Author:          熊哲
 * CreateTime:      2019-02-27 10:53:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    [ExecuteInEditMode]
    public class EZGlobalLight : MonoBehaviour
    {
        [SerializeField]
        private string keyword = "_EZGLOBALLIGHT";
        [SerializeField]
        private string lightVectorName = "_EZGlobalLightPosition";
        [SerializeField]
        private string lightColorName = "_EZGlobalLightColor";

        [SerializeField, ColorUsage(true, true)]
        private Color lightColor = Color.white;

        private string lastKeyword;
        private void OnEnable()
        {
            lastKeyword = keyword;
            Shader.EnableKeyword(keyword);
        }
        private void Update()
        {
            Shader.SetGlobalVector(lightVectorName, -transform.forward);
            Shader.SetGlobalColor(lightColorName, lightColor);
        }
        private void OnDisable()
        {
            Shader.DisableKeyword(keyword);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = lightColor;
            EZUtility.DrawGizmosArrow(transform.position, transform.forward, 0.2f, transform.up);
            EZUtility.DrawGizmosArrow(transform.position, transform.forward, 0.2f, transform.right);
        }
    }
}
