using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eri.Models
{
    class SummaryModel
    {
        private MyContext _context;

        private DateTime start_day;
        private DateTime end_day;

        public SummaryModel(MyContext context)
        {
            this._context = context;
        }

        //支出円グラフ用モデル
        public List<IncomePieModel> GetIncomePie(DateTime date)
        {
            SetDateTime(date);

            var _result = (from m_income in this._context.Mst_Income
                          join t_income in this._context.Tra_Income
                          on m_income.Id equals t_income.Income_Id
                          where t_income.Payment_Date >= start_day
                          where t_income.Payment_Date < end_day
                          group new { m_income, t_income } by new { m_income.Id, m_income.Income_Name } into X
                          orderby X.Key.Id
                          select new IncomePieModel
                          {
                              Label = X.Key.Income_Name,
                              Value = X.Sum(x => x.t_income.Money)
                          }).AsNoTracking().ToList();

            List<IncomePieModel> result = new List<IncomePieModel>();
            result = _result.Concat(GetFixed(start_day, end_day))
                            .OrderByDescending(x => x.Value)
                            .ToList(); //固定値取得メンバを呼び出し、末尾に結合する（金額の降順ソートする）
            return result;

        }

        //固定費を返すメンバ
        private List<IncomePieModel> GetFixed(DateTime start_day, DateTime end_day)
        {
            var _result = this._context.Mst_Fixed
                            .Where(x => x.Start_Date >= start_day)
                            .Where(x => x.End_Date < end_day)
                            .OrderBy(x => x.Id)
                            .Select(x => new IncomePieModel{Label = x.Fixed_Name, Value = x.Money })
                            .AsNoTracking()
                            .ToList();
            return _result;
        }

        //該当月の総支出額を求める
        public int GetTotalSpend(DateTime date)
        {
            int total = 0;
            SetDateTime(date);

            total = this._context.Tra_Spending
                                 .Where(x => x.Purchase_Date >= start_day)
                                 .Where(x => x.Purchase_Date < end_day)
                                 .Sum(x => x.Money);

            return total;
        }

        //該当月の総固定費計を算出
        public int GetTotalFixed(DateTime date)
        {
            int total = 0;
            SetDateTime(date);
            total = this._context.Mst_Fixed
                                 .Where(x => x.Start_Date <= start_day)
                                 .Where(x => x.End_Date > end_day)
                                 .Where(x => x.Del_Flag == false)
                                 .Sum(x => x.Money);

            return total;
        }

        //該当月の総食費目標計を算出
        public int GetTotalAvailable(DateTime date)
        {
            int total = 0;
            SetDateTime(date);
            total = this._context.Mst_Available
                                 .Where(x => x.Start_Date <= start_day)
                                 .Where(x => x.End_Date > end_day)
                                 .Where(x => x.Del_Flag == false)
                                 .Sum(x => x.Money);

            return total;
        }


        //該当月の総収入額を求める
        public int GetTotalIncome(DateTime date)
        {
            int total = 0;
            SetDateTime(date);

            total = this._context.Tra_Income
                                 .Where(x => x.Payment_Date >= start_day)
                                 .Where(x => x.Payment_Date < end_day)
                                 .Sum(x => x.Money);

            return total;
        }


        private void SetDateTime(DateTime date)
        {
            int yyyy = date.Year;
            int mm = date.Month;

            DateTime start_day = new DateTime(yyyy, mm, 1);
            DateTime end_day = start_day.AddMonths(1);
        }
    }

    public class IncomePieModel
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }
}
