using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Eri.Models;
using Eri.ViewModels;

namespace Eri.Views
{
    [DesignTimeVisible(false)]
    public partial class LinkPage : ContentPage
    {
        public LinkPage()
        {
            InitializeComponent();
        }

        public async void TransactionDownload(object sender, EventArgs s)
        {
            var connection = new ConnectionInformation();
            if (!connection.IsRegisted())
            {
                await DisplayAlert("Error", "接続情報が登録されていません。\n設定してください。", "OK");
                return;
            }

            MessagingCenter.Send(this, "dialog_progress", true);

            string test_result = await Link.TestConnect();
            if (test_result == "success")
            {
                await Link.UploadAsync("api/income");
                await Link.UploadAsync("api/spend");

                await Link.DownloadAsync("api/income");
                await Link.DownloadAsync("api/spend");
            }


            MessagingCenter.Send(this, "dialog_progress", false);

            if (test_result == "success")
            {
                await DisplayAlert("Info", "連携処理が終わりました", "OK");
            }
            else
            {
                await DisplayAlert("Error", "サーバとの接続に失敗しました", "OK");
            }
        }

        public async void MasterDownload(object sender, EventArgs s)
        {
            var connection = new ConnectionInformation();
            if (!connection.IsRegisted())
            {
                await DisplayAlert("Error", "接続情報が登録されていません。\n設定してください。", "OK");
                return;
            }

            MessagingCenter.Send(this, "dialog_progress", true);

            string test_result = await Link.TestConnect();
            if (test_result == "success")
            {
                await Link.DownloadAsync("api/fixed");
                await Link.DownloadAsync("api/spendmaster");
                await Link.DownloadAsync("api/incomemaster");
            }

            MessagingCenter.Send(this, "dialog_progress", false);

            if (test_result == "success")
            {
                await DisplayAlert("Info", "連携処理が終わりました", "OK");
            }
            else
            {
                await DisplayAlert("Error", "サーバとの接続に失敗しました", "OK");
            }
        }

        async void Setting_Clicked(object sender, EventArgs e)
        {
            var nav = new NavigationPage(new SettingPage());
            await Navigation.PushModalAsync(nav);
        }
    }
}