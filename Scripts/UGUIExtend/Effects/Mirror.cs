using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [RequireComponent(typeof(Image))]
    public class Mirror : BaseMeshEffect
    {
        public enum MirrorType
        {
            Horizontal,
            Vertical,
            Quarter,
        }

        [SerializeField]
        private MirrorType m_Mirror = MirrorType.Horizontal;

        public MirrorType mirror
        {
            get { return m_Mirror; }
            set
            {
                if (m_Mirror != value)
                {
                    m_Mirror = value;
                    if(graphic != null){
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

            var output = new List<UIVertex>();
            vh.GetUIVertexStream(output);

            int count = output.Count;

            var neededCapacity = count * 2;
            if (output.Capacity < neededCapacity)
            {
                output.Capacity = neededCapacity;
            }

            switch (m_Mirror)
            {
                case MirrorType.Horizontal:
                    DrawHorizontal(output, count);
                    break;
                case MirrorType.Vertical:
                    DrawVertical(output, count);
                    break;
                case MirrorType.Quarter:
                    DrawQuarter(output, count);
                    break;
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(output);
        }

        protected void DrawHorizontal(List<UIVertex> verts, int count)
        {
            Rect rect = graphic.GetPixelAdjustedRect();

            float halfWidth = rect.width * 0.5f;

            float centerX = rect.x + halfWidth;

            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                //原来偏移
                Vector3 position = vertex.position;

                position.x = (position.x - halfWidth) * 0.5f;

                //position.y = position.y - (i / 3) * 100.0f;

                vertex.position = position;

                verts[i] = vertex;

                //水平镜像
                position.x = centerX * 2 - position.x;

                vertex.position = position;

                verts.Add(vertex);
            }
        }

        protected void DrawVertical(List<UIVertex> verts, int count)
        {
            Rect rect = graphic.GetPixelAdjustedRect();

            float halfHeight = rect.height * 0.5f;

            float centerY = rect.y + halfHeight;

            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                //原来偏移
                Vector3 position = vertex.position;

                position.y = (position.y - halfHeight) * 0.5f;

                vertex.position = position;

                verts[i] = vertex;

                //垂直镜像
                position.y = centerY * 2 - position.y;

                vertex.position = position;

                verts.Add(vertex);
            }
        }

        protected void DrawQuarter(List<UIVertex> verts, int count)
        {
            
        }

    }
}
