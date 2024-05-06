using Assets.Dependent;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Chart
{
    public class NodeChart:System.IDisposable
    {
        static public int idGen = 1;
        public int id { private set; get; }
        /// <summary>
        /// 当前节点
        /// </summary>
        protected Node mNode;
        public Rect mRect;
        protected List<Node> mChildNodeList = new List<Node>();
        /// <summary>
        /// 节点父类图
        /// </summary>
        protected List<NodeChart> mParentChart = new List<NodeChart>();
        protected AssetNode mAssetNode;
        public string Name {
            get {
                return mNode.Name;
            }
        }
        public NodeChart(AssetNode node) {
            mAssetNode = node;
            id = idGen++;
        }
        public void Init(Rect rect)
        {
            mRect = rect;
            mNode = new Node(mAssetNode.mAssetPath);

            foreach (var anode in mAssetNode.mQuoteNodes) {
                var cnode = new Node(anode.mAssetPath);
                mChildNodeList.Add(cnode);
            }

            //获取父节点图
            foreach (var pnode in mAssetNode.mQuotedNodes) {
                var chart = ChartDirecfor.GetOrCreateNodeChart(pnode);
                mParentChart.Add(chart);
            }
        }
        public void SetRect(Rect rect) {
            mRect = rect;
        }
        Vector2 scrollPos;
        private bool disposedValue;

        /// <summary>
        /// 绘制节点图
        /// </summary>
        /// <param name="id"></param>
        public void DrawChart() {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(mRect.width), GUILayout.Height(mRect.height));
            mNode.Draw();
            EditorGUILayout.LabelField("children:");
            foreach (var node in mChildNodeList) {
                node.Draw();
            }
            EditorGUILayout.EndScrollView();
        }
        public void DoWindow(int id) {
            this.DrawChart();
            GUI.DragWindow();
        }
        /// <summary>
        /// 绘制节点图关联曲线
        /// </summary>
        public void DrawCurve() {
            //绘制父节点与当前节点关系图
            foreach (var chart in mParentChart)
            {
                NodeUtil.DrawNodeCurve(chart.mRect, mRect, Color.black);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                   
                }
                if (null != mChildNodeList)
                {
                    mChildNodeList.Clear();
                }
                if (null != mParentChart)
                {
                    mParentChart.Clear();
                }
                if (null != mNode)
                {
                    mNode = null;
                }
                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
         ~NodeChart()
         {
             // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
             Dispose(disposing: false);
         }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
