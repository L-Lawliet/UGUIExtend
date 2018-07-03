using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// name:ImageText
/// author:Waiting
/// date:2018/5/3 18:09:53
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUI.Graphics
{

    public class ImageText : MaskableGraphic, ICanvasRaycastFilter, ISerializationCallbackReceiver
    {
        
        /// <summary>
        /// 
        /// </summary>
        [System.Serializable]
        public class FontSpriteDictionary : Dictionary<string, Sprite>, ISerializationCallbackReceiver
        {
            [SerializeField]
            private List<string> m_Keys;

            [SerializeField]
            private List<Sprite> m_Values;
        

            public void OnBeforeSerialize()
            {
                m_Keys = new List<string>(this.Keys);

                m_Values = new List<Sprite>(this.Values);
            }

            public void OnAfterDeserialize()
            {
                this.Clear();

                int count = Mathf.Min(m_Keys.Count, m_Values.Count);

                for (int i = 0; i < count; ++i)
                {
                    this.Add(m_Keys[i], m_Values[i]);
                }

                m_Keys.Clear();
                m_Values.Clear();
            }
        }


        [SerializeField]
        protected string m_Text;

        public string text 
        {
            set
            {
                if (m_Text == value)
                {
                    return;
                }

                m_Text = value;

                SetAllDirty();
            }
            get
            {
                return m_Text;
            }
        }

        [SerializeField]
        protected FontSpriteDictionary m_SpriteList = new FontSpriteDictionary();

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();

            if (string.IsNullOrEmpty(m_Text))
            {
                return;
            }
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            return true;
        }

        public void OnAfterDeserialize()
        {
            
        }

        public void OnBeforeSerialize()
        {
            
        }
    }
}
