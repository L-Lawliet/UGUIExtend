using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

/// <summary>
///
/// name:ProgressBar
/// author:Administrator
/// date:2016/5/30 17:04:08
/// versions:
/// introduce:
/// note:
/// 进度条
/// </summary>
namespace Waiting.UGUI.Components
{
    [AddComponentMenu("UI/ProgressBar", 50)]
    public class ProgressBar : UIBehaviour, ICanvasElement
    {
        /// <summary>
        /// 进度条缩放类型
        /// </summary>
        public enum Type
        {
            fillAmount, //Image.fillAmount
            width,  //RectTransform.Width
            height, //RectTransform.height
        }

        [SerializeField]
        protected Type _type;

        public Type type 
        { 
            get { return _type; }
            set
            {
                if (_type == value)
                {
                    return;
                }

                _type = value;

                SetDirty();
            }
        }

        /// <summary>
        /// type为fillAmount时使用
        /// </summary>
        [SerializeField]
        protected Image _fill;

        public Image fill 
        { 
            get { return _fill; }
            set
            {
                if (_fill == value)
                {
                    return;
                }

                _fill = value;

                SetDirty();
            }
        }

        /// <summary>
        /// type为width/height时使用
        /// </summary>
        [SerializeField]
        protected RectTransform _fillTransform;

        public RectTransform fillTransform
        {
            get { return _fillTransform; }
            set
            {
                if (_fillTransform == value)
                {
                    return;
                }

                _fillTransform = value;

                SetDirty();
            }
        }

        /// <summary>
        /// type为width/height时使用
        /// width/height最大值
        /// </summary>
        [SerializeField]
        protected float _length = 100;

        public float length
        {
            get { return _length; }
            set
            {
                if (_length == value)
                {
                    return;
                }

                _length = value;

                SetDirty();
            }
        }

        [SerializeField]
        protected Text _label;

        public Text label
        {
            get { return _label; }
            set
            {
                if (_label == value)
                {
                    return;
                }

                _label = value;

                SetDirty();
            }
        }

        [SerializeField]
        protected bool _isPercent = false;

        public bool isPercent
        {
            get { return _isPercent; }
            set
            {
                if (_isPercent == value)
                {
                    return;
                }

                _isPercent = value;

                SetDirty();
            }
        }

        /// <summary>
        /// 小数精确位数
        /// </summary>
        [Range(0, 5)]
        [SerializeField]
        protected int _precision = 0;

        public int precision
        {
            get { return _precision; }
            set
            {
                if (_precision == value)
                {
                    return;
                }

                _precision = value;

                SetDirty();
            }
        }

        [SerializeField]
        protected float _totalValue;

        public float totalValue
        {
            get { return _totalValue; }
            set
            {
                if (_totalValue == value)
                {
                    return;
                }

                _totalValue = value;

                SetDirty();
            }
        }

        [Range(0,1)]
        [SerializeField]
        protected float _percent;

        public float percent
        {
            get { return _percent; }
            set
            {
                if (_percent == value)
                {
                    return;
                }

                value = Mathf.Max(Mathf.Min(value, 1), 0);

                _percent = value;

                SetDirty();
            }
        }

        public void UpdateFill()
        {
            switch (type)
            {
                case Type.fillAmount:
                    if (fill != null)
                    {
                        fill.fillAmount = _percent;
                    }
                    break;
                case Type.width:
                    if (fillTransform == null)
                    {
                        break;
                    }
                    
                    if (_percent == 0)
                    {
                        fillTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.001f);
                    }
                    else
                    {
                        fillTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, length * _percent);
                    }
                    break;
                case Type.height:
                    if (fillTransform == null)
                    {
                        break;
                    }

                    if (_percent == 0)
                    {
                        fillTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.001f);
                    }
                    else
                    {
                        fillTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, length * _percent);
                    }
                    break;
            }

            if (label != null)
            {
                float multiple = Mathf.Pow(10, precision);

                if (isPercent)
                {
                    label.text = (_percent * 100).ToString("f" + precision) + "%";
                }
                else
                {
                    label.text = (_percent * totalValue).ToString("f" + precision) + "/" + totalValue;
                }
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            SetDirty();
        }

#endif

        public void GraphicUpdateComplete()
        {

        }

        public void LayoutComplete()
        {

        }

        public void Rebuild(CanvasUpdate executing)
        {
            UpdateFill();
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

        void OnDestroy()
        {
            fill = null;

            label = null;
        }
    }
}
