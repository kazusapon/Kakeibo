using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

using Eri.Models;
using Eri.ViewModels;

namespace Eri.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class SpendDetailPage : ContentPage
    {
        //ItemDetailViewModel viewModel;
        SpendDetail viewModel;

        public SpendDetailPage(SpendDetail viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

        }

        public SpendDetailPage()
        {
            InitializeComponent();

            var item = new Tra_Spending();

            //viewModel = new ItemDetailViewModel(item);
            using (var db = new MyContext())
            {
                var result = db.Tra_Spending.Where(x => x.Id == item.Id).AsEnumerable();
                BindingContext = result;
            }
            //BindingContext = viewModel;
        }

        public async void Edit_Click_Spend(object sender, EventArgs e)
        {
            Spend spend = new Spend(new MyContext());
            var nav = new NavigationPage(new NewSpendPage(spend.Spend_Fetch(this.viewModel.Id)));
            await Navigation.PushModalAsync(nav);
        }

        public async void Delete_Clicked_Spend(object sender, EventArgs e)
        {
            var handan = await DisplayAlert("確認", "削除してもよろしいでしょうか？", "OK", "キャンセル");
            if (handan)
            {
                using (var db = new MyContext())
                {
                    BindingContext = this;
                    var item = new Tra_Income();
                    Spend spend = new Spend(db);
                    spend.Delete_Spend(this.viewModel.Id);
                }
                Application.Current.MainPage = new MainPage();
            }
            //await Navigation.PopModalAsync();
        }
    }
}