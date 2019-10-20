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

        [SerializeField]
        private int _drawIndex;

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

            ListPool<UIVertex>.Recycle(original);
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

            /*for (int i = 0; i < len-2; i++)
            {
                output.Add(GetVertex(0));
                output.Add(GetVertex(i+1));
                output.Add(GetVertex(i+2));

            }*/

            //return;

            List<int> indexList = new List<int>(len);

            for (int i = len - 1; i >= 0; i--)
            {
                indexList.Add(i);

                /*if (i == _drawIndex)
                {
                    output.Add(GetVertex(0));
                    output.Add(GetVertex(i + 1));
                    output.Add(GetVertex(i + 2));
                }*/
            }

            //return;

            while (indexList.Count > 2 && indexList.Count > _drawIndex)
            {
                int i;

                len = indexList.Count;

                bool isLeft = false;

                for (i = 0; i < len; i++)
                {
                    int p = indexList[(i + 0) % len];
                    int s = indexList[(i + 1) % len];
                    int q = indexList[(i + 2) % len];

                    if (len == 3)  //只剩下三个点了,直接绘制
                    {
                        output.Add(GetVertex(p));
                        output.Add(GetVertex(s));
                        output.Add(GetVertex(q));

                        indexList.RemoveAt(i + 1);

                        break;
                    }

                    isLeft = ToLeftTest(m_PolygonCollider2D.points, p, q, s);

                    if (isLeft) // s在左边，表示为嘴巴,对上一个三角形切耳
                    {
                        p = indexList[(i + len - 1) % len];
                        s = indexList[(i + 0) % len];
                        q = indexList[(i + 1) % len];

                        output.Add(GetVertex(p));
                        output.Add(GetVertex(s));
                        output.Add(GetVertex(q));

                        indexList.RemoveAt(i);

                        break;
                    }
                }

                if (!isLeft) //没有嘴巴，直接绘制
                {
                    for (i = 0; i < len - 2; i++)
                    {
                        int p = indexList[0];
                        int s = indexList[(i + 1) % len];
                        int q = indexList[(i + 2) % len];

                        output.Add(GetVertex(p));
                        output.Add(GetVertex(s));
                        output.Add(GetVertex(q));

                    }

                    break;
                }
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

        private bool ToLeftTest(Vector2[] points, int pIndex, int qIndex, int sIndex)
        {
            return ToLeftTest(points[pIndex], points[qIndex], points[sIndex]);
        }

        private bool ToLeftTest(Vector2 p, Vector2 q, Vector2 s)
        {
            return Area2(p,  q,  s) > 0;
        }

        private float Area2(Vector2 p, Vector2 q, Vector2 s)
        {
            return p.x * q.y - p.y * q.x + q.x * s.y - q.y * s.x + s.x * p.y - s.y * p.x;
        }
    }
}
