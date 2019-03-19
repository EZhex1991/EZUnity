/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-18 17:04:00
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EZUnity
{
    public class EZHideInInspectorNormal : PropertyAttribute
    {
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EZHideInInspectorNormal))]
    public class EZHideInInspectorNormalDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

        }
    }
#endif
}
