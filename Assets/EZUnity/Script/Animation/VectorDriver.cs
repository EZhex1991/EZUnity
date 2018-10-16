/*
 * Author:      熊哲
 * CreateTime:  12/11/2017 5:32:03 PM
 * Description:
 * 
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