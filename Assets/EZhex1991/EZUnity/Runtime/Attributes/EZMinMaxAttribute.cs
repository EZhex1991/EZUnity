/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-28 19:17:20
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZMinMaxAttribute : PropertyAttribute
    {
        public readonly bool fixedLimit;
        public float limitMin;
        public float limitMax;

        public EZMinMaxAttribute()
        {
            // limits will be retrived from zw component of the vector
            fixedLimit = false;
            limitMin = 0;
            limitMax = 1;
        }
        public EZMinMaxAttribute(float min, float max)
        {
            fixedLimit = true;
            this.limitMin = min;
            this.limitMax = max;
        }
    }
}
