using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Waiting.UGUI.Core;

/// <summary>
///
/// name:DrawExample
/// author:Administrator
/// date:2018/12/26 17:42:49
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUI.Example
{
    public class DrawExample : MaskableGraphic
    {
        public struct Point
        {
            public Vector2 position { get; set; }

            public Vector2 direction { get; set; }
        }

        [SerializeField]
        private int _pointCount = 1;

        [SerializeField]
        private float _pointSize = 3;

        [SerializeField]
        private float _lineWidth = 2.0f;

        [SerializeField]
        private float _pointSpeed = 10.0f;

        private List<Point> _pointList = new List<Point>();

        protected void Update()
        {
            CheckPoint();

            UpdatePosition();

            SetVerticesDirty();
        }

        private void CheckPoint()
        {
            if (_pointList.Count < _pointCount) 
            {
                _pointList.Capacity = _pointCount;

                while (_pointList.Count < _pointCount)
                {
                    _pointList.Add(GetPoint());
                }
            }
            else if (_pointList.Count > _pointCount)
            {
                _pointList.RemoveRange(_pointCount, _pointList.Count - _pointCount);
            }
        }

        private void UpdatePosition()
        {
            Rect rect = this.rectTransform.rect;

            for (int i = 0; i < _pointList.Count; i++)
            {
                Point point = _pointList[i];

                Vector2 position = point.position + point.direction;

                if (position.x < rect.xMin || position.x > rect.xMax || position.y < rect.yMin || position.y > rect.yMax)
                {
                    point.direction = GetDirection();
                    _pointList[i] = point;
                }
                else
                {
                    point.position = position;
                    _pointList[i] = point;
                }
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            Profiler.BeginSample("#OnPopulateMesh");
            vh.Clear();

            double dis = 100;
            double dis2 = dis * dis;

            for (int i = 0; i < _pointList.Count; i++)
            {
                Vector2 startPoint = _pointList[i].position;

                for (int j = i + 1; j < _pointList.Count; j++)
                {
                    Vector2 endPoint = _pointList[j].position;

                    Vector2 dValue = startPoint - endPoint;

                    double distance = dValue.x * dValue.x + dValue.y * dValue.y;

                    if (distance < dis2)
                    {
                        if (startPoint.x != endPoint.x && startPoint.y != endPoint.y)
                        {
                            Color startColor = color;

                            startColor.r = (startPoint.x - this.rectTransform.rect.xMin) / this.rectTransform.rect.width;
                            startColor.b = (startPoint.y - this.rectTransform.rect.yMin) / this.rectTransform.rect.height;

                            startColor.a *= 1 - (float)(distance / dis2);

                            Color endColor = color;

                            endColor.r = (endPoint.x - this.rectTransform.rect.xMin) / this.rectTransform.rect.width;
                            endColor.b = (endPoint.y - this.rectTransform.rect.yMin) / this.rectTransform.rect.height;

                            endColor.a *= 1 - (float)(distance / dis2);

                            //Profiler.BeginSample("#DrawLine");
                            DrawUtility.DrawLine(vh, startPoint, endPoint, startColor, endColor, _lineWidth);
                            //Profiler.EndSample();
                        }
                        
                    }
                }
            }
            Profiler.EndSample();
        }

        private Point GetPoint()
        {
            Point point = new Point();

            point.position = GetPosition();
            point.direction = GetDirection();

            return point;
        }

        private Vector2 GetPosition()
        {
            Vector2 v = this.rectTransform.rect.size;

            v = new Vector2(v.x * Random.Range(0.0f, 1.0f), v.y * Random.Range(0.0f, 1.0f));

            v = v + this.rectTransform.rect.min;

            return v;
        }

        private Vector2 GetDirection()
        {
            Vector2 v =  Vector2.zero;

            while (v.x == v.y && v.x == 0)
            {
                v = new Vector2(Random.Range(-_pointSpeed, _pointSpeed), Random.Range(-_pointSpeed, _pointSpeed));
            }

            return v;
        }
    }
}
