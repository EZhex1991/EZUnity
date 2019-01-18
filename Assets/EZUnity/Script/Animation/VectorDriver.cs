/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-12-11 17:32:03
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;

namespace EZUnity.Animation
{
    [Serializable]
    public struct V2Driver
    {
        public bool x;
        public bool y;

        public static implicit operator bool(V2Driver driver)
        {
            return driver.x || driver.y;
        }
    }

    [Serializable]
    public struct V3Driver
    {
        public bool x;
        public bool y;
        public bool z;

        public static implicit operator bool(V3Driver driver)
        {
            return driver.x || driver.y || driver.z;
        }
    }
}