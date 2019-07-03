/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-14 20:18:40
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomPropertyDrawer(typeof(EZDebugPropertyAttribute))]
    public class EZDebugPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

        }
    }
}
