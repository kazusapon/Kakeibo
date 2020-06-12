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
using OxyPlot.Axes;
using OxyPlot.Series;

using Eri.Models;
using Eri.Views;

namespace Eri.ViewModels
{
    public class SummaryViewModel : BaseViewModel
    {
        //円グラフのオブジェクト
        public PlotModel Pie_chart { get; private set; } = new PlotModel() { PlotMargins = new OxyThickness(0,25,0,0) };
        //折れ線グラフのオブジェクト
        public PlotModel Line_chart { get; private set; } = new PlotModel() {
            LegendPlacement = LegendPlacement.Outside,
            LegendBackground = OxyColors.White,
            LegendBorder = OxyColors.Gray,
            LegendTextColor = OxyColors.Black,
            LegendSymbolPlacement = LegendSymbolPlacement.Left,
            LegendPosition = LegendPosition.BottomCenter,
            PlotAreaBorderColor = OxyColors.Transparent
        };
        //横軸
        public LinearAxis AxisX { get; private set; }
        //縦軸
        public LinearAxis AxisY { get; private set; }
        //折れ線グラフの線のオブジェクト
        private List<DataPoint> spend_line = new List<DataPoint>();
        //円グラフの各弧を格納するオブジェクト
        private List<PieSlice> slice_data = new List<PieSlice>();

        public int Shokuhi { get; set; }
        public int TotalSpend { get; set; }
        public int TotalIncome { get; set; }
        public int Available { get; set; }
        public int Diff_Shokuhi { get; set; }
        public int Diff_Total { get; set; }

        //円グラフのプロパティ設定
        private PieSeries pieseries = new PieSeries()
        {
            StrokeThickness = 0.2, //ストロークの太さ
            InsideLabelPosition = 0.5, //表内ラベルの位置
            AngleSpan = 360, //表の角度（円グラフなら360）
            StartAngle = 270, //これなんだ？？
            AreInsideLabelsAngled = true //グラフ内の文字を中心から弧に向かう傾きにする

        };

        //折れ線グラフのプロパティ設定
        private LineSeries line_series = new LineSeries()
        {
            Title = "収入",
            DataFieldX = nameof(SpendLineModel.Mon),
            DataFieldY = nameof(SpendLineModel.Money)
    };




        public SummaryViewModel(int add_month = 0)
        {
            if (Now_Summary.Year < 2000)
            {
                Now_Summary = DateTime.Now;
            }

            if (add_month != 0)
            {
                //サマリ表示の日付の更新
                Now_Summary = Now_Summary.AddMonths(add_month);
            }

            //グラフデータを追加
            //円グラフの場合
            Pie_Init();
            pieseries.Slices = slice_data;
            Pie_chart.Series.Add(pieseries);

            //折れ線グラフの場合
            Line_Init();
            line_series.Points.AddRange(spend_line);
            Line_chart.Series.Add(line_series);
            
            Chart_Update(); //グラフの更新
            SetBarData();
        }


        //円グラフを再読み込みする
        public void Pie_Init()
        {
            //グラフを初期化
            slice_data.Clear();
            slice_data.AddRange(Create_Pie().Where(x => x.Value > 0).Select(x => new PieSlice(x.Label, x.Value))); //表示するデータを表示。
            
        }

        public void Line_Init()
        {
            //折れ線グラフの初期化
            spend_line.Clear();
            DateTimeAxis axisx = new DateTimeAxis {
                Minimum = DateTimeAxis.ToDouble(GetStartEndDate(Now_Summary,  1)), //グラフのX軸の最小値
                Maximum = DateTimeAxis.ToDouble(GetStartEndDate(Now_Summary, 12)), //グラフのX軸の最大値
                StringFormat = "MM月" //月のみを表示
            };
            spend_line.AddRange(CreateSpendLine().Select(x => new DataPoint(DateTimeAxis.ToDouble(x.Mon), x.Money)));
        }

        //グラフのアップデート
        public void Chart_Update()
        {
            Pie_chart.InvalidatePlot(true); //グラフを更新する。
            Line_chart.InvalidatePlot(true);
        }

        //プログレスバーの値を設定
        public void SetBarData()
        {
            using (var db = new MyContext())
            {
                SummaryModel summary = new SummaryModel(db);
                //Shokuhi = summary.GetTotalShokuhi(Now_Summary);
                TotalSpend = summary.GetTotalSpend(Now_Summary) + summary.GetTotalFixed(Now_Summary);
                TotalIncome = summary.GetTotalIncome(Now_Summary);
                //Available = summary.GetTotalAvailable(Now_Summary);
                //Diff_Shokuhi = Available - Shokuhi;
                Diff_Total = TotalIncome - TotalSpend;
            }
        }

        private List<SpendPieModel> Create_Pie()
        {
            using (var db = new MyContext())
            {
                SummaryModel summary = new SummaryModel(db);
                var piedata = summary.GetSpendPie(Now_Summary);
                Pie_chart.Title = $"合計：{summary.TotalSpend(Now_Summary).ToString()} 円";
                return piedata;
            }
        }
        private List<SpendLineModel> CreateSpendLine()
        {
            using (var db = new MyContext())
            {
                SummaryModel summary = new SummaryModel(db);
                var spend = summary.GetSpendLine(Now_Summary);
                return spend;
            }
        }

        private DateTime GetStartEndDate(DateTime date, int mon)
        {
            int yyyy = date.Year;
            DateTime day = new DateTime(yyyy, mon, 1, 0, 0, 0);
            return day;
        }

    }
}