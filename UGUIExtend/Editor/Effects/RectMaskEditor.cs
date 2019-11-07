using System;
using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using Waiting.UGUI.Effects;

/// <summary>
///
/// name:RectMaskEditor
/// author:Lawliet
/// date:2019/10/1 11:53:01
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUIEditor.Effects
{
    [CustomEditor(typeof(RectMask), true)]
    public class RectMaskEditor : Editor
    {
        protected SerializedProperty m_MaskRect;

        protected SerializedProperty m_MaskRectPosition;

        private GUIContent m_MaskRectContent;

        private Vector3 m_LastPosition;
        private Rect m_LastRect;

        protected virtual void OnEnable()
        {
            m_MaskRectContent = new GUIContent("Mask Rect");

            m_MaskRect = serializedObject.FindProperty("m_MaskRect");

            m_MaskRectPosition = serializedObject.FindProperty("m_MaskRect.position");

            EditorApplication.update += EditorUpdate;

        }

        protected virtual void OnDisable()
        {
            EditorApplication.update -= EditorUpdate;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_MaskRect, m_MaskRectContent);

            serializedObject.ApplyModifiedProperties();
        }

        private void EditorUpdate()
        {
            var rectMask = target as RectMask;
            
            if (rectMask == null)
            {
                return;
            }
            
            if (rectMask.maskRect == null)
            {
                return;
            }

            bool dirty = false;

            if (rectMask.rectTransform.position != m_LastPosition)
            {
                m_LastPosition = rectMask.rectTransform.position;
                dirty = true;
            }
            else if (rectMask.rectTransform.rect != m_LastRect)
            {
                m_LastRect = rectMask.rectTransform.rect;
                dirty = true;
            }

            if (dirty)
            {
                rectMask.SetDirty();
            }
        }
    }
}

