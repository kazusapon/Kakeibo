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
    public partial class IncomePage : ContentPage
    {
        ItemsViewModel viewModel;

        public IncomePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            //var item = args.SelectedItem as Item;
            var item = args.SelectedItem as Tra_Income;
            if (item == null)
                return;

            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            var nav = new ItemDetailPage(item);
            await Navigation.PushAsync(nav);

            // Manually deselect item.
            //ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            var nav = new NavigationPage(new NewItemPage());
            await Navigation.PushModalAsync(nav);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}