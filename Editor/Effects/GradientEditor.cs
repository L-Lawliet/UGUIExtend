using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
///
/// name:MirrorEditor
/// author:Lawliet
/// date:2019/11/7 10:18:04
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUIEditor.Effects
{
    [CustomEditor(typeof(Gradient), true)]
    [CanEditMultipleObjects]
    public class GradientEditor : Editor
    {
        protected SerializedProperty m_StartColor;
        protected SerializedProperty m_EndColor;
        protected SerializedProperty m_Angle;
        protected SerializedProperty m_Percent;

        private GUIContent m_StartColorContent;
        private GUIContent m_EndColorContent;
        private GUIContent m_FastAngleContent;
        private GUIContent m_PercentContent;

        private readonly GUIContent[] m_FastAngleTitle = new GUIContent[] { new GUIContent("Left"), new GUIContent("Up"), new GUIContent("Right"), new GUIContent("Down")};
        private readonly int[] m_FastAngleValue = new int[] { 0, 90, 180, 270 };

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnEnable()
        {
            m_StartColorContent = new GUIContent("Start Color");
            m_EndColorContent = new GUIContent("End Color");
            m_FastAngleContent = new GUIContent("Fast Angle");
            m_PercentContent = new GUIContent("Percent");

            m_StartColor = serializedObject.FindProperty("m_StartColor");
            m_EndColor = serializedObject.FindProperty("m_EndColor");
            m_Angle = serializedObject.FindProperty("m_Angle");
            m_Percent = serializedObject.FindProperty("m_Percent");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_StartColor, m_StartColorContent);
            EditorGUILayout.PropertyField(m_EndColor, m_EndColorContent);

            int angleValue = (int)m_Angle.floatValue;

            EditorGUI.BeginChangeCheck();

            angleValue = EditorGUILayout.IntPopup(m_FastAngleContent, angleValue, m_FastAngleTitle, m_FastAngleValue);

            if (EditorGUI.EndChangeCheck())
            {
                m_Angle.floatValue = angleValue;
            }

            //m_Percent.floatValue = EditorGUILayout.Slider(m_PercentContent, m_Percent.floatValue, 0, 1);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
