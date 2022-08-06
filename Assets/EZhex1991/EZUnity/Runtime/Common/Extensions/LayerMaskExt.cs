/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 15:55:04
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class LayerMaskExt
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask | (1 << layer)) == mask;
        }
    }
}
