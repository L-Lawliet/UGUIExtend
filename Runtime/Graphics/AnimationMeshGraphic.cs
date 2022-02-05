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
    [AddComponentMenu("UI/AnimationMeshGraphic", 52)]
    public class AnimationMeshGraphic : MeshGraphic
    {
        [Flags]
        public enum AnimDirtyMode
        {
            Layout = 1,
            Vertices = 2,
            Materal = 4,
            MateralParameter = 8,
        }

        [SerializeField]
        protected AnimDirtyMode m_AnimDirtyMode = 0;

        public AnimDirtyMode animDirtyMode
        {
            get
            {
                return m_AnimDirtyMode;
            }
            set
            {
                m_AnimDirtyMode = value;
            }
        }

        [SerializeField]
        protected Vector4 m_MainTexST = new Vector4(1, 1, 0, 0);

        public Vector4 mainTexST
        {
            get
            {
                return m_MainTexST;
            }
            set
            {
                m_MainTexST = value;
            }
        }
        
        protected override void OnDidApplyAnimationProperties()
        {
            if (m_AnimDirtyMode.HasFlag(AnimDirtyMode.Layout))
            {
                SetLayoutDirty();
            }

            if (m_AnimDirtyMode.HasFlag(AnimDirtyMode.Vertices))
            {
                SetVerticesDirty();
            }

            if (m_AnimDirtyMode.HasFlag(AnimDirtyMode.Materal))
            {
                UpdateMaterial();
            }

            if (m_AnimDirtyMode.HasFlag(AnimDirtyMode.MateralParameter))
            {
                UpdateMaterialParameter();
            }
        }

        protected void UpdateMaterialParameter()
        {
            UnityEngine.Profiling.Profiler.BeginSample("#UpdateMaterial()");
            material.SetVector("_MainTex_ST", mainTexST);

            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}

