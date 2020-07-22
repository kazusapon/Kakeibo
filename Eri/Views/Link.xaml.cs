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
            MessagingCenter.Send(this, "dialog_progress", true);

            await Link.RunGetAsunc();

            MessagingCenter.Send(this, "dialog_progress", false);
        }
    }
}