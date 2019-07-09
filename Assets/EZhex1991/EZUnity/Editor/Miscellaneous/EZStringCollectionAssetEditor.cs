/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-07 18:39:48
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZStringCollectionAsset))]
    public class EZStringCollectionAssetEditor : EZDictionaryAssetEditor
    {
        private EZStringCollectionAsset stringCollection;

        protected override void OnEnable()
        {
            base.OnEnable();
            stringCollection = target as EZStringCollectionAsset;
            itemList.elementHeightCallback = GetItemHeight;
        }

        protected override bool IsKeyDuplicate(SerializedProperty keyProperty)
        {
            return stringCollection.IsKeyDuplicate(keyProperty.stringValue);
        }

        private float GetItemHeight(int index)
        {
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 5;
        }

        protected override void SplitRect(Rect rect, out Rect keyRect, out Rect valueRect)
        {
            keyRect = valueRect = new Rect(rect);
            float gridWidth = rect.width / 5; float margin = 5;
            keyRect.width = gridWidth - margin;
            keyRect.height = EditorGUIUtility.singleLineHeight;
            valueRect.x += gridWidth;
            valueRect.width = gridWidth * 4;
        }

        protected override void DrawValueLabel(Rect rect)
        {
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.width /= 2;
            EditorGUI.LabelField(rect, "CH");
            rect.x += rect.width;
            EditorGUI.LabelField(rect, "EN");
        }
        protected override void DrawValueProperty(Rect rect, SerializedProperty valueProperty)
        {
            SerializedProperty ch = valueProperty.FindPropertyRelative("m_CH");
            SerializedProperty en = valueProperty.FindPropertyRelative("m_EN");

            float width = rect.width / 2; float margin = 5;
            rect.width = width - margin;
            EditorGUI.PropertyField(rect, ch, GUIContent.none);
            rect.x += width;
            EditorGUI.PropertyField(rect, en, GUIContent.none);
        }
    }
}
