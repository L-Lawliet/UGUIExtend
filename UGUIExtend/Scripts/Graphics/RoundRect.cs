using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Sprites;
using UnityEngine.UI;

/// <summary>
///
/// name:RoundRect
/// author:Waiting
/// date:2017/10/17 14:45:36
/// versions:
/// introduce:
/// note:
/// 圆角矩形
/// </summary>
namespace Waiting.UGUI.Graphics
{
    public class RoundRect : Image
    {
        /// <summary>
        /// 圆角半径
        /// </summary>
        [SerializeField]
        private float m_Radius = 4;

        public float radius
        {
            get { return m_Radius; }
            set
            {
                if (m_Radius != value)
                {
                    m_Radius = value;

                    this.SetVerticesDirty();
                }
            }
        }

        public override void SetVerticesDirty()
        {
            if (type != Type.Sliced)
            {
                type = Type.Sliced;
            }

            base.SetVerticesDirty();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (type != Type.Sliced)
            {
                type = Type.Sliced;
            }

            base.OnValidate();
        }

#endif

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (overrideSprite == null)
            {
                base.OnPopulateMesh(toFill);
                return;
            }

            if (!hasBorder)
            {
                GenerateSimpleSprite(toFill, false);
            }
            else
            {
                GenerateSlicedSprite(toFill);
            }
        }

        /// <summary>
        /// Generate vertices for a simple Image.
        /// </summary>
        protected void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
        {
            Vector4 v = GetDrawingDimensions(lPreserveAspect);
            var uv = (overrideSprite != null) ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;

            var color32 = color;
            vh.Clear();
            vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(uv.x, uv.y));
            vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(uv.x, uv.w));
            vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(uv.z, uv.w));
            vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(uv.z, uv.y));

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        static readonly Vector2[] s_VertScratch = new Vector2[4];
        static readonly Vector2[] s_UVScratch = new Vector2[4];

        protected void GenerateSlicedSprite(VertexHelper toFill)
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
                outer = Vector4.zero;
                inner = Vector4.zero;
                padding = Vector4.zero;
                border = Vector4.zero;
            }

            Rect rect = GetPixelAdjustedRect();

            border = GetAdjustedBorders(border / pixelsPerUnit, rect);

            padding = padding / pixelsPerUnit;

            s_VertScratch[0] = new Vector2(padding.x, padding.y);
            s_VertScratch[3] = new Vector2(rect.width - padding.z, rect.height - padding.w);

            s_VertScratch[1].x = border.x;
            s_VertScratch[1].y = border.y;
            s_VertScratch[2].x = rect.width - border.z;
            s_VertScratch[2].y = rect.height - border.w;

            for (int i = 0; i < 4; ++i)
            {
                s_VertScratch[i].x = s_VertScratch[i].x * 0.5f + rect.x;
                s_VertScratch[i].y = s_VertScratch[i].y * 0.5f + rect.y;
            }

            s_UVScratch[0] = new Vector2(outer.x, outer.y);
            s_UVScratch[1] = new Vector2(inner.x, inner.y);
            s_UVScratch[2] = new Vector2(inner.z, inner.w);
            s_UVScratch[3] = new Vector2(outer.z, outer.w);

            toFill.Clear();

            for (int x = 0; x < 2; x++)
            {
                int x2 = x + 1;

                for (int y = 0; y < 2; y++)
                {
                    if (!fillCenter && x == 1 && y == 1)
                    {
                        continue;
                    }
                        
                    int y2 = y + 1;

                    AddQuad(toFill,
                        new Vector2(s_VertScratch[x].x, s_VertScratch[y].y),
                        new Vector2(s_VertScratch[x2].x, s_VertScratch[y2].y),
                        color,
                        new Vector2(s_UVScratch[x].x, s_UVScratch[y].y),
                        new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y));

                    AddQuad(toFill,
                        new Vector2(-s_VertScratch[x].x, s_VertScratch[y].y),
                        new Vector2(-s_VertScratch[x2].x, s_VertScratch[y2].y),
                        color,
                        new Vector2(s_UVScratch[x].x, s_UVScratch[y].y),
                        new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y));

                    AddQuad(toFill,
                        new Vector2(s_VertScratch[x].x, -s_VertScratch[y].y),
                        new Vector2(s_VertScratch[x2].x, -s_VertScratch[y2].y),
                        color,
                        new Vector2(s_UVScratch[x].x, s_UVScratch[y].y),
                        new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y));

                    AddQuad(toFill,
                        new Vector2(-s_VertScratch[x].x, -s_VertScratch[y].y),
                        new Vector2(-s_VertScratch[x2].x, -s_VertScratch[y2].y),
                        color,
                        new Vector2(s_UVScratch[x].x, s_UVScratch[y].y),
                        new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y));
                }
            }
        }

        protected Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
        {
            var padding = overrideSprite == null ? Vector4.zero : DataUtility.GetPadding(overrideSprite);
            var size = overrideSprite == null ? Vector2.zero : new Vector2(overrideSprite.rect.width, overrideSprite.rect.height);

            Rect r = GetPixelAdjustedRect();
            // Debug.Log(string.Format("r:{2}, size:{0}, padding:{1}", size, padding, r));

            int spriteW = Mathf.RoundToInt(size.x);
            int spriteH = Mathf.RoundToInt(size.y);

            var v = new Vector4(
                    padding.x / spriteW,
                    padding.y / spriteH,
                    (spriteW - padding.z) / spriteW,
                    (spriteH - padding.w) / spriteH);

            if (shouldPreserveAspect && size.sqrMagnitude > 0.0f)
            {
                var spriteRatio = size.x / size.y;
                var rectRatio = r.width / r.height;

                if (spriteRatio > rectRatio)
                {
                    var oldHeight = r.height;
                    r.height = r.width * (1.0f / spriteRatio);
                    r.y += (oldHeight - r.height) * rectTransform.pivot.y;
                }
                else
                {
                    var oldWidth = r.width;
                    r.width = r.height * spriteRatio;
                    r.x += (oldWidth - r.width) * rectTransform.pivot.x;
                }
            }

            v = new Vector4(
                    r.x + r.width * v.x,
                    r.y + r.height * v.y,
                    r.x + r.width * v.z,
                    r.y + r.height * v.w
                    );

            return v;
        }

        protected Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
        {
            border.x = m_Radius;
            border.y = m_Radius;
            border.z = 0;
            border.w = 0;

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

        /// <summary>
        /// 绘制四边形
        /// </summary>
        /// <param name="vertexHelper"></param>
        /// <param name="posMin"></param>
        /// <param name="posMax"></param>
        /// <param name="color"></param>
        /// <param name="uvMin"></param>
        /// <param name="uvMax"></param>
        protected static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
        {
            //剔除无效的四边形
            if (posMin.x == posMax.x || posMin.y == posMax.y)
            {
                return;
            }

            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }
    }
}
