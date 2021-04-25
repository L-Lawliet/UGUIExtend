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
    [CustomEditor(typeof(MeshGraphic), true)]
    [CanEditMultipleObjects]
    public class MeshGraphicEditor : GraphicEditor
    {
        protected SerializedProperty m_Mesh;

        protected SerializedProperty m_Texture;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_Mesh = serializedObject.FindProperty("m_Mesh");

            m_Texture = serializedObject.FindProperty("m_Texture");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            BaseGUI();

            TextureGUI();

            AppearanceControlsGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected void BaseGUI()
        {
            EditorGUILayout.PropertyField(m_Script);

            EditorGUILayout.PropertyField(m_Mesh);
        }

        protected void TextureGUI()
        {
            EditorGUILayout.PropertyField(m_Texture);
        }
    }
}

