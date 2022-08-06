/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-11-07 17:22:30
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomPropertyDrawer(typeof(KeyCode))]
    public class EZKeyCodePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position.width -= 70;
            EditorGUI.PropertyField(position, property, label);
            position.x += position.width;
            position.width = 65;
            if (GUI.Button(position, "KeyCode", EditorStyles.miniButton))
            {
                DrawKeyCodeMenu((code) =>
                {
                    property.intValue = (int)code;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            EditorGUI.EndProperty();
        }
        private void DrawKeyCodeMenu(GenericMenu.MenuFunction2 callback)
        {
            GenericMenu menu = new GenericMenu();
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                string menuName;
                if (code.ToString().StartsWith("Joystick"))
                {
                    menuName = code.ToString();
                    menuName = menuName.Substring(0, menuName.LastIndexOf("Button")) + "/" + menuName;
                }
                else if (Enum.IsDefined(typeof(EZKeyCode), (int)code))
                {
                    EZKeyCode ezKeyCode = (EZKeyCode)code;
                    menuName = ezKeyCode.ToString().Replace("_", "/");
                }
                else
                {
                    menuName = "Other/" + code.ToString();
                }
                menu.AddItem(new GUIContent(menuName), false, callback, code);
            }
            menu.ShowAsContext();
        }
    }
}
