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
    public partial class NewSpendPage : ContentPage
    {
        public Tra_Spending Spends { get; set; }
        public IList<Mst_Spend> PickerListSpend { get; set; }
        public int Selected { get; set; }

        //public delegate void UpdateOrInsert(object sender, EventArgs s);
        //public List<string> PickerStrings { get; set; }
        //public string Methods { get; set; }

        public NewSpendPage(Tra_Spending viewdata=null)
        {
            InitializeComponent();
            SpendViewModel spend = new SpendViewModel();
            PickerListSpend = spend.Get_Picker_List().ToList();
            this.MyPicker.Title = "選択してください";
            if (viewdata == null)
            {
                Spends = new Tra_Spending();
                Spends.Purchase_Date = DateTime.Today;
                Selected = 0;
                this.button.Clicked += Save_Clicked;
            }
            else
            {
                Spends = viewdata;
                Selected = viewdata.Spend_Id - 1;
                this.button.Clicked += Update_Clicked;
            }
            BindingContext = this;
        }

        public void Update_Clicked(object sender, EventArgs s)
        {
            BindingContext = this;
            //ゼロ円の場合はインサートしない
            if (Spends.Money == 0)
            {
                MoneyCheck(Spends.Money);
                return;
            }
            
            //選択されたPickerを変数に代入する
            var picker_item = MyPicker.SelectedItem as Mst_Spend;

            using (var db = new MyContext())
            {
                Spends.Spend_Id = picker_item.Id;
                Spend spend = new Spend(db);
                spend.Update_Spend(Spends);
            }
            Application.Current.MainPage = new MainPage();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            //await DisplayAlert("aaa", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "OK", "キャンセル");
            BindingContext = this;
            //MessagingCenter.Send(this, "AddItem", Item);
            //await Navigation.PopModalAsync();
            
            using (var db = new MyContext())
            {
                var picker_item = MyPicker.SelectedItem as Mst_Spend;
                
                //ゼロ円の場合はインサートしない
                if (Spends.Money == 0)
                {
                    MoneyCheck(Spends.Money);
                    return;
                }

                db.Database.EnsureCreated();
                db.Tra_Spending.Add(new Tra_Spending
                {
                    User_Id = 1,
                    Spend_Id = picker_item.Id,
                    Purchase_Date = Spends.Purchase_Date.Date,
                    Money = Spends.Money,
                    Description = Spends.Description,
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

        private void MoneyCheck(int money)
        {

            DisplayAlert("Error", "ゼロ円の場合は、保存されません。", "OK");
        }
    }
}