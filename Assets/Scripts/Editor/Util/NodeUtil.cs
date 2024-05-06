
using UnityEditor;
using UnityEngine;

namespace Assets
{
    public class NodeUtil
    {
        public static void DrawNodeCurve(Rect start, Rect end, Color col)
        {
            Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;
            Color shadowCol = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, col, null, 1);
        }
        public static void DrawLine(Vector2 from, Vector2 to, Color col) {
            var tmpCol = Handles.color;
            Handles.color = col;
            Handles.DrawLine(from, to);
            Handles.color = tmpCol;
        }
        public static void DrawLine(Rect start, Rect end, Color col)
        {
            Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
            var tmpCol = Handles.color;
            Handles.color = col;
            Handles.DrawLine(startPos, endPos);
            Handles.color = tmpCol;
        }
    }
}
