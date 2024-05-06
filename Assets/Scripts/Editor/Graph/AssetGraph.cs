using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Assets.Dependent;
using UnityEditor;

namespace Assets.Graph
{
    public class AssetGraph:IDisposable
    {
        private static int idGen = 1;
        public int id { get; private set; } = idGen++;
        /// <summary>
        /// 打AssetBundle，需要去除父节点资源和子节点资源间接引用关系，以及循环依赖关系
        /// </summary>
        private AssetNode mAssetNode;
        public AssetNode GetNode() {
           return mAssetNode;
        }
        /// <summary>
        /// 存放父节点和子节点
        /// </summary>
        private List<INode> mParentNodeList = new List<INode>();
        private List<INode> mChildNodeList = new List<INode>();
        public int ChildCount { get { return mChildNodeList.Count; } }
        public int parentCount { get { return mParentNodeList.Count; } }
        const float vSpan = 30f;
        const float hSpan = 50f;
        public Vector2 mSize;
        public Rect mRect;
        public Rect mParentRect;
        public Rect mChildRect;
        public Node mCurNode;
        public string Name { get;private set; }
        public AssetGraph(string assetPath) {

            this.mAssetNode = AssetNode.Get(assetPath);
        }

        public void BuildGraph(Vector2 pos)
        {
            if (null == mAssetNode) {
                return;
            }
            mCurNode = new Node(mAssetNode);
            mCurNode.BuildGraph();
            this.Name = mCurNode.Name;

            mParentNodeList.Clear();
            mChildNodeList.Clear();
            var parents = CreateParentNodes(mAssetNode.mQuotedNodes);
            var childs = CreateParentNodes(mAssetNode.mQuoteNodes);
            mParentNodeList.AddRange(parents);
            mChildNodeList.AddRange(childs);
            float width = 100f;
            float height = 50;
            mRect = new Rect(pos.x + hSpan * 2, pos.y + vSpan * 2, width, height);

            mParentRect = new Rect(mRect.x - mRect.width - hSpan * 2, mRect.y, width * 2, height * 3);
            mChildRect = new Rect(mRect.x + mRect.width + hSpan * 2, mRect.y, width * 2, height * 3);
        }
        /// <summary>
        /// 创建父节点图
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private List<Node> CreateParentNodes(List<AssetNode> nodes) {
            List<Node> nodeLiset = new List<Node>();
            foreach (var childNode in nodes)
            {
                var gNode = new Node(childNode);
                nodeLiset.Add(gNode);
            }
            foreach (var node in nodeLiset)
            {
                node.BuildGraph();

            }
            return nodeLiset;
        }
       
        private Vector2 scrollPos;
        private Vector2 mScrollChilds;
        private Vector2 mScrollParents;
        private bool disposedValue;

        public void DrawGraph(int id)
        {
            if (null != mCurNode)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(mRect.width), GUILayout.Height(mRect.height));
                mCurNode.DrawNode();
                EditorGUILayout.EndScrollView();
                GUI.DragWindow();
            }
        }
        public void DrawChilds(int id)
        {
            mScrollChilds = EditorGUILayout.BeginScrollView(mScrollChilds, GUILayout.Width(mChildRect.width), GUILayout.Height(mChildRect.height));
            foreach (var node in mChildNodeList)
            {
                node.DrawNode();
            }
            EditorGUILayout.EndScrollView();
            GUI.DragWindow();
        }
        public void DrawParent(int id)
        {
            mScrollParents = EditorGUILayout.BeginScrollView(mScrollParents, GUILayout.Width(mParentRect.width), GUILayout.Height(mParentRect.height));
            foreach (var node in mParentNodeList)
            {
                node.DrawNode();
            }
            EditorGUILayout.EndScrollView();
            GUI.DragWindow();
        }
        public void DrawLine() {
            if (parentCount > 0)
            {
                NodeUtil.DrawNodeCurve(mParentRect, mRect, Color.blue);
            }
            if (ChildCount > 0)
            {
                NodeUtil.DrawNodeCurve(mRect, mChildRect, Color.green);
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
                if (null != mParentNodeList)
                {
                    mParentNodeList.Clear();
                }
                if (null != mCurNode)
                {
                    mCurNode.Dispose();
                    mCurNode = null;
                }
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~AssetGraph()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
