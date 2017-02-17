using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

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

            var output = new List<UIVertex>();
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
        }

        /// <summary>
        /// 绘制简单版
        /// </summary>
        /// <param name="output"></param>
        /// <param name="count"></param>
        protected void DrawSimple(List<UIVertex> output, int count)
        {
            switch (m_MirrorType)
            {
                case MirrorType.Horizontal:
                    DrawSimpleHorizontal(output, count);
                    break;
                case MirrorType.Vertical:
                    DrawSimpleVertical(output, count);
                    break;
                case MirrorType.Quarter:
                    DrawSimpleQuarter(output, count);
                    break;
            }
        }

        /// <summary>
        /// 绘制简单的水平镜像
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="count"></param>
        protected void DrawSimpleHorizontal(List<UIVertex> verts, int count)
        {
            Rect rect = graphic.GetPixelAdjustedRect();

            UIVertex[] addVerts = new UIVertex[count];

            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                //原来偏移
                Vector3 position = vertex.position;

                position.x = (position.x + rect.x) * 0.5f;

                vertex.position = position;

                verts[i] = vertex;

                //水平镜像
                position.x = rect.center.x * 2 - position.x;

                vertex.position = position;

                addVerts[i] = vertex;
            }

            verts.AddRange(addVerts);
        }

        /// <summary>
        /// 绘制简单的垂直镜像
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="count"></param>
        protected void DrawSimpleVertical(List<UIVertex> verts, int count)
        {
            Rect rect = graphic.GetPixelAdjustedRect();

            UIVertex[] addVerts = new UIVertex[count];

            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                //原来偏移
                Vector3 position = vertex.position;

                position.y = (position.y + rect.y) * 0.5f;

                vertex.position = position;

                verts[i] = vertex;

                //垂直镜像
                position.y = rect.center.y * 2 - position.y;

                vertex.position = position;

                addVerts[i] = vertex;
            }

            verts.AddRange(addVerts);
        }

        /// <summary>
        /// 绘制简单的四分之一镜像
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="count"></param>
        protected void DrawSimpleQuarter(List<UIVertex> verts, int count)
        {
            Rect rect = graphic.GetPixelAdjustedRect();

            UIVertex[] addVerts = new UIVertex[count * 3];

            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                //原来偏移 第三象限
                Vector3 position = vertex.position;

                position.x = (position.x + rect.x) * 0.5f;

                position.y = (position.y + rect.y) * 0.5f;

                vertex.position = position;

                verts[i] = vertex;

                //第二象限
                position.y = rect.center.y * 2 - position.y;

                vertex.position = position;

                addVerts[i] = vertex;

                //第一象限
                position.x = rect.center.x * 2 - position.x;

                vertex.position = position;

                addVerts[i + count] = vertex;

                //第四象限
                position.y = rect.center.y * 2 - position.y;

                vertex.position = position;

                addVerts[i + count * 2] = vertex;
            }

            verts.AddRange(addVerts);
        }
    }
}
