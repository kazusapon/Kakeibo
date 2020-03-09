using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Threading.Tasks;
using Eri.ViewModels;

using Eri.Models;

namespace Eri.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public Tra_Income Item { get; set; }
        public IEnumerable<Mst_Income> PickerList { get; set; }

        public NewItemPage(Tra_Income viewdata=null)
        {
            InitializeComponent();
            ItemsViewModel item = new ItemsViewModel();
            PickerList = item.Get_Picker_List();
            if (viewdata == null)
            {
                Item = new Tra_Income();
                Item.Payment_Date = DateTime.Today;
            }
            else
            {
                Item = viewdata;
            }
            BindingContext = this;
        }
        async void Save_Clicked(object sender, EventArgs e)
        {
            //await DisplayAlert("aaa", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "OK", "キャンセル");
            BindingContext = this;
            //MessagingCenter.Send(this, "AddItem", Item);
            //await Navigation.PopModalAsync();
            
            using (var db = new MyContext())
            {
                db.Database.EnsureCreated();
                db.Tra_Income.Add(new Tra_Income
                {
                    User_Id = 1,
                    Income_Id = 1,
                    Payment_Date = Item.Payment_Date.Date,
                    Money = Item.Money,
                    Description = Item.Description,
                    Linked_Flag = false
                });
                await db.SaveChangesAsync();
                //await DisplayAlert("aaaa", a.ToString(), "キャンセル", "OK");
            }
            Application.Current.MainPage = new MainPage();
            //await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}