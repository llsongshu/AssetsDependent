using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace Assets.Dependent
{
    /// <summary>
    /// 收集资源和依赖资源
    /// </summary>
    public class AssetNode : Node, IDependent
    {
        /// <summary>
        /// 所有节点数据
        /// </summary>
        private static Dictionary<string, AssetNode> mAllNodes = null;
        public static Dictionary<string, AssetNode> GetAllNode() {
            return mAllNodes;
        }
        public string mAssetPath;
        //引用哪些资源节点
        public List<AssetNode> mQuoteNodes = new List<AssetNode>();
        /// <summary>
        /// 该资源被哪些资源节点应用
        /// </summary>
        public List<AssetNode> mQuotedNodes = new List<AssetNode>();
        public bool mIsCollected { get; private set; }
        static AssetNode() {
            mAllNodes = new Dictionary<string, AssetNode>();
        }
        public AssetNode(string mAssetPath)
        {
            this.mAssetPath = mAssetPath;
        }
        /// <summary>
        /// node节点是否引用了该节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool QuotedOf(AssetNode node) {
            if (null == node || null == mQuotedNodes || mQuotedNodes.Count <= 0) {
                return false;
            }
            return mQuotedNodes.Contains(node);
        }
        /// <summary>
        /// 是否引用了node节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool QuoteOf(AssetNode node)
        {
            if (null == node || null == mQuoteNodes || mQuoteNodes.Count <= 0)
            {
                return false;
            }
            return mQuoteNodes.Contains(node);
        }
        /// <summary>
        /// 去除子节点
        /// </summary>
        /// <param name="node"></param>
        public void RemoveQuoteNode(AssetNode node) {
            this.mQuoteNodes.Remove(node);
        }
        /// <summary>
        /// 删除被引用资源
        /// </summary>
        /// <param name="node"></param>
        public void RemoveQuotedNode(AssetNode node)
        {
            this.mQuotedNodes.Remove(node);
        }
        /// <summary>
        /// 添加引用的资源节点
        /// </summary>
        /// <param name="node"></param>
        public void AddQuoteNode(AssetNode node) {
            mQuoteNodes.Add(node);
            node.AddQuotedNode(this);
        }
        /// <summary>
        /// 添加被引用资源的节点
        /// </summary>
        /// <param name="node"></param>
        public void AddQuotedNode(AssetNode node) {
            mQuotedNodes.Add(node);
        }
        /// <summary>
        /// 收集依赖,递归收集依赖资源还未测试
        /// </summary>
        /// <param name="excludeSelf"></param>
        /// <param name="recursive"></param>
        public void Collect(bool excludeSelf=true, bool recursive=false)
        {
            if (this.mIsCollected) {
                return;
            }
            var dependences = AssetDatabase.GetDependencies(this.mAssetPath, recursive);
            var hashSet = new HashSet<string>(dependences);
            if (recursive && excludeSelf) {
                hashSet.Remove(mAssetPath);
            }
            this.mIsCollected = true;
            foreach (var childAsset in hashSet) {
                if (AssetUtil.IsVaild(childAsset))
                {
                    var node = AssetNode.GetOrCreteNode(childAsset);
                    this.AddQuoteNode(node);
                    node.Collect(excludeSelf, recursive);
                }
            }
        }

        public override void Dispose()
        {
            mQuoteNodes.Clear();
            mQuoteNodes = null;
            mQuotedNodes.Clear();
            mQuotedNodes = null;
        }

        public static void Create(List<string> assets, Action<float, string> OnProgress=null) {
            int nCount = 0;
            foreach (var asset in assets) {
                nCount += 1;
                AssetNode.GetOrCreteNode(asset);
                if (null != OnProgress) {
                    OnProgress(nCount * 1f / assets.Count, asset);
                }
            }
        }
        private static AssetNode GetOrCreteNode(string assetPath)
        {
            AssetNode node = null;
            if (!mAllNodes.TryGetValue(assetPath, out node))
            {
                node = new AssetNode(assetPath);
                mAllNodes[assetPath] = node;
            }
            return node;
        }
        public static AssetNode Get(string assetPath) {
            if (mAllNodes.ContainsKey(assetPath))
            {
                return mAllNodes[assetPath];
            }
            return null;
        }
        /// <summary>
        /// 收集所有节点的资源和依赖资源
        /// </summary>
        public static void CollectRes(Action<float, string> OnProgress) {
            List<AssetNode> nodeList = new List<AssetNode>();
            var iter = mAllNodes.GetEnumerator();
            while (iter.MoveNext())
            {
                nodeList.Add(iter.Current.Value);
            }
            for (int index=0; index < nodeList.Count; index++) {
                var node = nodeList[index];
                if (null != OnProgress) {
                    OnProgress(index * 1f / nodeList.Count, node.mAssetPath);
                }
                node.Collect();
            }
        }
        /// <summary>
        /// 重现计算节点间的依赖关系，例如,A->B,B->C,A->C=>
        /// </summary>
        [MenuItem("Tools/Dependent/AssetNodes/重新计算节点间依赖关系")]
        public static void CalculateDependency()
        {
            var iter = AssetNode.GetAllNode().GetEnumerator();
            while (iter.MoveNext()) {
                var node = iter.Current.Value;
                for (int index= node.mQuotedNodes.Count-1; index >= 0; index--) {
                    var pNode = node.mQuotedNodes[index];
                    if (pNode.QuoteOf(node))
                    {
                        pNode.ParentRemoveIndirect(node);
                    }
                }
            }
        }
        /// <summary>
        /// 去除节点父节点对该节点的直接引用关系(如果存在循环依赖关系可能会导致该节点从所有的被依赖节点中移除或者不删除)
        /// </summary>
        /// <param name="node"></param>
        private void ParentRemoveIndirect(AssetNode node) {
            var nCount = this.mQuotedNodes.Count;
            for (int index=nCount - 1; index>=0; index--) {
                var pNode = this.mQuotedNodes[index];
                if (pNode.QuoteOf(node))
                {
                    pNode.RemoveQuoteNode(node);
                    node.RemoveQuotedNode(pNode);
                }
                pNode.ParentRemoveIndirect(node);//存在循环依赖可能导致死循环
            }
        }
        /// <summary>
        /// 循环依赖检测
        /// </summary>
        public static void CircleDependenceDetect()
        {

        }
        /// <summary>
        /// 清理所有资源节点
        /// </summary>
        public static void Clean() {
            var iter = mAllNodes.GetEnumerator();
            while (iter.MoveNext()) {
                iter.Current.Value.Dispose();
            }
            mAllNodes.Clear();
        }
        /// <summary>
        /// 打印节点引用关系
        /// </summary>
        [MenuItem("Tools/Dependent/AssetNodes/打印节点数据 #3")]
        public static void PrintAssetNode() {
            var filePath = Application.dataPath;
            filePath = filePath.Substring(0, filePath.Length - 6) + "/Logs/assetNodes.log";
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
            using (StreamWriter writer = new StreamWriter(File.Open(filePath,FileMode.CreateNew)))
            {
                var iter = mAllNodes.GetEnumerator();
                while (iter.MoveNext())
                {
                    var node = iter.Current.Value;
                    writer.WriteLine($"{node.mAssetPath}");
                    if (node.mQuoteNodes.Count > 0)
                    {
                        writer.WriteLine(@"==========引用资源==========");
                        foreach (var childNode in node.mQuoteNodes)
                        {
                            writer.WriteLine($"==> {childNode.mAssetPath}");
                        }
                    }
                    if (node.mQuotedNodes.Count > 0)
                    {
                        writer.WriteLine(@"==========被引用资源==========");
                        foreach (var childNode in node.mQuotedNodes)
                        {
                            writer.WriteLine($"==> {childNode.mAssetPath}");
                        }
                    }
                    writer.WriteLine();
                }
                writer.Flush();
            }
        }
    }
}