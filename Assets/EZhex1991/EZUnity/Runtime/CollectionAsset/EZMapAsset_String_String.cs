/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-08 11:55:28
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.EZCollectionAsset
{
    [CreateAssetMenu(fileName = "EZMapAsset String-String", menuName = "EZUnity/EZMapAsset/String-String", order = (int)EZAssetMenuOrder.EZMapAsset_String_String)]
    public class EZMapAsset_String_String : EZMapAsset<string, string>
    {
        public void GetTransformHierarchy(Transform transform, string prefix = "")
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                string path = prefix + child.name;
                Add(child.name, path);
                GetTransformHierarchy(child, path + "/");
            }
        }
    }
}
