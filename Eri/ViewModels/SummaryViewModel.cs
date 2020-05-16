﻿using System;
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

using OxyPlot.Series;

using Eri.Models;
using Eri.Views;

namespace Eri.ViewModels
{
    public class SummaryViewModel : BaseViewModel
    {
        public PlotModel Pie_chart { get; private set; } = new PlotModel() { };
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
            Pie_Init();
            pieseries.Slices = slice_data;
            Pie_chart.Series.Add(pieseries);
            Pie_Update();

            SetBarData();
        }


        //円グラフを再読み込みする
        public void Pie_Init()
        {
            //グラフを初期化
            slice_data.Clear();
            slice_data.AddRange(Create_Pie().Where(x => x.Value > 0).Select(x => new PieSlice(x.Label, x.Value))); //表示するデータを表示。
            
        }

        public void Pie_Update()
        {
            Pie_chart.InvalidatePlot(true); //グラフを更新する。
        }

        //プログレスバーの値を設定
        public void SetBarData()
        {
            using (var db = new MyContext())
            {
                SummaryModel summary = new SummaryModel(db);
                Shokuhi = summary.GetTotalShokuhi(Now_Summary);
                TotalSpend = summary.GetTotalSpend(Now_Summary) + summary.GetTotalFixed(Now_Summary);
                TotalIncome = summary.GetTotalIncome(Now_Summary);
                Available = summary.GetTotalAvailable(Now_Summary);
                Diff_Shokuhi = Available - Shokuhi;
                Diff_Total = TotalIncome - TotalSpend;
            }
        }

        private List<SpendPieModel> Create_Pie()
        {
            using (var db = new MyContext())
            {
                SummaryModel summary = new SummaryModel(db);
                var piedata = summary.GetSpendPie(Now_Summary);
                return piedata;
            }
        }

    }

}