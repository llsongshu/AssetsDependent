using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace Assets.Graph
{
    public class GraphDirector
    {
        public List<AssetGraph> mGraphNodeList = new List<AssetGraph>();
        public void CreateGraph(Object[] objs) {
            this.Clean();
            foreach (var gbo in objs)
            {
                var assetPath = AssetDatabase.GetAssetPath(gbo);
                mGraphNodeList.Add(new AssetGraph(assetPath));
            }

            foreach (var graph in mGraphNodeList)
            {
                Vector2 pos = new Vector2(Random.Range(200, 824), Random.Range(100, 440));
                graph.BuildGraph(pos);
            }
        }
        public void Clean() {
            mGraphNodeList.Clear();
            System.GC.Collect();
        }
        ~GraphDirector()
        {
            this.Clean();
        }
    }
}
