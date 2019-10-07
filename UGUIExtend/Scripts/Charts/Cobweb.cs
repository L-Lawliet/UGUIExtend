using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
///
/// name:Cobweb
/// author:Lawliet
/// date:2018/10/3 21:07:32
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUI.Charts
{
    public class Cobweb : MaskableGraphic, ICanvasRaycastFilter
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

        /// <summary>
        /// 限制最小百分比
        /// 1.当percent为0时，则绘制的范围为minPercent，不至于绘制不出来
        /// 2.使效果更加丰满
        /// </summary>
        [SerializeField]
        [Range(0.01f, 0.3f)]
        protected float m_MinPercent = 0.01f;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        protected float[] m_Percents;

        public Cobweb()
        {
            ChangeSideCount();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var color32 = color;
            vh.Clear();

            float size = Mathf.Min(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);

            float angle = 360.0f / m_Side;

            Vector2[] points = new Vector2[m_Side];

            for (int i = 0; i < m_Side; i++)
            {
                Vector2 point = new Vector2();

                float radius = size * 0.5f * (m_MinPercent + (1 - m_MinPercent) * m_Percents[i]);

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

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            return false;
        }

        public void SetPercent(int index, float percent)
        {
            if (index > m_Percents.Length - 1)
            {
                return;
            }

            m_Percents[index] = percent;

            base.SetVerticesDirty();
        }

        public float GetPercent(int index)
        {
            if (index > m_Percents.Length - 1)
            {
                return 0;
            }

            return m_Percents[index];
        }


        protected void ChangeSideCount()
        {

        }
    }
}
