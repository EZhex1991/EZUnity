/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-28 19:17:20
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZMinMaxSliderAttribute : PropertyAttribute
    {
        public float min;
        public float max;

        public EZMinMaxSliderAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
