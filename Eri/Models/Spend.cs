using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eri.Models
{
    public class Spend
    {
        private MyContext _context;
        public Spend(MyContext context)
        {
            this._context = context;
        }

        public IList<Mst_Spend> Get_MasterSpend()
        {
            this._context.Database.EnsureCreated();
            IList<Mst_Spend> _result = this._context.Mst_Spend.OrderBy(x => x.Id).ToList();
            return _result;
        }

        public Tra_Spending Spend_Fetch(int id)
        {
            var _result = this._context.Tra_Spending.Where(x => x.Id == id).Where(x => x.Del_Flag == false).FirstOrDefault();
            return _result;
        }

        public async void Delete_Spend(int id)
        {
            var _result = this._context.Tra_Spending.FirstOrDefault(x => x.Id == id);
            _result.Updated_At = DateTime.Now;
            _result.Del_Flag = true;
            _result.Linked_Flag = false;
            await this._context.SaveChangesAsync();
            return;
        }

        public async void Update_Spend(Tra_Spending spend)
        {
            //更新金額がゼロ円の場合は、そのままリターン
            if (spend.Money == 0)
            {
                return;
            }
            this._context.Database.EnsureCreated();
            var _result = Spend_Fetch(spend.Id);
            _result.User_Id = 1;
            _result.Spend_Id = spend.Spend_Id;
            _result.Purchase_Date = spend.Purchase_Date.Date;
            _result.Money = spend.Money;
            _result.Description = spend.Description;
            _result.Linked_Flag = false;
            _result.Updated_At = DateTime.Now;
            await this._context.SaveChangesAsync();
        }

        public IEnumerable<SpendDetail> Get_Spend_List(DateTime target_date)
        {
            IEnumerable<SpendDetail> result = null;
            /*result = (from income in this._context.Tra_Income
                      where income.Money > 0
                      where income.Payment_Date.Month == month
                      group income by new { income.Payment_Date.Date } into X
                      orderby X.Key.Date descending
                    select new Tra_Income
                    {
                        Payment_Date = X.Key.Date,
                        Money = X.Sum(x => x.Money)
                    }).AsEnumerable();*/
            int yyyy = target_date.Year;
            int mm = target_date.Month;

            DateTime start_day = new DateTime(yyyy, mm, 1);
            DateTime end_day = start_day.AddMonths(1);
            result = from tra_spend in this._context.Tra_Spending
                     join mst_spend in this._context.Mst_Spend
                     on tra_spend.Spend_Id equals mst_spend.Id
                     where tra_spend.Purchase_Date >= start_day
                     where tra_spend.Purchase_Date <= end_day
                     where tra_spend.Money > 0
                     where tra_spend.Del_Flag == false
                     orderby tra_spend.Purchase_Date descending
                     select new SpendDetail
                     {
                         Id = tra_spend.Id,
                         Spend_Name = mst_spend.Spend_Name,
                         Money = tra_spend.Money,
                         Description = tra_spend.Description,
                         Purchase_Date = tra_spend.Purchase_Date,
                         Icon_Name = mst_spend.Icon_Name
                     };
            /*
            result = this._context.Tra_Income.Where(x => x.Payment_Date >= start_day)
                    .Where(x => x.Payment_Date < end_day)
                    .Where(x => x.Money > 0)
                    .OrderByDescending(x => x.Payment_Date.Day)
                    .AsEnumerable();*/
                
            return result;
        }

        public async Task SpendMasterInsertAllAsync(List<Mst_Spend> spends)
        {
            await this._context.Mst_Spend.AddRangeAsync(spends);
            await this._context.SaveChangesAsync();
        }

        public void MstSpendDelete()
        {
            if (this._context.Mst_Spend.Count() == 0)
            {
                return;
            }

            var result = this._context.Mst_Spend.ToList();
            this._context.RemoveRange(result);

            this._context.SaveChanges();
        }

        public async Task SpendListInsertOrUpdateAllAsync(List<Tra_Spending> spends)
        {
            if (spends.Count == 0) { return; }

            List<Tra_Spending> insert = new List<Tra_Spending>();
            List<Tra_Spending> update = new List<Tra_Spending>();


            foreach (var spend in spends)
            {
                if (!this._context.Tra_Spending.Any(x => x.Created_At == spend.Created_At))
                {
                    insert.Add(spend); // 追加対象の追加
                }
                else if (this._context.Tra_Spending.Where(x => x.Created_At == spend.Created_At).Any(x => x.Updated_At < spend.Updated_At))
                {
                    update.Add(spend); // 更新対象の追加
                }
            }

            if (insert.Count == 0) { return; }

            // 追加処理

            //this._context.Entry(this._context.Tra_Spending).State = EntityState.Added;

            foreach (var ins in insert)
            {
                this._context.Add(new Tra_Spending()
                {
                    User_Id = 1,
                    Spend_Id = ins.Spend_Id,
                    Money = ins.Money,
                    Purchase_Date = ins.Purchase_Date,
                    Description = ins.Description,
                    Linked_Flag = true,
                    Created_At = ins.Created_At,
                    Updated_At = ins.Updated_At,
                    Del_Flag = false
                });
            }

            await this._context.SaveChangesAsync();


            //this._context.Entry(this._context.Tra_Spending).State = EntityState.Modified;

            if (update.Count == 0) { return;  }

            // 更新処理
            foreach (var upd in update)
            {
                var result = this._context.Tra_Spending.FirstOrDefault(x => x.Created_At == upd.Created_At);

                result.User_Id = upd.User_Id;
                result.Money = upd.Money;
                result.Linked_Flag = true;
                result.Purchase_Date = upd.Purchase_Date;
                result.Description = upd.Description;
                result.Spend_Id = upd.Spend_Id;
                result.Updated_At = upd.Updated_At;
                result.Del_Flag = upd.Del_Flag;
            }

            await this._context.SaveChangesAsync();
        }

        public async Task<List<Tra_Spending>> GetUploadSpendeDataAsync()
        {
            var result = await this._context.Tra_Spending
                                .Where(x => x.Linked_Flag == false)
                                .OrderBy(x => x.Id)
                                .ToListAsync();
            return result;
        }
    }

    public class SpendList
    {
        public DateTime Purchase_Date { get; set; }
        public int Money { get; set; }
    }

    public class SpendDetail
    {
        public int Id { get; set; }
        public string Spend_Name { get; set; }
        public int Money { get; set; }
        public DateTime Purchase_Date { get; set; }
        public string Description { get; set; }
        public string Icon_Name { get; set; }
    }
}