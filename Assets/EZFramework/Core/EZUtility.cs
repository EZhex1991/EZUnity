/*
 * Author:      熊哲
 * CreateTime:  1/24/2018 2:53:33 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZFramework
{
    public static partial class EZUtility
    {
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