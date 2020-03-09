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
    public class ItemSave : BaseViewModel
    {
        public string Member { get; set; }

        public ItemSave(int id = 0)
        {
            if (id > 0)
            {
                CreateMember();
            }
            else
            {
                EditMember();
            }
        }

        private void CreateMember()
        {
            Title = "収入登録";
            Member = "Save_Clicked";
        }

        private void EditMember()
        {
            Title = "収入編集";
            Member = "Update_Clicked";
        }
    }
}
