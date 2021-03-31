using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// 
/// name:MeshImage
/// author:Lawliet
/// vindicator:
/// versions:
/// introduce:
/// note:
/// 
/// 
/// list:
/// 
/// 
/// 
/// </summary>
namespace Waiting.UGUI.Graphics
{
    [AddComponentMenu("UI/MeshGraphic", 51)]
    public class MeshGraphic : MaskableGraphic, ICanvasRaycastFilter
    {
        [SerializeField]
        protected Mesh m_Mesh;

        public Mesh mesh
        {
            get
            {
                return m_Mesh;
            }
            set
            {
                if(m_Mesh != value)
                {
                    m_Mesh = value;

                    SetVerticesDirty();
                }
            }
        }

        [SerializeField]
        private Texture m_Texture;

        public override Texture mainTexture
        {
            get
            {
                return texture;
            }
        }

        public Texture texture
        {
            get
            {
                if (m_Texture == null)
                {
                    if (material != null && material.mainTexture != null)
                    {
                        return material.mainTexture;
                    }
                    return s_WhiteTexture;
                }

                return m_Texture;
            }
            set
            {
                if(m_Texture != value)
                {
                    m_Texture = value;

                    SetMaterialDirty();
                }
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (m_Mesh == null)
            {
                return;
            }

            Vector2 sizeDelta = rectTransform.sizeDelta;

            float width = sizeDelta.x;
            float height = sizeDelta.y;

            vh.Clear();

            for (int i = 0; i < m_Mesh.vertexCount; i++)
            {
                Vector3 v = m_Mesh.vertices[i];

                v = new Vector2(v.x * width, v.y * height);

                Color32 c = Color.white;

                if (i < m_Mesh.colors32.Length)
                {
                    c = m_Mesh.colors32[i] * color;
                }
                else
                {
                    c = color;
                }

                Vector2 uv = m_Mesh.uv[i];

                vh.AddVert(v, c, uv);
            }

            for (int i = 0; i < m_Mesh.triangles.Length / 3; i++)
            {
                int idx0 = m_Mesh.triangles[i * 3];
                int idx1 = m_Mesh.triangles[i * 3 + 1];
                int idx2 = m_Mesh.triangles[i * 3 + 2];

                vh.AddTriangle(idx0, idx1, idx2);
            }
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            return false;
        }
    }
}

