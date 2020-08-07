using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Threading.Tasks;
using Eri.ViewModels;

using Eri.Models;

namespace Eri.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class SettingPage : ContentPage
    {

        private SettingViewModel viewModel;

        public SettingPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SettingViewModel();
        }

        async void SaveClicked(object sender, EventArgs e)
        {

            ConnectionInformation connect = new ConnectionInformation();

            bool result = await connect.InsertOrUpdateAsync(this.viewModel.ConnectInfo);

            if (!result)
            {
                await DisplayAlert("Error", "登録できませんでした、\n入力内容を確認してください", "OK");
            }
            else
            {
                await DisplayAlert("Info", "登録完了しました", "OK");
            }
        }

        async void CancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}