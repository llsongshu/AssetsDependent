using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Dependent;
using UnityEditor;

namespace Assets.Chart
{
    public class ChartDirecfor
    {
        public Dictionary<AssetNode,NodeChart> mNodeCharts = new Dictionary<AssetNode, NodeChart>();
        static public ChartDirecfor _inst;

        static public ChartDirecfor Inst {
            get {
                if (null == _inst) {
                    _inst = new ChartDirecfor();
                }
                return _inst;
            }
        }
        protected static Rect GetRect() {

            return new Rect(Random.Range(200, 824), Random.Range(100, 440), 120f, 150f);
        }
        internal static NodeChart GetOrCreateNodeChart(AssetNode anode) {
            NodeChart nc = null;
            if (!ChartDirecfor.Inst.mNodeCharts.TryGetValue(anode, out nc)) {
                nc = new NodeChart(anode);
                ChartDirecfor.Inst.mNodeCharts[anode] = nc;
                nc.Init(ChartDirecfor.GetRect());
            }
            return nc;
        }
        public void CreateChart(Object[] objs) {
            this.Clean();
            NodeChart.idGen = 1;
            HashSet<NodeChart> chartSet = new HashSet<NodeChart>();
            foreach (var obj in objs) {
                var assetPath = AssetDatabase.GetAssetPath(obj);
                var assetNode = AssetNode.Get(assetPath);
                if (null != assetNode) {
                    var chart = ChartDirecfor.GetOrCreateNodeChart(assetNode);
                    chartSet.Add(chart);
                }
            }
        }

        protected virtual void Clean()
        {
            mNodeCharts.Clear();
            System.GC.Collect();
        }

        ~ChartDirecfor()
        {
            Clean();
        }
    }
}
