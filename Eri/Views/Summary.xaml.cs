using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.StyleSheets;
using Eri.Models;
using Eri.Views;
using Eri.ViewModels;


namespace Eri.Views
{
    [DesignTimeVisible(false)]
    public partial class Summary : ContentPage
    {

        SummaryViewModel viewModel;

        public Summary()
        {
            InitializeComponent();
            viewModel = new SummaryViewModel();
            SetBarLong(viewModel);
            BindingContext = viewModel;

        }

        void Chenge_Month_Add(object sender, EventArgs e)
        {
            viewModel = new SummaryViewModel(1);
            SetBarLong(viewModel);
            BindingContext = viewModel;
            OnAppearing();
        }

        void Chenge_Month_Remove(object sender, EventArgs e)
        {
            viewModel = new SummaryViewModel(-1);
            SetBarLong(viewModel);
            BindingContext = viewModel;
            OnAppearing();
        }

        //バーの進捗率を更新
        private void SetBarLong(SummaryViewModel viewmodel)
        {
            this.progress_total.Progress = CalcDecimal(viewmodel.TotalIncome, viewmodel.TotalSpend);
            //this.progress_shokuhi.Progress = CalcDecimal(viewmodel.Available, viewmodel.Shokuhi);
        }

        //プログレスバーに表示する少数値を求める
        //m1 : 金額の基準となるもの
        private double CalcDecimal(double m1, double m2)
        {
            double result;
            try
            {
                result = m2 / m1;
            }
            catch(Exception)
            {
                return 0;
            }
            if (result > 1.0)
            {
                result = 1.0;
            }
            return result;
        }

    }
}