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

        [MenuItem(MENU_NAME + "Align Parent Transform", false)]
        private static void AlignParentTransform(MenuCommand command)
        {
            Transform tf = (Transform)command.context;
            if (tf.parent == null) return;
            if (tf.localPosition != Vector3.zero || tf.localRotation != Quaternion.identity)
            {
                Undo.RecordObject(tf.parent, "Align Parent Transform");
                Undo.RecordObject(tf, "Align Parent Transform");
                tf.parent.SetPositionAndRotation(tf.position, tf.rotation);
                tf.localPosition = Vector3.zero;
                tf.localRotation = Quaternion.identity;
            }
        }
        [MenuItem(MENU_NAME + "Align Parent Transform", true)]
        private static bool AlignParentTransformValidation(MenuCommand command)
        {
            Transform tf = (Transform)command.context;
            if (tf.parent == null) return false;
            if (tf.localPosition == Vector3.zero && tf.localRotation == Quaternion.identity) return false;
            return true;
        }

        [MenuItem(MENU_NAME + "Align To Active Position", false)]
        private static void AlignToActivePosition(MenuCommand command)
        {
            Transform tf = (Transform)command.context;
            Transform alignTo = Selection.activeTransform;
            Undo.RecordObject(tf, "Align To Active Position");
            tf.position = alignTo.position;
        }
        [MenuItem(MENU_NAME + "Align To Active Rotation", false)]
        private static void AlignToActiveRotation(MenuCommand command)
        {
            Transform tf = (Transform)command.context;
            Transform alignTo = Selection.activeTransform;
            Undo.RecordObject(tf, "Align To Active Rotation");
            tf.rotation = alignTo.rotation;
        }
        [MenuItem(MENU_NAME + "Align To Active Transform", false)]
        private static void AlignToActiveTransform(MenuCommand command)
        {
            Transform tf = (Transform)command.context;
            Transform alignTo = Selection.activeTransform;
            Undo.RecordObject(tf, "Align To Active Transform");
            tf.position = alignTo.position;
            tf.rotation = alignTo.rotation;
        }
        [MenuItem(MENU_NAME + "Align To Active Position", true)]
        [MenuItem(MENU_NAME + "Align To Active Rotation", true)]
        [MenuItem(MENU_NAME + "Align To Active Transform", true)]
        private static bool AlignToActive_Validattion(MenuCommand command)
        {
            return Selection.transforms.Length > 1;
        }
    }
}
