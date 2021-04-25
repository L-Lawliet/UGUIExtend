using System;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

/// <summary>
///
/// name:RegularPolygon
/// author:Lawliet
/// date:2018/10/3 21:07:32
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUI.Components
{
    public class RegularPolygon : MaskableGraphic, ICanvasRaycastFilter
    {
        [SerializeField]
        protected Sprite m_OverrideSprite;

        public Sprite overrideSprite
        {
            get
            {
                return m_OverrideSprite;
            }
            set
            {
                m_OverrideSprite = value;

                this.SetAllDirty();
            }
        }

        [SerializeField]
        protected uint m_Side = 3;

        public uint side
        {
            set
            {
                m_Side = value;

                ChangeSideCount();

                base.SetVerticesDirty();
            }
            get
            {
                return m_Side;
            }
        }

        [SerializeField]
        [Range(0.0f, 0.999f)]
        protected float m_InnerPercent = 0.0f;

        public float innerPercent
        {
            set
            {
                m_InnerPercent = value;
                base.SetVerticesDirty();
            }
            get
            {
                return m_InnerPercent;
            }
        }

        public override Texture mainTexture
        {
            get
            {
                if (overrideSprite == null)
                {
                    if (material != null && material.mainTexture != null)
                    {
                        return material.mainTexture;
                    }
                    return s_WhiteTexture;
                }

                return overrideSprite.texture;
            }
        }

        public RegularPolygon()
        {
            ChangeSideCount();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (m_InnerPercent == 0)
            {
                DrawPolygon(vh);
            }
            else
            {
                DrawRing(vh);
            }
        }

        private void DrawPolygon(VertexHelper vh)
        {
            Rect rect = this.GetPixelAdjustedRect();

            Vector4 outer = new Vector4();

            if (overrideSprite != null)
            {
                outer = DataUtility.GetOuterUV(overrideSprite);
            }


            vh.Clear();

            float size = Mathf.Min(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);

            float angle = 360.0f / m_Side;

            Vector2[] points = new Vector2[m_Side];

            for (int i = 0; i < m_Side; i++)
            {
                Vector2 point = new Vector2();

                float radius = size * 0.5f;

                point.x = Mathf.Cos((angle * i + 90) * Mathf.Deg2Rad) * radius;
                point.y = Mathf.Sin((angle * i + 90) * Mathf.Deg2Rad) * radius;

                points[i] = point;
            }

            UIVertex center = GetVertex(Vector2.zero, rect, outer);

            vh.AddVert(center);

            for (int i = 0; i < m_Side; i++)
            {
                UIVertex a = GetVertex(points[i], rect, outer);

                vh.AddVert(a);
            }

            for (int i = 0; i < m_Side; i++)
            {
                int a = i + 1;
                int b = i + 2;

                if (i == m_Side - 1)
                {
                    b = 1;
                }

                vh.AddTriangle(0, b, a);
            }
        }

        private void DrawRing(VertexHelper vh)
        {
            Rect rect = this.GetPixelAdjustedRect();

            Vector4 outer = new Vector4();

            if (overrideSprite != null)
            {
                outer = DataUtility.GetOuterUV(overrideSprite);
            }

            vh.Clear();

            float size = Mathf.Min(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);

            float angle = 360.0f / m_Side;

            uint len = m_Side * 2;

            int sideCount = (int)m_Side;

            Vector2[] points = new Vector2[len];

            for (int i = 0; i < sideCount; i++)
            {
                Vector2 point = new Vector2();

                float outerRadius = size * 0.5f;
                float innerRadius = size * 0.5f * m_InnerPercent;

                ///添加外点
                point.x = Mathf.Cos((angle * i + 90) * Mathf.Deg2Rad) * outerRadius;
                point.y = Mathf.Sin((angle * i + 90) * Mathf.Deg2Rad) * outerRadius;

                points[i] = point;

                ///添加内点
                point.x = Mathf.Cos((angle * i + 90) * Mathf.Deg2Rad) * innerRadius;
                point.y = Mathf.Sin((angle * i + 90) * Mathf.Deg2Rad) * innerRadius;

                points[i + sideCount] = point;
            }

            for (int i = 0; i < len; i++)
            {
                UIVertex a = GetVertex(points[i], rect, outer);

                vh.AddVert(a);
            }

            for (int i = 0; i < sideCount; i++)
            {
                int a = i + 0;
                int b = i + 1;
                int c = i + 0 + sideCount;
                int d = i + 1 + sideCount;

                if (i == sideCount - 1)
                {
                    b = 0;
                    d = sideCount;
                }

                vh.AddTriangle(c, b, a);
                vh.AddTriangle(b, d, c);
            }
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            return false;
        }

        private UIVertex GetVertex(Vector2 vector, Rect rect, Vector4 inner)
        {
            UIVertex vertex = new UIVertex();
            vertex.position = vector;
            vertex.color = color;
            vertex.normal = new Vector3(0, 0, -1);

            float u = (vertex.position.x - rect.x) / rect.width * (inner.z - inner.x) + inner.x;
            float v = (vertex.position.y - rect.y) / rect.height * (inner.w - inner.y) + inner.y;

            vertex.uv0 = new Vector2(u, v);

            return vertex;
        }

        protected void ChangeSideCount()
        {

        }
    }
}
