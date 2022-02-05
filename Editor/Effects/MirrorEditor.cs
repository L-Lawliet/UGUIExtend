using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;
using Waiting.UGUI.Effects;

/// <summary>
///
/// name:MirrorEditor
/// author:Lawliet
/// date:2017/2/4 10:18:04
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUIEditor.Effects
{
    [CustomEditor(typeof(Mirror), true)]
    [CanEditMultipleObjects]
    public class MirrorEditor : Editor
    {
        protected SerializedProperty m_MirrorType;
        protected SerializedProperty m_IsReversed;

        private GUIContent m_CorrectButtonContent;
        private GUIContent m_MirrorTypeContent;
        private GUIContent m_IsReversedContent;

        protected virtual void OnDisable()
        {
            
        }

        protected virtual void OnEnable()
        {
            m_CorrectButtonContent = new GUIContent("Set Native Size", "Sets the size to match the content.");

            m_MirrorTypeContent = new GUIContent("Mirror Type");

            m_IsReversedContent = new GUIContent("Is Reversed");

            m_MirrorType = serializedObject.FindProperty("m_MirrorType");
            m_IsReversed = serializedObject.FindProperty("m_IsReversed");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_MirrorType, m_MirrorTypeContent);

            var canReverse = CanReverse((Mirror.MirrorType)m_MirrorType.enumValueIndex);

            using (new EditorGUI.DisabledGroupScope(!canReverse))
            {
                EditorGUILayout.PropertyField(m_IsReversed, m_IsReversedContent);
            }

            if (GUILayout.Button(m_CorrectButtonContent, EditorStyles.miniButton))
            {
                int len = targets.Length;

                for (int i = 0; i < len; i++)
                {
                    if (targets[i] is Mirror)
                    {
                        Mirror mirror = targets[i] as Mirror;

                        Undo.RecordObject(mirror.rectTransform, "Set Native Size");
                        mirror.SetNativeSize();
                        EditorUtility.SetDirty(mirror);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected bool CanReverse(Mirror.MirrorType type)
        {
            return type == Mirror.MirrorType.Horizontal || type == Mirror.MirrorType.Vertical;
        }
    }
}
