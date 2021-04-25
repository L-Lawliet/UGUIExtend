using System;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using Waiting.UGUI.Graphics;

/// <summary>
/// 
/// name:MeshGraphicEditor
/// author:罐子
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
namespace Waiting.UGUIEditor.Graphics
{
    [CustomEditor(typeof(AnimationMeshGraphic), true)]
    [CanEditMultipleObjects]
    public class AnimationMeshGraphicEditor : MeshGraphicEditor
    {
        protected SerializedProperty m_AnimDirtyMode;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_AnimDirtyMode = serializedObject.FindProperty("m_AnimDirtyMode");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            m_AnimDirtyMode.intValue = EditorGUILayout.MaskField(m_AnimDirtyMode.displayName, m_AnimDirtyMode.intValue, m_AnimDirtyMode.enumDisplayNames);

            BaseGUI();

            TextureGUI();

            AppearanceControlsGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}

