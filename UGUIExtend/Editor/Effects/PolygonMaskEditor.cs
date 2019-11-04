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
/// name:MirrorEditor
/// author:Lawliet
/// date:2019/10/1 11:53:01
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUIEditor.Effects
{
    [CustomEditor(typeof(PolygonMask), true)]
    public class PolygonMaskEditor : Editor
    {
        protected SerializedProperty m_MaskType;
        protected SerializedProperty m_MaskRect;
        protected SerializedProperty m_RegularPolygon;
        protected SerializedProperty m_PolygonCollider2D;
        //protected SerializedProperty m_DrawStep;

        private GUIContent m_MaskTypeContent;
        private GUIContent m_MaskRectContent;
        private GUIContent m_RegularPolygonContent;
        private GUIContent m_PolygonCollider2DContent;
        //private GUIContent m_DrawStepContent;
        

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnEnable()
        {
            m_MaskTypeContent = new GUIContent("Mask Type");
            m_MaskRectContent = new GUIContent("Mask Rect");
            m_RegularPolygonContent = new GUIContent("Regular Polygon");
            m_PolygonCollider2DContent = new GUIContent("Polygon Collider 2D");
            //m_DrawStepContent = new GUIContent("Draw Step");

            m_MaskType = serializedObject.FindProperty("m_MaskType");
            m_MaskRect = serializedObject.FindProperty("m_MaskRect");
            m_RegularPolygon = serializedObject.FindProperty("m_RegularPolygon");
            m_PolygonCollider2D = serializedObject.FindProperty("m_PolygonCollider2D");
            //m_DrawStep = serializedObject.FindProperty("m_DrawStep");
            
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_MaskType, m_MaskTypeContent);

            switch ((PolygonMask.MaskType)m_MaskType.enumValueIndex)
            {
                case PolygonMask.MaskType.Rect:
                    EditorGUILayout.PropertyField(m_MaskRect, m_MaskRectContent);
                    break;
                case PolygonMask.MaskType.RegularPolygon:
                    EditorGUILayout.PropertyField(m_RegularPolygon, m_RegularPolygonContent);
                    break;
                case PolygonMask.MaskType.Polygon:
                    EditorGUILayout.PropertyField(m_PolygonCollider2D, m_PolygonCollider2DContent);

                    if (this.target != null)
                    {
                        PolygonMask mask = this.target as PolygonMask;

                        if (mask.polygonCollider2D != null)
                        {
                            //EditorGUILayout.LabelField("Test");

                            //m_DrawStep.intValue = (int)EditorGUILayout.Slider(m_DrawStep.intValue, 0, mask.polygonCollider2D.points.Length - 2);
                        }
                    }
                    break;
                default:
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

