using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// name:Gradient
/// author:Lawliet
/// date:2019/11/7 10:18:04
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUIEditor
{
    [AddComponentMenu("UI/Effects/Gradient")]
    [RequireComponent(typeof(Graphic))]
    public class Gradient : BaseMeshEffect
    {
        [SerializeField]
        private Color32 m_StartColor = Color.white;

        public Color32 startColor
        {
            get { return m_StartColor; }
            set
            {
                m_StartColor = value;
            }
        }

        [SerializeField]
        private Color32 m_EndColor = Color.black;

        public Color32 endColor
        {
            get { return m_EndColor; }
            set
            {
                m_EndColor = value;
            }
        }

        [SerializeField]
        private float m_Angle = 0;

        public float angle
        {
            get { return m_Angle; }
            set
            {
                m_Angle = value;
            }
        }

        [SerializeField]
        private float m_Percent = 0;

        public float percent
        {
            get { return m_Percent; }
            set
            {
                m_Percent = value;
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            var count = vh.currentVertCount;
            if (count == 0)
            {
                return;
            }

            List<UIVertex> vertexs = new List<UIVertex>();

            vh.GetUIVertexStream(vertexs);

            Rect rect = graphic.GetPixelAdjustedRect();

            count = vertexs.Count;

            angle = angle % 360.0f;

            for (var i = 0; i < count; i++)
            {
                var vertex = vertexs[i];

                byte alpha = vertex.color.a;

                Color32 color;

                if (angle >= 0 && angle < 90)
                {
                    color = Color32.Lerp(m_StartColor, m_EndColor, (vertexs[i].position.x - rect.xMin) / rect.width);
                }
                else if (angle >= 90 && angle < 180)
                {
                    color = Color32.Lerp(m_EndColor, m_StartColor, (vertexs[i].position.y - rect.yMin) / rect.height);
                }
                else if (angle >= 180 && angle < 270)
                {
                    color = Color32.Lerp(m_EndColor, m_StartColor, (vertexs[i].position.x - rect.xMin) / rect.width);
                }
                else
                {
                    color = Color32.Lerp(m_StartColor, m_EndColor, (vertexs[i].position.y - rect.yMin) / rect.height);
                }

                color.a = (byte)(color.a * alpha / byte.MaxValue);

                vertex.color = color;

                vertexs[i] = vertex;
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(vertexs);
        }
    }
}
