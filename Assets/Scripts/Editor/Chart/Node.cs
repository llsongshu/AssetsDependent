using Assets.Dependent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Chart
{
    public class Node: System.IDisposable
    {
        public string assetPath;
        public Object mAsset;
        private bool disposedValue;

        public string Name {
            get {
                if (null != mAsset) {
                    return mAsset.name;
                }
                return "";
            }
        }
        public Node(string assetPath)
        {
            this.assetPath = assetPath;
            this.Init();
        }
        private void Init() {
            if (string.IsNullOrEmpty(assetPath)) {
                return;
            }
            mAsset = AssetDatabase.LoadMainAssetAtPath(assetPath);
        }
        public void Draw() {
            EditorGUILayout.ObjectField(mAsset, mAsset.GetType(), false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                mAsset = null;
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
