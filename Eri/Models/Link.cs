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
using System.Runtime.CompilerServices;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace Eri.Models
{
    public class Link
    {

        private static HttpClient httpClient = new HttpClient();

        private static string apiPath;

        public static async Task<string> UploadAsync(string _path)
        {
            InitHttpClient(); // 初期化処理

            apiPath = _path;

            if (apiPath.Equals("api/income"))
            {
                await UploadIncome();
                return "OK";
            }
            else if (apiPath.Equals("api/spend"))
            {
                await UploadSpend();
                return "OK";
            }

            return "NGProgram";
        }

        public static async Task<string> TestConnect()
        {
            try
            {
                InitHttpClient(); // 初期化処理
                string test_endpoint = "api/income";
                HttpResponseMessage responce = await httpClient.GetAsync(test_endpoint); // 接続テストをするためのエンドポイントを仮で設定する。
                return "success";
            }
            catch(Exception)
            {
                return "NG";
            }


        }

        public static async Task DownloadAsync(string _path)
        {
            InitHttpClient(); // 初期化処理

            apiPath = _path;

            if (apiPath.Equals("api/income"))
            {
                await GetIncomeDataFromApi();
            }
            else if (apiPath.Equals("api/spend"))
            {
                await GetSpendDataFromApi();
            }
            else if (apiPath.Equals("api/spendmaster"))
            {
                await GetSpendMasterFromApi();
            }
            else if (apiPath.Equals("api/incomemaster"))
            {
                await GetIncomeMasterFromApi();
            }
            else if (apiPath.Equals("api/fixed"))
            {
                await GetFiexdDataFromApi();
            }

            return;
        }

        private static async Task<Uri> UploadSpend()
        {
            SpendFormat spendFormat = new SpendFormat();
            using (var db = new MyContext())
            {
                Spend spend = new Spend(db);
                spendFormat.Spend = await spend.GetUploadSpendeDataAsync();

                foreach (var v in spendFormat.Spend)
                {
                    Console.WriteLine(v.Del_Flag);
                    Console.WriteLine(v.Updated_At);
                    Console.WriteLine(v.Money);
                }

                HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                    apiPath, // APIエンドポイント
                    spendFormat // JSONデータ（おそらく自動でシリアライズしてくれるはず）
                );

                response.EnsureSuccessStatusCode();

                return response.Headers.Location;
            }
        }

        private static async Task<Uri> UploadIncome()
        {
            IncomeFormat incomeFormat = new IncomeFormat();
            using (var db = new MyContext())
            {
                Income income = new Income(db);
                incomeFormat.Income = await income.GetUploadIncomeDataAsync();

                HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                    apiPath, // APIエンドポイント
                    incomeFormat // JSONデータ（おそらく自動でシリアライズしてくれるはず）
                );

                response.EnsureSuccessStatusCode();

                return response.Headers.Location;
            }
        }

        private static async Task GetSpendMasterFromApi()
        {
            List<Mst_Spend> spendMasterList = null;

            HttpResponseMessage responce = await httpClient.GetAsync(apiPath);
            if (responce.IsSuccessStatusCode)
            {
                spendMasterList = await responce.Content.ReadAsAsync<List<Mst_Spend>>();
                using (var db = new MyContext())
                {
                    Spend spend = new Spend(db);
                    spend.MstSpendDelete();
                    await spend.SpendMasterInsertAllAsync(spendMasterList);
                    //await spend.UpdateLinked();
                }
            }

            return;

        }

        private static async Task GetIncomeMasterFromApi()
        {
            List<Mst_Income> incomeMasterList = null;

            HttpResponseMessage responce = await httpClient.GetAsync(apiPath);
            if (responce.IsSuccessStatusCode)
            {
                incomeMasterList = await responce.Content.ReadAsAsync<List<Mst_Income>>();
                using (var db = new MyContext())
                {
                    Income income = new Income(db);
                    income.MstIncomeDelete();
                    await income.IncomeMasterInsertAllAsync(incomeMasterList);
                    //await spend.UpdateLinked();
                }
            }

            return;

        }

        private static async Task GetSpendDataFromApi()
        {
            List<Tra_Spending> spendList = null;

            HttpResponseMessage responce = await httpClient.GetAsync(apiPath);
            if (responce.IsSuccessStatusCode)
            {
                spendList = await responce.Content.ReadAsAsync<List<Tra_Spending>>();
                using (var db = new MyContext())
                {
                    Spend spend = new Spend(db);
                    // spend.TraSpendingDelete();
                    await spend.SpendListInsertOrUpdateAllAsync(spendList);
                    //await spend.UpdateLinked();
                }
            }

            return;
        }


        private static async Task GetIncomeDataFromApi()
        {
            List<Tra_Income> incomeList = null;

            HttpResponseMessage responce = await httpClient.GetAsync(apiPath);
            if (responce.IsSuccessStatusCode)
            {
                incomeList = await responce.Content.ReadAsAsync<List<Tra_Income>>();

                if (incomeList.Count == 0 || incomeList == null)
                {
                    return; // データなし
                }

                using (var db = new MyContext())
                {
                    Income income = new Income(db);
                    // income.TraIncomeDelete();
                    await income.IncomeListInsertOrUpdateAllAsync(incomeList);
                    //await income.UpdateLinked();
                }
            }

            return;

        }

        private static void InitHttpClient()
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            if (httpClient.BaseAddress == null)
            {
                var uri_str = new ConnectionInformation();

                httpClient.BaseAddress = new Uri(uri_str.BuildURLHostname());
                httpClient.Timeout = TimeSpan.FromSeconds(30);
            }

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        private static async Task<string> GetFiexdDataFromApi()
        {
            List<Mst_Fixed> fiexdMasterList = null;

            try
            {
                HttpResponseMessage responce = await httpClient.GetAsync(apiPath);
                if (responce.IsSuccessStatusCode)
                {
                    fiexdMasterList = await responce.Content.ReadAsAsync<List<Mst_Fixed>>();
                    using (var db = new MyContext())
                    {
                        Fixed fix = new Fixed(db);
                        fix.MstFiexdDelete();
                        await fix.FixedMasterInsertAllAsync(fiexdMasterList);
                        //await spend.UpdateLinked();
                    }
                }

                return responce.IsSuccessStatusCode.ToString();
            }
            catch (Exception)
            {
                return "NG";
            }
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
