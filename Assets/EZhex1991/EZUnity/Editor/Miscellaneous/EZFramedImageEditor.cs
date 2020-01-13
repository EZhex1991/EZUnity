/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-01-13 10:22:10
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZFramedImage))]
    public class EZFramedImageEditor : ImageEditor
    {
        private EZFramedImage framedImage;

        private SerializedProperty m_Sprite;
        //private SerializedProperty m_Color;
        //private SerializedProperty m_Material;
        //private SerializedProperty m_RaycastTarget;
        private SerializedProperty m_FillAmount;
        private SerializedProperty m_FillCenter;

        protected override void OnEnable()
        {
            base.OnEnable();
            framedImage = target as EZFramedImage;
            m_Sprite = serializedObject.FindProperty("m_Sprite");
            m_FillAmount = serializedObject.FindProperty("m_FillAmount");
            m_FillCenter = serializedObject.FindProperty("m_FillCenter");
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.MonoBehaviourTitle(framedImage);
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_Sprite);
            EditorGUILayout.PropertyField(m_Color);
            EditorGUILayout.PropertyField(m_Material);
            EditorGUILayout.PropertyField(m_RaycastTarget);
            EditorGUILayout.PropertyField(m_FillAmount);
            EditorGUILayout.PropertyField(m_FillCenter);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
