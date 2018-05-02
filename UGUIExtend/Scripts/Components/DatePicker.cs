using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///
/// name:DatePicker
/// author:Administrator
/// date:2017/12/5 17:30:02
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUI.Components
{
    public class DatePicker : UIBehaviour
    {
        protected readonly string[] WEEK_NAMES = new string[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };

        protected readonly string[] MONTH_NAMES = new string[] { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };

        protected class DayItem : MonoBehaviour, IPointerClickHandler
        {
            [SerializeField]
            private Text m_Label;

            [SerializeField]
            private Toggle m_Toggle;

            public Text label
            {
                get { return m_Label; }
                set { m_Label = value; }
            }

            public Toggle toggle
            {
                get { return m_Toggle; }
                set { m_Toggle = value; }
            }

            /*private int m_Day;

            public int day
            {
                get { return m_Day; }
                set 
                {
                    m_Day = value;
                    label.text = m_Day.ToString(); 
                }
            }*/

            private DateTime m_Date;

            public DateTime date
            {
                get { return m_Date; }
                set
                {
                    m_Date = value;
                    label.text = m_Date.Day.ToString();
                }
            }

            [SerializeField]
            private DateClickEvent m_OnClick = new DateClickEvent();
            public DateClickEvent onClick { get { return m_OnClick; } set { m_OnClick = value; } }

            public void OnPointerClick(PointerEventData eventData)
            {
                m_OnClick.Invoke(m_Date);
            }
        }

        [Serializable]
        public class DateChangeEvent : UnityEvent<DateTime> { }

        [SerializeField]
        public class DateClickEvent : UnityEvent<DateTime> { }

        [SerializeField]
        private Button m_LeftButton;

        [SerializeField]
        private Button m_RightButton;


        [SerializeField]
        private Text m_DateText;

        [SerializeField]
        private Button m_DateButton;

        [Space]

        [SerializeField]
        private Text m_TemplateWeekText;

        [SerializeField]
        private RectTransform m_TemplateDayItem;

        [SerializeField]
        private Text m_TemplateDayText;

        [Space]

        [SerializeField]
        private Color m_TextNormalColor = Color.black;

        [SerializeField]
        private Color m_TextDisabledColor = new Color(0.4f, 0.4f, 0.4f, 1);

        [Space]

        [SerializeField]
        private RectTransform m_DayGridContainer;

        [Space]

        [SerializeField]
        private DateChangeEvent m_OnChange = new DateChangeEvent();
        public DateChangeEvent onChange { get { return m_OnChange; } set { m_OnChange = value; } }

        [SerializeField]
        [HideInInspector]
        private List<Text> m_WeekTexts = new List<Text>();

        [SerializeField]
        [HideInInspector]
        private List<DayItem> m_DayItems = new List<DayItem>();

        [SerializeField]
        [HideInInspector]
        private DateTime m_SelectDate = DateTime.Now;

        public DateTime selectDate
        {
            get { return m_SelectDate; }
            set 
            {
                m_SelectDate = value;

                m_ShowYear = m_SelectDate.Year;

                m_ShowMonth = m_SelectDate.Month;

                SetLabel();
                SetDay();
            }
        }

        private int m_ShowYear;

        private int m_ShowMonth;

        protected override void Awake()
        {
            m_ShowYear = m_SelectDate.Year;

            m_ShowMonth = m_SelectDate.Month;

            m_LeftButton.onClick.AddListener(LeftButtonClickHandler);
            m_RightButton.onClick.AddListener(RightButtonClickHandler);

            SetLabel();
            SetupDay();
        }

        private void LeftButtonClickHandler()
        {
            if (m_ShowMonth > 1)
            {
                m_ShowMonth--;
            }
            else
            {
                m_ShowMonth = 12;
                m_ShowYear--;
            }

            SetLabel();
            SetDay();
        }

        private void RightButtonClickHandler()
        {
            if (m_ShowMonth < 12)
            {
                m_ShowMonth++;
            }
            else
            {
                m_ShowMonth = 1;
                m_ShowYear++;
            }

            SetLabel();
            SetDay();
        }

        private void DayItemClickHandler(DateTime value)
        {
            m_SelectDate = value;

            m_OnChange.Invoke(value);

            m_ShowYear = m_SelectDate.Year;
            m_ShowMonth = m_SelectDate.Month;

            SetLabel();
            SetDay();
        }

        protected override void OnEnable()
        {
            
        }

        private void SetupDay()
        {
            m_TemplateWeekText.gameObject.SetActive(true);

            for (int i = 0; i < 7; i++)
            {
                Text weekText = Text.Instantiate<Text>(m_TemplateWeekText);

                weekText.transform.SetParent(m_TemplateWeekText.transform.parent, false);

                m_WeekTexts.Add(weekText);

                weekText.text = WEEK_NAMES[i];
            }

            m_TemplateWeekText.gameObject.SetActive(false);

            m_TemplateDayItem.gameObject.SetActive(true);

            DayItem templateDayItem = m_TemplateDayItem.gameObject.GetComponent<DayItem>();

            if (DayItem.Equals(templateDayItem, null))
            {
                templateDayItem = m_TemplateDayItem.gameObject.AddComponent<DayItem>();
            }

            templateDayItem.toggle = templateDayItem.GetComponent<Toggle>();

            templateDayItem.label = m_TemplateDayText;

            for (int i = 0; i < 42; i++)
            {
                DayItem dayItem = DayItem.Instantiate<DayItem>(templateDayItem);

                dayItem.transform.SetParent(m_TemplateDayItem.parent, false);

                dayItem.onClick.AddListener(DayItemClickHandler);

                m_DayItems.Add(dayItem);
            }

            m_TemplateDayItem.gameObject.SetActive(false);

            SetDay();
        }

        private void SetDay()
        {
            DateTime firstDay = new DateTime(m_ShowYear, m_ShowMonth, 1);

            int week = (int)firstDay.DayOfWeek;

            DateTime startDay = firstDay.AddDays(-week);

            m_DayItems[0].toggle.group.SetAllTogglesOff();
            
            for (int i = 0; i < 42; i++)
            {
                DayItem dayItem = m_DayItems[i];

                DateTime date;

                date = startDay.AddDays(i);

                dayItem.date = date;

                if (date.Month == m_ShowMonth)
                {
                    dayItem.label.color = m_TextNormalColor;
                }
                else
                {
                    dayItem.label.color = m_TextDisabledColor;
                }

                if (m_SelectDate.Year == date.Year && m_SelectDate.Month == date.Month && m_SelectDate.Day == date.Day)
                {
                    dayItem.toggle.isOn = true;
                }
            }
        }

        private void SetLabel()
        {
            m_DateText.text = string.Format("{0} {1}", m_ShowYear, MONTH_NAMES[m_ShowMonth - 1]);
        }
    }
}
