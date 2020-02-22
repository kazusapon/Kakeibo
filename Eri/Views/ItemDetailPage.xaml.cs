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
        Tra_Income viewModel;

        public ItemDetailPage(Tra_Income viewModel)
        {
            InitializeComponent();
            //viewModel.P

            BindingContext = this.viewModel = viewModel;
            
        }

        public ItemDetailPage()
        {
            InitializeComponent();
            
            var item = new Tra_Income();

            //viewModel = new ItemDetailViewModel(item);
            using (var db = new MyContext())
            {
                var a = db.Tra_Income.Where(x => x.Id == item.Id).AsEnumerable();
                BindingContext = a;
            }
                //BindingContext = viewModel;
        }
    }
}