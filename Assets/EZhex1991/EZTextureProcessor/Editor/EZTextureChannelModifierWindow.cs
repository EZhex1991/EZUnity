/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-16 19:17:24
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    public class EZTextureChannelModifierWindow : EditorWindow
    {
        public EZTextureChannelModifier modifier;

        public Texture2D[] selectedTextures;

        private void RefreshSelectedTextures()
        {
            selectedTextures = Selection.GetFiltered<Texture2D>(SelectionMode.Assets | SelectionMode.Editable);
        }
        private void OverrideTexture(Texture2D texture)
        {
            string filePath = AssetDatabase.GetAssetPath(texture);
            string extension = Path.GetExtension(filePath).ToLower();
            if (extension == ".png")
            {
                File.WriteAllBytes(filePath, modifier.ResampleTexture(texture).Encode(TextureEncoding.PNG));
            }
            else if (extension == ".jpg")
            {
                File.WriteAllBytes(filePath, modifier.ResampleTexture(texture).Encode(TextureEncoding.JPG));
            }
            else if (extension == ".tga")
            {
                File.WriteAllBytes(filePath, modifier.ResampleTexture(texture).Encode(TextureEncoding.TGA));
            }
            else
            {
                Debug.LogError("Unknown texture extension, supported extensions are {'png'|'jpg'|'tga'}");
            }
        }

        private void OnEnable()
        {
            RefreshSelectedTextures();
        }
        private void OnGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(this as ScriptableObject), typeof(MonoScript), false);
            GUI.enabled = true;

            modifier = (EZTextureChannelModifier)EditorGUILayout.ObjectField("Channel Modifier", modifier, typeof(EZTextureChannelModifier), false);
            if (modifier == null) return;

            EditorGUILayout.Space();
            GUI.enabled = false;
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.EnumPopup("Channel R", modifier.overrideChannelR);
                EditorGUILayout.CurveField(modifier.overrideCurveR);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.EnumPopup("Channel G", modifier.overrideChannelG);
                EditorGUILayout.CurveField(modifier.overrideCurveG);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.EnumPopup("Channel B", modifier.overrideChannelB);
                EditorGUILayout.CurveField(modifier.overrideCurveB);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.EnumPopup("Channel A", modifier.overrideChannelA);
                EditorGUILayout.CurveField(modifier.overrideCurveA);
                EditorGUILayout.EndHorizontal();
            }
            GUI.enabled = true;

            EditorGUILayout.Space();
            if (selectedTextures == null || selectedTextures.Length == 0)
            {
                EditorGUILayout.HelpBox("No Editable Texture Selected", MessageType.Info);
            }
            else
            {
                if (GUILayout.Button("Apply Modifier To Textures"))
                {
                    for (int i = 0; i < selectedTextures.Length; i++)
                    {
                        OverrideTexture(selectedTextures[i]);
                    }
                    AssetDatabase.Refresh();
                }

                for (int i = 0; i < selectedTextures.Length; i++)
                {
                    EditorGUILayout.ObjectField(selectedTextures[i], typeof(Texture2D), false);
                }
            }
        }
        private void OnSelectionChange()
        {
            RefreshSelectedTextures();
            Repaint();
        }
    }
}
