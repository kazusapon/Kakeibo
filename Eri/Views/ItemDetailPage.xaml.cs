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
    public partial class ItemDetailPage : ContentPage
    {
        //ItemDetailViewModel viewModel;
        IncomeDetail viewModel;

        public ItemDetailPage(IncomeDetail viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Tra_Income();

            //viewModel = new ItemDetailViewModel(item);
            using (var db = new MyContext())
            {
                var result = db.Tra_Income.Where(x => x.Id == item.Id).AsEnumerable();
                BindingContext = result;
            }
            //BindingContext = viewModel;
        }

        public async void Edit_Click_Income(object sender, EventArgs e)
        {
            Income income = new Income(new MyContext());
            var nav = new NavigationPage(new NewItemPage(income.Income_Fetch(this.viewModel.Id)));
            await Navigation.PushModalAsync(nav);
        }

        public async void Delete_Clicked_Income(object sender, EventArgs e)
        {
            var handan = await DisplayAlert("確認", "削除してもよろしいでしょうか？", "OK", "キャンセル");
            if (handan)
            {
                using (var db = new MyContext())
                {
                    BindingContext = this;
                    var item = new Tra_Income();
                    Income income = new Income(db);
                    income.Delete_Income(this.viewModel.Id);
                }
                Application.Current.MainPage = new MainPage();
            }
            //await Navigation.PopModalAsync();
        }
    }
}