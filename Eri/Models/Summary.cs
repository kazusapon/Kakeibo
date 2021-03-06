﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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


        //収入の折れ線グラフのライン
        public List<SpendLineModel> GetIncomeLine(DateTime date)
        {
            var _result = this._context.Tra_Income.Where(x => x.Payment_Date.Year == date.Year)
                                                    .Where(x => x.Money > 0)
                                                    .Where(x => x.Del_Flag == false)
                                                    .GroupBy(x => new { x.Payment_Date.Month })
                                                    .Select(x => new SpendLineModel
                                                    {
                                                        Mon = x.Key.Month,
                                                        Money = x.Sum(x => x.Money)
                                                    }).ToList();
            return FillSpendLine(_result);
        }

        //支出の折れ線グラフのライン
        public List<SpendLineModel> GetSpendLine(DateTime date)
        {
            var _result = this._context.Tra_Spending.Where(x => x.Purchase_Date.Year == date.Year)
                                                    .Where(x => x.Money > 0)
                                                    .Where(x => x.Del_Flag == false)
                                                    .GroupBy(x => new { x.Purchase_Date.Month })
                                                    .Select(x => new SpendLineModel
                                                    {
                                                        Mon = x.Key.Month,
                                                        Money = x.Sum(x => x.Money)
                                                    }).ToList();
            return FillSpendLine(_result);
        }

        //01月～12月になるように追加する。
        private List<SpendLineModel> FillSpendLine(List<SpendLineModel> result)
        {
            
            for (int i = 1; i <= 12; i++)
            {
                if (!result.Where(x => x.Mon == i).Any())
                {
                    SpendLineModel spend = new SpendLineModel();
                    spend.Mon = i;
                    spend.Money = 0;
                    result.Add(spend);
                }
            }
            return result.OrderBy(x => x.Mon).ToList();
        }


        //支出円グラフ用モデル
        public List<SpendPieModel> GetSpendPie(DateTime date)
        {
            SetDateTime(date);

            var _result = (from m_spend in this._context.Mst_Spend
                          join t_spend in this._context.Tra_Spending
                          on m_spend.Id equals t_spend.Spend_Id
                          where t_spend.Purchase_Date >= start_day
                          where t_spend.Purchase_Date < end_day
                          where t_spend.Del_Flag == false
                          group new { m_spend, t_spend } by new { m_spend.Id, m_spend.Spend_Name } into X
                          orderby X.Key.Id
                          select new SpendPieModel
                          {
                              Label = X.Key.Spend_Name,
                              Value = X.Sum(x => x.t_spend.Money)
                          }).AsNoTracking().ToList();

            List<SpendPieModel> result = new List<SpendPieModel>();
            result = _result.Concat(GetFixed(start_day, end_day))
                            .OrderByDescending(x => x.Value)
                            .ToList(); //固定値取得メンバを呼び出し、末尾に結合する（金額の降順ソートする）
            return result;

        }

        public int TotalSpend(DateTime date)
        {
            SetDateTime(date);
            int result = 0;

            result =  this._context.Tra_Spending.Where(x => x.Purchase_Date >= start_day)
                                               .Where(x => x.Purchase_Date < end_day)
                                               .Where(x => x.Del_Flag == false)
                                               .Sum(x => x.Money);
            return result;
        }

        //固定費を返すメンバ
        private List<SpendPieModel> GetFixed(DateTime start_day, DateTime end_day)
        {
            var sum = this._context.Mst_Fixed
                            .Where(x => x.Start_Date >= start_day)
                            .Where(x => x.End_Date < end_day)
                            .Sum(x => x.Money);

            List<SpendPieModel> _result = new List<SpendPieModel>();
            _result.Add(new SpendPieModel { Label = "固定費", Value = sum });
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
                                 .Where(x => x.Del_Flag == false)
                                 .Sum(x => x.Money);

            return total;
        }

        //食費のみの合計を取得
        public int GetTotalShokuhi(DateTime date)
        { 
            int total = 0;
            SetDateTime(date);
            total = this._context.Tra_Spending
                                   .Where(x => x.Purchase_Date >= start_day)
                                   .Where(x => x.Purchase_Date < end_day)
                                   .Where(x => x.Spend_Id == 1)
                                   .Where(x => x.Del_Flag == false)
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
                                 .Where(x => x.End_Date >= end_day)
                                 .Where(x => x.Del_Flag == false)
                                 .Sum(x => x.Money);

            return total;
        }

        //該当月の総食費目標計を算出
        //現状は食費で固定する。
        //今後変わるかもしれない。
        public int GetTotalAvailable(DateTime date)
        {
            int total = 0;
            SetDateTime(date);
            total = this._context.Mst_Available
                                    .Where(x => x.Del_Flag == false)
                                    .Where(x => x.Start_Date <= start_day)
                                    .Where(x => x.End_Date >= end_day)
                                    .OrderBy(x => x.Id)
                                    .Select(x => x.Money)
                                    .FirstOrDefault();
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
                                 .Where(x => x.Del_Flag == false)
                                 .Sum(x => x.Money);

            return total;
        }



        private void SetDateTime(DateTime date)
        {
            int yyyy = date.Year;
            int mm = date.Month;

            this.start_day = new DateTime(yyyy, mm, 1);
            this.end_day = start_day.AddMonths(1);
        }
    }

    public class SpendPieModel
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }

    public class SpendLineModel
    {
        public int Mon { get; set; }
        public int Money { get; set; }
    }
}
