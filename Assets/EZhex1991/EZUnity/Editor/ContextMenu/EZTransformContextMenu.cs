/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-17 15:18:16
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZTransformContextMenu : EditorWindow
    {
        private const string MENU_NAME = "CONTEXT/Transform/";

        [MenuItem(MENU_NAME + "Reset World Position", false)]
        private static void ResetWorldPosition(MenuCommand command)
        {
            Transform tf = (Transform)command.context;
            if (tf.position != Vector3.zero)
            {
                Undo.RecordObject(tf, "Reset World Position");
                tf.position = Vector3.zero;
            }
        }
        [MenuItem(MENU_NAME + "Reset World Rotation", false)]
        private static void ResetWorldRotation(MenuCommand command)
        {
            Transform tf = (Transform)command.context;
            if (tf.rotation != Quaternion.identity)
            {
                Undo.RecordObject(tf, "Reset World Rotation");
                tf.rotation = Quaternion.identity;
            }
        }
        [MenuItem(MENU_NAME + "Reset World Transform", false)]
        private static void ResetWorldTransform(MenuCommand command)
        {
            Transform tf = (Transform)command.context;
            if (tf.position != Vector3.zero || tf.rotation != Quaternion.identity)
            {
                Undo.RecordObject(tf, "Reset World Transform");
                tf.position = Vector3.zero;
                tf.rotation = Quaternion.identity;
            }
        }
    }
}
