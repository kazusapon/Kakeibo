using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Xamarin.Forms;

using OxyPlot;
using OxyPlot.Xamarin;
using OxyPlot.Axes;
using OxyPlot.Series;

using Eri.Models;
using Eri.Views;

namespace Eri.ViewModels
{
    class SummaryViewModel : BaseViewModel
    {
        public PlotModel Pie_chart { get; set; }
        private List<PieSlice> slice_data = new List<PieSlice>();

        public SummaryViewModel()
        {
            if (Now_Summary.Year < 2000)
            {
                Now_Summary = DateTime.Now;
            }
            //グラフデータを追加
            pieseries.Slices = slice_data;
            Pie_chart.Series.Add(pieseries);

        }

        //円グラフのプロパティ設定
        public PieSeries pieseries = new PieSeries()
        {
            StrokeThickness = 2.0, //ストロークの太さ
            InsideLabelPosition = 0.6, //表内ラベルの位置
            AngleSpan = 360, //表の角度（円グラフなら360）
            StartAngle = 270 //これなんだ？？
        };

        //円グラフを再読み込みする
        public void Pie_Update()
        {
            //グラフを初期化
            slice_data.Clear();
            slice_data.AddRange(Create_Pie().Select(x => new PieSlice(x.Label, x.Value))); //表示するデータを表示。
            Pie_chart.InvalidatePlot(true); //グラフを更新する。

        }

        private List<IncomePieModel> Create_Pie()
        {
            using (var db = new MyContext())
            {
                SummaryModel summary = new SummaryModel(db);
                var piedata = summary.GetIncomePie(Now_Summary);
                return piedata;
            }
        }

    }
}
