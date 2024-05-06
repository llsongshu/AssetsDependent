using UnityEngine;
using UnityEditor;
using System.IO;

namespace Assets.Graph
{
    public class DependencyWindow : EditorWindow
    {
        public GraphDirector graphDirector = new GraphDirector();
        [MenuItem("Tools/Dependent/依赖关系图  #1")]
        static void DependenceWindow()
        {
            DependencyWindow window = EditorWindow.GetWindow<DependencyWindow>();
            window.minSize = new Vector2(1024, 540);
            window.maxSize = new Vector2(2048, 1080);
            window.Show();
            Selection.selectionChanged -= window.SelectionChanged;
            Selection.selectionChanged += window.SelectionChanged;
        }
        private void SelectionChanged()
        {
            this.graphDirector.CreateGraph(Selection.objects);
            this.Repaint();
        }

        void OnGUI()
        {
            if (GUILayout.Button("导出相关Prefab", GUILayout.MaxWidth(100), GUILayout.Height(30)))
            {
                foreach (var graph in graphDirector.mGraphNodeList)
                {
                    string filePath = $"{Application.dataPath}/../{graph.Name}.txt";
                    using (var fs = File.OpenWrite(filePath)) {
                        var writer = new StreamWriter(fs);
                        var node = graph.GetNode();
                        foreach (var p in node.mQuotedNodes) {
                            if (p.mAssetPath.EndsWith(".prefab"))
                            {
                                writer.WriteLine(p.mAssetPath);
                            }
                        }
                        writer.WriteLine();
                        foreach (var p in node.mQuoteNodes)
                        {
                            if (p.mAssetPath.EndsWith(".prefab"))
                            {
                                writer.WriteLine(p.mAssetPath);
                            }
                        }
                        writer.Flush();
                    }
                }
            }
            BeginWindows();
            foreach (var graph in graphDirector.mGraphNodeList) {
                graph.mRect = GUI.Window(graph.id, graph.mRect, graph.DrawGraph, graph.Name);
                if (graph.ChildCount > 0)
                {
                    graph.mChildRect = GUI.Window(graph.id * 1000 + 1, graph.mChildRect, graph.DrawChilds, "children");
                }
                if (graph.parentCount > 0)
                {
                    graph.mParentRect = GUI.Window(graph.id * 1000 + 2, graph.mParentRect, graph.DrawParent, "parents");
                }
                graph.DrawLine();
            }
            EndWindows();
        }
    }
}
