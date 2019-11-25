/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-05-23 18:20:26
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.XLuaExample
{
    public class LuaBehaviour : MonoBehaviour
    {
        public string fileName;

        private void Awake()
        {
            LuaManager.Instance.luaEnv.DoString("require('" + fileName + "')");
        }
    }
}