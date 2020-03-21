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
        public ObservableCollection<IncomeDetail> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public ImageSource ListImage { get; set; }

        public ItemsViewModel(int add_month=0)
        {
            Title = "収入管理";

            if (Now.Year < 2000)
            {
                Now = DateTime.Now;
            }

            DateTime target_date = Now.AddMonths(add_month);
            Now = Now.AddMonths(add_month);

            ListImage = ImageSource.FromResource("Eri.Imeges.pig.png");
            Items = new ObservableCollection<IncomeDetail>();
            
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(target_date));

            MessagingCenter.Subscribe<NewItemPage, IncomeDetail>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as IncomeDetail;
                Items.Add(newItem);
                //await DataStore.AddItemAsync(newItem);
            });
        }

        public IList<Mst_Income> Get_Picker_List()
        {
            using (var db = new MyContext())
            {
                Income income = new Income(db);
                var r = income.Get_MasterIncome();
                return r;
            }
        }

        async Task ExecuteLoadItemsCommand(DateTime target_date)
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

                    var result = income.Get_Income_List(target_date);

                    //var items = await DataStore.GetItemsAsync(true);
                    foreach (var item in result)
                    {
                        Items.Add(item);
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