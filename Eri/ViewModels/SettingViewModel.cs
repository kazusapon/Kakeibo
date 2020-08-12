using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Xamarin.Forms;

using Eri.Models;
using Eri.Views;

namespace Eri.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {

        public Mst_ConnectionInfo ConnectInfo { get; set; }

        public SettingViewModel()
        {
            ConnectionInformation connect = new ConnectionInformation();

            if (connect.IsCountZero())
            {
                ConnectInfo = new Mst_ConnectionInfo();
            }
            else
            {
                ConnectInfo = connect.GetConnectionInfoSingleOrDefault();
            }

        }

    }
}