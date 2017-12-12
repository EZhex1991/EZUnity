/*
 * Author:      熊哲
 * CreateTime:  12/11/2017 4:36:27 PM
 * Description:
 * 
*/
using System;

namespace EZComponent.EZAnimation
{
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