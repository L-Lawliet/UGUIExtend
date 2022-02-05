using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///
/// name:TimePicker
/// author:Administrator
/// date:2017/12/8 11:40:22
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUI.Components
{
    public class TimePicker : UIBehaviour
    {
        [SerializeField]
        private ScrollRect m_HourScrollRect;

        [SerializeField]
        private HorizontalLayoutGroup m_HourLayoutGroup;

        [SerializeField]
        private ScrollRect m_MinuteScrollRect;

        [SerializeField]
        private float m_Velocity = 0;

        private float m_ItemHeight;

        private bool m_Dragging = false;

        private bool m_Dirty = false;

        protected override void Awake()
        {
            m_HourScrollRect.onValueChanged.AddListener(HourScrollChangeHandler);
        }

        private void HourScrollChangeHandler(Vector2 value)
        {
            m_Dirty = true;

            m_Dragging = true;
        }

        private void UpdateScrollRect()
        {
            Rect viewport = m_HourScrollRect.viewport.rect;

            m_ItemHeight = viewport.height / 5;

            //m_HourScrollRect.preferredHeight = (m_HourScrollRect.transform as RectTransform).rect.height
        }

        protected virtual void LateUpdate()
        {
            if (!m_Dirty)
            {
                return;
            }

            if (m_Dragging)
            {
                m_Dragging = false;

                return;
            }

            Debug.Log(m_Dragging);

            /*float deltaTime = Time.unscaledDeltaTime;

            RectTransform contentTransform = m_HourScrollRect.content;

            //m_HourScrollRect

            float targetPosition = Mathf.Round(m_HourScrollRect.verticalNormalizedPosition * (12 + 4))/(12+4);

            float speed = m_Velocity;

            m_HourScrollRect.verticalNormalizedPosition = Mathf.SmoothDamp(m_HourScrollRect.verticalNormalizedPosition, targetPosition, ref speed, Mathf.Infinity, deltaTime);

            m_Velocity = speed;*/
        }
    }
}
