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
        public readonly bool fixedLimit;
        public bool showAsVectorValue;
        public float limitMin;
        public float limitMax;

        public EZMinMaxSliderAttribute()
        {
            // limit will be retrived from zw component of the vector
            // you can change limit on Debug(Inspector) Window
            fixedLimit = false;
            limitMin = 0;
            limitMax = 1;
        }
        public EZMinMaxSliderAttribute(float min, float max)
        {
            fixedLimit = true;
            this.limitMin = min;
            this.limitMax = max;
        }
    }
}
