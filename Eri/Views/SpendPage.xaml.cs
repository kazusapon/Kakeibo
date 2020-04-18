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
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer

    [DesignTimeVisible(false)]
    public partial class SpendPage : ContentPage
    {
        SpendViewModel viewModel;

        public SpendPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SpendViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            //var item = args.SelectedItem as Item;
            var item = args.SelectedItem as SpendDetail;
            if (item == null)
                return;

            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            var nav = new SpendDetailPage(item);
            SpendListView.SelectedItem = null;
            await Navigation.PushAsync(nav);

            // Manually deselect item.
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            var nav = new NavigationPage(new NewSpendPage());
            await Navigation.PushModalAsync(nav);

        }

        void Chenge_Month_Add(object sender, EventArgs e)
        {
            BindingContext = viewModel = new SpendViewModel(1);
            OnAppearing();
        }

        void Chenge_Month_Remove(object sender, EventArgs e)
        {
            BindingContext = viewModel = new SpendViewModel(-1);
            OnAppearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Spends.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}