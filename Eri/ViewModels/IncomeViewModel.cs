using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Xamarin.Forms;

using Eri.Models;
using Eri.Views;

namespace Eri.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        //public ObservableCollection<Tra_Income> Items { get; set; }
        public ObservableCollection<Tra_Income> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public ImageSource ListImage { get; set; }
        public ItemsViewModel()
        {
            Title = "収入管理";
            ListImage = ImageSource.FromResource("Eri.Imeges.pig.png");
            Items = new ObservableCollection<Tra_Income>();
            
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Tra_Income>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Tra_Income;
                Items.Add(newItem);
                //await DataStore.AddItemAsync(newItem);
            });
        }

        public IEnumerable<Mst_Income> Get_Picker_List()
        {
            using (var db = new MyContext())
            {
                Income income = new Income(db);
                var r = income.Get_MasterIncome();
                return r;
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                using (var db = new MyContext())
                {
                    ///var result = db.Tra_Income.Where(x => x.Money >= 1)
                    //                            .OrderByDescending(x => x.Payment_Date)
                    //                            .OrderByDescending(x => x.Id)
                    //                            .AsEnumerable();
                    Income income = new Income(db);

                    var result = income.Get_Income_List(DateTime.Now.Month);

                    //var items = await DataStore.GetItemsAsync(true);
                    foreach (var item in result)
                    {
                        Items.Add(new Tra_Income
                        {
                            Id = item.Id,
                            Money = item.Money,
                            Payment_Date = item.Payment_Date
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}