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
/// name:RectMask
/// author:Lawliet
/// date:2019/11/1 11:53:01
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUI.Effects
{
    [AddComponentMenu("UI/Effects/RectMask", 22)]
    [RequireComponent(typeof(Graphic))]
    public class RectMask : BaseMeshEffect
    {
        [SerializeField]
        private RectTransform m_MaskRect;

        /// <summary>
        /// 作为蒙版的矩形框
        /// </summary>
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

        [NonSerialized]
        private RectTransform m_RectTransform;

        /// <summary>
        /// 
        /// </summary>
        public RectTransform rectTransform
        {
            get
            {
                if (m_RectTransform == null)
                {
                    m_RectTransform = GetComponent<RectTransform>();
                }

                return m_RectTransform;
            }
        }

#if UNITY_EDITOR
        public void SetDirty()
        {
            if (graphic != null)
            {
                graphic.SetVerticesDirty();
            }
        }
#endif

        /// <summary>
        /// 绘制网格
        /// </summary>
        /// <param name="vh"></param>
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            if (m_MaskRect == null)
            {
                return;
            }

            var original = ListPool<UIVertex>.Get();
            var output = ListPool<UIVertex>.Get();
            vh.GetUIVertexStream(original);

            int count = original.Count;

            DrawRect(original, output, count);

            vh.Clear();
            vh.AddUIVertexTriangleStream(output);

            ListPool<UIVertex>.Recycle(original);
            ListPool<UIVertex>.Recycle(output);
        }

        /// <summary>
        /// 绘制矩形区域
        /// </summary>
        /// <param name="original"></param>
        /// <param name="output"></param>
        /// <param name="count"></param>
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

        private UIVertex GetVertex(Vector2[] list, int index, Rect rect, Sprite overrideSprite, Vector4 inner)
        {
            return GetVertex(list[index], rect, overrideSprite, inner);
        }

        /// <summary>
        /// 返回顶点位置
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="rect"></param>
        /// <param name="overrideSprite"></param>
        /// <param name="inner"></param>
        /// <returns></returns>
        private UIVertex GetVertex(Vector2 vector, Rect rect, Sprite overrideSprite, Vector4 inner)
        {
            UIVertex vertex = new UIVertex();
            vertex.position = vector;
            vertex.color = graphic.color;
            vertex.normal = new Vector3(0, 0, -1);

            float u = (vertex.position.x - rect.x) / rect.width * inner.z + inner.x;
            float v = (vertex.position.y - rect.y) / rect.height * inner.w + inner.y;

            vertex.uv0 = new Vector2(u, v);

            return vertex;
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
