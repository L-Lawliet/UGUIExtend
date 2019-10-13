using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
using Waiting.UGUI.Collections;

/// <summary>
///
/// name:Mirror
/// author:Administrator
/// date:2017/1/19 11:53:01
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUI.Effects
{
    [AddComponentMenu("UI/Effects/PolygonMask", 21)]
    [RequireComponent(typeof(Graphic))]
    public class PolygonMask : BaseMeshEffect
    {
        [SerializeField]
        private PolygonCollider2D m_PolygonCollider2D;

        public PolygonCollider2D polygonCollider2D
        {
            get { return m_PolygonCollider2D; }
            set
            {
                if (m_PolygonCollider2D != value)
                {
                    m_PolygonCollider2D = value;

                    if (graphic != null)
                    {
                        graphic.SetVerticesDirty();
                    }
                }
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            if (m_PolygonCollider2D == null)
            {
                return;
            }

            var original = ListPool<UIVertex>.Get();
            var output = ListPool<UIVertex>.Get();
            vh.GetUIVertexStream(original);

            int count = original.Count;

            Draw(original, output, count);

            vh.Clear();
            vh.AddUIVertexTriangleStream(output);

            ListPool<UIVertex>.Recycle(output);
        }


        /// <summary>
        /// 5
        /// 
        /// 0-1-2,2-3-4,4-0(5)-2(6+1)
        /// </summary>
        /// <param name="original"></param>
        /// <param name="output"></param>
        /// <param name="count"></param>
        private void Draw(List<UIVertex> original, List<UIVertex> output, int count)
        {
            var len = m_PolygonCollider2D.points.Length;

            var k = 0;

            for (int i = 0; i < len-2; i++)
            {
                output.Add(GetVertex(0));
                output.Add(GetVertex(i+1));
                output.Add(GetVertex(i+2));

            }
        }

        private UIVertex GetVertex(int index)
        {
            UIVertex v = new UIVertex();
            v.position = m_PolygonCollider2D.points[index];
            v.color = Color.white;
            v.normal = new Vector3(0, 0, -1);
            v.uv0 = Vector2.zero;

            return v;
        }
    }
}
