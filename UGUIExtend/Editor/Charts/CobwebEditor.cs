using System.Collections;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using Waiting.UGUI.Charts;

/// <summary>
///
/// name:CobwebEditor
/// author:Lawliet
/// date:2018/10/4 13:58:04
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUIEditor.Charts
{
    [CustomEditor(typeof(Cobweb), true)]
    [CanEditMultipleObjects]
    public class CobwebEditor : GraphicEditor
    {
        protected SerializedProperty m_Side;

        protected SerializedProperty m_MinPercent;

        protected SerializedProperty m_Percents;

        private GUIContent m_SideContent;

        private GUIContent m_MinPercentContent;

        private GUIContent m_PercentsContent;

        private bool _percentsFade = false; 

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_SideContent = new GUIContent("Side");

            m_MinPercentContent = new GUIContent("MinPercent");

            m_PercentsContent = new GUIContent("Percentse");

            m_Side = serializedObject.FindProperty("m_Side");

            m_MinPercent = serializedObject.FindProperty("m_MinPercent");

            m_Percents = serializedObject.FindProperty("m_Percents");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(m_Side, m_SideContent);

            if(EditorGUI.EndChangeCheck())
            {
                if (m_Side.intValue < 3)
                {
                    m_Side.intValue = 3;
                }

                m_Percents.arraySize = m_Side.intValue;
            }

            EditorGUILayout.PropertyField(m_MinPercent, m_MinPercentContent);

            if (_percentsFade = EditorGUILayout.Foldout(_percentsFade, m_PercentsContent))
            {
                EditorGUI.indentLevel++;

                for (int i = 0; i < m_Percents.arraySize; i++)
                {
                    var element = m_Percents.GetArrayElementAtIndex(i);

                    EditorGUILayout.PropertyField(element, new GUIContent(string.Format("percent {0}", i.ToString())));
                }

                EditorGUI.indentLevel--;
            }
            

            //EditorGUILayout.PropertyField(m_Percents, true);

            AppearanceControlsGUI();
            RaycastControlsGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
