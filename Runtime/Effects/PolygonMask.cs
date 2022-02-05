using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
using Waiting.UGUI.Collections;
using Waiting.UGUI.Components;

/// <summary>
///
/// name:PolygonMask
/// author:Lawliet
/// date:2019/10/1 11:53:01
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
        public enum MaskType
        {
            /// <summary>
            /// RectTransform
            /// </summary>
            //Rect,
            /// <summary>
            /// 正多边形
            /// </summary>
            RegularPolygon,
            /// <summary>
            /// 不规则多边形
            /// </summary>
            Polygon,
        }

        /// <summary>
        /// 镜像类型
        /// </summary>
        [SerializeField]
        private MaskType m_MaskType = MaskType.Polygon;

        public MaskType maskType
        {
            get { return m_MaskType; }
            set
            {
                if (m_MaskType != value)
                {
                    m_MaskType = value;

                    if (graphic != null)
                    {
                        graphic.SetVerticesDirty();
                    }
                }
            }
        }

        [SerializeField]
        private RectTransform m_MaskRect;

        public RectTransform maskRect
        {
            get { return m_MaskRect == null ? this.transform.parent as RectTransform : m_MaskRect; }
            set
            {
                if (m_MaskRect != value)
                {
                    m_MaskRect = value;

                    if (graphic != null)
                    {
                        graphic.SetVerticesDirty();
                    }
                }
            }
        }

        [SerializeField]
        private RegularPolygon m_RegularPolygon;

        public RegularPolygon regularPolygon
        {
            get { return m_RegularPolygon; }
            set
            {
                if (m_RegularPolygon != value)
                {
                    m_RegularPolygon = value;

                    if (graphic != null)
                    {
                        graphic.SetVerticesDirty();
                    }
                }
            }
        }

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

        /// <summary>
        /// 采用局部坐标
        /// 不采用的话，会根据Mask的RectTransform做偏移
        /// </summary>
        [SerializeField]
        private bool m_IsLocal;

        public bool isLocal
        {
            get { return m_IsLocal; }
            set
            {
                if(m_IsLocal != value)
                {
                    m_IsLocal = value;

                    if(graphic != null)
                    {
                        graphic.SetVerticesDirty();
                    }
                }
            }
        }

        [NonSerialized]
        private RectTransform m_RectTransform;

        public RectTransform rectTransform
        {
            get { return m_RectTransform ?? (m_RectTransform = GetComponent<RectTransform>()); }
        }

        /*[SerializeField]     
        private int m_DrawStep;*/

#if UNITY_EDITOR
        public void SetDirty()
        {
            if (graphic != null)
            {
                graphic.SetVerticesDirty();
            }
        }
