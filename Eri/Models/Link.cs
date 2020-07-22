using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Eri.Models
{
    public class Link
    {

        private static HttpClient httpClient = new HttpClient();

        public static async Task RunGetAsunc()
        {
            InitHttpClient(); // 初期化処理

            await GetIncomeData();
        }
        
        private static async Task<IncomeFormat> GetIncomeData()
        {
            IncomeFormat incomeFormat = null;

            HttpResponseMessage responce = await httpClient.GetAsync("/api/Income");
            if (responce.IsSuccessStatusCode)
            {
                incomeFormat = await responce.Content.ReadAsAsync<IncomeFormat>();

                using (var db = new MyContext())
                {
                    Income income = new Income(db);
                    await Task.Run(() => income.IncomeListInsertAllAsync(incomeFormat.Income));
                }
            }

            return incomeFormat;

        }

        private static void InitHttpClient()
        {
            httpClient.BaseAddress = new Uri("http://192.168.10.104:5000/");

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public async void SendIncomeData()
        {
            var senddata = await GetUploadSpendListAsync(); // 送信データの作成
            var json = JsonSerializer.Serialize<IncomeFormat>(senddata); // JSON作成


        }

        // 収入送信データを生成
        private async Task<IncomeFormat> GetUploadSpendListAsync()
        {
            using (var db = new MyContext())
            {
                var _result = await db.Tra_Income
                                .Where(x => x.Del_Flag == false)
                                .Where(x => x.Created_At < x.Updated_At)
                                .Where(x => x.Money == 0)
                                .ToListAsync();
                IncomeFormat spendFormat = new IncomeFormat()
                {
                    Income = _result
                };

                return spendFormat;
            }
        }
    }

    public class IncomeFormat
    {
        public List<Tra_Income> Income { get; set; }
    }

    public class SpendFormat
    {
        public List<Tra_Spending> Spend { get; set; }
    }
}
