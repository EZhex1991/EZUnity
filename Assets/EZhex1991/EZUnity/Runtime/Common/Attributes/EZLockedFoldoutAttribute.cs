/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-13 16:10:01
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZLockedFoldoutAttribute : PropertyAttribute
    {
        public bool foldout;

        public EZLockedFoldoutAttribute()
        {
            foldout = true;
        }
        public EZLockedFoldoutAttribute(bool foldout)
        {
            this.foldout = foldout;
        }
    }
}
