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
    [AddComponentMenu("UI/Effects/Mirror", 20)]
    [RequireComponent(typeof(Graphic))]
    public class Mirror : BaseMeshEffect
    {
        public enum MirrorType
        {
            /// <summary>
            /// 水平
            /// </summary>
            Horizontal, 

            /// <summary>
            /// 垂直
            /// </summary>
            Vertical,

            /// <summary>
            /// 四分之一
            /// 相当于水平，然后再垂直
            /// </summary>
            Quarter,
        }

        /// <summary>
        /// 镜像类型
        /// </summary>
        [SerializeField]
        private MirrorType m_MirrorType = MirrorType.Horizontal;

        public MirrorType mirrorType
        {
            get { return m_MirrorType; }
            set
            {
                if (m_MirrorType != value)
                {
                    m_MirrorType = value;
                    if(graphic != null){
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

        /// <summary>
        /// 设置原始尺寸
        /// </summary>
        public void SetNativeSize()
        {
            if (graphic != null && graphic is Image)
            {
                Sprite overrideSprite = (graphic as Image).overrideSprite;

                if(overrideSprite != null){
                    float w = overrideSprite.rect.width / (graphic as Image).pixelsPerUnit;
                    float h = overrideSprite.rect.height / (graphic as Image).pixelsPerUnit;
                    rectTransform.anchorMax = rectTransform.anchorMin;

                    switch (m_MirrorType)
                    {
                        case MirrorType.Horizontal:
                            rectTransform.sizeDelta = new Vector2(w * 2, h);
                            break;
                        case MirrorType.Vertical:
                            rectTransform.sizeDelta = new Vector2(w, h * 2);
                            break;
                        case MirrorType.Quarter:
                            rectTransform.sizeDelta = new Vector2(w * 2, h * 2);
                            break;
                    }

                    graphic.SetVerticesDirty();
                }
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            var output = ListPool<UIVertex>.Get();
            vh.GetUIVertexStream(output);

            int count = output.Count;

            if (graphic is Image)
            {
                Image.Type type = (graphic as Image).type;

                switch (type)
                {
                    case Image.Type.Simple:
                        DrawSimple(output, count);
                        break;
                    case Image.Type.Sliced:
                        
                        break;
                    case Image.Type.Tiled:

                        break;
                    case Image.Type.Filled:

                        break;
                }
            }
            else
            {
                DrawSimple(output, count);
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(output);

            ListPool<UIVertex>.Recycle(output);
        }

        /// <summary>
        /// 绘制简单版
        /// </summary>
        /// <param name="output"></param>
        /// <param name="count"></param>
        protected void DrawSimple(List<UIVertex> output, int count)
        {
            Rect rect = graphic.GetPixelAdjustedRect();

            SimpleScale(rect, output, count);

            switch (m_MirrorType)
            {
                case MirrorType.Horizontal:
                    ExtendCapacity(output, count);
                    MirrorVerts(rect, output, count, true);
                    break;
                case MirrorType.Vertical:
                    ExtendCapacity(output, count);
                    MirrorVerts(rect, output, count, false);
                    break;
                case MirrorType.Quarter:
                    ExtendCapacity(output, count * 3);
                    MirrorVerts(rect, output, count, true);
                    MirrorVerts(rect, output, count * 2, false);
                    break;
            }
        }

        protected void ExtendCapacity(List<UIVertex> verts, int addCount)
        {
            var neededCapacity = verts.Count + addCount;
            if (verts.Capacity < neededCapacity)
            {
                verts.Capacity = neededCapacity;
            }
        }

        protected void SimpleScale(Rect rect, List<UIVertex> verts, int count)
        {
            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                Vector3 position = vertex.position;

                if (m_MirrorType == MirrorType.Horizontal || m_MirrorType == MirrorType.Quarter)
                {
                    position.x = (position.x + rect.x) * 0.5f;
                }

                if (m_MirrorType == MirrorType.Vertical || m_MirrorType == MirrorType.Quarter)
                {
                    position.y = (position.y + rect.y) * 0.5f;
                }

                vertex.position = position;

                verts[i] = vertex;
            }
        }

        protected void MirrorVerts(Rect rect, List<UIVertex> verts, int count, bool isHorizontal = true)
        {
            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                Vector3 position = vertex.position;

                if (isHorizontal)
                {
                    position.x = rect.center.x * 2 - position.x;
                }
                else
                {
                    position.y = rect.center.y * 2 - position.y;
                }

                vertex.position = position;

                verts.Add(vertex);
            }
        }
    }
}
