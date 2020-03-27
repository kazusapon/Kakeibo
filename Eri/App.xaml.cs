using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Eri.Services;
using Eri.Views;
using Eri.Models;
namespace Eri
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            //Seedデータ
            SeedData seed = new SeedData();
            seed.InsertSeedData();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
