using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Assets.Dependent;

namespace Assets.Graph
{
    public class Node : INode
    {
        public AssetNode node = null;
        protected Object mAsset = null;
        private bool disposedValue;

        public string Name {
            get {
                if (null != mAsset) {
                    return mAsset.name;
                }
                return "";
            }
        }
        public Node(AssetNode _node)
        {
            this.node = _node;
        }
        public void BuildGraph()
        {
            if (null != this.node)
            {
                mAsset = AssetDatabase.LoadMainAssetAtPath(this.node.mAssetPath);
            }
            else {
                Debug.LogWarning($"Node Is Null.");
            }
        }
        public void DrawNode()
        {
            if (null != mAsset)
            {
                EditorGUILayout.ObjectField(mAsset, mAsset.GetType(), false);
            }
            else {
                Debug.LogWarning($"{node.mAssetPath}[asset is null]");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                this.mAsset = null;
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~Node()
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
