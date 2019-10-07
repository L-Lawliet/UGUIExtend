using System.Collections;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using Waiting.UGUI.Components;

/// <summary>
///
/// name:RegularPolygonEditor
/// author:Lawliet
/// date:2018/10/4 13:58:04
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUIEditor.Components
{
    [CustomEditor(typeof(RegularPolygon), true)]
    [CanEditMultipleObjects]
    public class RegularPolygonEditor : GraphicEditor
    {
        protected SerializedProperty m_Side;

        protected SerializedProperty m_InnerPercent;

        private GUIContent m_SideContent;

        private GUIContent m_InnerPercentContent;

        private bool _percentsFade = false;

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_SideContent = new GUIContent("Side");

            m_InnerPercentContent = new GUIContent("InnerPercent");

            m_Side = serializedObject.FindProperty("m_Side");

            m_InnerPercent = serializedObject.FindProperty("m_InnerPercent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(m_Side, m_SideContent);

            if (EditorGUI.EndChangeCheck())
            {
                if (m_Side.intValue < 3)
                {
                    m_Side.intValue = 3;
                }
            }

            EditorGUILayout.PropertyField(m_InnerPercent, m_InnerPercentContent);

            AppearanceControlsGUI();
            RaycastControlsGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
