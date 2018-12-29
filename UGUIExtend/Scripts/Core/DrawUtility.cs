using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
///
/// name:DrawUtility
/// author:Administrator
/// date:2018/12/26 16:33:57
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUI.Core
{
    public class DrawUtility
    {
        public static void DrawLine(VertexHelper vertexHelper, Vector2 startPoint, Vector2 endPoint, Color32 color, float width = 1)
        {
            DrawLine(vertexHelper, startPoint, endPoint, color, color, width);
        }

        public static void DrawLine(VertexHelper vertexHelper, Vector2 startPoint, Vector2 endPoint, Color32 startColor, Color32 endColor, float width = 1)
        {
            Assert.IsNotNull<VertexHelper>(vertexHelper, "Argument 'vertexHelper' cannot be null!");
            Assert.IsTrue((startPoint.x != endPoint.x && startPoint.y != endPoint.y), "'startPoint' can't be equal as 'endPoint'!");

            int startIndex = vertexHelper.currentVertCount;

            float angle = Mathf.Atan2(-endPoint.y + startPoint.y, endPoint.x - startPoint.x);

            float sw = Mathf.Sin(angle) * width * 0.5f;
            float sh = Mathf.Cos(angle) * width * 0.5f;

            float ew = Mathf.Sin(angle) * width * 0.5f;
            float eh = Mathf.Cos(angle) * width * 0.5f;

            vertexHelper.AddVert(new Vector3(startPoint.x - sw, startPoint.y - sh), startColor, new Vector2(0f, 0f));
            vertexHelper.AddVert(new Vector3(startPoint.x + sw, startPoint.y + sh), startColor, new Vector2(0f, 1f));
            vertexHelper.AddVert(new Vector3(endPoint.x + ew, endPoint.y + sh), endColor, new Vector2(1f, 1f));
            vertexHelper.AddVert(new Vector3(endPoint.x - ew, endPoint.y - sh), endColor, new Vector2(1f, 0f));

            vertexHelper.AddTriangle(startIndex + 0, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex + 0);
        }

        public static void DrawLine(VertexHelper vertexHelper, Vector2[] pointList, Color32 color, float width = 1)
        {
            Assert.IsNotNull<VertexHelper>(vertexHelper, "Argument 'vertexHelper' cannot be null!");
            Assert.IsNotNull<Vector2[]>(pointList, "Argument 'pointList' cannot be null!");
            Assert.IsTrue(pointList.Length > 1, "pointList's count must be more than one!");

            DrawLine(vertexHelper, pointList as IEnumerable<Vector2>, color, width);
        }

        public static void DrawLine(VertexHelper vertexHelper, List<Vector2> pointList, Color32 color, float width = 1)
        {
            Assert.IsNotNull<VertexHelper>(vertexHelper, "Argument 'vertexHelper' cannot be null!");
            Assert.IsNotNull<List<Vector2>>(pointList, "Argument 'pointList' cannot be null!");
            Assert.IsTrue(pointList.Count > 1, "pointList's count must be more than one!");

            DrawLine(vertexHelper, pointList as IEnumerable<Vector2>, color, width);
        }

        private static void DrawLine(VertexHelper vertexHelper, IEnumerable<Vector2> pointList, Color32 color, float width = 1)
        {
            var enumerator = pointList.GetEnumerator();

            Vector2 lastPoint = Vector2.zero;

            int i = 0;

            while (enumerator.MoveNext())
            {
                Vector3 current = enumerator.Current;

                if (i > 0)
                {
                    //TODO:直接绘制会产生接缝处开口，以后改成平滑接缝处
                    DrawLine(vertexHelper, lastPoint, current, color, width);
                }

                lastPoint = current;

                i++;
            }
        }

        private static void Draw()
        { 

}
    }
}
