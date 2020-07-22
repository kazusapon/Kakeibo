using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eri.Models
{
    public class Income
    {
        private MyContext _context;
        public Income(MyContext context)
        {
            this._context = context;
        }

        public IList<Mst_Income> Get_MasterIncome()
        {
            this._context.Database.EnsureCreated();
            IList<Mst_Income> _result = this._context.Mst_Income.ToList();
            return _result;
        }

        public Tra_Income Income_Fetch(int id)
        {
            var _result = this._context.Tra_Income.FirstOrDefault(x => x.Id == id);
            return _result;
        }

        public async void Delete_Income(int id)
        {
            var _result = this._context.Tra_Income.FirstOrDefault(x => x.Id == id);
            _result.Updated_At = DateTime.Now;
            _result.Del_Flag = true;
            await this._context.SaveChangesAsync();
            return;
        }

        public async void Update_Item(Tra_Income income)
        {
            //更新金額がゼロ円の場合は、そのままリターン
            if (income.Money == 0)
            {
                return;
            }
            this._context.Database.EnsureCreated();
            var _result = Income_Fetch(income.Id);
            _result.User_Id = 1;
            _result.Income_Id = income.Income_Id;
            _result.Payment_Date =income.Payment_Date.Date;
            _result.Money = income.Money;
            _result.Description = income.Description;
            _result.Updated_At = DateTime.Now;
            _result.Linked_Flag = false;
            await this._context.SaveChangesAsync();
        }

        public IEnumerable<IncomeDetail> Get_Income_List(DateTime target_date)
        {
            IEnumerable<IncomeDetail> result = null;
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
            result = from tra_income in this._context.Tra_Income
                     join mst_income in this._context.Mst_Income
                     on tra_income.Income_Id equals mst_income.Id
                     where tra_income.Payment_Date >= start_day
                     where tra_income.Payment_Date < end_day
                     where tra_income.Money > 0
                     where tra_income.Del_Flag == false
                     orderby tra_income.Payment_Date descending
                     orderby tra_income.Id descending
                     select new IncomeDetail
                     {
                         Id = tra_income.Id,
                         Income_Name = mst_income.Income_Name,
                         Money = tra_income.Money,
                         Description = tra_income.Description,
                         Payment_Date = tra_income.Payment_Date,
                         Icon_Name = mst_income.Icon_Name
                     };
            /*
            result = this._context.Tra_Income.Where(x => x.Payment_Date >= start_day)
                    .Where(x => x.Payment_Date < end_day)
                    .Where(x => x.Money > 0)
                    .OrderByDescending(x => x.Payment_Date.Day)
                    .AsEnumerable();*/
                
            return result;
        }

        public async void IncomeListInsertAllAsync(List<Tra_Income> incomes)
        {
            await this._context.Tra_Income.AddRangeAsync(incomes);
            await this._context.SaveChangesAsync();
        }
    }

    public class IncomeList
    {
        public DateTime Payment_Date { get; set; }
        public int Money { get; set; }
    }

    public class IncomeDetail
    {
        public int Id { get; set; }
        public string Income_Name { get; set; }
        public int Money { get; set; }
        public DateTime Payment_Date { get; set; }
        public string Description { get; set; }
        public string Icon_Name { get; set; }
    }
}