#endif

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            

            var original = ListPool<UIVertex>.Get();
            var output = ListPool<UIVertex>.Get();
            vh.GetUIVertexStream(original);

            int count = original.Count;

            switch (m_MaskType)
            {
                case MaskType.RegularPolygon:
                    if (regularPolygon != null)
                    {
                        DrawRegularPolygon(original, output, count);
                    }
                    else
                    {
                        return;
                    }
                    
                    break;
                case MaskType.Polygon:
                    if (m_PolygonCollider2D == null)
                    {
                        return;
                    }
                    DrawPolygon(original, output, count);
                    break;
                default:
                    break;
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(output);

            ListPool<UIVertex>.Recycle(original);
            ListPool<UIVertex>.Recycle(output);
        }

        private void DrawRect(List<UIVertex> original, List<UIVertex> output, int count)
        {
            Sprite overrideSprite = null;

            Rect rect = graphic.GetPixelAdjustedRect();

            Vector4 outer = new Vector4();

            if (graphic is Image)
            {
                overrideSprite = (graphic as Image).overrideSprite;

                if (overrideSprite != null)
                {
                    outer = DataUtility.GetOuterUV(overrideSprite);
                }
            }

            Vector2 offset = GetRectTransformOffset(rectTransform, maskRect);

            Vector2 v0 = new Vector2(maskRect.rect.xMin, maskRect.rect.yMin) - offset;
            Vector2 v1 = new Vector2(maskRect.rect.xMax, maskRect.rect.yMin) - offset;
            Vector2 v2 = new Vector2(maskRect.rect.xMax, maskRect.rect.yMax) - offset;
            Vector2 v3 = new Vector2(maskRect.rect.xMin, maskRect.rect.yMax) - offset;

            output.Add(GetVertex(v0, rect, overrideSprite, outer));
            output.Add(GetVertex(v1, rect, overrideSprite, outer));
            output.Add(GetVertex(v2, rect, overrideSprite, outer));

            output.Add(GetVertex(v0, rect, overrideSprite, outer));
            output.Add(GetVertex(v2, rect, overrideSprite, outer));
            output.Add(GetVertex(v3, rect, overrideSprite, outer));
        }

        private void DrawRegularPolygon(List<UIVertex> original, List<UIVertex> output, int count)
        {
            Sprite overrideSprite = null;

            Rect rect = graphic.GetPixelAdjustedRect();

            Vector4 inner = new Vector4();

            if (graphic is Image)
            {
                overrideSprite = (graphic as Image).overrideSprite;

                if (overrideSprite != null)
                {
                    inner = DataUtility.GetInnerUV(overrideSprite);
                }
            }

            uint side = regularPolygon.side;
            float innerPercent = regularPolygon.innerPercent;

            Vector2 offset = new Vector2();

            if (!m_IsLocal)
            {
                offset = GetRectTransformOffset(rectTransform, regularPolygon.rectTransform);
            }

            float size = Mathf.Min(regularPolygon.rectTransform.sizeDelta.x, regularPolygon.rectTransform.sizeDelta.y);

            float angle = 360.0f / side;

            uint len = side * 2;

            int sideCount = (int)side;

            Vector2[] points = new Vector2[len];

            for (int i = 0; i < sideCount; i++)
            {
                Vector2 point = new Vector2();

                float outerRadius = size * 0.5f;
                float innerRadius = size * 0.5f * innerPercent;

                ///添加外点
                point.x = Mathf.Cos((angle * i + 90) * Mathf.Deg2Rad) * outerRadius;
                point.y = Mathf.Sin((angle * i + 90) * Mathf.Deg2Rad) * outerRadius;

                points[i] = point;

                ///添加内点
                point.x = Mathf.Cos((angle * i + 90) * Mathf.Deg2Rad) * innerRadius;
                point.y = Mathf.Sin((angle * i + 90) * Mathf.Deg2Rad) * innerRadius;

                points[i + sideCount] = point;
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

                output.Add(GetVertex(points, c, offset, rect, overrideSprite, inner));
                output.Add(GetVertex(points, b, offset, rect, overrideSprite, inner));
                output.Add(GetVertex(points, a, offset, rect, overrideSprite, inner));

                output.Add(GetVertex(points, b, offset, rect, overrideSprite, inner));
                output.Add(GetVertex(points, d, offset, rect, overrideSprite, inner));
                output.Add(GetVertex(points, c, offset, rect, overrideSprite, inner));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="output"></param>
        /// <param name="count"></param>
        private void DrawPolygon(List<UIVertex> original, List<UIVertex> output, int count)
        {
            Vector2[] points = m_PolygonCollider2D.points;

            Sprite overrideSprite = null ;

            Rect rect = graphic.GetPixelAdjustedRect();

            Vector4 inner = new Vector4();

            if (graphic is Image)
            {
                overrideSprite = (graphic as Image).overrideSprite;

                if(overrideSprite != null)
                {
                    inner = DataUtility.GetInnerUV(overrideSprite);
                }
            }

            Vector2 offset = new Vector2();

            if (!m_IsLocal)
            {
                offset = GetRectTransformOffset(rectTransform, m_PolygonCollider2D.transform as RectTransform);
            }

            var len = points.Length;

            List<int> indexList = new List<int>(len);

#if UNITY_5  
            for (int i = len - 1; i >= 0; i--)
#else //Unity5 之后修改了PolygonCollider2D的绘制顺序
            for (int i = 0; i < len; i++)
#endif
            {
                indexList.Add(i);

            }

            while (indexList.Count > 2) //indexList.Count > points.Length - m_DrawStep
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
                        output.Add(GetVertex(points, p, offset, rect, overrideSprite, inner));
                        output.Add(GetVertex(points, s, offset, rect, overrideSprite, inner));
                        output.Add(GetVertex(points, q, offset, rect, overrideSprite, inner));

                        indexList.RemoveAt(i + 1);

                        isLeft = true;

                        break;
                    }

                    isLeft = ToLeftTest(m_PolygonCollider2D.points, p, q, s);

                    if (isLeft) // s在左边，表示为嘴巴,对上一个三角形切耳
                    {
                        p = indexList[(i + len - 1) % len];
                        s = indexList[(i + 0) % len];
                        q = indexList[(i + 1) % len];

                        output.Add(GetVertex(points, p, offset, rect, overrideSprite, inner));
                        output.Add(GetVertex(points, s, offset, rect, overrideSprite, inner));
                        output.Add(GetVertex(points, q, offset, rect, overrideSprite, inner));

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

                        output.Add(GetVertex(points, p, offset, rect, overrideSprite, inner));
                        output.Add(GetVertex(points, s, offset, rect, overrideSprite, inner));
                        output.Add(GetVertex(points, q, offset, rect, overrideSprite, inner));

                    }

                    break;
                }
            }
            
        }

        private UIVertex GetVertex(Vector2[] list, int index, Vector2 offset, Rect rect, Sprite overrideSprite, Vector4 inner)
        {
            return GetVertex(list[index] - offset, rect, overrideSprite, inner);
        }

        private UIVertex GetVertex(Vector2 vector, Rect rect, Sprite overrideSprite, Vector4 inner)
        {
            UIVertex vertex = new UIVertex();
            vertex.position = vector;
            vertex.color = graphic.color;
            vertex.normal = new Vector3(0, 0, -1);

            float u = (vertex.position.x - rect.x) / rect.width * (inner.z - inner.x) + inner.x;
            float v = (vertex.position.y - rect.y) / rect.height * (inner.w - inner.y) + inner.y;

            vertex.uv0 = new Vector2(u, v);

            return vertex;
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

        /// <summary>
        /// 返回两个RectTransform的偏移值，相对rect1来说。
        /// 只对同一个Canvas下的两个RectTransform有效
        /// </summary>
        /// <param name="rect1"></param>
        /// <param name="rect2"></param>
        /// <returns></returns>
        private Vector2 GetRectTransformOffset(RectTransform rect1, RectTransform rect2)
        {
            Vector2 offset1 = Vector2.zero;
            Vector2 offset2 = Vector2.zero;

            RectTransform temp = rect1;

            while (temp != null)
            {
                if(temp == rect2)
                {
                    return offset1;
                }

                offset1 += temp.anchoredPosition;

                temp = temp.parent as RectTransform;
            }

            temp = rect2;

            while (temp != null)
            {
                if (temp == rect1)
                {
                    return -offset2;
                }

                offset2 += temp.anchoredPosition;

                temp = temp.parent as RectTransform;
            }

            return offset1 - offset2;
        }
    }
}
