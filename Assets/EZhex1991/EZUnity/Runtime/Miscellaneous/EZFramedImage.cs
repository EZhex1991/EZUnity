/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-01-10 13:30:10
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace EZhex1991.EZUnity
{
    public class EZFramedImage : Image
    {
        private static readonly Vector2[] s_VertScratch = new Vector2[4];
        private static readonly Vector2[] s_UVScratch = new Vector2[4];
        private static void AddTriangle(VertexHelper vertexHelper,
            Vector2 vert0, Vector2 vert1, Vector2 vert2,
            Color32 color,
            Vector2 uv1, Vector2 uv2, Vector2 uv3
        )
        {
            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(vert0, color, uv1);
            vertexHelper.AddVert(vert1, color, uv2);
            vertexHelper.AddVert(vert2, color, uv3);

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
        }
        private static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax,
            Color32 color,
            Vector2 uvMin, Vector2 uvMax
        )
        {
            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            Vector4 outer, inner, padding, border;
            if (overrideSprite != null)
            {
                outer = DataUtility.GetOuterUV(overrideSprite);
                inner = DataUtility.GetInnerUV(overrideSprite);
                padding = DataUtility.GetPadding(overrideSprite);
                border = overrideSprite.border;
            }
            else
            {
                outer = inner = padding = border = Vector4.zero;
            }

            Rect rect = GetPixelAdjustedRect();
            border = GetAdjustedBorders(border / pixelsPerUnit, rect);
            padding = padding / pixelsPerUnit;

            s_VertScratch[0] = new Vector2(padding.x, padding.y);
            s_VertScratch[1] = new Vector2(border.x, border.y);
            s_VertScratch[2] = new Vector2(rect.width - border.z, rect.height - border.w);
            s_VertScratch[3] = new Vector2(rect.width - padding.z, rect.height - padding.w);
            float step1 = s_VertScratch[1].y / s_VertScratch[3].y;
            float step2 = s_VertScratch[2].y / s_VertScratch[3].y - step1;
            float step3 = 1 - step1 - step2;
            float step4 = s_VertScratch[1].x / s_VertScratch[3].x;
            float step5 = s_VertScratch[2].x / s_VertScratch[3].x - step4;
            float step6 = 1 - step4 - step5;

            for (int i = 0; i < 4; ++i)
            {
                s_VertScratch[i].x += rect.x;
                s_VertScratch[i].y += rect.y;
            }

            s_UVScratch[0] = new Vector2(outer.x, outer.y);
            s_UVScratch[1] = new Vector2(inner.x, inner.y);
            s_UVScratch[2] = new Vector2(inner.z, inner.w);
            s_UVScratch[3] = new Vector2(outer.z, outer.w);


            vh.Clear();

            if (fillCenter)
            {
                AddQuad(vh, 1, 1, 2, 2);
            }

            float fill = fillAmount * 2;
            if (fill < 0.001f) return;
            fill = Fill1(vh, fill, step1);
            {
                if (fill > 0)
                {
                    fill = Fill2(vh, fill, step2);
                    if (fill > 0)
                    {
                        fill = Fill3(vh, fill, step3);
                        if (fill > 0)
                        {
                            fill = Fill4(vh, fill, step4);
                            if (fill > 0)
                            {
                                fill = Fill5(vh, fill, step5);
                                if (fill > 0)
                                {
                                    fill = Fill6(vh, fill, step6);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void AddQuad(VertexHelper vh, int x1, int y1, int x2, int y2)
        {
            AddQuad(vh,
                new Vector2(s_VertScratch[x1].x, s_VertScratch[y1].y),
                new Vector2(s_VertScratch[x2].x, s_VertScratch[y2].y),
                color,
                new Vector2(s_UVScratch[x1].x, s_UVScratch[y1].y),
                new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y)
            );
        }
        private float Fill1(VertexHelper vh, float fill, float max)
        {
            if (fill >= max)
            {
                AddQuad(vh, 0, 0, 1, 1);
                return fill - max;
            }
            else
            {
                fill /= max;
                Vector2 vert0, vert1, vert2;
                Vector2 uv0, uv1, uv2;

                vert0 = s_VertScratch[0];
                vert1 = new Vector2(s_VertScratch[0].x, Mathf.Lerp(s_VertScratch[0].y, s_VertScratch[1].y, fill));
                vert2 = s_VertScratch[1];
                uv0 = s_UVScratch[0];
                uv1 = new Vector2(s_UVScratch[0].x, Mathf.Lerp(s_UVScratch[0].y, s_UVScratch[1].y, fill));
                uv2 = s_UVScratch[1];
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);

                vert0 = s_VertScratch[0];
                vert1 = s_VertScratch[1];
                vert2 = new Vector2(Mathf.Lerp(s_VertScratch[0].x, s_VertScratch[1].x, fill), s_VertScratch[0].y);
                uv0 = s_UVScratch[0];
                uv1 = s_UVScratch[1];
                uv2 = new Vector2(Mathf.Lerp(s_UVScratch[0].x, s_UVScratch[1].x, fill), s_UVScratch[0].y);
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);
                return 0;
            }
        }
        private float Fill2(VertexHelper vh, float fill, float max)
        {
            if (fill >= max)
            {
                AddQuad(vh, 0, 1, 1, 2);
                AddQuad(vh, 1, 0, 2, 1);
                return fill - max;
            }
            else
            {
                fill /= max;
                Vector2 vert0, vert1;
                Vector2 uv0, uv1;

                vert0 = new Vector2(s_VertScratch[0].x, s_VertScratch[1].y);
                vert1 = new Vector2(s_VertScratch[1].x, Mathf.Lerp(s_VertScratch[1].y, s_VertScratch[2].y, fill));
                uv0 = new Vector2(s_UVScratch[0].x, s_UVScratch[1].y);
                uv1 = new Vector2(s_UVScratch[1].x, Mathf.Lerp(s_UVScratch[1].y, s_UVScratch[2].y, fill));
                AddQuad(vh, vert0, vert1, color, uv0, uv1);

                vert0 = new Vector2(s_VertScratch[1].x, s_VertScratch[0].y);
                vert1 = new Vector2(Mathf.Lerp(s_VertScratch[1].x, s_VertScratch[2].x, fill), s_VertScratch[1].y);
                uv0 = new Vector2(s_UVScratch[1].x, s_UVScratch[0].y);
                uv1 = new Vector2(Mathf.Lerp(s_UVScratch[1].x, s_UVScratch[2].x, fill), s_UVScratch[1].y);
                AddQuad(vh, vert0, vert1, color, uv0, uv1);
                return 0;
            }
        }
        private float Fill3(VertexHelper vh, float fill, float max)
        {
            Vector2 vert0, vert1, vert2;
            Vector2 uv0, uv1, uv2;
            if (fill >= max)
            {
                vert0 = new Vector2(s_VertScratch[1].x, s_VertScratch[2].y);
                vert1 = new Vector2(s_VertScratch[0].x, s_VertScratch[2].y);
                vert2 = new Vector2(s_VertScratch[0].x, s_VertScratch[3].y);
                uv0 = new Vector2(s_UVScratch[1].x, s_UVScratch[2].y);
                uv1 = new Vector2(s_UVScratch[0].x, s_UVScratch[2].y);
                uv2 = new Vector2(s_UVScratch[0].x, s_UVScratch[3].y);
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);

                vert0 = new Vector2(s_VertScratch[2].x, s_VertScratch[1].y);
                vert1 = new Vector2(s_VertScratch[3].x, s_VertScratch[0].y);
                vert2 = new Vector2(s_VertScratch[2].x, s_VertScratch[0].y);
                uv0 = new Vector2(s_UVScratch[2].x, s_UVScratch[1].y);
                uv1 = new Vector2(s_UVScratch[3].x, s_UVScratch[0].y);
                uv2 = new Vector2(s_UVScratch[2].x, s_UVScratch[0].y);
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);
                return fill - max;
            }
            else
            {
                fill /= max;
                vert0 = new Vector2(s_VertScratch[1].x, s_VertScratch[2].y);
                vert1 = new Vector2(s_VertScratch[0].x, s_VertScratch[2].y);
                vert2 = new Vector2(s_VertScratch[0].x, Mathf.Lerp(s_VertScratch[2].y, s_VertScratch[3].y, fill));
                uv0 = new Vector2(s_UVScratch[1].x, s_UVScratch[2].y);
                uv1 = new Vector2(s_UVScratch[0].x, s_UVScratch[2].y);
                uv2 = new Vector2(s_UVScratch[0].x, Mathf.Lerp(s_UVScratch[2].y, s_UVScratch[3].y, fill));
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);

                vert0 = new Vector2(s_VertScratch[2].x, s_VertScratch[1].y);
                vert1 = new Vector2(Mathf.Lerp(s_VertScratch[2].x, s_VertScratch[3].x, fill), s_VertScratch[0].y);
                vert2 = new Vector2(s_VertScratch[2].x, s_VertScratch[0].y);
                uv0 = new Vector2(s_UVScratch[2].x, s_UVScratch[1].y);
                uv1 = new Vector2(Mathf.Lerp(s_UVScratch[2].x, s_UVScratch[3].x, fill), s_UVScratch[0].y);
                uv2 = new Vector2(s_UVScratch[2].x, s_UVScratch[0].y);
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);
                return 0;
            }
        }
        private float Fill4(VertexHelper vh, float fill, float max)
        {
            Vector2 vert0, vert1, vert2;
            Vector2 uv0, uv1, uv2;
            if (fill >= max)
            {
                vert0 = new Vector2(s_VertScratch[1].x, s_VertScratch[2].y);
                vert1 = new Vector2(s_VertScratch[0].x, s_VertScratch[3].y);
                vert2 = new Vector2(s_VertScratch[1].x, s_VertScratch[3].y);
                uv0 = new Vector2(s_UVScratch[1].x, s_UVScratch[2].y);
                uv1 = new Vector2(s_UVScratch[0].x, s_UVScratch[3].y);
                uv2 = new Vector2(s_UVScratch[1].x, s_UVScratch[3].y);
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);

                vert0 = new Vector2(s_VertScratch[2].x, s_VertScratch[1].y);
                vert1 = new Vector2(s_VertScratch[3].x, s_VertScratch[1].y);
                vert2 = new Vector2(s_VertScratch[3].x, s_VertScratch[0].y);
                uv0 = new Vector2(s_UVScratch[2].x, s_UVScratch[1].y);
                uv1 = new Vector2(s_UVScratch[3].x, s_UVScratch[1].y);
                uv2 = new Vector2(s_UVScratch[3].x, s_UVScratch[0].y);
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);
                return fill - max;
            }
            else
            {
                fill /= max;
                vert0 = new Vector2(s_VertScratch[1].x, s_VertScratch[2].y);
                vert1 = new Vector2(s_VertScratch[0].x, s_VertScratch[3].y);
                vert2 = new Vector2(Mathf.Lerp(s_VertScratch[0].x, s_VertScratch[1].x, fill), s_VertScratch[3].y);
                uv0 = new Vector2(s_UVScratch[1].x, s_UVScratch[2].y);
                uv1 = new Vector2(s_UVScratch[0].x, s_UVScratch[3].y);
                uv2 = new Vector2(Mathf.Lerp(s_UVScratch[0].x, s_UVScratch[1].x, fill), s_UVScratch[3].y);
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);

                vert0 = new Vector2(s_VertScratch[2].x, s_VertScratch[1].y);
                vert1 = new Vector2(s_VertScratch[3].x, Mathf.Lerp(s_VertScratch[0].y, s_VertScratch[1].y, fill));
                vert2 = new Vector2(s_VertScratch[3].x, s_VertScratch[0].y);
                uv0 = new Vector2(s_UVScratch[2].x, s_UVScratch[1].y);
                uv1 = new Vector2(s_UVScratch[3].x, Mathf.Lerp(s_UVScratch[0].y, s_UVScratch[1].y, fill));
                uv2 = new Vector2(s_UVScratch[3].x, s_UVScratch[0].y);
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);
                return 0;
            }
        }
        private float Fill5(VertexHelper vh, float fill, float max)
        {
            if (fill >= max)
            {
                AddQuad(vh, 1, 2, 2, 3);
                AddQuad(vh, 2, 1, 3, 2);
                return fill - max;
            }
            else
            {
                fill /= max;
                Vector2 vert0, vert1;
                Vector2 uv0, uv1;

                vert0 = new Vector2(s_VertScratch[1].x, s_VertScratch[2].y);
                vert1 = new Vector2(Mathf.Lerp(s_VertScratch[1].x, s_VertScratch[2].x, fill), s_VertScratch[3].y);
                uv0 = new Vector2(s_UVScratch[1].x, s_UVScratch[2].y);
                uv1 = new Vector2(Mathf.Lerp(s_UVScratch[1].x, s_UVScratch[2].x, fill), s_UVScratch[3].y);
                AddQuad(vh, vert0, vert1, color, uv0, uv1);

                vert0 = new Vector2(s_VertScratch[2].x, s_VertScratch[1].y);
                vert1 = new Vector2(s_VertScratch[3].x, Mathf.Lerp(s_VertScratch[1].y, s_VertScratch[2].y, fill));
                uv0 = new Vector2(s_UVScratch[2].x, s_UVScratch[1].y);
                uv1 = new Vector2(s_UVScratch[3].x, Mathf.Lerp(s_UVScratch[1].y, s_UVScratch[2].y, fill));
                AddQuad(vh, vert0, vert1, color, uv0, uv1);
                return 0;
            }
        }
        private float Fill6(VertexHelper vh, float fill, float max)
        {
            if (fill >= max)
            {
                AddQuad(vh, 2, 2, 3, 3);
                return fill - max;
            }
            else
            {
                fill /= max;
                Vector2 vert0, vert1, vert2;
                Vector2 uv0, uv1, uv2;

                vert0 = s_VertScratch[2];
                vert1 = new Vector2(s_VertScratch[2].x, s_VertScratch[3].y);
                vert2 = new Vector2(Mathf.Lerp(s_VertScratch[2].x, s_VertScratch[3].x, fill), s_VertScratch[3].y);
                uv0 = s_UVScratch[2];
                uv1 = new Vector2(s_UVScratch[2].x, s_UVScratch[3].y);
                uv2 = new Vector2(Mathf.Lerp(s_UVScratch[2].x, s_UVScratch[3].x, fill), s_UVScratch[3].y);
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);

                vert0 = s_VertScratch[2];
                vert1 = new Vector2(s_VertScratch[3].x, Mathf.Lerp(s_VertScratch[2].y, s_VertScratch[3].y, fill));
                vert2 = new Vector2(s_VertScratch[3].x, s_VertScratch[2].y);
                uv0 = s_UVScratch[2];
                uv1 = new Vector2(s_UVScratch[3].x, Mathf.Lerp(s_UVScratch[2].y, s_UVScratch[3].y, fill));
                uv2 = new Vector2(s_UVScratch[3].x, s_UVScratch[2].y);
                AddTriangle(vh, vert0, vert1, vert2, color, uv0, uv1, uv2);
                return 0;
            }
        }

        private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
        {
            for (int axis = 0; axis <= 1; axis++)
            {
                // If the rect is smaller than the combined borders, then there's not room for the borders at their normal size.
                // In order to avoid artefacts with overlapping borders, we scale the borders down to fit.
                float combinedBorders = border[axis] + border[axis + 2];
                if (rect.size[axis] < combinedBorders && combinedBorders != 0)
                {
                    float borderScaleRatio = rect.size[axis] / combinedBorders;
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }
            }
            return border;
        }

    }
}
