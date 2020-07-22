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
        public IList<Mst_Income> PickerList { get; set; }
        public int Selected { get; set; }

        //public delegate void UpdateOrInsert(object sender, EventArgs s);
        //public List<string> PickerStrings { get; set; }
        //public string Methods { get; set; }

        public NewItemPage(Tra_Income viewdata=null)
        {
            InitializeComponent();
            ItemsViewModel item = new ItemsViewModel();
            PickerList = item.Get_Picker_List().ToList();
            this.MyPicker.Title = "選択してください";
            if (viewdata == null)
            {
                Item = new Tra_Income();
                Item.Payment_Date = DateTime.Today;
                Selected = 0;
                this.button.Clicked += Save_Clicked;
            }
            else
            {
                Item = viewdata;
                Selected = viewdata.Income_Id - 1;
                this.button.Clicked += Update_Clicked;
            }
            BindingContext = this;
        }

        public void Update_Clicked(object sender, EventArgs s)
        {
            BindingContext = this;
            //ゼロ円の場合はインサートしない
            if (Item.Money == 0)
            {
                MoneyCheck(Item.Money);
                return;
            }
            
            //選択されたPickerを変数に代入する
            var picker_item = MyPicker.SelectedItem as Mst_Income;

            using (var db = new MyContext())
            {
                Item.Income_Id = picker_item.Id;
                Income income = new Income(db);
                income.Update_Item(Item);
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
                var picker_item = MyPicker.SelectedItem as Mst_Income;
                
                //ゼロ円の場合はインサートしない
                if (Item.Money == 0)
                {
                    MoneyCheck(Item.Money);
                    return;
                }

                db.Database.EnsureCreated();

                DateTime now = DateTime.Now;

                db.Tra_Income.Add(new Tra_Income
                {
                    User_Id = 1,
                    Income_Id = picker_item.Id,
                    Payment_Date = Item.Payment_Date.Date,
                    Money = Item.Money,
                    Description = Item.Description,
                    Created_At = now,
                    Updated_At = now,
                    Del_Flag = false,
                    Linked_Flag = false
                }) ;
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