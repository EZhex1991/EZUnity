/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-22 19:09:42
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZColorBlender : EditorWindow
    {
        public bool useHDR;

        public Color srcColor = Color.white;
        public Color dstColor = Color.grey;

        public Dictionary<string, Color> resultColors = new Dictionary<string, Color>();
        public Dictionary<string, string> resultStrings = new Dictionary<string, string>();

        private void OnEnable()
        {
            CalculateAll();
        }
        private void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this as EditorWindow);

            EditorGUILayout.LabelField("Factors", EditorStyles.boldLabel);
            useHDR = EditorGUILayout.Toggle("Use HDR", useHDR);
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Source Color");
                EditorGUILayout.LabelField("Destination Color");
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                srcColor = EditorGUILayout.ColorField(GUIContent.none, srcColor);
                dstColor = EditorGUILayout.ColorField(GUIContent.none, dstColor);
                EditorGUILayout.EndHorizontal();
            }

            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.EndHorizontal();
            }

            float width = Mathf.Max(50, EditorGUIUtility.currentViewWidth / 4 - 5);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Results", EditorStyles.boldLabel);

            DrawResultsSingleLine(width, "Add", "Minus", "Multiply", "Divide");
            DrawResultsSingleLine(width, "Opacity", "Darken", "Lighten", "Screen");
            DrawResultsSingleLine(width, "ColorBurn", "ColorDodge", "LinearBurn", "LinearDodge");
            DrawResultsSingleLine(width, "Overlay", "HardLight", "SoftLight", "VividLight");
            DrawResultsSingleLine(width, "LinearLight", "PinLight", "HardMix", "Difference");
            DrawResultsSingleLine(width, "Exclusion");
            DrawResultsSingleLine(width, "HSVBlend_H", "HSVBlend_S", "HSVBlend_V", "HSVBlend_HS");

            if (GUI.changed) CalculateAll();
        }

        private void DrawResultsSingleLine(float width, params string[] resultNames)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < resultNames.Length; i++)
            {
                string name = resultNames[i];
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField(name, GUILayout.Width(width));
#if UNITY_2018_1_OR_NEWER
                EditorGUILayout.ColorField(GUIContent.none, resultColors[name], false, false, useHDR, GUILayout.Width(width));
#else
                EditorGUILayout.ColorField(GUIContent.none, resultColors[name], false, false, useHDR, null, GUILayout.Width(width));
#endif
                EditorGUILayout.TextField(GUIContent.none, resultStrings[name], GUILayout.Width(width));
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        public void CalculateAll()
        {
            Calculate("Add", Add(srcColor, dstColor));
            Calculate("Minus", Minus(srcColor, dstColor));
            Calculate("Multiply", Multiply(srcColor, dstColor));
            Calculate("Divide", Divide(srcColor, dstColor));
            Calculate("Opacity", Opacity(srcColor, dstColor));
            Calculate("Darken", Darken(srcColor, dstColor));
            Calculate("Lighten", Lighten(srcColor, dstColor));
            Calculate("Screen", Screen(srcColor, dstColor));
            Calculate("ColorBurn", ColorBurn(srcColor, dstColor));
            Calculate("ColorDodge", ColorDodge(srcColor, dstColor));
            Calculate("LinearBurn", LinearBurn(srcColor, dstColor));
            Calculate("LinearDodge", LinearDodge(srcColor, dstColor));
            Calculate("Overlay", Overlay(srcColor, dstColor));
            Calculate("HardLight", HardLight(srcColor, dstColor));
            Calculate("SoftLight", SoftLight(srcColor, dstColor));
            Calculate("VividLight", VividLight(srcColor, dstColor));
            Calculate("LinearLight", LinearLight(srcColor, dstColor));
            Calculate("PinLight", PinLight(srcColor, dstColor));
            Calculate("HardMix", HardMix(srcColor, dstColor));
            Calculate("Difference", Difference(srcColor, dstColor));
            Calculate("Exclusion", Exclusion(srcColor, dstColor));
            Vector3 srcHSV;
            Vector3 dstHSV;
            Color.RGBToHSV(srcColor, out srcHSV.x, out srcHSV.y, out srcHSV.z);
            Color.RGBToHSV(dstColor, out dstHSV.x, out dstHSV.y, out dstHSV.z);
            Calculate("HSVBlend_H", HSVBlend_H(srcHSV, dstHSV));
            Calculate("HSVBlend_S", HSVBlend_S(srcHSV, dstHSV));
            Calculate("HSVBlend_V", HSVBlend_V(srcHSV, dstHSV));
            Calculate("HSVBlend_HS", HSVBlend_HS(srcHSV, dstHSV));
        }
        public void Calculate(string name, Color color)
        {
            resultColors[name] = color;
            resultStrings[name] = ColorUtility.ToHtmlStringRGBA(color);
        }

        public static Color CalcPerComponent(Color src, Color dst, Func<float, float, float> func)
        {
            return new Color
            (
                func(src.r, dst.r),
                func(src.g, dst.g),
                func(src.b, dst.b),
                func(src.a, dst.a)
            );
        }

        public static Color Inverse(Color src)
        {
            return new Color(1 - src.r, 1 - src.g, 1 - src.b, 1 - src.a);
        }

        public static Color Add(Color src, Color dst)
        {
            return src + dst;
        }
        public static Color Minus(Color src, Color dst)
        {
            return src - dst;
        }
        public static Color Multiply(Color src, Color dst)
        {
            return src * dst;
        }
        public static Color Divide(Color src, Color dst)
        {
            return CalcPerComponent(src, dst, (a, b) => a / b);
        }
        public static Color Min(Color src, Color dst)
        {
            return CalcPerComponent(src, dst, (a, b) => Mathf.Min(a, b));
        }
        public static Color Max(Color src, Color dst)
        {
            return CalcPerComponent(src, dst, (a, b) => Mathf.Max(a, b));
        }

        // 不透明度
        public static Color Opacity(Color src, Color dst)
        {
            return src.a * src + (1 - src.a) * dst;
        }
        // 变暗
        public static Color Darken(Color src, Color dst)
        {
            return Min(src, dst);
        }
        // 变亮
        public static Color Lighten(Color src, Color dst)
        {
            return Max(src, dst);
        }
        // 滤色
        public static Color Screen(Color src, Color dst)
        {
            return Inverse(Inverse(src) * Inverse(dst));
        }
        // 颜色加深
        public static Color ColorBurn(Color src, Color dst)
        {
            return src - Divide(Inverse(src) * Inverse(dst), dst);
        }
        // 颜色减淡
        public static Color ColorDodge(Color src, Color dst)
        {
            return src + Divide(src * dst, Inverse(dst));
        }
        // 线性加深
        public static Color LinearBurn(Color src, Color dst)
        {
            return src + dst - Color.white;
        }
        // 线性减淡
        public static Color LinearDodge(Color src, Color dst)
        {
            return src + dst;
        }
        // 叠加
        public static Color Overlay(Color src, Color dst)
        {
            return CalcPerComponent(src, dst, (a, b) =>
            {
                if (a <= 0.5f) return a * b;
                else return 1 - (1 - a) * (1 - b) * 2;
            });
        }
        // 强光
        public static Color HardLight(Color src, Color dst)
        {
            return CalcPerComponent(src, dst, (a, b) =>
            {
                if (b <= 0.5f) return a * b * 2;
                else return 1 - (1 - a) * (1 - b) * 2;
            });
        }
        // 柔光
        public static Color SoftLight(Color src, Color dst)
        {
            return CalcPerComponent(src, dst, (a, b) =>
            {
                if (b <= 0.5f) return a * b * 2 + a * a * (1 - 2 * b);
                else return a * (1 - b) * 2 + Mathf.Sqrt(a * (2 * b - 1));
            });
        }
        // 亮光
        public static Color VividLight(Color src, Color dst)
        {
            return CalcPerComponent(src, dst, (a, b) =>
            {
                if (b <= 0.5f) return a - (1 - a) * (1 - 2 * b) / (2 * b);
                else return a + a * (2 * b - 1) / (2 * (1 - b));
            });
        }
        // 线性光
        public static Color LinearLight(Color src, Color dst)
        {
            return src - 2 * dst - Color.white;
        }
        // 点光
        public static Color PinLight(Color src, Color dst)
        {
            return CalcPerComponent(src, dst, (a, b) =>
            {
                if (b <= 0.5f) return Mathf.Min(a, 2 * b);
                else return Mathf.Min(a, 2 * b - 1);
            });
        }
        // 实色混合
        public static Color HardMix(Color src, Color dst)
        {
            return CalcPerComponent(src, dst, (a, b) =>
            {
                return a < 1 - b ? 0 : 1;
            });
        }
        // 差值
        public static Color Difference(Color src, Color dst)
        {
            return CalcPerComponent(src, dst, (a, b) =>
            {
                return Mathf.Abs(a - b);
            });
        }
        // 排除
        public static Color Exclusion(Color src, Color dst)
        {
            return CalcPerComponent(src, dst, (a, b) =>
            {
                return a + b - a * b * 2;
            });
        }

        public static Color HSVBlend_H(Vector3 srcHSV, Vector3 dstHSV)
        {
            return Color.HSVToRGB(dstHSV.x, srcHSV.y, srcHSV.z);
        }
        public static Color HSVBlend_S(Vector3 srcHSV, Vector3 dstHSV)
        {
            return Color.HSVToRGB(srcHSV.x, dstHSV.y, srcHSV.z);
        }
        public static Color HSVBlend_V(Vector3 srcHSV, Vector3 dstHSV)
        {
            return Color.HSVToRGB(srcHSV.x, srcHSV.y, dstHSV.z);
        }
        public static Color HSVBlend_HS(Vector3 srcHSV, Vector3 dstHSV)
        {
            return Color.HSVToRGB(dstHSV.x, dstHSV.y, srcHSV.z);
        }
    }
}
