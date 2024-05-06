
using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Chart
{
    public class ChartWindow:EditorWindow
    {
        [MenuItem("Tools/Dependent/被依赖关系图 #2")]
        static void OpenChartWindow() {
            var window = EditorWindow.GetWindow<ChartWindow>();
            window.Init();
            window.Show();
            Selection.selectionChanged -= window.SelectionChanged;
            Selection.selectionChanged += window.SelectionChanged;

        }
        private void SelectionChanged()
        {
            ChartDirecfor.Inst.CreateChart(Selection.objects);
            this.Repaint();
        }

        private void Init() {
            this.minSize = new Vector2(1024, 540);
            this.maxSize = new Vector2(2048, 1080);
        }
        private void OnGUI()
        {
            BeginWindows();
            var iter = ChartDirecfor.Inst.mNodeCharts.GetEnumerator();
            while (iter.MoveNext()) {
                var chart = iter.Current.Value;
                chart.mRect = GUI.Window(chart.id, chart.mRect, chart.DoWindow, chart.Name);
            }
            iter = ChartDirecfor.Inst.mNodeCharts.GetEnumerator();
            while (iter.MoveNext())
            {
                iter.Current.Value.DrawCurve();
            }
            EndWindows();
        }
    }
}
