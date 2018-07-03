using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using Waiting.UGUI.Graphics;

/// <summary>
///
/// name:ImageTextEditor
/// author:Administrator
/// date:2018/5/4 10:00:06
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUIEditor.Graphics
{
    [CustomEditor(typeof(ImageText), true)]
    [CanEditMultipleObjects]
    public class ImageTextEditor : GraphicEditor
    {
        [System.Serializable]
        public struct CharSprite
        {
            public string key;

            public Sprite sprite;

            public CharSprite(string key, Sprite sprite)
            {
                this.key = key;
                this.sprite = sprite;
            }
        }
        

        protected SerializedProperty m_Text;

        protected SerializedProperty m_SpriteList;

        protected SerializedProperty m_SpriteSet;

        protected ReorderableList m_ReorderableList;

        protected List<CharSprite> m_List;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_Text = serializedObject.FindProperty("m_Text");

            m_SpriteList = serializedObject.FindProperty("m_SpriteList");

            m_SpriteSet = serializedObject.FindProperty("m_SpriteSet");

            SerializedProperty m_SpriteListKeys = serializedObject.FindProperty("m_SpriteList.m_Keys");

            SerializedProperty m_SpriteListValues = serializedObject.FindProperty("m_SpriteList.m_Values");

            m_List = new List<CharSprite>();

            int len =  m_SpriteListKeys.arraySize;

            /*for (int i = 0; i < len; i++)
            {
                string key = (string)m_SpriteListKeys.GetArrayElementAtIndex(i);
                Sprite sprite = (Sprite)m_SpriteListKeys.GetArrayElementAtIndex(i);

                CharSprite charSprite = new CharSprite(key, sprite);

                m_List.Add(charSprite);
            }*/

            m_ReorderableList = new ReorderableList(serializedObject, m_SpriteListKeys);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            EditorGUILayout.PropertyField(m_Text, GUIContent.none, GUILayout.Height(50));

            AppearanceControlsGUI();
            RaycastControlsGUI();

            SpriteControlsGUI();

            serializedObject.ApplyModifiedProperties();
        }

        private void SpriteControlsGUI()
        {
            /*IEnumerator enumerator = m_SpriteSet.GetEnumerator();

            enumerator.Reset();

            while (enumerator.MoveNext())
            {
                ImageText.CharSprite charSprite = (ImageText.CharSprite)enumerator.Current;
            }*/

            /*foreach(var item in m_SpriteList)
            {

            }*/

            m_ReorderableList.DoLayoutList();
        }
    }
}
