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

        public IEnumerable<Mst_Income> Get_MasterIncome()
        {
            IEnumerable<Mst_Income> _result = this._context.Mst_Income.OrderBy(x => x.Id).AsEnumerable();
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
            this._context.Tra_Income.Remove(_result);
            await this._context.SaveChangesAsync();
            return;
        }

        public IEnumerable<Tra_Income> Get_Income_List(int month)
        {
            IEnumerable<Tra_Income> result = null;
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
            result = this._context.Tra_Income.Where(x => x.Payment_Date.Month == month)
                    .Where(x => x.Money > 0)
                    .OrderByDescending(x => x.Payment_Date.Day)
                    .AsEnumerable();
                
            return result;
        }
    }

    public class IncomeList
    {
        public DateTime Payment_Date { get; set; }
        public int Money { get; set; }
    }
}