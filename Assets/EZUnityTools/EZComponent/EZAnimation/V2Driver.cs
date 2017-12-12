/*
 * Author:      熊哲
 * CreateTime:  12/11/2017 5:32:03 PM
 * Description:
 * 
*/
using System;

namespace EZComponent.EZAnimation
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
}