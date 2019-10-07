using System;
using UnityEngine;
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
            var color32 = color;
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

            vh.AddVert(Vector2.zero, color32, new Vector2(0, 1));

            for (int i = 0; i < m_Side; i++)
            {
                Vector2 a = points[i];

                vh.AddVert(a, color32, new Vector2(0, 1));
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
            var color32 = color;
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
                Vector2 a = points[i];

                vh.AddVert(a, color32, new Vector2(0, 1));
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

        protected void ChangeSideCount()
        {

        }
    }
}
