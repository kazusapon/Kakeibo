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
    public class SpendViewModel : BaseViewModel
    {
        //public ObservableCollection<Tra_Income> Items { get; set; }
        public ObservableCollection<SpendDetail> Spends { get; set; }
        public Command LoadItemsCommand { get; set; }
        //public ImageSource ListImage { get; set; }

        public SpendViewModel(int add_month=0)
        {
            Title = "支出管理";

            if (Now_Spend.Year < 2000)
            {
                Now_Spend = DateTime.Now;
            }

            DateTime target_date = Now_Spend.AddMonths(add_month);
            Now_Spend = Now_Spend.AddMonths(add_month);

            //ListImage = ImageSource.FromResource("Eri.Imeges.pig.png");
            Spends = new ObservableCollection<SpendDetail>();
            
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(target_date));

            MessagingCenter.Subscribe<NewSpendPage, SpendDetail>(this, "AddItem", async (obj, spend) =>
            {
                var newspend = spend as SpendDetail;
                Spends.Add(newspend);
                //await DataStore.AddItemAsync(newItem);
            });
        }

        public IList<Mst_Spend> Get_Picker_List()
        {
            using (var db = new MyContext())
            {
                Spend spend = new Spend(db);
                var r = spend.Get_MasterSpend();
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
                Spends.Clear();
                using (var db = new MyContext())
                {
                    ///var result = db.Tra_Income.Where(x => x.Money >= 1)
                    //                            .OrderByDescending(x => x.Payment_Date)
                    //                            .OrderByDescending(x => x.Id)
                    //                            .AsEnumerable();
                    Spend spend = new Spend(db);

                    var result = spend.Get_Spend_List(target_date);

                    //var items = await DataStore.GetItemsAsync(true);
                    foreach (var item in result)
                    {
                        Spends.Add(item);
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