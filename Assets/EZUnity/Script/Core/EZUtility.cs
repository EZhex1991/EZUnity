/*
 * Author:      熊哲
 * CreateTime:  1/24/2018 2:53:33 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZUnity
{
    public static partial class EZUtility
    {
        public const int AssetOrder = 1000;

        public static bool IsNetAvailable
        {
            get { return Application.internetReachability != NetworkReachability.NotReachable; }
        }
        public static bool IsNetLocal
        {
            get { return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork; }
        }
    }
}