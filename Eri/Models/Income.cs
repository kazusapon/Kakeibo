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
                    .OrderByDescending(x => x.Payment_Date)
                    .OrderByDescending(x => x.Id).AsEnumerable();
                
            return result;
        }
    }

    public class IncomeList
    {
        public DateTime Payment_Date { get; set; }
        public int Money { get; set; }
    }
}